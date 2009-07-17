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

public partial class Default_SelecionarCalendario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        this.selecionaCalendario.CalendarioSelecionado += new EventHandler(SelecionaCalendario_CalendarioSelecionado);

    }

    protected void SelecionaCalendario_CalendarioSelecionado(object sender, EventArgs e)
    {
        ConfigBO controleConfiguracoes =  new ConfigBO();
        Session["AppState"] = controleConfiguracoes.GetAppState(Session["Calendario"] as Calendario);
        
        if (User.IsInRole("admin"))
        {
            Response.Redirect("~/Default/PaginaInicial.aspx");
        }
        if (User.IsInRole("Secretario"))
        {
            Response.Redirect("~/Alocacoes/Default.aspx");
        }
        if (User.IsInRole("professor"))
        {
            AppState estadoAtual = (AppState)Session["AppState"];
            if (estadoAtual != AppState.Admin)
            {
                Response.Redirect("~/AcessoProfessores/SelecionaTurma.aspx");
            }
            ScriptManager.RegisterClientScriptBlock(this,this.GetType(),"Alerta",@"alert('O sistema está bloqueado');",true);
        }
    }

    
}
