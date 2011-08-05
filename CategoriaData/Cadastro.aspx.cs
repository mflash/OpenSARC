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
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System.Security;
using System.Collections.Generic;

public partial class CategoriaData_Cadastro : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        CategoriaData c = CategoriaData.NewCategoriaData(txtDescricao.Text, ddlCor.SelectedColor,Convert.ToBoolean(rbDiaLetivo.SelectedIndex));
        CategoriaDataBO categoriaBO = new CategoriaDataBO();
        try
        {
            categoriaBO.InsereCategoriaData(c);
            lblstatus.Text = "Categoria de Datas cadastrada com sucesso.";
            lblstatus.Visible = true;
            txtDescricao.Text = "";

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
