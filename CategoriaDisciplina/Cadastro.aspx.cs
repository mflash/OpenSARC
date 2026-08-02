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

public partial class CategoriaDisciplina_Cadastro: System.Web.UI.Page
{
    CategoriaRecursoBO crBo = new CategoriaRecursoBO();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            PopulaCategoriasRecurso();
        }

    }

    protected void PopulaCategoriasRecurso()
    {
        grvListaCatDisciplina.DataSource = null;
        grvListaCatDisciplina.DataBind();
        grvListaCatDisciplina.DataSource = crBo.GetCategoriaRecurso();
        grvListaCatDisciplina.DataBind();
    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.Dictionary<CategoriaRecurso, double> prioridades = new System.Collections.Generic.Dictionary<CategoriaRecurso,double>();

        for (int i = 0; i < grvListaCatDisciplina.DataKeys.Count; i++)
        { 
            try 
	        {	
                Guid id = (Guid)grvListaCatDisciplina.DataKeys[i].Value;
                CategoriaRecurso cat = crBo.GetCategoriaRecursoById(id);
                double p = Convert.ToDouble(((TextBox)(grvListaCatDisciplina.Rows[i].FindControl("txtPrioridade"))).Text);

                prioridades.Add(cat, p);
	        }
	        catch (Exception ex)
	        {
        	    Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
	        }
            
        }
        
        CategoriaDisciplina cd = CategoriaDisciplina.NewCategoriaDisciplina(txtDescricao.Text, prioridades);

        try
        {
            CategoriaDisciplinaBO cdBo = new CategoriaDisciplinaBO();
            cdBo.InsereCategoriaDisciplina(cd);
            lblStatus.Text = "Categoria de Disciplinas inserida com sucesso.";
            lblStatus.Visible = true;
            txtDescricao.Text = "";
            //Usado aqui para zerar todos os campos
            PopulaCategoriasRecurso();
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
