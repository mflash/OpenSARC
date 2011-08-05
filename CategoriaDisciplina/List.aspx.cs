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

public partial class CategoriaDisciplina_List: System.Web.UI.Page
{
    CategoriaDisciplinaBO cdBo = new CategoriaDisciplinaBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            if (!Page.IsPostBack)
            {
                PopulaCategoriaDisciplinas();
                
            }
        }
        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }  
    }

    protected void PopulaCategoriaDisciplinas()
    {
        List<CategoriaDisciplina> listCat = cdBo.GetCategoriaDisciplinas();
        if (listCat.Count == 0)
        {
            lblStatus.Text = "Nenhuma Categoria de Disciplinas cadastrada";
            lblStatus.Visible = true;
        }
        else
        {
            grvListaDisciplinas.DataSource = listCat;
            grvListaDisciplinas.DataBind();
        }
    }

    

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }

    protected void grvListaDisciplinas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Guid id = (Guid)grvListaDisciplinas.DataKeys[e.RowIndex].Value;


            cdBo.DeletaCategoriaDisciplina(id);

            lblStatus.Text = "Categoria de Disciplinas excluída com sucesso.";
            lblStatus.Visible = true;

            PopulaCategoriaDisciplinas();
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
    protected void grvListaDisciplinas_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Guid id = (Guid)grvListaDisciplinas.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/CategoriaDisciplina/Edit.aspx?GUID=" + id.ToString());
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
}
