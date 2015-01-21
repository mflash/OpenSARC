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
}

public partial class Pagina2 : System.Web.UI.Page
{
    private AulaBO aulaBo;
//    private TurmaBO turmaBo;
//    private CategoriaDataBO catDataBo;
//    private CategoriaAtividadeBO categoriaAtivBo;
//    List<CategoriaAtividade> listaAtividades;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            aulaBo = new AulaBO();
//            turmaBo = new TurmaBO();
//            catDataBo = new CategoriaDataBO();
            //RequisicoesBO reqBo = new RequisicoesBO();
//            categoriaAtivBo = new CategoriaAtividadeBO();
//            listaAtividades = categoriaAtivBo.GetCategoriaAtividade();
            try
            {
                Calendario cal = (Calendario)Session["Calendario"];
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
                        verificaTurma(ref nova, t.Disciplina.G2);
                        turmas.Add(nova);
                    }
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
            for (int pos = 6; pos <= 9; pos++)
                if (e.Row.Cells[pos].Text == "OK" || e.Row.Cells[pos].Text == "N/A")
                    e.Row.Cells[pos].ForeColor = System.Drawing.Color.Green;
                else
                    e.Row.Cells[pos].ForeColor = System.Drawing.Color.Red;
        }
    }

    private void verificaTurma(ref TurmaVerifica nova, bool temG2)
    {
        if(nova.Disciplina.Nome.ToLower().StartsWith("trabalho de conclus�o")) {
            nova.G2OK = nova.AulasOK = nova.ProvasOK = nova.TrabalhosOK = "N/A";
            return;
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

        nova.G2OK = "N�O";
        nova.AulasOK = "N�O"; 
        nova.ProvasOK = "N�O";
        nova.TrabalhosOK = "N�O";

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

        if (!temG2 || nova.Curso.Nome == "PPGCC")
            nova.G2OK = "N/A";
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
