using BusinessData.BusinessLogic;
using BusinessData.Distribuicao.Entities;
using BusinessData.Entities;
using BusinessData.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default_ExportIcal : System.Web.UI.Page
{
    private BusinessData.Entities.Calendario cal;
    private CategoriaDataBO cdataBo = new CategoriaDataBO();
    List<CategoriaData> listCData;

    protected Data VerificaData(DateTime dt)
    {
        foreach (Data data in cal.Datas)
            if (dt == data.Date)
                return data;
        return null;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        cal = null;
        Guid idturma = new Guid();
        bool mostraAval = true;

        if (Request.QueryString["id"] != null)
        {
            listCData = cdataBo.GetCategoriaDatas();
            try
            {
                idturma = new Guid(Request.QueryString["id"]);
                int ano = int.Parse(Request.QueryString["ano"]);
                int semestre = int.Parse(Request.QueryString["sem"]);
                CalendariosBO calBO = new CalendariosBO();
                cal = calBO.GetCalendarioByAnoSemestre(ano, semestre);
                string strProx = Request.QueryString["aval"];
                if (string.IsNullOrEmpty(strProx))
                    mostraAval= false;
            }
            catch (FormatException)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Codigo de turma inválido!");
            }

            Session["TurmaId"] = idturma;
            CategoriaAtividadeBO cateBO = new CategoriaAtividadeBO();
            List<CategoriaAtividade> listaAtividades = cateBO.GetCategoriaAtividade();
            AulaBO AulaBO = new AulaBO();
            List<Aula> listaAulas = null;
            try
            {
                listaAulas = AulaBO.GetAulas(idturma);
            }
            catch (Exception)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Codigo de turma inválido!");
            }
            Disciplina d = listaAulas[0].TurmaId.Disciplina;
            CategoriaDisciplina cat = d.Categoria;

            string sala = Regex.Replace(listaAulas[0].TurmaId.Sala, "32/A", "32");
            string titulo = d.Cod + "-" + d.Cred + " " + d.Nome + " (" + listaAulas[0].TurmaId.Numero + ") - " + sala;//" "+facin;
            lblTitulo.Text = titulo;

            string shortTitle = "";
            foreach(string part in d.Nome.Split())
            {
                if (part.Length == 1 && part[0] != 'I')
                    continue;
                if (part == "de" || part == "do" || part == "da" || part == "em")
                    continue;
                if (part == "I" || part == "II" || part == "III" || part == "IV" || part == "V"
                    || part == "A" || part == "B" || part == "C")
                    shortTitle += part;
                else
                    shortTitle += char.ToUpper(part[0]);
            }
            string st = shortTitle;

            string output = "BEGIN:VCALENDAR\n";
            output += "PRODID:-//SARC//" + d.Cod + "-" + d.Cred + " " + d.Nome + "//PT\n";
            output += "VERSION:2.0\n";
            output += "CALSCALE:GREGORIAN\n";
            output += "METHOD:PUBLISH\n";
            output += "X-WR-CALNAME:" + st + "\n";
            output += "X-WR-TIMEZONE:America/Sao_Paulo\n";
            output += "X-WR-CALDESC:" + st + "\n";

            DateTime hoje = DateTime.Now;

            DataTable tabela = new DataTable();

            AlocacaoBO alocBO = new AlocacaoBO();

            foreach (DataGridColumn coluna in dgAulas.Columns)
            {
                tabela.Columns.Add(coluna.HeaderText);
            }

            int totalAulas = 1;
            DataRow dr;
            bool notset = true;
            foreach (Aula aula in listaAulas)
            {
                string hi = Horarios.ParseToDateTime(aula.Hora).TimeOfDay.ToString().Substring(0, 5);
                string hf = Horarios.ParseToDateTime(aula.Hora).TimeOfDay.Add(TimeSpan.FromMinutes(90)).ToString().Substring(0,5);
                dr = tabela.NewRow();
                dr["#"] = totalAulas++;
                dr["Dia"] = DataHelper.GetDia(aula.Data.DayOfWeek);
                dr["Data"] = aula.Data.Date.ToShortDateString();
                dr["Hora"] = aula.Hora + "<br>" + hi + " - " + hf;
                dr["Descrição"] = aula.DescricaoAtividade;
                dr["Atividade"] = aula.CategoriaAtividade.Descricao;
                dr["Prox"] = "";              

                DateTime startDt = aula.Data.Add(Horarios.ParseToDateTime(aula.Hora).TimeOfDay).AddHours(3);
                DateTime endDt = startDt.AddMinutes(90);

                List<BusinessData.Entities.Recurso> recAlocados = alocBO.GetRecursoAlocadoByAula(aula.Data, aula.Hora, (Guid)aula.Id);
                string aux = "";
                string recHoje = "";
                shortTitle = st;
                foreach (var rec in recAlocados)
                {
                    if (aux != String.Empty)
                        aux += "<br/>" + rec.Descricao;
                    else
                    {
                        aux = rec.Descricao;
                        //Debug.WriteLine("Tipo recurso:" + rec.Tipo);
                        if (rec.Tipo == 'L' || rec.Tipo == 'A' || rec.Tipo == 'D')
                        {
                            recHoje = rec.Descricao;
                            shortTitle += " "+recHoje.Split().Last();
                            break;
                        }
                    }
                }
                dr["Recursos"] = aux;
                dr["CorDaData"] = aula.CategoriaAtividade.Cor.ToArgb();

                if (aula.Data.Date >= cal.InicioG2)
                {                    
                    dr["#"] = "";
                    totalAulas--;
                }

                Data data = VerificaData(aula.Data.Date);
                bool diaLetivo = true;
                if (data != null)
                {
                    foreach (CategoriaData c in listCData)
                        if (c.Id == data.Categoria.Id) { 
                            if (!c.DiaLetivo)
                            {
                                dr["Descrição"] = c.Descricao + (aula.DescricaoAtividade != "Feriado" ? " (era " + aula.DescricaoAtividade + ")" : "");
                                dr["#"] = "";
                                totalAulas--;
                                diaLetivo = false;
                                break;
                            }
                        }
                }
                //Debug.WriteLine("Cor aula: " + aula.CategoriaAtividade.Cor.ToString());
                tabela.Rows.Add(dr);

                if (recHoje == string.Empty)
                {
                    recHoje = sala;
                    shortTitle += " "+recHoje;
                }

                if (diaLetivo)
                {
                    if (mostraAval && aula.CategoriaAtividade.Descricao != "Prova" && aula.CategoriaAtividade.Descricao != "Prova de Substituição"
                        && aula.CategoriaAtividade.Descricao != "Prova de G2" && aula.CategoriaAtividade.Descricao != "Trabalho")
                        continue;
                    output += "BEGIN:VEVENT\n";
                    output += "DESCRIPTION: " + aula.DescricaoAtividade + "\n";
                    output += "SUMMARY: " + shortTitle+"\n";
                    output += string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}\n", DateTime.Now);
//                    output += "DTSTART:"+startDt.Year+startDt.Month+startDt.Day+startDt.Hour+startDt.Minute+startDt.Second+"\n";
                    //output += "DTSTART:" + startDt.ToString() + "\n";
                    output += string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}\n", startDt);
                    output += string.Format("DTEND:{0:yyyyMMddTHHmmssZ}\n", endDt);
                    output += "LOCATION:" + recHoje + "\n";
                    output += "SEQUENCE:0\n";
                    output += "TRANSP:OPAQUE\n";
                    SHA256 sha = SHA256.Create();                    
                    byte[] uidarray = sha.ComputeHash(Encoding.Unicode.GetBytes(aula.DescricaoAtividade+startDt.ToString()+shortTitle+endDt.ToString()));
                    string uid = Convert.ToBase64String(uidarray);
                    output += "UID:" + uid+"\n";
                    output += "END:VEVENT\n";
                }
            }
            dgAulas.DataSource = tabela;
            dgAulas.ItemDataBound += new DataGridItemEventHandler(dgAux_ItemDataBound);
            dgAulas.DataBind();

            output += "END:VCALENDAR\n";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + st+".ics");
            Response.ContentType = "text/calendar";
            Response.Write(output);

            Response.End();
        }
    }

    void dgAux_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lblData = (Label)e.Item.FindControl("lblData");
            DateTime dataAtual = Convert.ToDateTime(lblData.Text);
            Label lblAux = (Label)e.Item.FindControl("lblCorDaData");
            Label lblDescricao = (Label)e.Item.FindControl("lblDescricao");
            Label lblProx = (Label)e.Item.FindControl("lblProx");
            Color cor = Color.FromArgb(int.Parse(lblAux.Text));
            if ((dataAtual >= cal.InicioG2))
            {
                cor = Color.LightGray;
            }
            if (lblDescricao.Text.StartsWith("Feriado") || lblDescricao.Text.StartsWith("Suspensão"))
                cor = Color.Red;
            e.Item.BackColor = cor;
            if(lblProx.Text == "X")
            {
                e.Item.Cells[2].BackColor = Color.DarkBlue;
                e.Item.Cells[2].ForeColor = Color.White;
                //e.Item.ForeColor = Color.White;
                //e.Item.BackColor = Color.DarkBlue;
                e.Item.Font.Bold = true;
                //e.Item.BorderColor = Color.Red;
            }
            //Debug.WriteLine("Cor: " + e.Item.BackColor.ToString());
        }
    }
}
