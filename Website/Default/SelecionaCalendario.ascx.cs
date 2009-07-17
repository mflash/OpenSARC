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
using System.Collections.Generic;

public partial class Default_SelecionaCalendario_ : System.Web.UI.UserControl
{
    public event EventHandler CalendarioSelecionado; 
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CalendariosBO cadastroCalendarios = new CalendariosBO();
            List<Calendario> listaCalendarios = cadastroCalendarios.GetCalendarios();

            if (listaCalendarios.Count == 0)
            {
                Response.Redirect("~/Calendario/Cadastro.aspx");
            }

            ddlCalendarios.DataSource = listaCalendarios;
            ddlCalendarios.DataTextField = "PorExtenso";
            ddlCalendarios.DataValueField = "id";
            ddlCalendarios.DataBind();

            Calendario atual = Session["Calendario"] as Calendario;
            if(atual != null)
                ddlCalendarios.SelectedValue = atual.Id.ToString();
        }
    }

    protected void Selecionar_Click(object sender, EventArgs e)
    {
        Guid Cal = new Guid(ddlCalendarios.SelectedValue);
        CalendariosBO calBo = new CalendariosBO();
        Session["Calendario"] = calBo.GetCalendario(Cal);
        if (CalendarioSelecionado != null)
        {
            ConfigBO controleConfiguracoes = new ConfigBO();
            Session["AppState"] = controleConfiguracoes.GetAppState(Session["Calendario"] as Calendario);


            if (Roles.IsUserInRole("Professor"))
            {
                AppState estadoAtual = (AppState)Session["AppState"];
                if (estadoAtual == AppState.Admin)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alerta", @"alert('O sistema está bloqueado');", true);
                    return;
                }
                
            }

            CalendarioSelecionado(this, new EventArgs());
        }
    }
}
