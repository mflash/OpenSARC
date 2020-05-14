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
    public class Resumo
    {
        public string Sala { get; set; }
        public string Hosts { get; set; }
        public int Fail { get; set; }
    }

    public class Acessos
    {
        public string Host { get; set; }
        public string Sala { get; set; }
        public string Pos { get; set; }
        public int Fail { get; set; }
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
        public string Mac { get; set; }
        public String Obs { get; set; }
    }

    public class Maquina
    {
        public string Mac { get; set; }
        public string Sala { get; set; }
        public string Host { get; set; }
        public string Pos { get; set; }
    }

    private List<Resumo> listaResumo;
    private List<Details> listaDetails;

    private SortedDictionary<string, Resumo> dic;
    private SortedDictionary<string, Maquina> maquinas;

    private String db;

    private String dataBase;

    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime data = DateTime.Now;
        dataBase = String.Format("{0}-{1}-{2}", data.Year, data.Month > 7 ? "8" : "1", 1);

        db = "SARCFACINcs"; // Session["DB"].ToString();
        string sala = Request.QueryString["sala"];
        string user = Request.QueryString["user"];
        string host = Request.QueryString["host"];
        listaResumo = PreencheAcessos();
        grvAccessStats.DataSource = listaResumo;
        grvAccessStats.DataBind();
        lblSala.Text = "";

        if(sala != null)
        {
            //lblSala.Text = "Sala: "+sala;
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
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString))
        {
            con.Open();

            string sql = "select logg.host, usuario, datahora, salas.pos "
                       + "from Loggin logg "
                       + "inner join Loggin_Salas salas on logg.host = salas.host "
                       + "where status = 'FAIL' and sala = '"+sala+"' and datalength(usuario) > 0 "
                       + "and datahora > '"+dataBase+"' "
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
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString))
        {
            con.Open();

            string sql = "select logg.host, salas.sala, datahora, salas.pos, status from Loggin logg inner join Loggin_Salas salas on logg.host = salas.host where usuario = '"+user+"' and datahora > '"+dataBase+"' order by datahora desc";
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
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString))
        {
            con.Open();

            string sql = "select salas.sala, usuario, datahora, salas.pos, status, mac "
                       + "from Loggin logg "
                       + "inner join Loggin_Salas salas on logg.host = salas.host "
                       + "where logg.host = '" + host + "' and datalength(usuario) > 0 "
                       + "and usuario <> 'gtitadm' "
                       + "and datahora > '"+dataBase+"' "
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
                        string mac = "?";
                        if(!reader.IsDBNull(5))
                            mac = reader.GetString(5);
                        mac = mac.ToUpper();
                        Details temp = new Details
                        {
                            Host = host,
                            Sala = sala,
                            User = user,
                            Datahora = datahora,
                            Pos = pos,
                            OkHost = status,
                            Mac = mac
                        };
                        if (maquinas.ContainsKey(mac))
                        {
                            Maquina maq = maquinas[mac];
                            if (maq != null)
                            {
                                if (maq.Sala != sala || maq.Pos != pos)
                                    temp.Obs = maq.Host + " - " + maq.Sala + " (" + maq.Pos + ")";
                                else
                                    temp.Obs = "";
                            }
                        }
                        else
                            temp.Obs = "No info";

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
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString))
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

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString))
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


    private List<Resumo> PreencheAcessos()
    {
        List<Acessos> lista = new List<Acessos>();
        dic = new SortedDictionary<string, Resumo>();
        maquinas = new SortedDictionary<string, Maquina>();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString))
        {
            con.Open();

            string sql = @"SELECT logg.host, sala, pos, COUNT(logg.host) totalfail
FROM Loggin_Salas salas
inner join Loggin logg
on salas.host = logg.host
where status='FAIL'
and datalength(logg.usuario) > 0
and logg.datahora > DATEADD(day,-7,DATEADD(day,datediff(day,0,getdate()),0))
and logg.host not in (
select host from Loggin
where datahora > logg.datahora
and status = 'OK'
)
group by logg.host, sala, pos
order by totalfail desc";

            using (SqlCommand selCommand = new SqlCommand(sql, con))
            {
                try
                {
                    SqlDataReader reader = selCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        string host = reader.GetString(0);
                        string sala = reader.GetString(1);
                        string pos = reader.GetString(2);
                        int totalSala = reader.GetInt32(3);
                        Acessos temp = new Acessos
                        {
                            Host = host,
                            Sala = sala,
                            Pos = pos,
                            Fail = totalSala
                        };
                        lista.Add(temp);
                        if (dic.ContainsKey(sala))
                        {
                            Resumo res = dic[sala];
                            res.Fail += totalSala;
                            res.Hosts += ", " + host + " (" + totalSala + ")";
                        }
                        else
                        {
                            Resumo res = new Resumo();
                            res.Sala = sala;
                            res.Fail = totalSala;
                            res.Hosts = host + " (" + totalSala + ")";
                            dic.Add(sala, res);
                        }
                        //dic.Add(sala, temp);
                    }
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

            sql = @"select macaddr, host, sala, pos from Loggin_Salas where macaddr is not null";
            using (SqlCommand selCommand = new SqlCommand(sql, con))
            {
                try
                {
                    SqlDataReader reader = selCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        string mac = reader.GetString(0);
                        string host = reader.GetString(1);
                        string sala = reader.GetString(2);
                        string pos = reader.GetString(3);
                        Maquina temp = new Maquina
                        {
                            Mac = mac,
                            Host = host,
                            Sala = sala,
                            Pos = pos
                        };
                        maquinas.Add(mac, temp);
                        //lista.Add(temp);
                        //dic.Add(sala, temp);
                    }
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

            return new List<Resumo>(dic.Values).OrderByDescending(v => v.Fail).ToList<Resumo>();
        }
    }
}