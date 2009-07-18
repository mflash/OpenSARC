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


public partial class Pagina6 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request.QueryString["GUID"] != null)
            {
                try
                {
                    CursosBO cursoBO = new CursosBO();
                    DisciplinasBO discBO = new DisciplinasBO();
                    ProfessoresBO profBO = new ProfessoresBO();
                    TurmaBO boTurma = new TurmaBO();
                    Calendario cal = (Calendario)Session["Calendario"];

                    try
                    {
                        Turma turma = boTurma.GetTurmaById(new Guid(Request.QueryString["GUID"]), cal);
                        ddlDisciplina.DataSource = discBO.GetDisciplinas(cal);
                        ddlDisciplina.DataTextField = "Nome";
                        ddlDisciplina.DataValueField = "Cod";
                        ddlDisciplina.SelectedValue = turma.Disciplina.Cod;
                        ddlDisciplina.DataBind();
                        
                        txtNumero.Text = Convert.ToString(turma.Numero);
                        txtDataHora.Text = turma.DataHora;
                        
                        ddlProfessor.DataSource = profBO.GetProfessores();
                        ddlProfessor.DataTextField = "Nome";
                        ddlProfessor.DataValueField = "Id";
                        ddlProfessor.SelectedValue = (turma.Professor.Id).ToString();
                        ddlProfessor.DataBind();

                        ddlCurso.DataSource = cursoBO.GetCursos();
                        ddlCurso.DataTextField = "Nome";
                        ddlCurso.DataValueField = "Codigo";
                        ddlCurso.SelectedValue = turma.Curso.Codigo;
                        ddlCurso.DataBind();

                    }
                    catch(FormatException )
                    {
                        Response.Redirect("~/Turmas/ListaTurmas.aspx");
                    }

                }
                catch (BusinessData.DataAccess.DataAccessException )
                {
                    Response.Redirect("~/Turmas/ListaTurmas.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Turmas/ListaTurmas.aspx");
            }
        }
    }

   

    protected void btnConfirmar_Click1(object sender, EventArgs e)
    {
        try
        {
            Disciplina.ValidaHorario(txtDataHora.Text);
            lblStatus.Text = "";

            Calendario cal = (Calendario)Session["Calendario"];

            TurmaBO turmaBO = new TurmaBO();
            Turma turma = turmaBO.GetTurmaById(new Guid(Request.QueryString["GUID"]), cal);

            if (turma != null)
            {
                DisciplinasBO controleDisciplinas = new DisciplinasBO();
                Disciplina disc = controleDisciplinas.GetDisciplina(ddlDisciplina.SelectedValue, cal);

                ProfessoresBO professorBO = new ProfessoresBO();
                Professor prof = (Professor)professorBO.GetPessoaById(new Guid(ddlProfessor.SelectedValue));

                CursosBO cursoBO = new CursosBO();
                Curso curso = cursoBO.GetCursoByCodigo(ddlCurso.SelectedValue);
                
                
                int testaCreditos = Disciplina.GetNumeroDeCreditos(txtDataHora.Text);
                if (testaCreditos == disc.Cred)
                {
                    turma.Disciplina = disc;
                    turma.Numero = Convert.ToInt32(txtNumero.Text);
                    turma.DataHora = txtDataHora.Text;
                    turma.Professor = prof;
                    turma.Curso = curso;
                    turmaBO.UpdateTurma(turma);
                    lblStatus.Text = "Turma atualizada com sucesso.";
                    lblStatus.Visible = true;
                    Response.Redirect("~/Turmas/ListaTurmas.aspx");
                }
                
                else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Número de créditos incorreto para esta disciplina.");
            }

            else
            {
                lblStatus.Text = "Turma não pode ser atualizada.";
                lblStatus.Visible = true;
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
    }
    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Turmas/ListaTurmas.aspx");
    }
}
