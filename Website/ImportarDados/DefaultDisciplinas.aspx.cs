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

public partial class ImportarDados_DefaultDisciplinas : System.Web.UI.Page
{

    CalendariosBO calBo = new CalendariosBO();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        PopulaCalendarios();
    }

    protected void lbtnImportarDisciplinas_Click(object sender, EventArgs e)
    {
        ImportarDisciplinas(new Guid(ddlCalendario.SelectedValue));
    }

    private void ImportarDisciplinas(Guid calendarioId)
    {
        SPABO sistAcademico = new SPABO();
        try
        {
            sistAcademico.ImportarDiscplinas(calendarioId);
        }
        catch(Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    
    }

    protected void PopulaCalendarios()
    {
        try
        {
            ddlCalendario.DataSource = calBo.GetCalendarios();
            ddlCalendario.DataTextField = "PorExtenso";
            ddlCalendario.DataValueField = "id";
            ddlCalendario.DataBind();
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        { }
    }

    
}
