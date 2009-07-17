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

public partial class Cursos_Cadastro : System.Web.UI.Page
{
    FaculdadesBO boFaculdade = new FaculdadesBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlFaculdade.DataSource = boFaculdade.GetFaculdades();
            ddlFaculdade.DataTextField = "Nome";
            ddlFaculdade.DataValueField = "Id";
            ddlFaculdade.DataBind();
        }
    }
    
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        FaculdadesBO controleFaculdades = new FaculdadesBO();
        Faculdade fac = controleFaculdades.GetFaculdadeById(new Guid(ddlFaculdade.SelectedValue));
        
        Curso c = Curso.NewCurso(txtCodigo.Text, txtNome.Text, fac);
        try
        {
            CursosBO boCursos = new CursosBO();
            boCursos.InsereCurso(c);
            lblStatus.Text = "Curso cadastrado com sucesso.";
            lblStatus.Visible = true;
            txtNome.Text = "";
            txtCodigo.Text = "";
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
