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
using System.Collections.Generic;
using BusinessData.Entities;

public partial class AcessoProfessores_TrocarRecurso : System.Web.UI.Page
{
    RecursosBO controleRecursos = new RecursosBO();
    AlocacaoBO alocBO = new AlocacaoBO();
    AulaBO aulaBO = new AulaBO();
    TrocaBO trocaBo = new TrocaBO();

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

                if (Request.QueryString["AulaId"] != String.Empty)
                {
                    string recursosIds = (string)Session["RecursosIds"];
                    // verifica se jah ha recursos alocados deste professor
                    if (recursosIds != "")
                    {
                        Guid aulaId = new Guid(Request.QueryString["AulaId"]);
                        Session["AulaId"] = aulaId;

                        DateTime data = Convert.ToDateTime(Session["DataAula"]);
                        string horario = (string)Session["Horario"];

                        lblDataHorario.Text = "Data: " + data.ToShortDateString() + " Horário: " + horario;
                        List<Alocacao> recAlocados = alocBO.GetRecursosAlocados(data, horario);

                        string[] recIds = recursosIds.Split(',');

                        List<Recurso> recursosAtuais = new List<Recurso>();

                        //monta lista de recursos do professor solicitante apartir da string
                        foreach (string s in recIds)
                        {
                            try
                            {
                                recursosAtuais.Add(controleRecursos.GetRecursoById(new Guid(s)));
                            }
                            catch (Exception)
                            {
                                Response.Redirect("~/Default/Erro.aspx?Erro=Código do recurso inválido!");
                            }
                        }
                        //popula o radio button list
                        foreach (Recurso r in recursosAtuais)
                        {
                            ListItem li = new ListItem(r.Descricao, r.Id.ToString());
                            rblRecursos.Items.Add(li);
                        }
                        List<Alocacao> listaRecursos = new List<Alocacao>();
                        if (recAlocados.Count != 0)
                        {
                            
                            foreach (Alocacao a in recAlocados)
                            {
                                foreach (Recurso rec in recursosAtuais)
                                {
                                    if(a.Recurso.Id.Equals(rec.Id))
                                    {
                                        listaRecursos.Add(a);
                                    }
                                }
                            }
                            //remove seus recursos para n ir pro datagrid
                            foreach (Alocacao a in listaRecursos)
                            {
                                recAlocados.Remove(a);
                            }

                            Session["RecAlocados"] = recAlocados;
                            dgRecusrosAlocados.DataSource = recAlocados;
                            dgRecusrosAlocados.DataBind();
                        }
                        else
                        {
                            dgRecusrosAlocados.Visible = false;
                            lblDgRecursos.Text = "Não existe nenhum recurso alocado neste horário.";
                            lblDgRecursos.Visible = true;
                        }
                    }
                    else
                    {
                        dgRecusrosAlocados.Visible = false;
                        lblStatus.Text = "Você não tem recursos disponíveis para trocar";
                        lblStatus.Visible = true;
                    }

                }
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

    protected void dgRecusrosAlocados_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lblTurmaEvento = (Label)e.Item.FindControl("lblTurmaEvento");
            Label lblTurmaEventoId = (Label)e.Item.FindControl("lblTurmaEventoId");
            Label lblResponsavel = (Label)e.Item.FindControl("lblResponsavel");
            
            List<Alocacao> alocacoes = (List<Alocacao>)Session["RecAlocados"];

            //verifica se o recurso esta alocado pra um evento ou aula
            //se for aula senao eh evento
            if (alocacoes[e.Item.ItemIndex].Aula != null)
            {
                lblTurmaEvento.Text = alocacoes[e.Item.ItemIndex].Aula.TurmaId.Numero.ToString();
                lblResponsavel.Text = alocacoes[e.Item.ItemIndex].Aula.TurmaId.Professor.Nome;
                lblTurmaEventoId.Text = alocacoes[e.Item.ItemIndex].Aula.Id.ToString();
            }
            else
            {
                lblTurmaEvento.Text = alocacoes[e.Item.ItemIndex].Evento.Titulo;
                lblResponsavel.Text = alocacoes[e.Item.ItemIndex].Evento.Responsavel;
                lblTurmaEventoId.Text = alocacoes[e.Item.ItemIndex].Evento.EventoId.ToString();
            }
        }
    }

    protected void dgRecusrosAlocados_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Troca")
        {
            Label lblResponsavel = (Label)e.Item.FindControl("lblResponsavel");
            Label lblTurmaEventoId = (Label)e.Item.FindControl("lblTurmaEventoId");
            Label lblRecursoId = (Label)e.Item.FindControl("lblRecursoId");

            DateTime data = Convert.ToDateTime(Session["DataAula"]);
            string horario = (string)Session["Horario"];
            Guid aulaId = new Guid(Session["AulaId"].ToString());

            Troca temTrocaRequerente = trocaBo.GetJaPropos(aulaId);
            Troca temTrocaSolicidado = trocaBo.GetJaPropos(new Guid(lblTurmaEventoId.Text));

            if (temTrocaRequerente == null && temTrocaSolicidado == null)
            {

                Aula aula = null;
                try
                {
                    aula = aulaBO.GetAulaById(aulaId);
                }
                catch (Exception)

                {
                    Response.Redirect("~/Default/Erro.aspx?Erro=Código da aula inválido!");
                }

                //monta alocacao desejada
                Guid recIdDesejado = new Guid(lblRecursoId.Text);
                Alocacao alocDesejada = alocBO.GetAlocacao(recIdDesejado, data, horario);
                    
                //monta alocacao atual
                if (rblRecursos.SelectedValue != "")
                {
                    Recurso recAtual = controleRecursos.GetRecursoById(new Guid(rblRecursos.SelectedValue));
                    Alocacao alocAtual = new Alocacao(recAtual, data, horario, aula, null);

                    Troca troca = new Troca(Guid.NewGuid(), alocAtual, alocDesejada, null, true, false,horario,data);

                    try
                    {
                        trocaBo.InsereTroca(troca);
                    }
                    catch (Exception)
                    {
                        Response.Redirect("~/Default/Erro.aspx?Erro=Proposta não pode ser cadastrada!");
                    }

                    lblStatus.Text = "A proposta de troca de recurso foi enviada para " + lblResponsavel.Text + ".";

                }
                else
                {
                    lblStatus.Text = "Selecione um recurso próprio para propor uma troca!";
                }
            }
            else
            {
                lblStatus.Text = "Existe uma troca em andamento.";
            }

        }
    }
}