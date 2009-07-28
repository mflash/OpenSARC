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
using System.Security;
using BusinessData.DataAccess;

public partial class CategoriaRecurso_Edit: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["GUID"] != null)
            {

                try
                {
                    CategoriaRecursoBO boCategoriaRecurso = new CategoriaRecursoBO();

                    try
                    {
                        CategoriaRecurso cat = boCategoriaRecurso.GetCategoriaRecursoById(new Guid(Request.QueryString["GUID"]));
                        txtDescricao.Text = cat.Descricao;
                    }
                    catch (FormatException ex)
                    {
                        Response.Redirect("~/CategoriaRecurso/List.aspx");
                    }


                }
                catch (BusinessData.DataAccess.DataAccessException)
                {
                    Response.Redirect("~/CategoriaRecurso/List.aspx");
                }
            }
            else
            {
                Response.Redirect("~/CategoriaRecurso/List.aspx");
            }
        }
    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            CategoriaRecursoBO boCategoriaRecurso = new CategoriaRecursoBO();
            CategoriaRecurso cat = boCategoriaRecurso.GetCategoriaRecursoById(new Guid(Request.QueryString["GUID"]));

            if (cat != null)
            {
                cat.Descricao = txtDescricao.Text;

                boCategoriaRecurso.UpdateCategoriaRecurso(cat);
                Response.Redirect("~/CategoriaRecurso/List.aspx");
            }
            else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Categoria não existente.");
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
        Response.Redirect("~/CategoriaRecurso/List.aspx");
    }
}
