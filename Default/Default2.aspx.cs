// $Id$
using BusinessData.BusinessLogic;
using BusinessData.DataAccess;
using BusinessData.Entities;
using System;
using System.Configuration;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Roles.GetUsersInRole("Admin").Length == 0)
                Server.Transfer("~/Default/CadastrarAdmin.aspx");

            if (User.Identity.IsAuthenticated)
                Server.Transfer("~/Default/PaginaInicial2.aspx");
        }
    }

    protected void loginEntrada_LoginError(object sender, EventArgs e)
    {
        MembershipUser usr = Membership.GetUser(loginEntrada.UserName);
        if (usr != null && (!usr.IsApproved || usr.IsLockedOut))
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Conta Bloqueada",
                "alert(' Sua conta está bloqueada. Contate o administrador do sistema para mais informações');", true);
    }

    protected bool LDAPAuth(string user, string pass)
    {
        bool result = false;
        string domain = ConfigurationManager.AppSettings["ldapDomain"];
        string serviceUser = ConfigurationManager.AppSettings["ldapServiceUser"];
        string servicePass = ConfigurationManager.AppSettings["ldapServicePasswd"];
        try
        {
            using (var context = new PrincipalContext(ContextType.Domain, domain, serviceUser, servicePass))
            {
                result = context.ValidateCredentials(user, pass);
            }
        }
        catch (PrincipalServerDownException)
        {
            return false;
        }
        return result;
    }

    protected bool moodleAuth(string user, string pass, out string reason)
    {
        reason = "";
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var request = WebRequest.Create("https://moodle.pucrs.br/cead/sarcauth.php");
        var postdata = "user=" + user + "&pass=" + Uri.EscapeDataString(pass);
        var data = Encoding.ASCII.GetBytes(postdata);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;
        try
        {
            using (var stream = request.GetRequestStream())
                stream.Write(data, 0, data.Length);
        }
        catch (WebException ex)
        {
            Debug.WriteLine(ex.ToString());
            reason = ex.Message;
            return false;
        }
        var response = request.GetResponse();
        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        Debug.WriteLine("Moodle:" + responseString);
        return responseString != "FAIL\n";
    }

    protected void loginEntrada_Authenticate(object sender, AuthenticateEventArgs e)
    {
        if (Membership.ValidateUser(loginEntrada.UserName, loginEntrada.Password))
            e.Authenticated = true;
        else if (LDAPAuth(loginEntrada.UserName, loginEntrada.Password))
            e.Authenticated = true;
        else
            e.Authenticated = false;
    }
}
