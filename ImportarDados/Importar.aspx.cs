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
using BusinessData.BusinessLogic;
using BusinessData.DataAccess;
using BusinessData.Entities;

public partial class ImportarDados_Importar : System.Web.UI.Page
{
    SPABO sistAcademico = new SPABO();
    CalendariosBO calBo = new CalendariosBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void lbImportar_Click(object sender, EventArgs e)
    {
            Calendario calId = (Calendario)(Session["Calendario"]);
			//return;
	        //Application["CalendarioImporta"] = calId; 
            lblStatus.Text = "Importando Faculdades... Aguarde.";
//			return;
			//return;
            ImportarFaculdades();
			lblStatus.Text = "Faculdades...";
//			return;
			//return;
            lblStatus.Text = "Importando Cursos... Aguarde.";
            ImportarCursos();
//			return;
            lblStatus.Text = "Importando Disciplinas... Aguarde.";
            ImportarDisciplinas(calId.Id);
			lblStatus.Text = "Disciplinas... feito.";
//			return;
            lblStatus.Text = "Importando Professores... Aguarde.";
            ImportarProfessores();
			lblStatus.Text = "Professores... feito.";
//			return;
            lblStatus.Text = "Importando Turmas... Aguarde.";
            Response.Redirect("~/ImportarDados/ImportarTurmas.aspx");
    }

            
    private void ImportarFaculdades()
    {
        try
        {
            sistAcademico.ImportarFaculdades();
        }
        catch(Exception ex)
        {
			//lblStatus.Text = ex.ToString();
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.InnerException.Message); //ToString());
        }
    
    }

    private void ImportarCursos()
    {
        try
        {
            sistAcademico.ImportarCursos();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.InnerException.Message); //ex.ToString());
        }

    }

    private void ImportarDisciplinas(Guid calendarioId)
    {
        try
        {
            sistAcademico.ImportarDiscplinas(calendarioId);
        }
        catch(Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.InnerException.Message); // ex.ToString());
        }

    }

    private void ImportarProfessores()
    {
        try
        {
            sistAcademico.ImportarProfessores();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message); //ToString()); //InnerException.Message); //ex.ToString());
        }

    }

}
