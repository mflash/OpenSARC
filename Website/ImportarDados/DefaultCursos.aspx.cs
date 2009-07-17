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

public partial class ImportarDados_DefaultCursos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lbtnImportarCursos_Click(object sender, EventArgs e)
    {
        ImportarCursos();
        lblSucesso.Text = "Cursos importados com sucesso!";

    }

    private void ImportarCursos()
    {
        SPABO sistAcademico = new SPABO();
        try
        {
            sistAcademico.ImportarCursos();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }
}
