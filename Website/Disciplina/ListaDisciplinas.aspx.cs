using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System.Security;
using System.Text;
using BusinessData.DataAccess;

public partial class Disciplina_ListaDisciplinas : System.Web.UI.Page
{
    DisciplinasBO disciBo = new DisciplinasBO();
    FaculdadesBO faculBo = new FaculdadesBO();
    CalendariosBO calBo = new CalendariosBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            
            PopulaListaDisciplinas();
        }
    }

    protected void PopulaListaDisciplinas()
    {
        try
        {
            Calendario cal = (Calendario)Session["Calendario"];
            
            List < Disciplina > listaDisciplinas = disciBo.GetDisciplinas(cal.Id);
            if (listaDisciplinas.Count == 0)
            {
                lblStatus.Text = "Nenhuma disciplina cadastrada.";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaDisciplinas.DataSource = listaDisciplinas;
                grvListaDisciplinas.DataBind();
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void grvListaDisciplinas_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Calendario cal = (Calendario)Session["Calendario"];

            Guid calendarioId = cal.Id;
            string disciplinaCod = (string)grvListaDisciplinas.DataKeys[e.NewEditIndex].Value;
            
            StringBuilder b = new StringBuilder();
            b.Append("~/Disciplina/AlteraDisciplina.aspx?");
            b.Append("CALENDARIO=");
            b.Append(calendarioId.ToString());
            b.Append("&");
            b.Append("DISCIPLINA=");
            b.Append(disciplinaCod);

            Response.Redirect(b.ToString());
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (SecurityException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void grvListaDisciplinas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Calendario cal = (Calendario)Session["Calendario"];
            Guid calendarioId = cal.Id;
            string disciplinaCod = (string)grvListaDisciplinas.DataKeys[e.RowIndex].Value;

            disciBo.DeletaDisciplina(disciplinaCod, calendarioId);

            PopulaListaDisciplinas();
        }
        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (SecurityException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
    
}
