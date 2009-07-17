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
public partial class ImportarDados_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void lbtnImportarProfessores_Click(object sender, EventArgs e)
    {
        ImportarProfessores();
        lblSucesso.Text = "Professores importados com sucesso!";
    }

    private void ImportarProfessores()
    {
        SPABO sistAcademico = new SPABO();
        try
        {
            sistAcademico.ImportarProfessores();
        }
        catch(Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    
    }

}
