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
using System.Collections.Generic;
using BusinessData.Entities;
using BusinessData.BusinessLogic;
using BusinessData.DataAccess;
using System.Security;
using System.Net.Mail;

public partial class Eventos_Default : System.Web.UI.Page
{
    private IList<HorariosEvento> listaHorarios = new List<HorariosEvento>();
    private HorariosEventoBO horariosEventoBO = new HorariosEventoBO();
    private EventoBO eventoBO = new EventoBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["listaHorarios"] = listaHorarios;
            PopulaDDLHorario();
        }
    }

    public string[] GetHorariosFuturos(string horarioInicial)
    {
        string[] horariosPUCRS = { "AB", "CD", "E", "FG", "HI", "JK", "LM", "NP" };
        int indice = 0;
        for (; indice < horariosPUCRS.Length; indice++)
        {
            if(horarioInicial.Equals(horariosPUCRS[indice]))
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

    protected void ddlInicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulaDDLHorario();
    }

    protected void ddlHoraInicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulaDDLHorario();
    }

    private void PopulaDDLHorario() 
    {
        ddlFim.DataSource = GetHorariosFuturos(ddlInicio.SelectedValue);
        ddlFim.DataBind();
        ddlHoraFim.DataSource = GetHorariosFuturos(ddlHoraInicio.SelectedValue);
        ddlHoraFim.DataBind();
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

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = RadioButtonList1.SelectedIndex;
        switch (index)
        {
            case 0 :
                pnlDias.Visible = false;
                pnlSelecionarDatas.Visible = true;
                Panel2.Visible = false;
                Panel1.Visible = true;
                break;

            case 1 :
                pnlDias.Visible = true;
                pnlSelecionarDatas.Visible = true;
                Panel2.Visible = false;
                Panel1.Visible = true;
                break;

            case 2 :
                pnlDias.Visible = false;
                pnlSelecionarDatas.Visible = false;
                Panel2.Visible = true;
                Panel1.Visible = false;
                break;
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //Montar o evento
            Calendario cal = (Calendario)Session["Calendario"];
            MembershipUser usr = Membership.GetUser();
            PessoaFactory fabricaPessoas = PessoaFactory.GetInstance();
            PessoaBase pessoa = fabricaPessoas.CreatePessoa((Guid)usr.ProviderUserKey);
            Evento evento = Evento.newEvento(pessoa, txtaDescricao.Text, cal, txtResponsavel.Text, txtTitulo.Text, txtUnidade.Text);

            DateTime dataAtual;
            DateTime dataFinal;
            HorariosEvento horariosEvento;
            int index = RadioButtonList1.SelectedIndex;
            listaHorarios = (IList<HorariosEvento>)Session["listaHorarios"];
            switch (index)
            {
                #region cadastra evento diariamente
                case 0:
                    //Montar o HorariosEvento
                    dataAtual = new DateTime();
                    dataAtual = DateTime.Parse(txtData.Text);
                    dataFinal = new DateTime();
                    dataFinal = DateTime.Parse(txtDataFinal.Text);

                    //Valida datas
                    if (dataAtual > dataFinal)
                        lblResultado.Text = "Data final deve ser maior que a inicial!";
                    
                    else
                    {
                        //Insere Evento
                        eventoBO.InsereEvento(evento);

                        //Insere HorariosEvento
                        while (dataAtual <= dataFinal)
                        {
                            horariosEvento = HorariosEvento.NewHorariosEvento(dataAtual, evento, ddlInicio.Text, ddlFim.Text);
                            horariosEventoBO.InsereHorariosEvento(horariosEvento);
                            dataAtual = dataAtual.AddDays(1);
                        }

                        //Envia e-mail para a secretaria
                        dataAtual = DateTime.Parse(txtData.Text);
                        EnviarEmail(pessoa.Nome, evento.Descricao, dataAtual, dataFinal);
                        lblResultado.Text = "Evento cadastrado com sucesso.";
                        LimpaCampos();
                    }
                    break;
                #endregion

                #region cadastra evento nos dias escolhidos
                case 1 :
                    if ((seg.Checked == false) &&
                        (ter.Checked == false) &&
                        (qua.Checked == false) &&
                        (qui.Checked == false) &&
                        (sex.Checked == false) &&
                        (sab.Checked == false) &&
                        (dom.Checked == false))
                        lblResultado.Text = "Selecione pelo menos um horário.";
                    else
                    {

                        IList<HorariosEvento> horarios = new List<HorariosEvento>();

                        //Montar o HorariosEvento
                        dataAtual = new DateTime();
                        dataAtual = DateTime.Parse(txtData.Text);
                        dataFinal = new DateTime();
                        dataFinal = DateTime.Parse(txtDataFinal.Text);

                        //Valida datas
                        if (dataAtual > dataFinal)
                            lblResultado.Text = "Data final deve ser maior que a inicial!";
                        
                        else
                        {
                            //Insere Evento
                            eventoBO.InsereEvento(evento);
                            //Insere HorariosEvento
                            while (dataAtual <= dataFinal)
                            {
                                if ((dataAtual.DayOfWeek == DayOfWeek.Monday && seg.Checked == true) ||
                                    (dataAtual.DayOfWeek == DayOfWeek.Tuesday && ter.Checked == true) ||
                                    (dataAtual.DayOfWeek == DayOfWeek.Wednesday && qua.Checked == true) ||
                                    (dataAtual.DayOfWeek == DayOfWeek.Thursday && qui.Checked == true) ||
                                    (dataAtual.DayOfWeek == DayOfWeek.Friday && sex.Checked == true) ||
                                    (dataAtual.DayOfWeek == DayOfWeek.Saturday && sab.Checked == true) ||
                                    (dataAtual.DayOfWeek == DayOfWeek.Sunday && dom.Checked == true))
                                {
                                    horariosEvento = HorariosEvento.NewHorariosEvento(dataAtual, evento, ddlInicio.Text, ddlFim.Text);
                                    horarios.Add(horariosEvento);
                                    horariosEventoBO.InsereHorariosEvento(horariosEvento);
                                }
                                dataAtual = dataAtual.AddDays(1);
                            }
                            //Envia e-mail para a secretaria
                            if (horarios.Count < 2)
                                EnviarEmail(pessoa.Nome, evento.Descricao, horarios[0].Data);
                            else
                                EnviarEmail(pessoa.Nome, evento.Descricao, horarios[0].Data, horarios[horarios.Count - 1].Data);

                            lblResultado.Text = "Evento cadastrado com sucesso.";
                            LimpaCampos();
                        }
                    }
                    break;
#endregion

                #region cadastra eventos nas datas escolhidas manualmente
                case 2 :
                    if (listaHorarios.Count != 0)
                    {
                        //Insere Evento
                        eventoBO.InsereEvento(evento);

                        //Insere HorariosEvento
                        foreach (HorariosEvento horario in listaHorarios)
                        {
                            horario.EventoId = evento;
                            horariosEventoBO.InsereHorariosEvento(horario);
                        }

                        //Envia e-mail para a secretaria
                        if (listaHorarios.Count < 2)
                            EnviarEmail(pessoa.Nome, evento.Descricao, listaHorarios[0].Data);
                        else
                            EnviarEmail(pessoa.Nome, evento.Descricao, listaHorarios[0].Data, listaHorarios[listaHorarios.Count - 1].Data);
                        lblResultado.Text = "Evento cadastrado com sucesso.";
                        LimpaCampos();
                        listaHorarios = new List<HorariosEvento>();
                        
                    }
                    else
                        lblResultado.Text = "Nenhum horário escolhido para o evento.";
                    break;
                #endregion
                
                #region cadastra evento apenas em um dia
                default :
                    dataAtual = new DateTime();
                    dataAtual = DateTime.Parse(txtData.Text);
                   
                        //Insere Evento
                        eventoBO.InsereEvento(evento);

                        //Insere HorariosEvento
                        horariosEvento = HorariosEvento.NewHorariosEvento(dataAtual, evento, ddlInicio.Text, ddlFim.Text);
                        horariosEventoBO.InsereHorariosEvento(horariosEvento);

                        //Envia E-Mail
                        EnviarEmail(pessoa.Nome, evento.Descricao, dataAtual);
                        lblResultado.Text = "Evento cadastrado com sucesso.";
                        LimpaCampos();
                
                    break;
                #endregion
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
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

    protected void btnAdicionar_Click1(object sender, EventArgs e)
    {
        bool jaCadastrado = false;
        DateTime dataAtual = new DateTime();
        dataAtual = DateTime.Parse(txtDataFim.Text);
        Calendario cal = (Calendario)Session["Calendario"];
       
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
                listaHorarios.Add(horarioEvento);
                ((List<HorariosEvento>)listaHorarios).Sort();
                Session["listaHorarios"] = listaHorarios;
                grdHorarios.DataSource = listaHorarios;
                grdHorarios.DataBind();
                lblResultado.Text = "Horário adicionado.";
            }
        
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
            listaHorarios = (IList<HorariosEvento>)Session["listaHorarios"];
            listaHorarios.RemoveAt(e.RowIndex);
            Session["listaHorarios"] = listaHorarios;
            grdHorarios.DataSource = listaHorarios;
            grdHorarios.DataBind();
            lblResultado.Text = "Horário deletado.";
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

    protected void lbtnVoltar_Click1(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }

    private void EnviarEmail(string pessoa, string descricaoEvento, DateTime data)
    {
        string para = ConfigurationManager.AppSettings["MailMessageSecretaria"];
        string de = ConfigurationManager.AppSettings["MailMessageFrom"];
        MailMessage email = new MailMessage(de, para);
        email.Subject = "Alocação de Recursos";
        email.Body = "Sistema de Alocação de Recursos Computacionais  \n\n" +
               "Um novo evento foi cadastrado por " + pessoa + "." +
               "\nDescrição: " + descricaoEvento +
               "\nData do evento: " + data.ToShortDateString() +
               "\nPara mais detalhes acesse a lista de eventos.";

        SmtpClient client = new SmtpClient();
        client.Send(email);
    }

    private void EnviarEmail(string pessoa, string descricaoEvento, DateTime dataInicial, DateTime dataFinal)
    {
        string para = ConfigurationManager.AppSettings["MailMessageSecretaria"];
        string de = ConfigurationManager.AppSettings["MailMessageFrom"];
        MailMessage email = new MailMessage(de, para);
        email.Subject = "Alocação de Recursos";
        email.Body = "Sistema de Alocação de Recursos Computacionais  \n\n" +
               "Um novo evento foi cadastrado por " + pessoa + "." +
               "\nDescrição: " + descricaoEvento +
               "\nData inicial do evento: " + dataInicial.ToShortDateString() +
               "\nData final do evento: " + dataFinal.ToShortDateString() +
               "\nPara mais detalhes acesse a lista de eventos.";

        SmtpClient client = new SmtpClient();
        client.Send(email);
    }
}
