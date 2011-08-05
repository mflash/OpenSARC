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
using System.Drawing;
using BusinessData.DataAccess;

public partial class CategoriaAtividades_Edit: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request.QueryString["GUID"] != null)
            {
                try
                {
                    CategoriaAtividadeBO boCategoriaAtividade = new CategoriaAtividadeBO();

                    try
                    {
                        CategoriaAtividade cateAtividade = boCategoriaAtividade.GetCategoriaAtividadeById(new Guid(Request.QueryString["GUID"]));
                        txtDescricao.Text = cateAtividade.Descricao;
                        ddlCor.SelectedColor = cateAtividade.Cor;
                    }
                    catch(FormatException )
                    {
                        Response.Redirect("~/CategoriaAtividade/List.aspx");
                    }

                }
                catch (DataAccessException)
                {
                    Response.Redirect("~/CategoriaAtividade/List.aspx");
                }
            }
            else
            {
                Response.Redirect("~/CategoriaAtividade/List.aspx");
            }
        }
    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            Color cor = ddlCor.SelectedColor;
            CategoriaAtividadeBO cateAtividadeBO = new CategoriaAtividadeBO();
            CategoriaAtividade cat = cateAtividadeBO.GetCategoriaAtividadeById(new Guid(Request.QueryString["GUID"]));
            if (cat != null)
            {
                cat.Descricao = txtDescricao.Text;
                cat.Cor = cor;

                cateAtividadeBO.UpdateCategoriaAtividade(cat);
                Response.Redirect("~/CategoriaAtividade/List.aspx");
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
        Response.Redirect("~/CategoriaAtividade/List.aspx");
    }
    
    
}
