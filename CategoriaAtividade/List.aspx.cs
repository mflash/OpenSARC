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

public partial class CategoriaAtividades_List: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            CategoriaAtividadeBO categAtividadeBO = new CategoriaAtividadeBO();
            List<CategoriaAtividade> listCat = categAtividadeBO.GetCategoriaAtividade();
            if (listCat.Count == 0)
            {
                lblStatus.Text = "Nenhuma Categoria de Atividades cadastrada.";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaAtividades.DataSource = listCat;
                grvListaAtividades.DataBind();
            }
        }
        catch(BusinessData.DataAccess.DataAccessException ex) 
        {
           Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void grvListaAtividades_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            CategoriaAtividadeBO categorias = new CategoriaAtividadeBO();
            Guid id = (Guid)grvListaAtividades.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/CategoriaAtividade/Edit.aspx?GUID=" + id.ToString());
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch(SecurityException ex)
        {
           Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }
    protected void grvListaAtividades_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            CategoriaAtividadeBO categoria = new CategoriaAtividadeBO();
            Guid id = (Guid)grvListaAtividades.DataKeys[e.RowIndex].Value;

            categoria.DeleteCategoriaAtividade(id);
            lblStatus.Text = "Categoria de Atividades excluida com sucesso.";
            lblStatus.Visible = true;

            grvListaAtividades.DataSource = categoria.GetCategoriaAtividade();
            grvListaAtividades.DataBind();
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
