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
                
                // Obt�m as requisi��es do semestre (todas)
                IList<Requisicao> listaReq = reqBo.GetRequisicoesPorCalendario(cal);

                // Cria um conjunto para armazenar os n�meros das turmas com pedidos de recursos
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
                    // Turmas sem solicita��es de recursos, n�o te�ricas e n�o de p�s
                    List<TurmaVerifica> turmasRecursos = new List<TurmaVerifica>();
                    // Turmas restantes (todas as outras)
                    List<TurmaVerifica> turmasRestante = new List<TurmaVerifica>();
                    // Total de turmas restantes
                    int totalTurmasRestante = 0;
                    // Total de turmas que deveriam solicitar recursos
                    int totalTurmasRecursos = 0;
                    // Total de turmas com falta de preenchimento em algum aspecto
                    int totalRestanteFalta = 0;
                    // Total de turmas sem solicita��o de recursos
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
                        // "teorica" representa turmas te�ricas ou de algum p�s
                        bool teorica = nova.Disciplina.Categoria.Descricao.Contains("Te�rica") ||
                            nova.Disciplina.Categoria.Descricao.Contains("PPG") ||
                            nova.Curso.Nome.Contains("PPG") ||
                            nova.Curso.Nome.StartsWith("F�sica");

                        // Se a turma n�o � "te�rica", ela deveria solicitar recursos
                        if (!teorica)
                            totalTurmasRecursos++;
                        else
                            // Caso contr�rio, conta nas demais
                            totalTurmasRestante++;

                        // Verifica o preenchimento dos dados, retorna true se
                        // a turma est� incompleta
                        if (verificaTurma(ref nova, t.Disciplina.G2))
                        {
                            // Se a turma deveria solicitar recursos e n�o o fez,
                            // conta mais uma e adiciona na lista de pendentes
                            if (nova.RecursosOK == "N�O" && !teorica)
                            {
                                turmasRecursos.Add(nova);
                                totalRecursosFalta++;
                            }
                            // Se a turma n�o precisa solicitar recursos, conta
                            // nas restantes e adiciona na lista
                            if(teorica)
                            {
                                turmasRestante.Add(nova);
                                totalRestanteFalta++;
                            }
                        }
                    }

                    // Calcula os percentuais
                    double percRecursos = totalRecursosFalta / (double)totalTurmasRecursos;
                    lblPercentualRecursos.Text = String.Format("Turmas sem solicita��es de recursos (excluindo te�ricas e p�s): {1} de {2} ({0:P})", percRecursos, totalRecursosFalta, totalTurmasRecursos);

                    double percPreench = totalRestanteFalta / (double)totalTurmasRestante;
                    lblPercentualGeral.Text = String.Format("Demais turmas com pend�ncias: {1} de {2} ({0:P})", percPreench, totalRestanteFalta, totalTurmasRestante);

                    grvListaTurmas.RowDataBound += grvListaTurmas_RowDataBound;
                    grvListaTurmas.DataSource = turmasRecursos;
                    grvListaTurmas.DataBind();

                    grvListaTurmasGeral.RowDataBound += grvListaTurmasGeral_RowDataBound;
                    grvListaTurmasGeral.DataSource = turmasRestante;
                    grvListaTurmasGeral.DataBind();
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

    void SetRowColor(GridViewRow row)
    {
        for (int pos = 7; pos <= 11; pos++)
            if (row.Cells[pos].Text == "OK" || row.Cells[pos].Text == "N/A")
                row.Cells[pos].ForeColor = System.Drawing.Color.Green;
            else
                row.Cells[pos].ForeColor = System.Drawing.Color.Red;
    }

    void grvListaTurmasGeral_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SetRowColor(e.Row);
        }
    }

    void grvListaTurmas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SetRowColor(e.Row);
        }
    }

    private bool verificaTurma(ref TurmaVerifica nova, bool temG2)
    {
        nova.G2OK = "N�O";
        nova.AulasOK = "N�O";
        nova.ProvasOK = "N�O";
        nova.TrabalhosOK = "N�O";
        nova.RecursosOK = "N�O";

        if(nova.Disciplina.Nome.ToLower().StartsWith("trabalho de conclus�o")) {
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
            Response.Redirect("~/Default/Erro.aspx?Erro=Codigo de turma inv�lido!");
        }

        // Se a turma est� no conjunto, � porque h� pelo menos uma solicita��o de recurso
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

        // Se pelo menos um dos itens estiver com "N�O", o preenchimento n�o foi conclu�do (teoricamente...)
        if (nova.AulasOK == "N�O" || nova.G2OK == "N�O" || nova.ProvasOK == "N�O" || nova.TrabalhosOK == "N�O"
            || nova.RecursosOK == "N�O")
            return true;
        return false;
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
