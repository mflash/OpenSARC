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
using BusinessData.DataAccess;
using BusinessData.Entities.Util;

public partial class Secretarios_ListaEvento : System.Web.UI.Page
{
    TrocaBO trocaBO = new TrocaBO();
    SecretariosBO controleSecretarios = new SecretariosBO();
    AlocacaoBO alocBO = new AlocacaoBO();
    TransferenciaBO transBO = new TransferenciaBO();
    EventoBO eventoBO = new EventoBO();
    HorariosEventoBO horariosEventoBO = new HorariosEventoBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["Calendario"] == null)
                {
                    Response.Redirect("../Default/SelecionarCalendario.aspx");
                }
                if (Session["AppState"] != null && ((AppState)Session["AppState"]) == AppState.Admin)
                {
                    Server.Transfer("~/Default/Erro.aspx?Erro=O sistema está bloqueado.");
                }
                else
                {
                    Calendario cal = (Calendario)Session["Calendario"];
                    MembershipUser user = Membership.GetUser();
                    Guid secretarioId = new Guid(user.ProviderUserKey.ToString());

                    Secretario sec = (Secretario)controleSecretarios.GetPessoaById(secretarioId);
                    
                    List<Evento> listaEventos = null;

                    try
                    {
                        listaEventos = eventoBO.GetEventos(sec.Id, cal);
                        
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
                    }

                    VerificaTrocas();                   
                    VerificaTransferencias();

                    if (listaEventos.Count == 0)
                    {
                        lblEventos.Visible = true;
                    }
                    else
                    {
                        IList<Evento> listaEventosNaoOcorridos = new List<Evento>();
                        IList<HorariosEvento> listaHorariosEvento;
                        foreach (Evento evento in listaEventos)
                        {
                            listaHorariosEvento = horariosEventoBO.GetHorariosEventosById(evento.EventoId);
                            if (!jaOcorreu(listaHorariosEvento))
                                listaEventosNaoOcorridos.Add(evento);
                        }
                        if (listaEventosNaoOcorridos.Count == 0)
                            lblEventos.Visible = true;
                        else
                        {
                            dgEventos.DataSource = listaEventosNaoOcorridos;
                            dgEventos.DataBind();
                        }
                    }
                }
            }
            else
            {
                if (Session["Calendario"] == null)
                {
                    Response.Redirect("../Default/SelecionarCalendario.aspx");
                }

                VerificaTransferencias();
                VerificaTrocas();
                
            }
        }

        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void dgTroca_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (Session["Trocas"] != null)
            {
                List<Troca> trocas = (List<Troca>)Session["Trocas"];

                Label lblEvento = (Label)e.Item.FindControl("lblEvento");
                Label lblData = (Label)e.Item.FindControl("lblData");
                Label lblHorario = (Label)e.Item.FindControl("lblHorario");
                Label lblRecProposto = (Label)e.Item.FindControl("lblRecProposto");
                Label lblRecOferecido = (Label)e.Item.FindControl("lblRecOferecido");
                Label lblAutor = (Label)e.Item.FindControl("lblAutor");

                lblEvento.Text = trocas[e.Item.DataSetIndex].AlocacaoDesejada.Evento.Titulo;
                lblData.Text = trocas[e.Item.DataSetIndex].AlocacaoDesejada.Data.ToShortDateString();
                lblHorario.Text = trocas[e.Item.DataSetIndex].AlocacaoDesejada.Horario;
                lblRecProposto.Text = trocas[e.Item.DataSetIndex].AlocacaoAtual.Recurso.Descricao;
                lblRecOferecido.Text = trocas[e.Item.DataSetIndex].AlocacaoDesejada.Recurso.Descricao;
                if (trocas[e.Item.DataSetIndex].AlocacaoAtual.Evento == null)
                    lblAutor.Text = trocas[e.Item.DataSetIndex].AlocacaoAtual.Aula.TurmaId.Professor.Nome;
                else
                    lblAutor.Text = trocas[e.Item.DataSetIndex].AlocacaoAtual.Evento.AutorId.Nome;

            }

        }
    }

    protected void dgTroca_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Aceitou")
        {
            List<Troca> trocas = (List<Troca>)Session["Trocas"];
            try
            {
                Troca troca = trocas[e.Item.ItemIndex];
                troca.EstaPendente = false;
                troca.FoiAceita = true;
                troca.FoiVisualizada = true;

                Aula aulaAtual = troca.AlocacaoAtual.Aula;
                Aula aulaDesejada = troca.AlocacaoDesejada.Aula;
                Evento eventoAtual = troca.AlocacaoAtual.Evento;
                Evento eventoDesejado = troca.AlocacaoDesejada.Evento;

                Swapper.Swap<Aula>(ref aulaAtual, ref aulaDesejada);
                Swapper.Swap<Evento>(ref eventoAtual, ref eventoDesejado);

                trocaBO.UpDateTroca(troca);

                troca.AlocacaoAtual.Aula = aulaAtual;
                troca.AlocacaoDesejada.Aula = aulaDesejada;
                troca.AlocacaoAtual.Evento = eventoAtual;
                troca.AlocacaoDesejada.Evento = eventoDesejado;

                alocBO.UpdateAlocacao(troca.AlocacaoAtual);
                alocBO.UpdateAlocacao(troca.AlocacaoDesejada);

                VerificaTrocas();
            }
            catch (Exception)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Erro ao aceitar a troca.");
            }


        }

        if (e.CommandName == "Recusou")
        {
            List<Troca> trocas = (List<Troca>)Session["Trocas"];
            try
            {
                Troca troca = trocas[e.Item.ItemIndex];
                troca.EstaPendente = false;
                troca.FoiAceita = false;
                troca.FoiVisualizada = true;

                trocaBO.UpDateTroca(troca);

                VerificaTrocas();
            }
            catch (Exception)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Erro ao recusar a troca.");
            }
        }
    }

    protected void VerificaTrocas()
    {
        List<Troca> trocasAulas = new List<Troca>();
        List<Troca> trocasEventos = new List<Troca>();

        Calendario cal = (Calendario)Session["Calendario"];
        MembershipUser user = Membership.GetUser();
        Guid secId = new Guid(user.ProviderUserKey.ToString());

        try
        {
            trocasEventos = trocaBO.GetTrocasEventosByAutor(secId, cal);
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

        Session["Trocas"] = trocasEventos;

        if (trocasEventos.Count != 0)
        {
            dgTroca.DataSource = trocasEventos;
            dgTroca.DataBind();
            lblRotulo.Visible = true;
            dgTroca.Visible = true;
        }
        else
        {
            lblRotulo.Visible = false;
            dgTroca.Visible = false;
        }
        
    }

    protected void dgTransferencias_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Viu")
        {
            Label lbltransId = (Label)e.Item.FindControl("lblTransId");
            Guid transId = new Guid(lbltransId.Text);
            try
            {
                Calendario cal = (Calendario)Session["Calendario"];

                Transferencia trans = transBO.GetTransferencia(transId, cal);

                trans.FoiVisualizada = true;

                transBO.TransferenciaUpdate(trans);

                VerificaTransferencias();
            }
            catch (Exception)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Erro ao visualizar a transferência.");
            }
        }

    }
   
    protected void VerificaTransferencias()
    {
        Calendario cal = (Calendario)Session["Calendario"];
        MembershipUser user = Membership.GetUser();
        Guid professorId = new Guid(user.ProviderUserKey.ToString());

        List<Transferencia> trans = transBO.GetTransferencias(professorId, cal);

        Session["Transferencias"] = trans;
        if (trans.Count != 0)
        {
            dgTransferencias.DataSource = trans;
            dgTransferencias.DataBind();
            dgTransferencias.Visible = true;
            lblTransfencia.Visible = true;
        }
        else
        {
            dgTransferencias.Visible = false;
            lblTransfencia.Visible = false;
        }
    }

    protected void dgTransferencias_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (Session["Transferencias"] != null)
            {
                List<Transferencia> trans = (List<Transferencia>)Session["Transferencias"];

                Label lblAutor = (Label)e.Item.FindControl("lblAutor");
                Label lblEvento = (Label)e.Item.FindControl("lblEvento");

                if (trans[e.Item.ItemIndex].EventoTransferiu == null)
                {
                    lblAutor.Text = trans[e.Item.ItemIndex].TurmaTransferiu.Professor.Nome;
                }
                else
                {
                    lblAutor.Text = trans[e.Item.ItemIndex].EventoTransferiu.AutorId.Nome;
                }
            }
        }
    }

    protected void dgEventos_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        //TODO: REDICIONAR PARA A PAGINA TRATANDO EVENTOS
        //
        if (e.CommandName == "Horarios")
        {
            Label lblEventoId = (Label)e.Item.FindControl("lblEventoId");
            Response.Redirect("~/Common/DetalhesEvento.aspx?Evento=" + lblEventoId.Text);

        }
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

    }