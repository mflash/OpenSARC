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
using System.Collections.Generic;
using BusinessData.BusinessLogic;

public partial class Secretarios_TransferirRecurso : System.Web.UI.Page
{
    RecursosBO recBO = new RecursosBO();
    EventoBO eventoBO = new EventoBO();
    TurmaBO turmaBO = new TurmaBO();
    TransferenciaBO transBO = new TransferenciaBO();
    AlocacaoBO alocBO = new AlocacaoBO();
    TrocaBO trocaBO = new TrocaBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["AppState"] != null && ((AppState)Session["AppState"]) == AppState.Admin)
            {
                Server.Transfer("~/Default/Erro.aspx?Erro=O sistema está bloqueado.");
            }
            else
            {
                if (Session["Calendario"] == null)
                {
                    Response.Redirect("../Default/SelecionarCalendario.aspx");
                }

                if (Request.QueryString["EventoId"] != String.Empty)
                {
                    Guid eventoId = new Guid(Request.QueryString["EventoId"]);
                    Session["EventoId"] = eventoId;

                    DateTime data = Convert.ToDateTime(Session["Data"]);
                    string horario = (string)Session["Horario"];

                    lblDataHorario.Text = "Data: " + data.ToShortDateString() + " Horário: " + horario;
                    string recursosIds = (string)Session["RecursosIds"];

                    if (recursosIds != "")
                    {
                        string[] recIds = recursosIds.Split(',');

                        List<Recurso> recursosAtuais = new List<Recurso>();

                        foreach (string s in recIds)
                        {
                            try
                            {
                                recursosAtuais.Add(recBO.GetRecursoById(new Guid(s)));
                            }
                            catch (Exception)
                            {
                                Response.Redirect("~/Default/Erro.aspx?Erro=Código do recurso inválido!");
                            }
                        }
                        foreach (Recurso r in recursosAtuais)
                        {
                            ListItem li = new ListItem(r.Descricao, r.Id.ToString());
                            rblRecursos.Items.Add(li);
                        }

                        
                        MembershipUser usr = Membership.GetUser();
                        Guid requerente = new Guid(usr.ProviderUserKey.ToString());

                        List<PessoaBase> listaResponsaveis = transBO.GetResponsaveisByDataHora(horario, data, requerente);

                        if (listaResponsaveis.Count != 0)
                        {
                            ddlResponsavel.DataSource = listaResponsaveis;
                            ddlResponsavel.DataTextField = "Nome";
                            ddlResponsavel.DataValueField = "Id";
                            ddlResponsavel.DataBind();
                            ddlResponsavel.Items.Insert(0, "Selecione");
                        }
                        else
                        {
                            lblStatus.Text = "Nenhuma pessoa disponível para transferência nesta data e horario!";
                            ddlResponsavel.Visible = false;
                            lblResponsavel.Visible = false;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Você não tem recursos para transferir!";
                        ddlResponsavel.Visible = false;
                        lblResponsavel.Visible = false;
                        lblRectrans.Visible = false;
                    }
                
                }
            }
        }
    }

    protected void ddlResponsavel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlResponsavel.SelectedIndex == 0)
        {
            EscondeComponentes();
            lblStatus.Text = "";
        }
        else
        {
            EscondeComponentes();
            Calendario cal = (Calendario)Session["Calendario"];
            Guid respId = new Guid(ddlResponsavel.SelectedValue);
            DateTime data = Convert.ToDateTime(Session["Data"]);
            string horario = (string)Session["Horario"];

            PessoaFactory pFac = PessoaFactory.GetInstance();
            PessoaBase responsavel = pFac.CreatePessoa(respId);
            List<Turma> listaTurmas = null;
            List<Evento> listaEventos = null;


            if (responsavel is Professor)
            {
                listaTurmas = turmaBO.GetTurmas(cal, respId, data, horario);
                listaEventos = eventoBO.GetEventos(respId, data, horario);
                Session["Turmas"] = listaTurmas;
                Session["Eventos"] = listaEventos;
            }
            else
            {
                listaEventos = eventoBO.GetEventos(respId, data, horario);
                Session["Eventos"] = listaEventos;
            }
            if (listaTurmas != null)
            {
                if (listaTurmas.Count != 0)
                {
                    dgTurmas.DataSource = listaTurmas;
                    dgTurmas.DataBind();
                    dgTurmas.Visible = true;
                    lblTurmas.Visible = true;
                }
            }

            if (listaEventos != null)
            {
                if (listaEventos.Count != 0)
                {
                    dgEventos.DataSource = listaEventos;
                    dgEventos.DataBind();
                    dgEventos.Visible = true;
                    lblEventos.Visible = true;
                }
            }
            
        }
    }

    protected void dgTurmas_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            List<Turma> listaTurmas = null;
            if (Session["Turmas"] != null)
                listaTurmas = (List<Turma>)Session["Turmas"];
            
            Label lblAulaId = (Label)e.Item.FindControl("lblAulaId");

            DateTime data = Convert.ToDateTime(Session["Data"]);
            string horario = (string)Session["Horario"];

            AulaBO aulaBO = new AulaBO();
            Aula aula = aulaBO.GetAula(listaTurmas[e.Item.DataSetIndex].Id, data, horario);

            lblAulaId.Text = aula.Id.ToString();

        }
    }

    protected void dgTurmas_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Transferir")
        {
            if (rblRecursos.SelectedValue != "")
            {
                DateTime data = Convert.ToDateTime(Session["Data"]);
                Guid eventoId = new Guid(Session["EventoId"].ToString());
                string horario = (string)Session["Horario"];

                Guid recId = new Guid(rblRecursos.SelectedValue);
                List<Troca> listaTrocas = trocaBO.GetNaoVisualizadasByEvento(eventoId, data, horario);

                bool controle = false;
                //verifica se o recurso n esta envolvido em alguma troca
                if (listaTrocas.Count != 0)
                {
                    foreach (Troca t in listaTrocas)
                    {
                        if (t.AlocacaoAtual.Recurso.Id == recId || t.AlocacaoDesejada.Recurso.Id == recId)
                            controle = true;
                    }

                }
                if (!controle)
                {
                    Alocacao aloc = alocBO.GetAlocacao(recId, data, horario);

                    Label lblAulaId = (Label)e.Item.FindControl("lblAulaId");
                    Label lblTurmaId = (Label)e.Item.FindControl("lblTurmaId");

                    Turma turmaRecebeu = turmaBO.GetTurmaById(new Guid(lblTurmaId.Text));
                    Turma turmaTrans = null;

                    Guid eventoTransId = new Guid(Session["EventoId"].ToString());
                    Evento eventoTrans = eventoBO.GetEventoById(eventoTransId);
                    Evento eventoRec = null;

                    Transferencia trans = new Transferencia(Guid.NewGuid(), aloc.Recurso, data, horario, turmaRecebeu, turmaTrans, false, eventoRec, eventoTrans);

                    aloc.Horario = horario;

                    AulaBO aulaBO = new AulaBO();
                    aloc.Aula = aulaBO.GetAulaById(new Guid(lblAulaId.Text));
                    aloc.Evento = null;

                    alocBO.UpdateAlocacao(aloc);

                    transBO.InsereTransferencia(trans);

                    FechaJanela();
                }
                else
                {
                    lblStatus.Text = "Este recurso não pode ser transferido por estar envolvido numa troca.";
                    ddlResponsavel.SelectedIndex = 0;
                    dgTurmas.Visible = false;
                }
            }
            else
            {
                lblStatus.Text = "Selecione um recurso para efetuar a transferência";
            }
        }
    }

    private void FechaJanela()
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "fecha", "window.close()", true);
    }

    protected void btnFinalizar_Click(object sender, EventArgs e)
    {
        FechaJanela();
    }

    protected void dgEventos_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Transferir")
        {
            if (rblRecursos.SelectedValue != "")
            {
                DateTime data = Convert.ToDateTime(Session["Data"]);
                Guid aulaId = new Guid(Session["EventoId"].ToString());
                string horario = (string)Session["Horario"];

                Guid recId = new Guid(rblRecursos.SelectedValue);
                List<Troca> listaTrocas = trocaBO.GetNaoVisualizadasByEvento(aulaId, data, horario);

                bool controle = false;
                //verifica se o recurso n esta envolvido em alguma troca
                if (listaTrocas.Count != 0)
                {
                    foreach (Troca t in listaTrocas)
                    {
                        if (t.AlocacaoAtual.Recurso.Id == recId || t.AlocacaoDesejada.Recurso.Id == recId)
                            controle = true;
                    }

                }
                if (!controle)
                {
                    Alocacao aloc = alocBO.GetAlocacao(recId, data, horario);

                    Label lblEventoId = (Label)e.Item.FindControl("lblEventoId");

                    Turma turmaRecebeu = null;
                    Turma turmaTrans = null;

                    Guid eventoTransId = new Guid(Session["EventoId"].ToString());
                    Evento eventoTrans = eventoBO.GetEventoById(eventoTransId);
                    Evento eventoRec = eventoBO.GetEventoById(new Guid(lblEventoId.Text));

                    Transferencia trans = new Transferencia(Guid.NewGuid(), aloc.Recurso, data, horario, turmaRecebeu, turmaTrans, false, eventoRec, eventoTrans);

                    aloc.Horario = horario;
                    aloc.Evento = eventoRec;
                        
                    alocBO.UpdateAlocacao(aloc);

                    transBO.InsereTransferencia(trans);

                    FechaJanela();
                }
                else
                {
                    lblStatus.Text = "Este recurso não pode ser transferido por estar envolvido numa troca.";
                    ddlResponsavel.SelectedIndex = 0;
                    dgEventos.Visible = false;
                }
            }
            else
            {
                lblStatus.Text = "Selecione um recurso para efetuar a transferência";
            }
        }
    }

    protected void EscondeComponentes()
    {
        dgTurmas.Visible = false;
        dgEventos.Visible = false;
        lblEventos.Visible = false;
        lblTurmas.Visible = false;
    }
}
