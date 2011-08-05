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

public partial class Recursos_Disciplina : System.Web.UI.Page
{
    DisciplinasBO disciBo = new DisciplinasBO();
    CalendariosBO calBo = new CalendariosBO();
    CategoriaDisciplinaBO catdisciBo = new CategoriaDisciplinaBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["CALENDARIO"] != null)
            {
                Guid idCal = new Guid(Request.QueryString["CALENDARIO"]);
                string DisciCod = Request.QueryString["DISCIPLINA"];
                
                try
                {
                    Calendario c = calBo.GetCalendario(idCal);
                    Disciplina d = disciBo.GetDisciplina(DisciCod, c);

                    lblCodigo.Text = d.Cod;
                    txtCreditos.Text = d.Cred.ToString();
                    txtNome.Text = d.Nome;
                    rdbG2.SelectedIndex = Convert.ToInt32(d.G2);
                    ddlCategoria.DataSource = catdisciBo.GetCategoriaDisciplinas();
                    ddlCategoria.DataTextField = "Descricao";
                    ddlCategoria.DataValueField = "Id";
                    ddlCategoria.DataBind();
                    ddlCategoria.SelectedValue = d.Categoria.Id.ToString();

                }
                catch (BusinessData.DataAccess.DataAccessException)
                {
                    Response.Redirect("~/Disciplina/ListaDisciplinas.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Disciplina/ListaDisciplinas.aspx");
            }
        }

    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {

        try
        {
        Guid idCal = new Guid(Request.QueryString["CALENDARIO"]);
        Calendario c = calBo.GetCalendario(idCal);

        Guid idCat = new Guid(ddlCategoria.SelectedValue);
        CategoriaDisciplina cat = catdisciBo.GetCategoriaDisciplina(idCat);

        Disciplina d = Disciplina.GetDisciplina(lblCodigo.Text,Convert.ToInt32(txtCreditos.Text),txtNome.Text,Convert.ToBoolean(rdbG2.SelectedIndex),c,cat);

        if (d != null)
        {
            disciBo.UpdateDisciplina(d);
            Response.Redirect("ListaDisciplinas.aspx");

        }
        else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Disciplina não existente.");

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
        Response.Redirect("~/Disciplina/ListaDisciplinas.aspx");
    }
}
