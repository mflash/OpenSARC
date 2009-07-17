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
using System.Text;
using BusinessData.DataAccess;

public partial class Pagina2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CalendariosBO calBo = new CalendariosBO();
            List<Calendario> listaCal = calBo.GetCalendarios();
            if (listaCal.Count == 0)
            {
                lblStatus.Text = "Nenhum calendário cadastrado";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaCalendarios.DataSource = listaCal;
                grvListaCalendarios.DataBind();
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }

    protected void grvListaCalendarios_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Guid id = (Guid)grvListaCalendarios.DataKeys[e.NewEditIndex].Value;

            StringBuilder b = new StringBuilder();
            b.Append("~/Calendario/Cadastro.aspx?");
            b.Append("CALENDARIO=");
            b.Append(id.ToString());

            Response.Redirect(b.ToString());
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void grvListaDatas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DateTime data = (DateTime)grvListaDatas.DataKeys[e.RowIndex].Value;
            
            DatasBO dBo = new DatasBO();

            dBo.DeletaData((Guid)Session["CalendarioId"], Data.GetData(data, null));
            lblStatus.Text = "Data excluida com sucesso";
            lblStatus.Visible = true;

            //Para atualizar precisa apagar e atualizar.
            grvListaDatas.DataSource = null;
            grvListaDatas.DataBind();
            grvListaDatas.DataSource = dBo.GetDatasByCalendario((Guid)Session["CalendarioId"]);
            grvListaDatas.DataBind();
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void grvListaCalendarios_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            Guid id = (Guid)grvListaCalendarios.DataKeys[e.NewSelectedIndex].Value;
            Session["CalendarioId"] = id;
            DatasBO dBo = new DatasBO();

            grvListaDatas.DataSource = dBo.GetDatasByCalendario(id);
            grvListaDatas.DataBind();

            lblStatus.Visible = false;


        }
        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
