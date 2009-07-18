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
using BusinessData.DataAccess;
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System.Security;

public partial class Cursos_Edit : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["GUID"] != null)
            {
                try
                {                    
                    try
                    {
                        FaculdadesBO boFaculdade = new FaculdadesBO();
                        ddlFaculdade.DataSource = boFaculdade.GetFaculdades();
                        ddlFaculdade.DataTextField = "Nome";
                        ddlFaculdade.DataValueField = "Id";
                        ddlFaculdade.DataBind();

                        CursosBO boCurso = new CursosBO();
                        Curso curso = boCurso.GetCursoByCodigo(Request.QueryString["GUID"]);
                        txtCodigo.Text = curso.Codigo;
                        txtNome.Text = curso.Nome;
                        ddlFaculdade.SelectedValue = curso.Vinculo.Id.ToString();
                    }
                    catch (FormatException )
                    {
                        Response.Redirect("~/Cursos/List.aspx");
                    }
                    
                }
                catch (BusinessData.DataAccess.DataAccessException)
                {
                    Response.Redirect("~/Cursos/List.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Cursos/List.aspx");
            }

           
        }
    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            
            CursosBO boCurso = new CursosBO();
            Curso curso = boCurso.GetCursoByCodigo(Request.QueryString["GUID"]);
            FaculdadesBO controleFaculdades = new FaculdadesBO();
            Faculdade fac = controleFaculdades.GetFaculdadeById(new Guid(ddlFaculdade.SelectedValue));
            if (fac != null)
            {
                curso.Codigo = txtCodigo.Text;
                curso.Nome = txtNome.Text;
                curso.Vinculo = fac;
                boCurso.UpdateCurso(curso);
                lblStatus.Text = "Curso alterado com sucesso";
                lblStatus.Visible = true;
                txtCodigo.Text = "";
                txtNome.Text = "";
                Response.Redirect("~/Cursos/List.aspx");
            }
            else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Curso não existente.");
          
        }
        catch (ArgumentException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }

    
    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Cursos/List.aspx");
    }
}
