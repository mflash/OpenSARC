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

public partial class CategoriaData_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            CategoriaDataBO boCategoriaDataList = new CategoriaDataBO();
            List<CategoriaData> listaCat = boCategoriaDataList.GetCategoriaDatas();
            if (listaCat.Count == 0)
            {
                lblStatus.Text = "Nenhuma Categoria de Datas cadastrada.";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaCategoriaData.DataSource = listaCat;
                grvListaCategoriaData.DataBind();
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }   
    }

    protected void grvListaCategoriaData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            CategoriaDataBO categoriaDataList = new CategoriaDataBO();
            Guid id = (Guid)grvListaCategoriaData.DataKeys[e.RowIndex].Value;

            categoriaDataList.DeleteCategoriaData(id);
            lblStatus.Text = "Categoria de Datas excluída com sucesso.";
            lblStatus.Visible = true;

            grvListaCategoriaData.DataSource = categoriaDataList.GetCategoriaDatas();
            grvListaCategoriaData.DataBind();
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

    protected void grvListaCategoriaData_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            //CategoriaDataBO categoriaDataList = new CategoriaDataBO();
            Guid id = (Guid)grvListaCategoriaData.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/CategoriaData/Edit.aspx?GUID=" + id.ToString());
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
