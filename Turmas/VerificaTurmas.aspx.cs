using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using BusinessData.DataAccess;

using System.Security;
using System.Linq;
using System.Diagnostics;

public class TurmaVerifica
{
    public Guid Id { get; set; }
    public int Numero { get; set; }
    public Calendario Calendario { get; set; }
    public Disciplina Disciplina { get; set; }
    public string DataHora { get; set; }
    public Professor Professor { get; set; }
    public Curso Curso { get; set; }
    public string G2OK { get; set; }
    public string AulasOK { get; set; }
    public string ProvasOK { get; set; }
    public string TrabalhosOK { get; set; }
    public string RecursosOK { get; set; }
}

public partial class Pagina2 : System.Web.UI.Page
{
    private AulaBO aulaBo;
    private RequisicoesBO reqBo;
    private Calendario cal;
    private HashSet<Guid> reqTurmas;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            aulaBo = new AulaBO();
            reqBo = new RequisicoesBO();            
            try
            {
                cal = (Calendario)Session["Calendario"];
                
                // Obtém as requisições do semestre (todas)
                IList<Requisicao> listaReq = reqBo.GetRequisicoesPorCalendario(cal);

                // Cria um conjunto para armazenar os números das turmas com pedidos de recursos
                reqTurmas = new HashSet<Guid>();
                foreach (Requisicao req in listaReq)
                    reqTurmas.Add(req.Aula.TurmaId.Id);

                TurmaBO turma = new TurmaBO();
                List<Turma> listaTurma = turma.GetTurmas(cal);
                listaTurma.Sort();
                if (listaTurma.Count == 0)
                {
                    lblStatus.Text = "Nenhuma turma cadastrada.";
                    lblStatus.Visible = true;
                }
                else
                {
                    List<TurmaVerifica> turmas = new List<TurmaVerifica>();
                    int totalTurmas = listaTurma.Count;
                    int totalFalta = 0;
                    int totalRecursosFalta = 0;
                    foreach (Turma t in listaTurma)
                    {
                        TurmaVerifica nova = new TurmaVerifica
                        {
                            Id = t.Id,
                            Numero = t.Numero,
                            Calendario = t.Calendario,
                            Disciplina = t.Disciplina,
                            DataHora = t.DataHora,
                            Professor = t.Professor,
                            Curso = t.Curso
                        };
                        if (verificaTurma(ref nova, t.Disciplina.G2))
                        {
                            turmas.Add(nova);
                            totalFalta++;
                            if (nova.RecursosOK == "NÃO")// && !nova.Disciplina.Categoria.Descricao.Contains("Teórica"))
                                totalRecursosFalta++;
                        }
                    }
                    double percPreench = (totalTurmas-totalFalta) / (double)totalTurmas;
                    lblPercentual.Text = String.Format("Preenchimento geral: {0:P} (faltam {1} de {2})", percPreench, totalFalta, totalTurmas);
//                    lblPercentual.Text = "Percentual de preenchimento: " + percPreench + " (" + totalFalta + " de " + totalTurmas + ")";
                    double percRecursos = (totalTurmas - totalRecursosFalta) / (double)totalTurmas;
                    lblPercentualRecursos.Text = String.Format("Solicitações de recursos: {0:P} (faltam {1} de {2})", percRecursos, totalRecursosFalta, totalTurmas);

                    grvListaTurmas.RowDataBound += grvListaTurmas_RowDataBound;
                    grvListaTurmas.DataSource = turmas;
                    grvListaTurmas.DataBind();
                }
            }
            catch (BusinessData.DataAccess.DataAccessException ex)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
            }
        }
    }

    void grvListaTurmas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int pos = 6; pos <= 10; pos++)
                if (e.Row.Cells[pos].Text == "OK" || e.Row.Cells[pos].Text == "N/A")
                    e.Row.Cells[pos].ForeColor = System.Drawing.Color.Green;
                else
                    e.Row.Cells[pos].ForeColor = System.Drawing.Color.Red;
        }
    }

    private bool verificaTurma(ref TurmaVerifica nova, bool temG2)
    {
        nova.G2OK = "NÃO";
        nova.AulasOK = "NÃO";
        nova.ProvasOK = "NÃO";
        nova.TrabalhosOK = "NÃO";
        nova.RecursosOK = "NÃO";

        if(nova.Disciplina.Nome.ToLower().StartsWith("trabalho de conclusão")) {
            nova.G2OK = nova.AulasOK = nova.ProvasOK = nova.TrabalhosOK = nova.RecursosOK = "N/A";
            return false;
        } 

        if(nova.Curso.Nome.Contains("PPG")) {
            nova.G2OK = nova.AulasOK = nova.ProvasOK = nova.TrabalhosOK = "N/A";        
        }

        List<Aula> listaAulas = null;
        try
        {
            listaAulas = aulaBo.GetAulas(nova.Id);
        }
        catch (Exception)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=Codigo de turma inválido!");
        }

        // Se a turma está no conjunto, é porque há pelo menos uma solicitação de recurso
        if (reqTurmas.Contains(nova.Id))
            nova.RecursosOK = "OK";

        int totalAulas = listaAulas.Count;
        int preenchidas = 0;
        foreach (Aula aula in listaAulas)
        {        
            if (aula.DescricaoAtividade.Trim().Length > 3)
                preenchidas++;
            if (aula.CategoriaAtividade.Descricao == "Prova de G2")
                nova.G2OK = "OK";
            if (aula.CategoriaAtividade.Descricao == "Prova")
                nova.ProvasOK = "OK";
            if (aula.CategoriaAtividade.Descricao == "Trabalho")
                nova.TrabalhosOK = "OK";
        }
        
        double perc = (double)preenchidas / totalAulas;
        if (perc > 0.5)
            nova.AulasOK = "OK";// (" + (int)perc * 100 + "%)";

        if (!temG2)
            nova.G2OK = "N/A";

        // Se pelo menos um dos itens estiver com "NÃO", o preenchimento não foi concluído (teoricamente...)
        if (nova.AulasOK == "NÃO" || nova.G2OK == "NÃO" || nova.ProvasOK == "NÃO" || nova.TrabalhosOK == "NÃO"
            || nova.RecursosOK == "NÃO")
            return true;
        return false;
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
