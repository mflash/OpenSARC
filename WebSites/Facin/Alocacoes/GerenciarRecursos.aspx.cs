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

public partial class Alocacoes_GerenciarRecursos : System.Web.UI.Page
{
    TurmaBO turmaBO = new TurmaBO();
    EventoBO eventoBO = new EventoBO();
    ProfessoresBO profBO = new ProfessoresBO(); 
    HorariosEventoBO heBO = new HorariosEventoBO();
    AulaBO aulaBO = new AulaBO();
    AlocacaoBO alocBO = new AlocacaoBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["Calendario"] == null)
            {
                Response.Redirect("../Default/SelecionarCalendario.aspx");
            }
            List<PessoaBase> pessoas = profBO.GetProfessoresESecretarios();

            ddlProfAutor.DataSource = pessoas;
            ddlProfAutor.DataTextField = "Nome";
            ddlProfAutor.DataValueField = "Id";
            ddlProfAutor.DataBind();
            ddlProfAutor.Items.Insert(0, "Selecione");

            ddlTurmas.Items.Add("Selecione");
            ddlEventos.Items.Add("Selecione");
        }
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }

    protected void ddlProfAutor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProfAutor.SelectedIndex != 0)
        {
            Calendario cal = (Calendario)Session["Calendario"];
            Guid id = new Guid(ddlProfAutor.SelectedValue);

            PessoaBase pessoa = PessoaFactory.GetInstance().CreatePessoa(id);

            List<Turma> turmas = new List<Turma>();
            List<Evento> eventos = new List<Evento>();

            if (pessoa is Professor)
            {
                turmas = turmaBO.GetTurmas(cal, pessoa.Id);

                ddlTurmas.DataSource = turmas;
                ddlTurmas.DataTextField = "Numero";
                ddlTurmas.DataValueField = "Id";
                ddlTurmas.DataBind();
                ddlTurmas.Items.Insert(0, "Selecione");

                ddlTurmas.Enabled = true;

                eventos = eventoBO.GetEventos(pessoa.Id, cal);

                List<Evento> listaEventosNaoOcorridos = new List<Evento>();
                List<HorariosEvento> listaHorariosEvento;
                foreach (Evento evento in eventos)
                {
                    listaHorariosEvento = heBO.GetHorariosEventosById(evento.EventoId);
                    if (!jaOcorreu(listaHorariosEvento))
                        listaEventosNaoOcorridos.Add(evento);
                }

                ddlEventos.DataSource = listaEventosNaoOcorridos;
                ddlEventos.DataTextField = "Titulo";
                ddlEventos.DataValueField = "EventoId";
                ddlEventos.DataBind();
                ddlEventos.Items.Insert(0, "Selecione");

                ddlEventos.Enabled = true;
            }
            else
            {
                eventos = eventoBO.GetEventos(pessoa.Id, cal);

                List<Evento> listaEventosNaoOcorridos = new List<Evento>();
                List<HorariosEvento> listaHorariosEvento;
                foreach (Evento evento in eventos)
                {
                    listaHorariosEvento = heBO.GetHorariosEventosById(evento.EventoId);
                    if (!jaOcorreu(listaHorariosEvento))
                        listaEventosNaoOcorridos.Add(evento);
                }

                ddlEventos.DataSource = listaEventosNaoOcorridos;
                ddlEventos.DataTextField = "Titulo";
                ddlEventos.DataValueField = "EventoId";
                ddlEventos.DataBind();
                ddlEventos.Items.Insert(0, "Selecione");

                ddlTurmas.Enabled = false;
                ddlEventos.Enabled = true;
            }
            dgAulas_HorariosEvento.Visible = false;
        }
        else
        {
            ddlEventos.SelectedIndex = 0;
            ddlTurmas.SelectedIndex = 0;
            ddlEventos.Enabled = false;
            ddlTurmas.Enabled = false;
            dgAulas_HorariosEvento.Visible = false;
        }
        

    }

    protected void ddlEventos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEventos.SelectedIndex != 0)
        {
            List<HorariosEvento> horariosEvento = heBO.GetHorariosEventosByIdDetalhados(new Guid(ddlEventos.SelectedValue));
            horariosEvento.Sort();
            Session["HorariosEvento"] = horariosEvento;
            Session["Aulas"] = null;
            dgAulas_HorariosEvento.DataSource = horariosEvento;
            dgAulas_HorariosEvento.DataBind();

            dgAulas_HorariosEvento.Visible = true;
            ddlTurmas.SelectedIndex = 0;
        }
        else 
        {
            dgAulas_HorariosEvento.Visible = false;    
        }
    }

    protected void ddlTurmas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTurmas.SelectedIndex != 0)
        {
            List<Aula> aulas = aulaBO.GetAulas(new Guid(ddlTurmas.SelectedValue));
            Session["Aulas"] = aulas;
            Session["HorariosEvento"] = null;
            dgAulas_HorariosEvento.DataSource = aulas;
            dgAulas_HorariosEvento.DataBind();

            dgAulas_HorariosEvento.Visible = true;
            ddlEventos.SelectedIndex = 0;
        }
        else
        {
            dgAulas_HorariosEvento.Visible = false;
        }
    }

    protected void dgAulas_HorariosEvento_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lblAulaIdEventoId = (Label)e.Item.FindControl("lblAulaIdEventoId");
            Label lblHorario = (Label)e.Item.FindControl("lblHorario");
            Label lblRecursosAlocadosId = (Label)e.Item.FindControl("lblRecursosAlocadosId");
            Label lblRecursosAlocados = (Label)e.Item.FindControl("lblRecursosAlocados");
            Label lblTipo = (Label)e.Item.FindControl("lblTipo");
            
            List<HorariosEvento> horariosEvento;
            List<Aula> aulas;
            if (Session["HorariosEvento"] != null)
            {
                horariosEvento = (List<HorariosEvento>)Session["HorariosEvento"];

                lblAulaIdEventoId.Text = horariosEvento[e.Item.ItemIndex].EventoId.EventoId.ToString();
                lblTipo.Text = "Ev";
                if (horariosEvento[e.Item.ItemIndex].HorarioInicio == "EE")
                {
                    lblHorario.Text = "E";
                }
                else lblHorario.Text = horariosEvento[e.Item.ItemIndex].HorarioInicio;


                //preeche label dos recursos alocados
                List<Recurso> recAlocados = alocBO.GetRecursoAlocadoByEvento(horariosEvento[e.Item.ItemIndex].Data, horariosEvento[e.Item.ItemIndex].HorarioInicio, new Guid(lblAulaIdEventoId.Text));

                if (recAlocados.Count != 0)
                {
                    for (int i = 0; i < recAlocados.Count - 1; i++)
                    {

                        lblRecursosAlocados.Text += recAlocados[i].Descricao + ", ";
                        lblRecursosAlocadosId.Text += recAlocados[i].Id + ",";
                    }
                    lblRecursosAlocados.Text += recAlocados[recAlocados.Count - 1].Descricao;
                    lblRecursosAlocadosId.Text += recAlocados[recAlocados.Count - 1].Id.ToString();
                }
                else lblRecursosAlocados.Text = "";
            }
            else
            {
                aulas = (List<Aula>)Session["Aulas"];

                lblAulaIdEventoId.Text = aulas[e.Item.ItemIndex].Id.ToString();
                lblTipo.Text = "Au";
                lblHorario.Text = aulas[e.Item.ItemIndex].Hora;


                //preeche label dos recursos alocados          
                List<Recurso> recAlocados = alocBO.GetRecursoAlocadoByAula(aulas[e.Item.ItemIndex].Data, lblHorario.Text, new Guid(lblAulaIdEventoId.Text));

                if (recAlocados.Count != 0)
                {
                    for (int i = 0; i < recAlocados.Count - 1; i++)
                    {

                        lblRecursosAlocados.Text += recAlocados[i].Descricao + ", ";
                        lblRecursosAlocadosId.Text += recAlocados[i].Id + ",";
                    }
                    lblRecursosAlocados.Text += recAlocados[recAlocados.Count - 1].Descricao;
                    lblRecursosAlocadosId.Text += recAlocados[recAlocados.Count - 1].Id.ToString();
                }
                else lblRecursosAlocados.Text = "";
            }
        }
    }

    private bool jaOcorreu(List<HorariosEvento> listaHorariosEvento)
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

    protected void dgAulas_HorariosEvento_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "AlocarLiberar")
        {
            Label lblHorario = (Label)e.Item.FindControl("lblHorario");
            Label lblData = (Label)e.Item.FindControl("lblData");
            Label lblAulaIdEventoId = (Label)e.Item.FindControl("lblAulaIdEventoId");
            Label lblTipo = (Label)e.Item.FindControl("lblTipo");

            string id = lblAulaIdEventoId.Text;
            Session["Horario"] = lblHorario.Text;

            // abre a popup de selecao de recursos
            if (lblTipo.Text != "Au")
            {
                Session["Data"] = lblData.Text;
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
                @"window.open('../Secretarios/Recursos.aspx?EventoId=" + id + "', '','width=350, height=220, menubar=no, resizable=no');", true);
            }
            else
            {
                Session["DataAula"] = lblData.Text;
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
                @"window.open('../AcessoProfessores/Recursos.aspx?AulaId=" + id + "', '','width=350, height=220, menubar=no, resizable=no');", true);
            }
        }
    }

    
}
