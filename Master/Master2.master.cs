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

            //ASP.default_selecionacalendario_ascx calendar = (ASP.default_selecionacalendario_ascx)LoadControl("~/Default/SelecionaCalendario.ascx");
            Default_SelecionaCalendario_ calendar = (Default_SelecionaCalendario_)LoadControl("~/UserControls/SelecionaCalendario.ascx");
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
                Control menu = LoadControl("~/UserControls/MenuAdmin.ascx");
                phMenu.Controls.Add(menu);
            }
            else if (Roles.IsUserInRole("Professor"))
            {
                if (Session["AppState"] != null && ((AppState)Session["AppState"]) != AppState.Admin)
                {
                    ProfessoresBO controleProfessores = new ProfessoresBO();
                    TurmaBO turmaBO = new TurmaBO();
                    Control menu = LoadControl("~/UserControls/MenuProfessor2.ascx");
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
                        listaTurmas.Sort();
                        foreach (Turma t in listaTurmas)
                        {

                            Label x = new Label();
                            x.Text = "<span style=\"padding:2px\"> <a  href=\"" + baseURL + t.Id + "\">" + getNomeCurtoDisciplina(t.Disciplina) + " - " + t.Numero + "</a></span><br/>";
                            x.CssClass = "ms-toolbar-small";

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
                    Control menu = LoadControl("~/UserControls/MenuSecretario.ascx");
                    phMenu.Controls.Add(menu);
                }
            }
        }
    }

    /*
     * Retorna um nome curto para a disciplina, se o tamanho passar de 20 caracteres
     * (o que cabe no menu lateral com fonte 7, mais o número da turma)
     */
    string getNomeCurtoDisciplina(Disciplina disc)
    {
        string nome = disc.Nome;
        if (nome.Length <= 20)
            return nome;
        string curto = "";
        foreach (string pal in nome.Split())
        {
            // Se a palavra tiver menos de 5 caracteres (ex: "de", "para", "(SI)") usa como está
            string palCurta = pal;
            if (pal.Length > 4)
            {
                // Pega as 3 primeiras letras da palavra
                palCurta = pal.Substring(0, 3);
                // Se terminar com uma vogal, acrescenta mais uma letra
                if (palCurta[2] == 'a' || palCurta[2] == 'á' || palCurta[2]== 'ê' || palCurta[2] == 'e' || palCurta[2] == 'i' || palCurta[2] == 'o'
                    || palCurta[2] == 'u')
                    palCurta = pal.Substring(0, 4);
                palCurta += ". ";
            }
            curto += palCurta + " ";
        }
        return curto;
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
