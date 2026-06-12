using System;
using System.Web.UI;
using BusinessData.BusinessLogic;
using BusinessData.Entities;

public partial class _Painel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnConsultaMatricula_Click(object sender, EventArgs e)
    {
        ConsultarMatricula(txtMatricula.Text.Trim());

        ScriptManager.RegisterStartupScript(this, GetType(), "focusRecurso",
            string.Format("document.getElementById('{0}').focus();", txtRecurso.ClientID), true);
    }

    protected void btnConsultaRecurso_Click(object sender, EventArgs e)
    {
        ConsultarRecurso(txtRecurso.Text.Trim());
    }

    private void ConsultarMatricula(string matricula)
    {
        try
        {
            ProfessoresBO bo = new ProfessoresBO();
            Professor professor = Professor.NewProfessor("10049190");
            professor.Nome = "Marcelo Cohen";

            if (professor != null)
            {
                lblAviso.CssClass = "text-success small";
                lblAviso.Text = string.Format("<i class='bi bi-person-check-fill me-1'></i>{0}", professor.Nome);
            }
            else
            {
                lblAviso.CssClass = "text-danger small";
                lblAviso.Text = "<i class='bi bi-person-x-fill me-1'></i>Matrícula não encontrada.";
            }
        }
        catch (Exception)
        {
            lblAviso.CssClass = "text-warning small";
            lblAviso.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i>Erro ao consultar matrícula.";
        }
    }

    private void ConsultarRecurso(string abrev)
    {
        try
        {
            RecursosBO bo = new RecursosBO();
            Recurso recurso = Recurso.NewRecurso("Sala 101", "S101", 'S', null, null, true, null);

            if (recurso != null)
            {
                lblAviso.CssClass = "text-success small";
                lblAviso.Text = string.Format("<i class='bi bi-building-check me-1'></i>{0}", recurso.Descricao);
            }
            else
            {
                lblAviso.CssClass = "text-danger small";
                lblAviso.Text = "<i class='bi bi-building-x me-1'></i>Recurso não encontrado.";
            }
        }
        catch (Exception)
        {
            lblAviso.CssClass = "text-warning small";
            lblAviso.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i>Erro ao consultar recurso.";
        }
    }
}
