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
public partial class Vinculos_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FaculdadesBO boVinculos = new FaculdadesBO();
            List<Faculdade> listaFacul = boVinculos.GetFaculdades();
            if (listaFacul.Count == 0)
            {
                lblStatus.Text = "Nenhuma faculdade cadastrada.";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaVinculos.DataSource = listaFacul;
                grvListaVinculos.DataBind();
            }
        }
        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }   
    }
    
    protected void grvListaVinculos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FaculdadesBO vinculos = new FaculdadesBO();
            Guid id = (Guid)grvListaVinculos.DataKeys[e.RowIndex].Value;
       
            
            vinculos.DeletaFaculdade(id);
            lblStatus.Text = "Faculdade Excluída com sucesso.";
            lblStatus.Visible = true;
       
            grvListaVinculos.DataSource = vinculos.GetFaculdades();
            grvListaVinculos.DataBind();
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
    protected void grvListaVinculos_RowEditing(object sender, GridViewEditEventArgs e)
    {
 
        try
        {
            FaculdadesBO vinculos = new FaculdadesBO();
            Guid id = (Guid)grvListaVinculos.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/Faculdades/Edit.aspx?GUID=" + id.ToString());
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
 
    
protected void  lbtnVoltar_Click(object sender, EventArgs e)
{
    Response.Redirect("~/Default/PaginaInicial.aspx");
}
}
