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
using BusinessData.DataAccess;

public partial class CategoriaRecurso_List: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            CategoriaRecursoBO categoriaRecurso = new CategoriaRecursoBO();
            List<CategoriaRecurso> listCat = categoriaRecurso.GetCategoriaRecurso();
            if (listCat.Count == 0)
            {
                lblStatus.Text = "Nehuma Categoria de Recursos cadastrada.";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaRecursos.DataSource = listCat;
                grvListaRecursos.DataBind();
            }
        }
        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }  
    }
    
    protected void grvListaRecursos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            CategoriaRecursoBO categoriaRecurso = new CategoriaRecursoBO();
            Guid id = (Guid)grvListaRecursos.DataKeys[e.RowIndex].Value;


            categoriaRecurso.DeletaCategoriaRecurso(id);
            lblStatus.Text = "Categoria de Recursos excluída com sucesso.";
            lblStatus.Visible = true;

            grvListaRecursos.DataSource = categoriaRecurso.GetCategoriaRecurso();
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

    protected void grvListaRecursos_RowEditing(object sender, GridViewEditEventArgs e)
    {

        try
        {
            CategoriaRecursoBO categoriaRecurso = new CategoriaRecursoBO();
            Guid id = (Guid)grvListaRecursos.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/CategoriaRecurso/Edit.aspx?GUID=" + id.ToString());
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
    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
