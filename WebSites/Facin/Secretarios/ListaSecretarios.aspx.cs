using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System.Security;

public partial class Secretarios_ListaSecretarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SecretariosBO boSecretario = new SecretariosBO();
            List<Secretario> listaSec = boSecretario.GetSecretarios();
            if (listaSec.Count == 0)
            {
                lblStatus.Text = "Nenhum secretário cadastrado";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaSecretarios.DataSource = listaSec;
                grvListaSecretarios.DataBind();
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void grvListaSecretarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            SecretariosBO boSecretario = new SecretariosBO();
            Secretario s = (Secretario)boSecretario.GetPessoaById((Guid)grvListaSecretarios.DataKeys[e.RowIndex].Value);

            boSecretario.DeletePessoa(s);
            lblStatus.Text = "Secretário excluído com sucesso";
            lblStatus.Visible = true;

            grvListaSecretarios.DataSource = boSecretario.GetSecretarios();
            grvListaSecretarios.DataBind();
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

    protected void grvListaSecretarios_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            SecretariosBO boSecretario = new SecretariosBO();
            Guid id = (Guid)grvListaSecretarios.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/Secretarios/AlteraSecretarios.aspx?GUID=" + id.ToString());
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
