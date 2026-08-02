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

public partial class Professores_ImportarProfessores : System.Web.UI.Page
{
    private SRRCDAO srrcDAO = new SRRCDAO();

    protected void Page_Load(object sender, EventArgs e)
    {
		List<Usuario> listaProf = null;
        try
        {

            listaProf = srrcDAO.LoadProfs();           
            if (listaProf.Count == 0)
            {
                lblStatus.Text = "Nenhum professor disponível";
                lblStatus.Visible = true;
                lblOnline.Visible = false;
            }
            else
            {
                grvListaProfessores.DataSource = listaProf;
                grvListaProfessores.DataBind();
                lblOnline.Text = "Total: " + listaProf.Count;
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }               
	}

    protected void btnImportar_Click(object sender, EventArgs e)
    {
        List<Usuario> listaProf = srrcDAO.ImportProfs();
        if(listaProf.Count == 0)
        {
            lblStatus.Text = "ERRO! Nenhum professor disponível na base";
            lblStatus.Visible = true;
            return;
        }
        srrcDAO.ImportProfsToSARC(listaProf);        
    }


    protected void grvListaProfessores_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            ProfessoresBO boProfessor = new ProfessoresBO();
            Professor p = (Professor)boProfessor.GetPessoaById((Guid)grvListaProfessores.DataKeys[e.RowIndex].Value);

            boProfessor.DeletePessoa(p);
            lblStatus.Text = "Professor excluído com sucesso";
            lblStatus.Visible = true;

            grvListaProfessores.DataSource = boProfessor.GetProfessores();
            grvListaProfessores.DataBind();
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
    
    protected void grvListaProfessores_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            ProfessoresBO boProfessor = new ProfessoresBO();
            Guid id = (Guid)grvListaProfessores.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/Professores/AlteraProfessores.aspx?GUID=" + id.ToString());
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
   
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
