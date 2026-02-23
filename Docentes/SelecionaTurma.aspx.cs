using BusinessData.BusinessLogic;
using BusinessData.DataAccess;
using BusinessData.Entities;
using BusinessData.Entities.Util;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using Microsoft.ReportingServices.Diagnostics;

public partial class Docentes_SelecionaTurma : System.Web.UI.Page
{
    TrocaBO trocaBO = new TrocaBO();
    ProfessoresBO controleProfessores = new ProfessoresBO();
    TurmaBO turmaBO = new TurmaBO();
    AlocacaoBO alocBO = new AlocacaoBO();
    TransferenciaBO transBO = new TransferenciaBO();
    EventoBO eventoBO = new EventoBO();
    HorariosEventoBO horariosEventoBO = new HorariosEventoBO();
    DatasBO datasBO = new DatasBO();

    DateTime hoje = DateTime.Today;

    int hojeDiaAno, hojeDiaSemana, inicioSemanaDiaAno, fimSemanaDiaAno;

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
                    Guid professorId = new Guid(user.ProviderUserKey.ToString());

                    Professor prof = (Professor)controleProfessores.GetPessoaById(professorId);

                    List<Turma> listaTurmas = null;
                    List<Evento> listaEventos = null;

                    try
                    {
                        listaTurmas = turmaBO.GetTurmas(cal, prof);
                        listaTurmas.Sort();
                        foreach (var t in listaTurmas)
                        {
                            string laptop = "\u00a0\u00a0\u00a0\u00a0";
                            if (t.Notebook)
                                laptop = "\U0001f4bb ";
                            t.Disciplina.Nome = laptop + t.Disciplina.Nome;
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
                    }

                    try
                    {
                        listaEventos = eventoBO.GetEventos(prof.Id, cal);
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
                    }

                    VerificaTrocas();
                    VerificaTransferencias();

                    if (listaTurmas.Count == 0)
                    {
                        lblTurmas.Visible = true;
                    }
                    else
                    {
                        grvListaTurmas.DataSource = listaTurmas;
                        grvListaTurmas.DataBind();
                    }

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

                    var motd = File.ReadAllLines(Server.MapPath("motd.html"));
                    String str = String.Join("\n", motd);
                    htmlMOTD.Text = str;

                    List<Aniversario> anivers = datasBO.GetAniversarios();

                    hojeDiaAno = hoje.DayOfYear;
                    hojeDiaSemana = (int)hoje.DayOfWeek;   // Domingo => 0

                    inicioSemanaDiaAno = hojeDiaAno - hojeDiaSemana;
                    fimSemanaDiaAno = hojeDiaAno + (6 - hojeDiaSemana);

                    var aniversariosDaSemana = anivers.Where(b => TemAniversarioNaSemana(b.Aniver))
                        .OrderBy(b => b.Aniver.DayOfYear)
                        .ToList();

                    string html = "<b><h2>Aniversariantes da Semana</h2></b><table><tr><td>";
                    bool first = true;
                    int count = 0;
                    foreach (var item in aniversariosDaSemana)
                    {
                        if (!first)
                        {
                            html += ", ";
                        }
                        else
                        {
                            html += "</td></tr><tr><td>";
                            first = false;
                        }

                        string party = "";
                        if (hoje.Day == item.Aniver.Day && hoje.Month == item.Aniver.Month)
                            party = "&#x1F389;";

                        DateTime aniv = new DateTime(hoje.Year, item.Aniver.Month, item.Aniver.Day);
                        string shortDay = aniv.ToString("ddd", CultureInfo.GetCultureInfo("pt-BR"));
                        string diaMes = aniv.ToString("dd", CultureInfo.InvariantCulture);

                        html += ToProperCase(item.Nome) + party + " (" + diaMes + "/"+  shortDay + ")";
                        if (++count > 3)
                        {
                            count = 0;
                            first = true;
                        }
                    }
                    html += "</td></tr></table>";
                    htmlAniver.Text = html;
                }
            }
            else
            {
                if (Session["Calendario"] == null)
                {
                    Response.Redirect("../Default/SelecionarCalendario.aspx");
                }

                VerificaTrocas();
                VerificaTransferencias();
				butMoodle.Visible = false; // ugly hack! should check something in the DB
            }
        }

        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    public static string ToProperCase(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return fullName;

        return CultureInfo.InvariantCulture.TextInfo
            .ToTitleCase(fullName.ToLowerInvariant());
    }

    protected bool TemAniversarioNaSemana(DateTime aniver)
    {
        int diaDoAno = aniver.DayOfYear;
        int totalDiasAno = DateTime.IsLeapYear(aniver.Year) ? 366 : 365;

        if (inicioSemanaDiaAno >= 1)
            return diaDoAno >= inicioSemanaDiaAno && diaDoAno <= fimSemanaDiaAno;

        // Semana ultrapassa o final do ano
        return
            diaDoAno >= 1 && diaDoAno <= fimSemanaDiaAno ||
            diaDoAno >= totalDiasAno + inicioSemanaDiaAno && diaDoAno <= totalDiasAno;
    }

    protected void grvListaTurmas_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Guid id = (Guid)grvListaTurmas.DataKeys[e.NewEditIndex].Value;
        AppState estado = (AppState)Session["AppState"];
        if (estado == AppState.Requisicoes)
            Response.Redirect("~/Docentes/EditarAula.aspx?GUID=" + id);
        if (estado == AppState.AtivoSemestre)
            Response.Redirect("~/Docentes/EditarAulaSemestre.aspx?GUID=" + id);
    }

    protected void dgTroca_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (Session["Trocas"] != null)
            {
                List<Troca> trocas = (List<Troca>)Session["Trocas"];

                Label lblTurmaEvento = (Label)e.Item.FindControl("lblTurmaEvento");
                Label lblData = (Label)e.Item.FindControl("lblData");
                Label lblHorario = (Label)e.Item.FindControl("lblHorario");
                Label lblRecProposto = (Label)e.Item.FindControl("lblRecProposto");
                Label lblRecOferecido = (Label)e.Item.FindControl("lblRecOferecido");
                Label lblResponsavel = (Label)e.Item.FindControl("lblResponsavel");

                if (trocas[e.Item.ItemIndex].AlocacaoDesejada.Aula != null)
                    lblTurmaEvento.Text = trocas[e.Item.DataSetIndex].AlocacaoDesejada.Aula.TurmaId.Numero.ToString();
                else lblTurmaEvento.Text = trocas[e.Item.DataSetIndex].AlocacaoDesejada.Evento.Titulo;
                
                lblData.Text = trocas[e.Item.DataSetIndex].AlocacaoDesejada.Data.ToShortDateString();
                lblHorario.Text = trocas[e.Item.DataSetIndex].AlocacaoDesejada.Horario;
                lblRecProposto.Text = trocas[e.Item.DataSetIndex].AlocacaoAtual.Recurso.Descricao;
                lblRecOferecido.Text = trocas[e.Item.DataSetIndex].AlocacaoDesejada.Recurso.Descricao;
                if (trocas[e.Item.DataSetIndex].AlocacaoAtual.Aula != null)
                    lblResponsavel.Text = trocas[e.Item.DataSetIndex].AlocacaoAtual.Aula.TurmaId.Professor.Nome;
                else lblResponsavel.Text = trocas[e.Item.DataSetIndex].AlocacaoAtual.Evento.AutorId.Nome;

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
        Guid professorId = new Guid(user.ProviderUserKey.ToString());

        try
        {
            trocasAulas = trocaBO.GetTrocasAulasByProfessor(professorId, cal);
            trocasEventos = trocaBO.GetTrocasEventosByAutor(professorId, cal);
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

        foreach (Troca t in trocasEventos)
        {
            trocasAulas.Add(t);
        }

        Session["Trocas"] = trocasAulas;
        if (trocasAulas.Count != 0)
        {
            dgTroca.DataSource = trocasAulas;
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
                Response.Redirect("~/Default/Erro.aspx?Erro=Erro ao visualizar uma transferência.");
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
                Label lblTurmaEvento = (Label)e.Item.FindControl("lblTurmaEvento");

                if (trans[e.Item.ItemIndex].EventoTransferiu == null)
                {
                    if (trans[e.Item.ItemIndex].TurmaTransferiu != null)
                    {
                        string nome = "(Desconhecido)";
                        Professor prof = trans[e.Item.ItemIndex].TurmaTransferiu.Professor;
                        if (prof != null)
                            nome = prof.Nome;
                        lblAutor.Text = nome;
                    }
                }
                else
                {
                    if (trans[e.Item.ItemIndex].EventoTransferiu.AutorId == null)
                    {
                        lblAutor.Text = "(Desconhecido)";
                    }
                    else
                    lblAutor.Text = trans[e.Item.ItemIndex].EventoTransferiu.AutorId.Nome;
                }

                if (trans[e.Item.ItemIndex].TurmaRecebeu == null)
                    lblTurmaEvento.Text = trans[e.Item.ItemIndex].EventoRecebeu.Titulo;
                else
                    lblTurmaEvento.Text = trans[e.Item.ItemIndex].TurmaRecebeu.Numero.ToString();
            }
        }
    }


    protected void dgEventos_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Horarios")
        {
            Label lblEventoId = (Label)e.Item.FindControl("lblEventoId");
            Response.Redirect("~/Common/DetalhesEvento.aspx?Evento=" + lblEventoId.Text);
        }
    }

    private void Foo()
    {
        
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
	
	protected void butMoodle_Click(object sender, EventArgs e)
	{
		Calendario cal = (Calendario)Session["Calendario"];
		MembershipUser user = Membership.GetUser();
        Guid professorId = new Guid(user.ProviderUserKey.ToString());

        Professor prof = (Professor) controleProfessores.GetPessoaById(professorId);
		
		List<Turma> listaTurmas = null;        

        try
        {
			listaTurmas = turmaBO.GetTurmas(cal, prof);
            listaTurmas.Sort();
        }
        catch (Exception ex)
        {
			Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
	
	    string para = ConfigurationManager.AppSettings["MailMessageSecretaria"];
        string de = ConfigurationManager.AppSettings["MailMessageFrom"];
		String pessoa = prof.Nome.ToUpper();
		
        MailMessage email = new MailMessage(de, para);
		email.CC.Add(new MailAddress(prof.Email));
        email.Subject = "Solicitação de áreas no Moodle - prof. "+prof.Nome;
        email.Body = "OpenSARC - Sistema de Alocação de Recursos Computacionais - FACIN \n\n" +
			   "Prezados,\n\n"+
               pessoa + " solicita a criação de áreas no Moodle para as seguintes turmas:\n\n";

		foreach(Turma t in listaTurmas)
			email.Body += t.Disciplina.Cod + "-" + t.Disciplina.Cred + " - " +
				t.Disciplina.Nome + " - " + t.Numero + "\n";

        SmtpClient client = new SmtpClient();
        client.Send(email);
		//Response.Redirect("SelecionaTurma2.aspx");
	}
}
