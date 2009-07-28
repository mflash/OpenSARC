using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Usuarios_GerenciarUsuarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ddlCategoriasUsuario.Items.Add("Secretario");
        ddlCategoriasUsuario.Items.Add("Professor");
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCategoriasUsuario.SelectedValue.Equals("Secretario"))
                Response.Redirect("~/Secretarios/ListaSecretarios.aspx");
            else if(ddlCategoriasUsuario.SelectedValue.Equals("Professor"))
                Response.Redirect("~/Professores/ListaProfessores.aspx");

        }
        catch (ArgumentException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }


}
