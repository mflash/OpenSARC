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

public partial class Cursos_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            CursosBO boCurso = new CursosBO();
            List<Curso> listaCursos = boCurso.GetCursos();
            if (listaCursos.Count == 0)
            {
                lblStatus.Text = "Nenhum curso cadastrado.";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaCursos.DataSource = listaCursos;
                grvListaCursos.DataBind();
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }
     
    protected void grvListaCursos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            CursosBO boCurso = new CursosBO();
            Curso curso = boCurso.GetCursoByCodigo(grvListaCursos.DataKeys[e.RowIndex].Value.ToString());
            boCurso.DeletaCurso(curso.Codigo);
            lblStatus.Text = "Curso excluído com sucesso";
            lblStatus.Visible = true;

            grvListaCursos.DataSource = boCurso.GetCursos();
            grvListaCursos.DataBind();
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
    protected void grvListaCursos_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            CursosBO boCurso = new CursosBO();
            string id = grvListaCursos.DataKeys[e.NewEditIndex].Value.ToString();
            Response.Redirect("~/Cursos/Edit.aspx?GUID=" + id);
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
    protected void grvListaProfessores_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
