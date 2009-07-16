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

public partial class Default_ListaAdmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cblAdmins.DataSource = Roles.GetUsersInRole("Admin");
            cblAdmins.DataBind();
            cblSecretarios.DataSource = Roles.GetUsersInRole("Secretario");
            cblSecretarios.DataBind();
        }
    }
    protected void btnExcluir_Click(object sender, EventArgs e)
    {
        foreach (ListItem admin in cblAdmins.Items)
        {
            if (admin.Selected)
            {
                try
                {
                    Membership.DeleteUser(admin.Value);
                    lblStatus.Text = "Usuário excluído com sucesso";
                    lblStatus.Visible = true;
                }
                catch (ArgumentException)
                {
                    Response.Redirect("~/Default/Erro.aspx?Erro=Impossível deletar usuário " + admin.Value);
                }
            }
        }

        BusinessData.BusinessLogic.SecretariosBO secBo = new BusinessData.BusinessLogic.SecretariosBO(); 
        foreach (ListItem secretario in cblSecretarios.Items)
        {
            if (secretario.Selected)
            {
                try
                {
                    MembershipUser mu = Membership.GetUser(secretario.Text);
                    secBo.DeletePessoa(secBo.GetPessoaById((Guid)mu.ProviderUserKey));
                    lblStatus.Text = "Usuário excluído com sucesso";
                    lblStatus.Visible = true;
                }
                catch (ArgumentException)
                {
                    Response.Redirect("~/Default/Erro.aspx?Erro=Impossível deletar usuário " + secretario.Value);
                }
            }
        }

        cblAdmins.DataSource = Roles.GetUsersInRole("Admin");
        cblAdmins.DataBind();
        cblSecretarios.DataSource = Roles.GetUsersInRole("Secretario");
        cblSecretarios.DataBind();
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
