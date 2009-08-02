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
using System.Collections.Generic;

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
                    ProfessoresBO controleProfessores = new ProfessoresBO();
                    TurmaBO turmaBO = new TurmaBO();
                    Control menu = LoadControl("~/Default/MenuProfessor.ascx");
                    Calendario cal = (Calendario)Session["Calendario"];
                    //MembershipUser user = Membership.GetUser();
                    Guid professorId = new Guid(user.ProviderUserKey.ToString());

                    Professor prof = (Professor)controleProfessores.GetPessoaById(professorId);
                    AppState estado = (AppState)Session["AppState"];
                    string baseURL = "../Docentes/";
                    if (estado == AppState.Requisicoes)
                        baseURL += "EditarAula.aspx?GUID=";
                    if (estado == AppState.AtivoSemestre)
                        baseURL += "EditarAulaSemestre.aspx?GUID=";

                    try
                    {
                        int pos = 1;
                        List<Turma> listaTurmas = turmaBO.GetTurmas(cal, prof);
                        foreach (Turma t in listaTurmas)
                        {

                            Label x = new Label();
                            x.Text = "<span style=\"padding:4px\"> <a  href=\"" + baseURL + t.Id + "\">" + t.Disciplina + " - " + t.Numero + "</a> </span>";
                            x.CssClass = "ms-toolbar";

                            //x.("left=3px;top=3px");
                            menu.Controls.AddAt(pos++, x);
                        }
                    }
                    finally
                    {
                        phMenu.Controls.Add(menu);
                    }                    
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
