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
using System.Collections.Generic;

public partial class Teste_Default2 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
 
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        this.programmaticModalPopup.Show();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        this.programmaticModalPopup.Show();
    }
    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {

    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Hide();
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Hide();
    }
    protected void btnRemover_Click(object sender, EventArgs e)
    {

    }
    protected void btnAdicionar_Click(object sender, EventArgs e)
    {
        
    }
    protected void ddlCategoriaRecurso_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnNovaOpcao_Click(object sender, EventArgs e)
    {

    }
    protected void ddlRequisicoes_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
