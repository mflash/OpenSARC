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

public partial class CategoriaData_Edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["GUID"] != null)
            {

                try
                {
                    CategoriaDataBO boCategoriaDataList = new CategoriaDataBO();

                    try
                    {
                        CategoriaData categ = boCategoriaDataList.GetCategoriaDataById(new Guid(Request.QueryString["GUID"]));
                        txtDescricao.Text = categ.Descricao;
                        ddlCor.SelectedColor = categ.Cor;
                        rbDiaLetivo.SelectedIndex = Convert.ToInt32(categ.DiaLetivo);
                    }
                    catch (FormatException )
                    {
                        Response.Redirect("~/CategoriaData/List.aspx");
                    }


                }
                catch (BusinessData.DataAccess.DataAccessException)
                {
                    Response.Redirect("~/CategoriaData/List.aspx");
                }
            }
            else
            {
                Response.Redirect("~/CategoriaData/List.aspx");
            }
        }

    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            CategoriaDataBO boCategoriaDataList = new CategoriaDataBO();
            CategoriaData categ = boCategoriaDataList.GetCategoriaDataById(new Guid(Request.QueryString["GUID"]));
            if (categ != null)
            {
                categ.Descricao = txtDescricao.Text;
                categ.Cor = ddlCor.SelectedColor;
                categ.DiaLetivo = Convert.ToBoolean(rbDiaLetivo.SelectedIndex);

                boCategoriaDataList.UpdateCategoriaData(categ);
                lblstatus.Text = "CategoriaData atualizada com sucesso.";
                lblstatus.Visible = true;
                Response.Redirect("~/CategoriaData/List.aspx");
            }
            else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Categoria não existente.");
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
        Response.Redirect("~/CategoriaData/List.aspx");
    }
}
