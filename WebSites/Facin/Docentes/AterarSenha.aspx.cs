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
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using System.Net;
using BusinessData.BusinessLogic;
using BusinessData.Entities;

public partial class Docentes_AterarSenhaaspx : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ChangePassword1_ChangedPassword(object sender, EventArgs e)
    {
        MembershipUser user = Membership.GetUser();
        PessoaFactory factory = PessoaFactory.GetInstance();
        PessoaBase pes =  factory.CreatePessoa((Guid)user.ProviderUserKey);
        LogEntry log = new LogEntry();
        log.Message = "Usuário: " + user.UserName + "; Id: " + pes.Id + "; Nome: " + pes.Nome;
        log.TimeStamp = DateTime.Now;
        log.Severity = TraceEventType.Information;
        log.Title = "Troca senha Usuário";
        log.MachineName = Dns.GetHostName();
        Logger.Write(log);
    }
}
