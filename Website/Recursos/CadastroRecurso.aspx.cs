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
using BusinessData.Entities;
using BusinessData.BusinessLogic;
using System.Collections.Generic;

public partial class Pagina6 : System.Web.UI.Page
{

    RecursosBO recursoBO = new RecursosBO();
    CategoriaRecursoBO categBO = new CategoriaRecursoBO();
    FaculdadesBO faculBO = new FaculdadesBO();
    List<HorarioBloqueado> listaHorarios = new List<HorarioBloqueado>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            populaCategoria();
            populaVinculo();
            Session["Horarios"] = listaHorarios;
        }
    }

    public void populaCategoria()
    {
        try
        {
            ddlCategoria.DataSource = categBO.GetCategoriaRecurso();
            ddlCategoria.DataTextField = "Descricao";
            ddlCategoria.DataValueField = "Id";
            ddlCategoria.DataBind();
        }
        catch (BusinessData.DataAccess.DataAccessException )
        { 
        //TODO
        }
    }

    public void populaVinculo()
    {
        try
        {
            ddlVinculo.DataSource = faculBO.GetFaculdades();
            ddlVinculo.DataTextField = "Nome";
            ddlVinculo.DataValueField = "Id";
            ddlVinculo.DataBind();
        }
        catch (BusinessData.DataAccess.DataAccessException )
        { 
        //TODO
        }
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            Calendario cal = (Calendario)Session["Calendario"];
            Recurso recurso;
            listaHorarios = (List<HorarioBloqueado>)Session["Horarios"];
            CategoriaRecurso categoriaRecurso = categBO.GetCategoriaRecursoById(new Guid(ddlCategoria.SelectedValue));
            Faculdade vinculo = faculBO.GetFaculdadeById(new Guid(ddlVinculo.SelectedValue));

            if (listaHorarios.Count != 0)
                recurso = Recurso.NewRecurso(txtDescricao.Text, vinculo, categoriaRecurso, Convert.ToBoolean(Convert.ToInt16(rblDisponivel.SelectedValue)), listaHorarios);
            else recurso = Recurso.NewRecurso(txtDescricao.Text, vinculo, categoriaRecurso, Convert.ToBoolean(Convert.ToInt16(rblDisponivel.SelectedValue)), null);

            recursoBO.InsereRecurso(recurso, cal);

            txtDescricao.Text = "";
            ddlHorarioInicio.SelectedIndex = 0;
            ddlHorarioFim.SelectedIndex = 0;
            dgHorarios.DataSource = null;
            dgHorarios.DataBind();
            pnlHorarios.Visible = false;
            listaHorarios.Clear();
            Session["Horarios"] = listaHorarios;
            lblStatus.Text = "Recurso cadastrado com sucesso.";

            lblStatus.Visible = true;
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }

    protected void lbCadastrarHorarios_Click(object sender, EventArgs e)
    {
        if (pnlHorarios.Visible == false)
            pnlHorarios.Visible = true;
        else pnlHorarios.Visible = false;
    }

    protected void btnAdicionar_Click(object sender, EventArgs e)
    {
        listaHorarios = (List<HorarioBloqueado>)Session["Horarios"];
        if (ddlHorarioInicio.SelectedValue.CompareTo(ddlHorarioFim.SelectedValue) >0) 
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Erro", @"alert('Horario de início deve ser menor ou igual ao horário final.')", true);
        }
        else
        {
            HorarioBloqueado hb = new HorarioBloqueado(ddlHorarioInicio.SelectedItem.Text, ddlHorarioFim.SelectedItem.Text);
            if (listaHorarios.Contains(hb))
                return;
            listaHorarios.Add(hb);
            dgHorarios.DataSource = listaHorarios;
            dgHorarios.DataBind();
            Session["Horarios"] = listaHorarios;
            ddlHorarioInicio.SelectedIndex = 0;
            ddlHorarioFim.SelectedIndex = 0;
        }
    }

    protected void dgHorarios_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            listaHorarios = (List<HorarioBloqueado>)Session["Horarios"];

            listaHorarios.RemoveAt(e.Item.ItemIndex);
            Session["Horarios"] = listaHorarios;

            dgHorarios.DataSource = listaHorarios;
            dgHorarios.DataBind();

        }
    }
}