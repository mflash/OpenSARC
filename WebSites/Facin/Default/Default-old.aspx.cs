// $Id: Default.aspx.cs 117 2010-01-19 20:50:42Z mflashbr $
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
using BusinessData.Entities;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Roles.GetUsersInRole("Admin").Length == 0)
            {
                Server.Transfer("~/Default/CadastrarAdmin.aspx");
            }
            if (User.Identity.IsAuthenticated == true)
            {
                Server.Transfer("~/Default/PaginaInicial.aspx");
            }

            //ACESSOS
            Acesso a = new Acesso(Guid.NewGuid(), DateTime.Now);
            AcessosBO controladorAcessos = new AcessosBO();
            controladorAcessos.InserirAcesso(a);
        }
    }
    protected void loginEntrada_LoginError(object sender, EventArgs e)
    {
        MembershipUser usr = Membership.GetUser(loginEntrada.UserName);
        if (usr != null && (!usr.IsApproved || usr.IsLockedOut))
            {
                ScriptManager.RegisterClientScriptBlock(this,GetType(), "Conta Bloqueada","alert(' Sua conta está bloqueada. Contate o administrador do sistema para mais informações');", true);
            }
        
    }
    protected void loginEntrada_Authenticate(object sender, AuthenticateEventArgs e)
    {

    }
}
