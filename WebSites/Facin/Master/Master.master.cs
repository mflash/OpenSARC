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

public partial class Master_MasterFacin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Calendario"] != null)
        {
            Label lbl = new Label();
            lbl.Text = "Calendário:";
            lbl.CssClass = "ms-toolbar";
            phCalendario.Controls.Add(lbl);
            
            Default_SelecionaCalendario_ calendar = (Default_SelecionaCalendario_)LoadControl("~/Default/SelecionaCalendario.ascx");
            calendar.CalendarioSelecionado += new EventHandler(calendar_CalendarioSelecionado);
            phCalendario.Controls.Add(calendar);

            Label userEmail = new Label();
            MembershipUser user = Membership.GetUser();
            if (user != null)
            {
                userEmail.Text = user.Email;
                userEmail.CssClass = "ms-toolbar";
                phActiveUser.Controls.Add(userEmail);
            }
            if (Roles.IsUserInRole("Admin"))
            {
                Control menu = LoadControl("~/Default/MenuAdmin.ascx");
                phMenu.Controls.Add(menu);
            }
            else if (Roles.IsUserInRole("Professor"))
            {
                if (Session["AppState"] != null && ((AppState)Session["AppState"]) != AppState.Admin)
                {
                    Control menu = LoadControl("~/Default/MenuProfessor.ascx");
                    phMenu.Controls.Add(menu);
                }
            }
            else if (Roles.IsUserInRole("Secretario"))
            {
                if (Session["AppState"] != null && ((AppState)Session["AppState"]) != AppState.Admin)
                {
                    Control menu = LoadControl("~/Default/MenuSecretario.ascx");
                    phMenu.Controls.Add(menu);
                }
            }
        }
    }

    void calendar_CalendarioSelecionado(object sender, EventArgs e)
    {
        Response.Redirect("../Default/PaginaInicial.aspx");
    }
    
    protected void lsLogin_LoggedOut(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("~/Default/Default.aspx");
    }
}
