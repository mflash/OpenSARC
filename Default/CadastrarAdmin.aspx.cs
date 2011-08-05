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
using System.Security;

public partial class Default_CadastrarAdmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Roles.GetUsersInRole("Admin").Length != 0)
            {
                cuUsuarios.RedirectUrl = "~/Default/Default.aspx";
                if (!User.IsInRole("Admin"))
                    Response.Redirect("~/Default/Erro.aspx?Erro=Usuário logado não possui privilégios de admin");
            }
            else { cuUsuarios.RedirectUrl = "~/Default/PaginaInicial.aspx"; }
        }
    }



    protected void cuUsuarios_Load(object sender, EventArgs e)
    {

    }
    protected void cuUsuarios_Load1(object sender, EventArgs e)
    {

    }
}
