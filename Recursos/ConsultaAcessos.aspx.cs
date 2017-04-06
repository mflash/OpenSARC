using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Recursos_ConsultaAcessos : System.Web.UI.Page
{
    public class Acessos
    {
        public string Sala { get; set; }
        public int Ok { get; set; }
        public int Fail { get; set; }
        public float PercOK { get; set; }
    }

    public class Details
    {
        public string Sala { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public DateTime Datahora { get; set; }
        public string Pos { get; set; }
        public string OkUser { get; set; }
        public string OkHost { get; set; }
    }

    private List<Acessos> listaAcessos;
    private List<Details> listaDetails;

    private SortedDictionary<string, Acessos> dic;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sala = Request.QueryString["sala"];
        string user = Request.QueryString["user"];
        string host = Request.QueryString["host"];
        listaAcessos = PreencheAcessos();
        grvAccessStats.DataSource = listaAcessos;
        grvAccessStats.DataBind();
        lblSala.Text = "";

        if(sala != null)
        {
            lblSala.Text = "Sala: "+sala;
            listaDetails = PreencheDetalhesSala(sala);
            grvDetails.DataSource = listaDetails;
            grvDetails.DataBind();
            imgMapa.ImageUrl = "~/Recursos/img/" + sala + ".png";
        }
        if(user != null)
        {
            lblSala.Text = "Usuário: " + user;
            listaDetails = PreencheDetalhesUser(user);
            grvDetails.DataSource = listaDetails;
            grvDetails.DataBind();            
        }
        if(host != null)
        {
            lblSala.Text = "Host: " + host;
            listaDetails = PreencheDetalhesHost(host);
            grvDetails.DataSource = listaDetails;
            grvDetails.DataBind();
            if (listaDetails.Count > 0)
            {
                sala = listaDetails[0].Sala.Trim();
                imgMapa.ImageUrl = "~/Recursos/img/" + sala + ".png";
            }            
        }
    }

    private List<Details> PreencheDetalhesSala(string sala)
    {
        List<Details> lista = new List<Details>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SARCFACINcs"].ConnectionString))
        {
            con.Open();

            string sql = "select logg.host, usuario, datahora, salas.pos "
                       + "from Loggin logg "
                       + "inner join Loggin_Salas salas on logg.host = salas.host "
                       + "where status = 'FAIL' and sala = '"+sala+"' and datalength(usuario) > 0 "
                       + "order by datahora desc";
            using (SqlCommand selCommand = new SqlCommand(sql, con))
            {
                try
                {
                    SqlDataReader reader = selCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        string host = reader.GetString(0);
                        string user = reader.GetString(1);
                        DateTime datahora = reader.GetDateTime(2);
                        string pos = reader.GetString(3);
                        Details temp = new Details
                        {
                            Sala = sala,
                            Host = host,
                            User = user,
                            Datahora = datahora,
                            Pos = pos
                        };
                        VerificaAcesso(temp);                            
                        lista.Add(temp);
                    }
                    //Response.Redirect("login.aspx");
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Response.Write("<b>something really bad happened.....Please try again</b> ");
                }
                finally
                {
                    //con.Close();                    
                }
            }
        }
        return lista;
    }

    private List<Details> PreencheDetalhesUser(string user)
    {
        List<Details> lista = new List<Details>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SARCFACINcs"].ConnectionString))
        {
            con.Open();

            string sql = "select logg.host, salas.sala, datahora, salas.pos, status from Loggin logg inner join Loggin_Salas salas on logg.host = salas.host where usuario = '"+user+"' order by datahora desc";
            using (SqlCommand selCommand = new SqlCommand(sql, con))
            {
                try
                {
                    SqlDataReader reader = selCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        string host = reader.GetString(0);
                        string sala = reader.GetString(1);
                        DateTime datahora = reader.GetDateTime(2);
                        string pos = reader.GetString(3);
                        string status = reader.GetString(4);
                        Details temp = new Details
                        {
                            Host = host,
                            Sala = sala,
                            User = user,
                            Datahora = datahora,
                            Pos = pos,
                            OkUser = status
                        };
                        /*if (!VerificaAcesso(temp))
                            temp.Ok = "NÃO";
                        else
                            temp.Ok = "OK";*/
                        lista.Add(temp);
                    }
                    //Response.Redirect("login.aspx");
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Response.Write("<b>something really bad happened.....Please try again</b> ");
                }
                finally
                {
                    //con.Close();                    
                }
            }
        }
        return lista;
    }

    private List<Details> PreencheDetalhesHost(string host)
    {
        List<Details> lista = new List<Details>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SARCFACINcs"].ConnectionString))
        {
            con.Open();

            string sql = "select salas.sala, usuario, datahora, salas.pos, status "
                       + "from Loggin logg "
                       + "inner join Loggin_Salas salas on logg.host = salas.host "
                       + "where logg.host = '" + host + "' and datalength(usuario) > 0 "
                       + "order by datahora desc";
            using (SqlCommand selCommand = new SqlCommand(sql, con))
            {
                try
                {
                    SqlDataReader reader = selCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        string sala = reader.GetString(0);
                        string user = reader.GetString(1);
                        DateTime datahora = reader.GetDateTime(2);
                        string pos = reader.GetString(3);
                        string status = reader.GetString(4);
                        Details temp = new Details
                        {
                            Host = host,
                            Sala = sala,
                            User = user,
                            Datahora = datahora,
                            Pos = pos,
                            OkHost = status
                        };
                        /*if (!VerificaAcesso(temp))
                            temp.Ok = "NÃO";
                        else
                            temp.Ok = "OK";*/
                        lista.Add(temp);
                    }
                    //Response.Redirect("login.aspx");
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Response.Write("<b>something really bad happened.....Please try again</b> ");
                }
                finally
                {
                    //con.Close();                    
                }
            }
        }
        return lista;
    }

    private void VerificaAcesso(Details aux)
    {
        string d = aux.Datahora.ToString("yyyy-MM-dd hh:mm:ss");
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SARCFACINcs"].ConnectionString))
        {
            con.Open();            
            string sql = "select host, usuario, datahora from Loggin where status = 'OK' and host = '" + aux.Host + "' and datahora > '" + d+"'";
            using (SqlCommand selCommand = new SqlCommand(sql, con))
            {
                try
                {
                    SqlDataReader reader = selCommand.ExecuteReader();
                    if (reader.Read())
                        aux.OkHost = "OK";
                    else
                        aux.OkHost = "NÃO";
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Response.Write("<b>something really bad happened.....Please try again</b> ");
                }
                finally
                {
                    con.Close();                    
                }
            }
        }

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SARCFACINcs"].ConnectionString))
        {
            con.Open();            
            string sql = "select host, usuario, datahora from Loggin where status = 'OK' and usuario = '" + aux.User + "' and datahora > '" + d + "'";
            using (SqlCommand selCommand = new SqlCommand(sql, con))
            {
                try
                {
                    SqlDataReader reader = selCommand.ExecuteReader();
                    if (reader.Read())
                        aux.OkUser = "OK";
                    else
                        aux.OkUser = "NÃO";
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Response.Write("<b>something really bad happened.....Please try again</b> ");
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }


    private List<Acessos> PreencheAcessos()
    {
        List<Acessos> lista = new List<Acessos>();
        dic = new SortedDictionary<string, Acessos>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SARCFACINcs"].ConnectionString))
        {
            con.Open();

            string sql = "select salas.sala, count(logg.host) "
                       + "from Loggin logg "
                       + "inner join Loggin_Salas salas on logg.host = salas.host "
                       + "where status = 'OK' "
                       + "group by salas.sala";
            using (SqlCommand selCommand = new SqlCommand(sql, con))
            {                
                try
                {                    
                    SqlDataReader reader = selCommand.ExecuteReader();
                    while(reader.Read()) {                       
                        string sala = reader.GetString(0);
                        int totalSala = reader.GetInt32(1);
                        Acessos temp = new Acessos
                        {
                            Sala = sala,                            
                            Ok = totalSala 
                        };
                        //lista.Add(temp);
                        dic.Add(sala, temp);
                    }                    
                    //Response.Redirect("login.aspx");
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Response.Write("<b>something really bad happened.....Please try again</b> ");
                }
                finally
                {
                    //con.Close();                    
                }
            }

            // Verifica FAIL
            sql = "select salas.sala, count(logg.host) "
                + "from Loggin logg "
                + "inner join Loggin_Salas salas on logg.host = salas.host "
                + "where status = 'FAIL' "
                + "and datalength(logg.usuario) > 0"
                + "group by salas.sala";
            using (SqlCommand selCommand = new SqlCommand(sql, con))
            {
                try
                {
                    SqlDataReader reader = selCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        string sala = reader.GetString(0);
                        int totalSala = reader.GetInt32(1);
                        Acessos temp = dic[sala];
                        temp.Fail = totalSala;
                        //Acessos temp = new Acessos
                        //{
                        //    Sala = sala,
                        //    PercOK = totalSala
                        //};
                        //lista.Add(temp);
                    }
                    //Response.Redirect("login.aspx");
                }
                catch (Exception ex)
                {
                    Response.Write("<b>something really bad happened.....Please try again</b> ");
                }
                finally
                {
                    con.Close();
                }
            }
        }
        foreach (var keyval in dic)
        {
            Acessos temp = keyval.Value;
            temp.PercOK = ((float)temp.Ok / (temp.Fail+temp.Ok)) * 100;
            lista.Add(temp);
            //Debug.WriteLine(keyval.Value.Sala + " - " + keyval.Value.Fail);
        }
        return lista;
    }
}