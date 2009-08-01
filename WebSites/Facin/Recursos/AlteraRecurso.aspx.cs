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

public partial class Recursos_AlteraRecurso : System.Web.UI.Page
{
    List<HorarioBloqueado> listaHorarios;
    RecursosBO recursosBO = new RecursosBO();

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
                        Recurso recurso = recursosBO.GetRecursoById(new Guid(Request.QueryString["GUID"]));

                        FaculdadesBO vinculos = new FaculdadesBO();
                        ddlVinculo.DataSource = vinculos.GetFaculdades();
                        ddlVinculo.DataTextField = "Nome";
                        ddlVinculo.DataValueField = "Id";
                        ddlVinculo.DataBind();
                        ddlVinculo.SelectedValue = recurso.Vinculo.Id.ToString();

                        CategoriaRecursoBO categoriaRecurso = new CategoriaRecursoBO();
                        ddlCategoria.DataSource = categoriaRecurso.GetCategoriaRecurso();
                        ddlCategoria.DataTextField = "Descricao";
                        ddlCategoria.DataValueField = "Id";
                        ddlCategoria.DataBind();
                        ddlCategoria.SelectedValue = recurso.Categoria.Id.ToString();

                        if (recurso != null)
                        {
                            txtDescricao.Text = recurso.Descricao;
                            if (recurso.EstaDisponivel)
                                rblDisponivel.SelectedIndex = 0;
                            else rblDisponivel.SelectedIndex = 1;

                            listaHorarios = recurso.HorariosBloqueados;
                            Session["Horarios"] = listaHorarios;

                            dgHorarios.DataSource = recurso.HorariosBloqueados;
                            dgHorarios.DataBind();
                        }
                        else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Categoria não existente.");


                    }
                    catch (FormatException )
                    {
                        //throw ex;
                        Response.Redirect("~/Recursos/ListaRecursos.aspx");
                    }

                }
                catch (BusinessData.DataAccess.DataAccessException)
                {
                    Response.Redirect("~/Recursos/ListaRecursos.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Recursos/ListaRecursos.aspx");
            }
        }
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            RecursosBO recursosBO = new RecursosBO();
            Recurso rec = recursosBO.GetRecursoById(new Guid(Request.QueryString["GUID"]));

            if (rec != null)
            {

                listaHorarios = (List<HorarioBloqueado>)Session["Horarios"];
                if (listaHorarios.Count != 0)
                    rec.HorariosBloqueados = listaHorarios;
                else rec.HorariosBloqueados = null;

                rec.Descricao = txtDescricao.Text;
                rec.Categoria.Descricao = ddlCategoria.SelectedItem.Text;
                Guid id = new Guid(ddlCategoria.SelectedValue.ToString());
                rec.Categoria.Id = id;

                if (rblDisponivel.SelectedValue.Equals("Sim")) rec.EstaDisponivel = true;
                else rec.EstaDisponivel = false;

                rec.Vinculo.Nome = ddlVinculo.SelectedItem.Text;
                id = new Guid(ddlVinculo.SelectedValue.ToString());
                rec.Vinculo.Id = id;

                recursosBO.UpdateRecurso(rec);
                lblStatus.Text = "Categoria Atividade atualizada com sucesso.";
                lblStatus.Visible = true;
                txtDescricao.Text = "";
                Response.Redirect("~/Recursos/ListaRecursos.aspx");
            }
            else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Recurso não existente.");
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
        Response.Redirect("~/Recursos/ListaRecursos.aspx");
    }
    
    protected void btnAdicionar_Click(object sender, EventArgs e)
    {
        listaHorarios = (List<HorarioBloqueado>)Session["Horarios"];
        if (ddlHorarioInicio.SelectedValue.CompareTo(ddlHorarioFim.SelectedValue) > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Erro", @"alert('Horario de início deve ser menor ou igual ao horário final.')", true);
        }
        else
        {
            HorarioBloqueado hb = new HorarioBloqueado(ddlHorarioInicio.SelectedItem.Text, ddlHorarioFim.SelectedItem.Text);
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
            Label lblInicio = (Label)e.Item.FindControl("lblInicio");
            Label lblFim = (Label)e.Item.FindControl("lblFim");
            listaHorarios = (List<HorarioBloqueado>)Session["Horarios"];

            listaHorarios.RemoveAt(e.Item.ItemIndex);
            

            Recurso rec = recursosBO.GetRecursoById(new Guid(Request.QueryString["GUID"]));
            HorarioBloqueado hb = new HorarioBloqueado(lblInicio.Text, lblFim.Text);
           
            recursosBO.DeletaHorarioBloqueado(rec.Id, hb);
            
            Session["Horarios"] = listaHorarios;
            dgHorarios.DataSource = listaHorarios;
            dgHorarios.DataBind();

        }
    }

}
