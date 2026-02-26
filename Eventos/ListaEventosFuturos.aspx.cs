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

public partial class Eventos_ListaEventosFuturos : System.Web.UI.Page
{
    private EventoBO eventoBO = new EventoBO();
    HorariosEventoBO horariosEventoBO = new HorariosEventoBO();
    Calendario cal;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            PopulaListaEventos();
        }
    }

    private void PopulaListaEventos()
    {
        IList<Evento> listaEventosNaoOcorridos = new List<Evento>();
        IList<HorariosEvento> listaHorariosEvento;
        try
        {
            cal = (Calendario)Session["Calendario"];
            IList<Evento> listaEventos = eventoBO.GetEventosByCal(cal.Id);
            foreach (Evento evento in listaEventos)
            {
                listaHorariosEvento = horariosEventoBO.GetHorariosEventosById(evento.EventoId);
                if (!jaOcorreu(listaHorariosEvento))
                    listaEventosNaoOcorridos.Add(evento);
            }
            grvListaEventos.DataSource = listaEventosNaoOcorridos;
            grvListaEventos.DataBind();
            if (listaEventos.Count == 0)
            {
                lblStatus.Text = "Nenhum evento futuro.";
                lblStatus.Visible = true;
                btnExportarHtml.Visible = false;
            } 
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void grvListaEventos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            MembershipUser usr = Membership.GetUser();
            PessoaFactory fabricaPessoas = PessoaFactory.GetInstance();
            PessoaBase pessoa = fabricaPessoas.CreatePessoa((Guid)usr.ProviderUserKey);
            Evento evento = eventoBO.GetEventoById((Guid)grvListaEventos.DataKeys[e.RowIndex].Value);
            if (evento.AutorId.Equals(pessoa))
            {
                eventoBO.DeletaEvento(evento.EventoId);
                cardDatas.Visible = false;
                PopulaListaEventos();
                grdDatas.Visible = false;
                MostrarAlerta("Evento excluído com sucesso!", "success");
            }
            else
            {
                MostrarAlerta("Evento deve ser excluído pelo seu autor.", "warning");
            }
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

    protected void grvListaEventos_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            MembershipUser usr = Membership.GetUser();
            PessoaFactory fabricaPessoas = PessoaFactory.GetInstance();
            PessoaBase pessoa = fabricaPessoas.CreatePessoa((Guid)usr.ProviderUserKey);
            Guid id = (Guid)grvListaEventos.DataKeys[e.NewEditIndex].Value;
            Evento evento = eventoBO.GetEventoById(id);
            if (evento.AutorId.Equals(pessoa))
            {
                IList<HorariosEvento> listaHorariosEvento = horariosEventoBO.GetHorariosEventosById(id);
                if (!jaOcorreu(listaHorariosEvento))
                    Response.Redirect("~/Eventos/EditarEventosFuturos.aspx?GUID=" + id.ToString());
                else
                {
                    MostrarAlerta("Evento não pode ser editado, pois ele já ocorreu ou está ocorrendo.", "warning");
                }
            }
            else
            {
                MostrarAlerta("Evento deve ser editado pelo seu autor.", "warning");
            }
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

    protected void grvListaEventos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            IList<HorariosEvento> horarios = (List<HorariosEvento>)horariosEventoBO.GetHorariosEventosById((Guid)grvListaEventos.SelectedDataKey.Value);
            if (horarios.Count != 0)
            {
                ((List<HorariosEvento>)horarios).Sort();
                grdDatas.DataSource = horarios;
                grdDatas.DataBind();
                grdDatas.Visible = true;
                lblDatas.Visible = true;
                cardDatas.Visible = true;
                lblStatus.Visible = false;
            }
            else
            {
                MostrarAlerta("Nenhuma data para o evento.", "info");
                grdDatas.Visible = false;
                cardDatas.Visible = false;
                lblDatas.Visible = false;
            }
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

    protected void btnExportarHtml_Click(object sender, EventArgs e)
    {
        ExportarHtml();
    }

    private void ExportarHtml()
    {
        DataTable tabela = new DataTable();

        foreach (DataControlField coluna in grvListaEventos.Columns)
        {
            tabela.Columns.Add(coluna.HeaderText);
        }

        DataRow dr;
        Label lblAux;
        foreach (GridViewRow linha in grvListaEventos.Rows)
        {
            dr = tabela.NewRow();
            lblAux = (Label)linha.FindControl("lblTitulo");
            dr["Título"] = lblAux.Text;

            lblAux = (Label)linha.FindControl("lblResponsavel");
            dr["Responsável"] = lblAux.Text;

            lblAux = (Label)linha.FindControl("lblDescricao");
            dr["Descrição"] = lblAux.Text;

            lblAux = (Label)linha.FindControl("lblUnidade");
            dr["Unidade"] = lblAux.Text;

            lblAux = (Label)linha.FindControl("lblAutor");
            dr["Autor"] = lblAux.Text;

            tabela.Rows.Add(dr);
        }

        Session["DownHtml"] = tabela;
        Response.Redirect("DownloadHTML.aspx");

    }

    private bool jaOcorreu(IList<HorariosEvento> listaHorariosEvento)
    {
        ((List<HorariosEvento>)listaHorariosEvento).Sort();
        HorariosEvento ultimoHorario = listaHorariosEvento[listaHorariosEvento.Count - 1];
        if (ultimoHorario.Data.Date.CompareTo(DateTime.Now.Date) == 0)
        {
            DateTime data = BusinessData.Distribuicao.Entities.Horarios.ParseToDateTime(ultimoHorario.HorarioInicio);
            if (data.TimeOfDay.CompareTo(DateTime.Now.TimeOfDay) >= 0)
                return false;
            else
                return true;
        }
        else if (ultimoHorario.Data.Date.CompareTo(DateTime.Now.Date) > 0)
            return false;
        else
            return true;
    }

    private void MostrarAlerta(string mensagem, string tipo)
    {
        string icone = tipo == "success" ? "check-circle" : 
                       tipo == "warning" ? "exclamation-triangle" : 
                       tipo == "error" ? "x-circle" : "info-circle";

        string mensagemEscapada = mensagem.Replace("'", "\\'");

        string script = "Swal.fire({" +
                       "icon: '" + tipo + "'," +
                       "title: '" + mensagemEscapada + "'," +
                       "showConfirmButton: true," +
                       "confirmButtonText: 'OK'," +
                       "confirmButtonColor: '#2563eb'" +
                       "});";

        ScriptManager.RegisterStartupScript(this, GetType(), "MostrarAlerta", script, true);
    }
}
