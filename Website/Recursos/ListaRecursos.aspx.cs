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

public partial class Pagina2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            RecursosBO recursosBO = new RecursosBO();
            List<Recurso> listaRec = recursosBO.GetRecursos();
            if (listaRec.Count == 0)
            {
                lblStatus.Text = "Nenhum recurso cadastrado.";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaRecursos.DataSource = listaRec;
                grvListaRecursos.DataBind();
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void grvListaRecursos_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            
            Guid id = (Guid)grvListaRecursos.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/Recursos/AlteraRecurso.aspx?GUID=" + id.ToString());
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

    protected void grvListaRecursos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            RecursosBO recursos = new RecursosBO();
            Guid id = (Guid)grvListaRecursos.DataKeys[e.RowIndex].Value;

            recursos.DeletaRecurso(id);
            lblStatus.Text = "Recurso excluido com sucesso.";
            lblStatus.Visible = true;

            grvListaRecursos.DataSource = recursos.GetRecursos();
            grvListaRecursos.DataBind();
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

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
