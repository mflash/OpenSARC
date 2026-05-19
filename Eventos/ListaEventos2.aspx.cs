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

public partial class Eventos_ListaEventos : System.Web.UI.Page
{
    EventoBO eventoBO = new EventoBO();
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
        try
        {
            cal = (Calendario)Session["Calendario"];
            IList<Evento> listaEventos = eventoBO.GetEventosByCal(cal.Id);

            if (ckbApenasEventosFuturos.Checked)
            {
                var listaFiltrada = new List<Evento>();
                foreach (Evento evento in listaEventos)
                {
                    IList<HorariosEvento> horarios = horariosEventoBO.GetHorariosEventosByIdCal(evento.EventoId, cal);
                    if (horarios.Count > 0 && !jaOcorreu(horarios))
                        listaFiltrada.Add(evento);
                }
                listaEventos = listaFiltrada;
            }

            grvListaEventos.DataSource = listaEventos;
            grvListaEventos.DataBind();
            if (listaEventos.Count == 0)
            {
                lblStatus.Text = ckbApenasEventosFuturos.Checked
                    ? "Nenhum evento futuro cadastrado."
                    : "Nenhum evento cadastrado.";
                lblStatus.Visible = true;
                btnExportarHtml.Visible = false;
            }
            else
            {
                lblStatus.Visible = false;
                btnExportarHtml.Visible = true;
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    // Exibe uma mensagem de aviso no modal Bootstrap
    private void ExibirAviso(string mensagem)
    {
        hdnMensagemAviso.Value = mensagem;
        lblStatus.Visible = false;
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
                lblStatus.Text = "Evento excluído com sucesso";
                lblStatus.Visible = true;
                PopulaListaEventos();
                grdDatas.Visible = false;
            }
            else
            {
                ExibirAviso("Evento deve ser excluído pelo seu autor.");
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
                    Response.Redirect("~/Eventos/EditarEventos2.aspx?GUID=" + id.ToString());
                else
                {
                    e.Cancel = true;
                    grvListaEventos.EditIndex = -1;
                    PopulaListaEventos();
                    ExibirAviso("Evento não pode ser editado, pois ele já ocorreu ou está ocorrendo.");
                }
            }
            else
            {
                e.Cancel = true;
                grvListaEventos.EditIndex = -1;
                PopulaListaEventos();
                ExibirAviso("Evento deve ser editado pelo seu autor.");
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
        Response.Redirect("~/Default/PaginaInicial2.aspx");
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

                var lblTituloEvento = (Label)grvListaEventos.SelectedRow.FindControl("lblTituloEvento");
                lblDatas.Text = lblTituloEvento != null
                    ? "Datas: " + lblTituloEvento.Text
                    : "Datas do Evento";

                hdnAbrirModal.Value = "true";
                lblStatus.Visible = false;
            }
            else
            {
                lblStatus.Text = "Nenhuma data para o evento.";
                lblStatus.Visible = true;
                hdnAbrirModal.Value = "false";
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
            lblAux = (Label)linha.FindControl("lblTituloEvento"); // match the actual markup ID
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

    protected void ckbApenasEventosFuturos_CheckedChanged(object sender, EventArgs e)
    {
        PopulaListaEventos();
    }
}
