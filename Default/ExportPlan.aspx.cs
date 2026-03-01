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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default_Export : System.Web.UI.Page
{
    private BusinessData.Entities.Calendario cal;
    private CategoriaDataBO cdataBo = new CategoriaDataBO();
    private RequisicoesBO reqBO = new RequisicoesBO();
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

            DateTime hoje = DateTime.Now;
            //           if (mostraProx)
            //               lblProx.Text = "26/08/2019 [QUA LM] - Apresentação (Sala de aula)";

            DataTable tabela = new DataTable();

            foreach (DataGridColumn coluna in dgAulas.Columns)
            {
                tabela.Columns.Add(coluna.HeaderText);
            }
            //            tabela.Columns.Add("Recursos");

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

                List<BusinessData.Entities.Requisicao> recSolicitados = reqBO.GetRequisicoesPorAula(aula.Id, cal);
                //                dr["Recursos"] = "";
                string aux = "";
                string recHoje = "";
                foreach (var rec in recSolicitados)
                {
                    if (aux != String.Empty)
                        aux += "<br/>" + rec.Prioridade+ ": " + rec.CategoriaRecurso.Descricao;
                    else
                    {
                        aux = rec.CategoriaRecurso.Descricao;
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
                if (data != null)
                {
                    foreach (CategoriaData c in listCData)
                        if (c.Id == data.Categoria.Id) { 
                            if (!c.DiaLetivo)
                            {
                                dr["Descrição"] = c.Descricao; //+ (aula.DescricaoAtividade != "Feriado" ? " (era " + aula.DescricaoAtividade + ")" : "");
                                dr["#"] = "";
                                totalAulas--;
                                break;
                            }
                        }
                }
                //Debug.WriteLine("Cor aula: " + aula.CategoriaAtividade.Cor.ToString());
                tabela.Rows.Add(dr);

                if (recHoje == string.Empty)
                    recHoje = sala;
                else if (recHoje.StartsWith("Retirar"))
                    recHoje = sala + " - " + recHoje;

                if (notset)
                {// && hoje.Date == aula.Data.Date)
                    //Debug.WriteLine("Aula: " + aula.Data.ToString() + " - hoje: "+hoje.ToString());
                    switch (aula.Hora)
                    {
                        case "AB":
                            aula.Data = aula.Data.AddHours(8);
                            break;
                        case "CD":
                            aula.Data = aula.Data.AddHours(9).AddMinutes(45);
                            break;
                        case "EX":
                            aula.Data = aula.Data.AddHours(11).AddMinutes(30);
                            break;
                        case "FG":
                            aula.Data = aula.Data.AddHours(14);
                            break;
                        case "HI":
                            aula.Data = aula.Data.AddHours(15).AddMinutes(45);
                            break;
                        case "JK":
                            aula.Data = aula.Data.AddHours(17).AddMinutes(30);
                            break;
                        case "LM":
                            aula.Data = aula.Data.AddHours(19).AddMinutes(15);
                            break;
                        case "NP":
                            aula.Data = aula.Data.AddHours(21).AddMinutes(0);
                            break;
                    }
                    long tickDiff = aula.Data.Ticks - hoje.Ticks;
                }
            }
            dgAulas.DataSource = tabela;
            dgAulas.ItemDataBound += new DataGridItemEventHandler(dgAux_ItemDataBound);
            dgAulas.DataBind();            
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
