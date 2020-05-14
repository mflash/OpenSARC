using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessData.Entities;
using BusinessData.BusinessLogic;
using System.Security;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public partial class Pagina6 : System.Web.UI.Page
{
    DisciplinasBO discBO = new DisciplinasBO();
    ProfessoresBO profBO = new ProfessoresBO();
    CalendariosBO calBO = new CalendariosBO();
    TurmaBO turmabo = new TurmaBO();
    CursosBO cursoBO = new CursosBO();  
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populaDisciplina();
            populaProfessores();
            populaCurso();          
        }
    }

     public void populaDisciplina()
    {
        try
        {
            Calendario cal = (Calendario)Session["Calendario"];
            //ddlDisciplina.DataSource = discBO.GetDisciplinas(cal);
            //ddlDisciplina.DataTextField = "NomeCodCred";
            //ddlDisciplina.DataValueField = "Cod";
            //ddlDisciplina.DataBind();
            cbDisciplina.DataSource = discBO.GetDisciplinas(cal);
            cbDisciplina.DataTextField = "NomeCodCred";
            cbDisciplina.DataValueField = "Cod";
            cbDisciplina.DataBind();
        }
        catch (BusinessData.DataAccess.DataAccessException)
        { }
    }

    public void populaProfessores()
    {
        try
        {
            //ddlProfessor.DataSource = profBO.GetProfessores();
            //ddlProfessor.DataTextField = "Nome";
            //ddlProfessor.DataValueField = "Id";
            //ddlProfessor.DataBind();
            cbProfessor.DataSource = profBO.GetProfessores();
            cbProfessor.DataTextField = "Nome";
            cbProfessor.DataValueField = "Id";
            cbProfessor.DataBind();
        }
        catch (BusinessData.DataAccess.DataAccessException)
        { }
    }

    public void populaCurso()
    {
        try
        {
            //ddlCurso.DataSource = cursoBO.GetCursos();
            //ddlCurso.DataTextField = "Nome";
            //ddlCurso.DataValueField = "Codigo";
            //ddlCurso.DataBind();
            cbCurso.DataSource = cursoBO.GetCursos();
            cbCurso.DataTextField = "Nome";
            cbCurso.DataValueField = "Codigo";
            cbCurso.DataBind();
        }
        catch (BusinessData.DataAccess.DataAccessException)
        { }
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {

        try
        {
            Disciplina.ValidaHorario(txtDataHora.Text);
            lblStatus.Text = "";
            Calendario cal = (Calendario)Session["Calendario"];
            List<Turma> tumasCadastradas = turmabo.GetTurmas(cal);

            int numero = Convert.ToInt32(txtNumero.Text);
            Disciplina disc = discBO.GetDisciplina(cbDisciplina.SelectedValue, cal);
            Professor prof = (Professor)profBO.GetPessoaById(new Guid(cbProfessor.SelectedValue));
            Curso curso = cursoBO.GetCursoByCodigo(cbCurso.SelectedValue);
            bool achou = false;
            foreach (Turma t in tumasCadastradas)
            {
                if ((t.Numero == numero) && (t.Disciplina.Nome.Equals(disc.Nome)) && (t.Curso.Nome.Equals(curso.Nome)))
                {
                    achou = true;
                    break;
                }
            }

            if (!achou)
            {
                int testaCreditos = Disciplina.GetNumeroDeCreditos(txtDataHora.Text);
                if (testaCreditos == disc.Cred || testaCreditos == disc.Cred+1)
                {
                    string dh = txtDataHora.Text;
                    Turma t = Turma.NewTurma(numero, cal, disc, dh, prof, curso);

                    turmabo.InsereTurma(t,Session["Calendario"] as Calendario);

                    lblStatus.Text = "Turma inserida com sucesso.";
                    lblStatus.Visible = true;
                    txtDataHora.Text = "";
                    txtNumero.Text = "";

                }
                else
                {
                    lblStatus.Text = "Turma não pode ser inserida. Número de créditos diferente do número de aulas";
                    lblStatus.Visible = true;
                    txtDataHora.Text = "";
                }
            }
            else
            {
                lblStatus.Text = "Turma já cadastrada neste curso para esta disciplina.";
                lblStatus.Visible = true;
                txtDataHora.Text = "";
                txtNumero.Text = "";
            }

        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (SecurityException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (FormatException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
