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
using System.Collections.Generic;
using BusinessData.Entities;

public partial class Docentes_Recursos : System.Web.UI.Page
{
    RecursosBO controleRecursos = new RecursosBO();
    AlocacaoBO alocBO = new AlocacaoBO();
    AulaBO aulaBO = new AulaBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["AppState"] != null && ((AppState)Session["AppState"]) == AppState.Admin)
            {
                Server.Transfer("~/Default/Erro.aspx?Erro=O sistema está bloqueado.");
            }
            else
            {
                if (Session["Calendario"] == null)
                {
                    Response.Redirect("../Default/SelecionarCalendario.aspx");
                }

                if (Request.QueryString["AulaId"] != String.Empty)
                {
                    Guid aulaId = new Guid(Request.QueryString["AulaId"]);
                    DateTime data = Convert.ToDateTime(Session["DataAula"]);
                    string horario = (string)Session["Horario"];

                    List<Recurso> recAlocados = alocBO.GetRecursoAlocadoByAula(data, horario, aulaId);
                    LBoxAlocados.DataSource = recAlocados;
                    LBoxAlocados.DataTextField = "Descricao";
                    LBoxAlocados.DataValueField = "Id";
                    LBoxAlocados.DataBind();
                }

            }
        }
    }

    
   
    private void FechaJanela()
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "fecha", "window.close()", true);
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        FechaJanela();
    }

    protected void btnLiberar_Click(object sender, EventArgs e)
    {
        if (LBoxAlocados.SelectedValue != "")
        {
            DateTime data = Convert.ToDateTime(Session["DataAula"]);
            string horario = (string)Session["Horario"];
            Guid recId = new Guid(LBoxAlocados.SelectedValue);

            Recurso rec = controleRecursos.GetRecursoById(recId);
            Alocacao aloc = new Alocacao(rec, data, horario, null, null);
            alocBO.UpdateAlocacao(aloc);

            LBoxAlocados.Items.RemoveAt(LBoxAlocados.SelectedIndex);

            lblStatus.Text = "Recurso liberado!";
        }
        else
        {
            lblStatus.Text = "Selecione um recurso para liberá-lo!";
        }
    }
}
