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

    private void ConsultarMatricula(string matricula)
    {
        try
        {
            Professor professor = Professor.NewProfessor("10049190");
            professor.Nome = "Cohen";

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
}
