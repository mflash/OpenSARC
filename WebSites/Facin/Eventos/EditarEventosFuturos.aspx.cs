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
using System.Collections.Generic;
using BusinessData.DataAccess;
using System.Security;

public partial class Eventos_EditarEventosFuturos : System.Web.UI.Page
{
    private IList<HorariosEvento> listaHorarios = new List<HorariosEvento>();
    private IList<HorariosEvento> listaHorariosAdicionados = new List<HorariosEvento>();
    private IList<HorariosEvento> listaHorariosExcluidos = new List<HorariosEvento>();
    private HorariosEventoBO horariosEventoBO = new HorariosEventoBO();
    private EventoBO eventoBO = new EventoBO();
    private Evento evento;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["listaHorariosExcluidos"] = listaHorariosExcluidos;
            Session["listaHorariosAdicionados"] = listaHorariosAdicionados;
            PopulaDDLFim();
            if (Request.QueryString["GUID"] != null)
            {
                Session["EventoId"] = new Guid(Request.QueryString["GUID"]);
                try
                {
                    EventoBO eventoBO = new EventoBO();
                    try
                    {
                        evento = eventoBO.GetEventoById(new Guid(Request.QueryString["GUID"]));
                        if (evento == null)
                            Response.Redirect("~/Eventos/ListaEventos.aspx");
                        ativaHorarios();
                        txtTitulo.Text = evento.Titulo;
                        txtResponsavel.Text = evento.Responsavel;
                        txtUnidade.Text = evento.Unidade;
                        txtaDescricao.Text = evento.Descricao;
                    }
                    catch (FormatException ex)
                    {
                        Response.Redirect("~/Eventos/ListaEventosFuturos.aspx");
                    }
                }
                catch (BusinessData.DataAccess.DataAccessException ex)
                {
                    Response.Redirect("~/Eventos/ListaEventosFuturos.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Eventos/ListaEventosFuturos.aspx");
            }
        }
    }

    private void ativaHorarios()
    {
        ckbEhRecorrente.Checked = true;
        RadioButtonList1.SelectedIndex = 2;
        RadioButtonList1.Enabled = false;
        RadioButtonList1.Visible = true;
        listaHorarios = horariosEventoBO.GetHorariosEventosById(evento.EventoId);
        ((List<HorariosEvento>)listaHorarios).Sort();
        Session["listaHorarios"] = listaHorarios;
        pnlDias.Visible = false;
        pnlSelecionarDatas.Visible = false;
        Panel2.Visible = true;
        Panel1.Visible = false;
        PopulaDDLFim2();
        grdHorarios.DataSource = listaHorarios;
        grdHorarios.DataBind();
        grdHorarios.Visible = true;
    }

    protected void ddlInicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulaDDLFim();
    }

    private void PopulaHorarios()
    {
        listaHorarios = (IList<HorariosEvento>)Session["listaHorarios"];
        grdHorarios.DataSource = listaHorarios;
        grdHorarios.DataBind();
    }

    public string[] GetHorariosFuturos(string horarioInicial)
    {
        string[] horariosPUCRS = { "AB", "CD", "E", "FG", "HI", "JK", "LM", "NP" };
        int indice = 0;
        for (; indice < horariosPUCRS.Length; indice++)
        {
            if (horarioInicial.Equals(horariosPUCRS[indice]))
                break;
        }

        int tam = horariosPUCRS.Length - indice;
        string[] result = new string[tam];
        for (int i = 0; i < tam; i++)
        {
            result[i] = horariosPUCRS[indice + i];
        }

        return result;
    }

    private void PopulaDDLFim()
    {
        ddlFim.DataSource = GetHorariosFuturos(ddlInicio.SelectedValue);
        ddlFim.DataBind();
    }

    protected void ckbEhRecorrente_CheckedChanged(object sender, EventArgs e)
    {
        lblResultado.Text = "";
        if (RadioButtonList1.Visible == true)
        {
            RadioButtonList1.Visible = false;
            Panel1.Visible = true;
            LimpaCampos();
        }
        else RadioButtonList1.Visible = true;
    }

    private void LimpaCampos()
    {
        txtTitulo.Text = "";
        txtData.Text = "";
        txtDataFinal.Text = "";
        txtDataFim.Text = "";
        txtResponsavel.Text = "";
        txtUnidade.Text = "";
        txtaDescricao.Text = "";
        ckbEhRecorrente.Checked = false;
        RadioButtonList1.Visible = false;
        RadioButtonList1.SelectedIndex = -1;
        pnlSelecionarDatas.Visible = false;
        Panel1.Visible = true;
        Panel2.Visible = false;
        pnlDias.Visible = false;
        seg.Checked = false;
        ter.Checked = false;
        qua.Checked = false;
        qui.Checked = false;
        sex.Checked = false;
        sab.Checked = false;
        dom.Checked = false;
        listaHorarios = null;
        grdHorarios.DataSource = null;
        grdHorarios.DataBind();
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = RadioButtonList1.SelectedIndex;
        switch (index)
        {
            case 0:
                pnlDias.Visible = false;
                pnlSelecionarDatas.Visible = true;
                Panel2.Visible = false;
                Panel1.Visible = true;
                break;

            case 1:
                pnlDias.Visible = true;
                pnlSelecionarDatas.Visible = true;
                Panel2.Visible = false;
                Panel1.Visible = true;
                break;

            case 2:
                pnlDias.Visible = false;
                pnlSelecionarDatas.Visible = false;
                Panel2.Visible = true;
                Panel1.Visible = false;
                break;
        }
    }

    protected void ddlHoraInicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulaDDLFim2();
    }

    private void PopulaDDLFim2()
    {
        ddlHoraFim.DataSource = GetHorariosFuturos(ddlHoraInicio.SelectedValue);
        ddlHoraFim.DataBind();
    }

    protected void btnAdicionar_Click1(object sender, EventArgs e)
    {
        lblResultado.Text = "";
        bool jaCadastrado = false;
        if (txtDataFim.Text != "")
        {
            string aux = txtDataFim.Text;
            DateTime dataAtual = new DateTime();
            dataAtual = DateTime.Parse(aux);
            Calendario cal = (Calendario)Session["Calendario"];
            if (dataAtual.Year < cal.Ano ||
            dataAtual.Year > cal.Ano ||
            dataAtual.Month > cal.FimG2.Month ||
            dataAtual.Month < cal.InicioG1.Month)
                lblResultado.Text = "A data não pertence ao intervalo do calendário selecionado.";
            else
            {
                HorariosEvento horarioEvento = HorariosEvento.NewHorariosEvento(dataAtual, null, ddlHoraInicio.SelectedItem.ToString(), ddlHoraFim.SelectedItem.ToString());
                listaHorarios = (IList<HorariosEvento>)Session["listaHorarios"];
                foreach (HorariosEvento horarios in listaHorarios)
                {
                    if (horarioEvento.Equals(horarios) || jaAdicionado(dataAtual, ddlHoraInicio.SelectedItem.ToString(), ddlHoraFim.SelectedItem.ToString()))
                    {
                        lblResultado.Text = "Horário já adicionado!";
                        jaCadastrado = true;
                        break;
                    }
                }
                if (!jaCadastrado)
                {
                    listaHorariosAdicionados = (List<HorariosEvento>)Session["listaHorariosAdicionados"];
                    listaHorariosAdicionados.Add(horarioEvento);
                    listaHorarios.Add(horarioEvento);
                    ((List<HorariosEvento>)listaHorarios).Sort();
                    Session["listaHorarios"] = listaHorarios;
                    Session["listaHorariosAdicionados"] = listaHorariosAdicionados;
                    grdHorarios.DataSource = listaHorarios;
                    grdHorarios.DataBind();
                    lblResultado.Text = "Horário adicionado.";
                }
            }
        }
        else
            lblResultado.Text = "Selecione uma data.";
    }

    private bool jaAdicionado(DateTime data, string horarioInicial, string horarioFinal)
    {
        listaHorarios = (IList<HorariosEvento>)Session["listaHorarios"];
        foreach (HorariosEvento horario in listaHorarios)
        {
            if (data == horario.Data)
            {
                if (horarioInicial.CompareTo(horario.HorarioInicio) == 0 || horarioFinal.CompareTo(horario.HorarioFim) == 0 ||
                    horarioInicial.CompareTo(horario.HorarioFim) == 0 || horarioFinal.CompareTo(horario.HorarioInicio) == 0)
                    return true;
                else if (horarioInicial.CompareTo(horario.HorarioInicio) == 1 && horarioFinal.CompareTo(horario.HorarioFim) == -1)
                    return true;
                else if (horarioInicial.CompareTo(horario.HorarioInicio) == -1 && horarioFinal.CompareTo(horario.HorarioFim) == 1)
                    return true;
            }
        }
        return false;
    }

    protected void grdHorarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            lblResultado.Text = "";
            listaHorarios = (IList<HorariosEvento>)Session["listaHorarios"];
            if (listaHorarios.Count > 1)
            {
                listaHorariosExcluidos = (IList<HorariosEvento>)Session["listaHorariosExcluidos"];
                listaHorariosExcluidos.Add(listaHorarios[e.RowIndex]);
                Session["listaHorariosExcluidos"] = listaHorariosExcluidos;
                listaHorarios.RemoveAt(e.RowIndex);
                Session["listaHorarios"] = listaHorarios;
                grdHorarios.DataSource = listaHorarios;
                grdHorarios.DataBind();
                lblResultado.Text = "Horário deletado.";
            }
            else
                lblResultado.Text = "Este evento deve ter no mínimo uma data.";
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

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Guid eventoId = (Guid)Session["EventoId"];
            evento = eventoBO.GetEventoById(eventoId);

            if (evento != null)
            {
                listaHorarios = (List<HorariosEvento>)Session["listaHorarios"];
                listaHorariosExcluidos = (List<HorariosEvento>)Session["listaHorariosExcluidos"];
                listaHorariosAdicionados = (List<HorariosEvento>)Session["listaHorariosAdicionados"];
                evento.Titulo = txtTitulo.Text;
                evento.Responsavel = txtResponsavel.Text;
                evento.Unidade = txtUnidade.Text;
                evento.Descricao = txtaDescricao.Text;
                eventoBO.UpdateEvento(evento);
                if (listaHorarios.Count != 0)
                {
                    foreach (HorariosEvento horario in listaHorariosAdicionados)
                    {
                        horario.EventoId = evento;
                        horariosEventoBO.InsereHorariosEvento(horario);
                    }
                    foreach (HorariosEvento horario in listaHorariosExcluidos)
                    {
                        horariosEventoBO.DeletaHorariosEvento(horario.HorariosEventoId);
                    }
                    Response.Redirect("~/Eventos/ListaEventos.aspx");
                }
                else
                    lblResultado.Text = "Nenhum horário escolhido para o evento.";
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        catch (InvalidOperationException)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + "Evento não existente.");
        }
        catch (System.Threading.ThreadAbortException)
        {
            Response.Redirect("~/Eventos/ListaEventos.aspx");
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void lbtnVoltar_Click1(object sender, EventArgs e)
    {
        Response.Redirect("~/Eventos/ListaEventosFuturos.aspx");
    }

    private void EnviarEmail(string pessoa, string descricaoEvento, DateTime data)
    {

    }

    private void EnviarEmail(string pessoa, string descricaoEvento, DateTime dataInicial, DateTime dataFinal)
    {

    }
}
