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
using BusinessData.DataAccess;

public partial class Recursos_Disciplina : System.Web.UI.Page
{
    DisciplinasBO disciBo = new DisciplinasBO();
    CalendariosBO calBo = new CalendariosBO();
    CategoriaDisciplinaBO catdiscipBo = new CategoriaDisciplinaBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            PopulaCalendarios();
            PopulaCategorias();
        }
    }

    protected void PopulaCalendarios()
    {
        try
        {
            ddlCalendario.DataSource = calBo.GetCalendarios();
            ddlCalendario.DataTextField = "PorExtenso";
            ddlCalendario.DataValueField = "Id";
            ddlCalendario.DataBind();
        }
        catch (BusinessData.DataAccess.DataAccessException )
        {
            ddlCalendario.Text = "Problema ao carregar esta lista";
        }
    }

    protected void PopulaCategorias()
    {
        try
        {
            ddlCategoria.DataSource = catdiscipBo.GetCategoriaDisciplinas();
            ddlCategoria.DataTextField = "Descricao";
            ddlCategoria.DataValueField = "Id";
            ddlCategoria.DataBind();
        }
        catch (BusinessData.DataAccess.DataAccessException )
        {
            ddlCategoria.Text = "Problema ao carregar esta lista";
        }
    }

    protected void btnAdicionar_Click(object sender, EventArgs e)
    {
        try
        {
            Guid idCal = new Guid(ddlCalendario.SelectedValue);
            Calendario c = calBo.GetCalendario(idCal);

            Guid idCat = new Guid(ddlCategoria.SelectedValue);
            BusinessData.Entities.CategoriaDisciplina cat = catdiscipBo.GetCategoriaDisciplina(idCat);


            Disciplina d = Disciplina.GetDisciplina(txtCodigo.Text,
                                                    Convert.ToInt32(txtCreditos.Text),
                                                    txtNome.Text,
                                                    Convert.ToBoolean(rdbG2.SelectedIndex),
                                                    c,
                                                    cat
                                                    );
            
            disciBo.InsereDisciplina(d);
            
            lblStatus.Text = "Cadastro realizado com sucesso";
            txtCodigo.Text = "";
            txtCreditos.Text = "";
            txtNome.Text = "";
            rdbG2.SelectedIndex = -1;
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
