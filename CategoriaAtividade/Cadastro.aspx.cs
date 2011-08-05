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
using System.Drawing;
using System.Collections.Generic;
using BusinessData.DataAccess;

public partial class CategoriaAtividades_Cadastro : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
   
    protected void btnConfirmar_Click1(object sender, EventArgs e)
    {
        try
        {
            Color cor = ddlCor.SelectedColor;
            CategoriaAtividade categoria = CategoriaAtividade.NewCategoriaAtividade(txtDescricao.Text,cor);
        
            CategoriaAtividadeBO categoriaBO = new CategoriaAtividadeBO();
            List<CategoriaAtividade> lista = categoriaBO.GetCategoriaAtividade();
            bool achou = false;
            
            foreach (CategoriaAtividade c in lista)
            {
                if (c.Descricao == txtDescricao.Text)
                {
                    txtDescricao.Text = "";
                    lblstatus.Text = "Descrição já cadastrada.";
                    lblstatus.Visible = true;
                    achou = true;
                    break;
                }
            }

            if (!achou)
            {
                categoriaBO.InsereCategoriaAtividade(categoria);
                txtDescricao.Text = "";
                lblstatus.Text = "Categoria de Atividades cadastrada com sucesso.";
                lblstatus.Visible = true;
            }


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
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
   
}
