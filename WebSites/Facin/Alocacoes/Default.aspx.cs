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
using BusinessData.DataAccess;
using System.Collections.Generic;
using BusinessData.Distribuicao.Entities;

public partial class Alocacoes_Default : System.Web.UI.Page
{
    private CategoriaRecursoBO controladorCategorias;
    private RecursosBO controladorRecursos;
    private AlocacaoBO controladorAlocacoes;
    
    private void PopulaCategorias()
    {
        try
        {
            ddlCategorias.DataSource = controladorCategorias.GetCategoriaRecurso();
            ddlCategorias.DataValueField = "Id";
            ddlCategorias.DataTextField = "Descricao";
            ddlCategorias.DataBind();
			
			ddlCategorias2.DataSource = controladorCategorias.GetCategoriaRecurso();
            ddlCategorias2.DataValueField = "Id";
            ddlCategorias2.DataTextField = "Descricao";
            ddlCategorias2.DataBind();
        }
        catch (DataAccessException)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=Erro ao ler dados");
        }
    }

    private void PopulaRecursos()
    {
        ddlRecursos.Items.Clear();
        ddlRecursos.Items.Add("Selecione");
        try
        {
        ddlRecursos.DataSource = controladorRecursos.GetRecursosPorCategoria(
            controladorCategorias.GetCategoriaRecursoById(new Guid(ddlCategorias.SelectedValue)));
        ddlRecursos.DataValueField = "Id";
        ddlRecursos.DataTextField = "Descricao";
        ddlRecursos.DataBind();
        }
        catch (FormatException)
        {
            lblStatus.Text = "Selecione uma categoria de recurso";
        }
        catch (DataAccessException)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=Erro ao ler dados");
        }
    }

    private void PopulaProfessores()
    {
        ProfessoresBO controladorProfessores = new ProfessoresBO();
        ddlProfessores.DataSource = controladorProfessores.GetProfessores();
        ddlProfessores.DataTextField = "Nome";
        ddlProfessores.DataValueField = "Id";
        ddlProfessores.DataBind();
    }

    private void PopulaSecretarios()
    {
        SecretariosBO controladorSecretarios = new SecretariosBO();
        ddlSecretario.DataSource = controladorSecretarios.GetSecretarios();
        ddlSecretario.DataTextField = "Nome";
        ddlSecretario.DataValueField = "Id";
        ddlSecretario.DataBind();
    }
	
	private void PopulaHorarios()
	{
		ddlHorarios.DataSource = Enum.GetNames(typeof(Horarios.HorariosPUCRS));
		ddlHorarios.DataBind();
	}

    protected void Page_Load(object sender, EventArgs e)
    {
        controladorCategorias = new CategoriaRecursoBO();
        controladorRecursos = new RecursosBO();
        controladorAlocacoes = new AlocacaoBO();
        if (!IsPostBack)
        {
            PopulaCategorias();
            PopulaProfessores();
            PopulaSecretarios();
			PopulaHorarios();
        }
    }

    protected void ddlCategorias_SelectedIndexChanged(object sender, EventArgs e)
    {
            PopulaRecursos();
            lblStatus.Visible = false;
    }
	
	protected void ddlCategorias2_SelectedIndexChanged(object sender, EventArgs e)
    {
            //PopulaRecursos();
            lblStatus.Visible = false;
    }

    private void VisualizarAlocacoesRecurso()
    {
        List<Alocacao> listaAlocacoes;
        try
        {
            if (ddlCategorias.SelectedIndex != 0)
            {
                if (ddlRecursos.SelectedIndex != 0)
                {
                    Guid recursoId = new Guid(ddlRecursos.SelectedValue);
                    if (txtData.Text.Length != 0)
                    {
                        DateTime data = DateTime.Parse(txtData.Text);
                        listaAlocacoes = controladorAlocacoes.GetAlocacoes((BusinessData.Entities.Calendario)Session["Calendario"], data, recursoId);
                        if (listaAlocacoes.Count != 0)
                        {
                            dgAlocacoes.DataSource = listaAlocacoes;
                            dgAlocacoes.Visible = true;
                            dgAlocacoes.DataBind();
                            lblStatus.Visible = false;
                        }
                        else
                        {
                            lblStatus.Text = "Não existem alocações para este recurso na data selecionada.";
                            lblStatus.Visible = true;
                            dgAlocacoes.Visible = false;
                        }
                    }
                    else
                    {
                        listaAlocacoes = controladorAlocacoes.GetAlocacoesSemData((BusinessData.Entities.Calendario)Session["Calendario"], recursoId);
                        if (listaAlocacoes.Count != 0)
                        {
                            ((List<Alocacao>)listaAlocacoes).Sort();
                            dgAlocacoes.DataSource = listaAlocacoes;
                            dgAlocacoes.Visible = true;
                            dgAlocacoes.DataBind();
                            lblStatus.Visible = false;
                        }
                        else
                        {
                            lblStatus.Text = "Não existem alocações para este recurso.";
                            lblStatus.Visible = true;
                            dgAlocacoes.Visible = false;
                        }
                    }
                }
                else
                {
                    dgAlocacoes.Visible = false;
                    lblStatus.Text = "Selecione um Recurso Válido.";
                }
            }
            else
            {
                dgAlocacoes.Visible = false;
                lblStatus.Text = "Selecione uma Categoria de Recurso Válido.";
            }
        }
        catch (FormatException)
        {
            dgAlocacoes.Visible = false;
            lblStatus.Text = "Digite uma data válida!";
        }
        catch (System.Data.SqlTypes.SqlTypeException)
        {
            dgAlocacoes.Visible = false;
            lblStatus.Text = "Digite uma data válida!";
        }
    }

    protected void btnVisualizarAlocacoes_Click(object sender, EventArgs e)
    {
        switch (rblAlocacoes.SelectedValue)
        {
            case "Data":
                lblStatus.Visible = true;
                VisualizarAlocacoesData();
                break;
            case "Recurso":
                lblStatus.Visible = true;
                VisualizarAlocacoesRecurso();
                break;
            case "Professor":
                lblStatus.Visible = true;
                VisualizarAlocacoesProfessor();
                break;
            case "Secretário":
                lblStatus.Visible = true;
                VisualizarAlocacoesSecretario();
                break;
        }        
    }
    
	private void hidePanels()
	{
		pnlVisualizarPorCategoria.Visible = false;
		pnlVisualizarPorRecurso.Visible = false;
		pnlVisualizarPorProfessor.Visible = false;
        pnlVisualizarPorSecretario.Visible = false;
	}
	
    protected void rblAlocacoes_SelectedIndexChanged(object sender, EventArgs e)
    {
		hidePanels();
        switch (rblAlocacoes.SelectedValue)
        {
            case "Recurso":
				pnlVisualizarPorRecurso.Visible = true;                
                ResetaComponentes();
                lblOpcional.Visible = true;
                dgAlocacoes.Visible = false;
                break;
			case "Categoria":
				pnlVisualizarPorCategoria.Visible = true;				
                ResetaComponentes();
                lblOpcional.Visible = true;
                dgAlocacoes.Visible = false;
                break;
            case "Professor":                
                pnlVisualizarPorProfessor.Visible = true;               
                ResetaComponentes();
                lblOpcional.Visible = true;
                dgAlocacoes.Visible = false;
                break;
            case "Data":                
                pnlVisualizarPorSecretario.Visible = false;
                ResetaComponentes();
                lblOpcional.Visible = false;
                dgAlocacoes.Visible = false;
                break;
            case "Secretário":                
                pnlVisualizarPorSecretario.Visible = true;
                ResetaComponentes();
                lblOpcional.Visible = true;
                dgAlocacoes.Visible = false;
                break;
        }
    }
    
    private void VisualizarAlocacoesProfessor()
    {
        try
        {
            List<Alocacao> listaAlocacoes;
            ProfessoresBO controladorProfessores = new ProfessoresBO();
            Professor prof;
            if (ddlProfessores.SelectedIndex != 0)
            {
                if (txtData.Text.Length == 0)
                {
                    prof = (Professor)controladorProfessores.GetPessoaById(new Guid(ddlProfessores.SelectedValue));
                    listaAlocacoes = controladorAlocacoes.GetAlocacoesSemData((BusinessData.Entities.Calendario)Session["Calendario"], prof);
                    if (listaAlocacoes.Count != 0)
                    {
                        ((List<Alocacao>)listaAlocacoes).Sort();
                        dgAlocacoes.DataSource = listaAlocacoes;
                        dgAlocacoes.Visible = true;
                        dgAlocacoes.DataBind();
                        lblStatus.Visible = false;
                    }
                    else
                    {
                        lblStatus.Text = "Não existem alocações para este professor.";
                        lblStatus.Visible = true;
                        dgAlocacoes.Visible = false;
                    }
                }
                else
                {
                    prof = (Professor)controladorProfessores.GetPessoaById(new Guid(ddlProfessores.SelectedValue));
                    listaAlocacoes = controladorAlocacoes.GetAlocacoes((BusinessData.Entities.Calendario)Session["Calendario"], DateTime.Parse(txtData.Text), prof);
                    if (listaAlocacoes.Count != 0)
                    {
                        dgAlocacoes.DataSource = listaAlocacoes;
                        dgAlocacoes.Visible = true;
                        dgAlocacoes.DataBind();
                        lblStatus.Visible = false;
                    }
                    else
                    {
                        lblStatus.Text = "Não existem alocações para este professor na data selecionada.";
                        lblStatus.Visible = true;
                        dgAlocacoes.Visible = false;
                    }
                }
            }
            else
            {
                dgAlocacoes.Visible = false;
                lblStatus.Visible = true;
                lblStatus.Text = "Selecione um professor.";
            }
        }
        catch (FormatException)
        {
            dgAlocacoes.Visible = false;
            lblStatus.Text = "Digite uma data válida!";
        }
        catch (System.Data.SqlTypes.SqlTypeException)
        {
            dgAlocacoes.Visible = false;
            lblStatus.Text = "Digite uma data válida!";
        }
    }

    private void VisualizarAlocacoesSecretario()
    {
        try
        {
            List<Alocacao> listaAlocacoes;
            SecretariosBO controladorSecretarios = new SecretariosBO();
            Secretario secretario;
            if (ddlSecretario.SelectedIndex != 0)
            {
                if (txtData.Text.Length == 0)
                {
                    secretario = (Secretario)controladorSecretarios.GetPessoaById(new Guid(ddlSecretario.SelectedValue));
                    listaAlocacoes = controladorAlocacoes.GetAlocacoesSemData((BusinessData.Entities.Calendario)Session["Calendario"], secretario);
                    if (listaAlocacoes.Count != 0)
                    {
                        ((List<Alocacao>)listaAlocacoes).Sort();
                        dgAlocacoes.DataSource = listaAlocacoes;
                        dgAlocacoes.Visible = true;
                        dgAlocacoes.DataBind();
                        lblStatus.Visible = false;
                    }
                    else
                    {
                        lblStatus.Text = "Não existem alocações para este secretário.";
                        lblStatus.Visible = true;
                        dgAlocacoes.Visible = false;
                    }
                }
                else
                {
                    secretario = (Secretario)controladorSecretarios.GetPessoaById(new Guid(ddlSecretario.SelectedValue));
                    listaAlocacoes = controladorAlocacoes.GetAlocacoes((BusinessData.Entities.Calendario)Session["Calendario"], DateTime.Parse(txtData.Text), secretario);
                    if (listaAlocacoes.Count != 0)
                    {
                        dgAlocacoes.DataSource = listaAlocacoes;
                        dgAlocacoes.Visible = true;
                        dgAlocacoes.DataBind();
                        lblStatus.Visible = false;
                    }
                    else
                    {
                        lblStatus.Text = "Não existem alocações para este professor na data selecionada.";
                        lblStatus.Visible = true;
                        dgAlocacoes.Visible = false;
                    }
                }
            }
            else
            {
                dgAlocacoes.Visible = false;
                lblStatus.Visible = true;
                lblStatus.Text = "Selecione um secretário.";
            }
        }
        catch (FormatException)
        {
            dgAlocacoes.Visible = false;
            lblStatus.Text = "Digite uma data válida!";
        }
        catch (System.Data.SqlTypes.SqlTypeException)
        {
            dgAlocacoes.Visible = false;
            lblStatus.Text = "Digite uma data válida!";
        }
    }

    private void VisualizarAlocacoesData()
    {
        try
        {
            if (txtData.Text.Length != 0)
            {
                List<Alocacao> listaAlocacoes = controladorAlocacoes.GetAlocacoesByData(DateTime.Parse(txtData.Text), (BusinessData.Entities.Calendario)Session["Calendario"]);
                if (listaAlocacoes.Count != 0)
                {
                    dgAlocacoes.DataSource = listaAlocacoes;
                    dgAlocacoes.Visible = true;
                    dgAlocacoes.DataBind();
                    lblStatus.Visible = false;
                }
                else
                {
                    lblStatus.Text = "Não existem recursos alocados na data informada.";
                    lblStatus.Visible = true;
                    dgAlocacoes.Visible = false;
                }
            }
            else
            {
                lblStatus.Visible = true;
                FormatException excp = new FormatException();
                throw excp;
            }
        }
        catch (FormatException)
        {
            dgAlocacoes.Visible = false;
            lblStatus.Text = "Digite uma data válida!";
            dgAlocacoes.Visible = false;
        }
    }

    protected void dgAlocacoes_ItemDataBound(object sender, DataGridItemEventArgs e)
    { 
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lblTurmaEvento = (Label)e.Item.FindControl("lblTurmaEvento");
            Label lblDiscCod = (Label)e.Item.FindControl("lblDiscCod");
            Label lblDisc = (Label)e.Item.FindControl("lblDisc");
            Label lblResponsavel = (Label)e.Item.FindControl("lblResponsavel");
            Label lblCurso = (Label)e.Item.FindControl("lblCurso");
            
            Alocacao aloc = (Alocacao)e.Item.DataItem;

            if (aloc.Aula != null)
            {
                lblDiscCod.Text = aloc.Aula.TurmaId.Disciplina.Cod.ToString();
                lblDisc.Text = aloc.Aula.TurmaId.Disciplina.Nome + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                //lblTurmaEvento.Text = aloc.Aula.TurmaId.Numero.ToString(); ;
                lblResponsavel.Text = aloc.Aula.TurmaId.Professor.Nome;
                lblCurso.Text = aloc.Aula.TurmaId.Curso.Nome;
            }
            else 
            {
                lblDisc.Text = aloc.Evento.Titulo;
                lblResponsavel.Text = aloc.Evento.AutorId.Nome;
            }
        }
    }

    private void ResetaComponentes()
    {
        txtData.Text = "";
        lblStatus.Text = "";
        lblStatus.Visible = true;
        dgAlocacoes.Visible = false;
        ddlCategorias.SelectedIndex = 0;
		ddlCategorias2.SelectedIndex = 0;
        ddlProfessores.SelectedIndex = 0;
        ddlRecursos.SelectedIndex = 0;
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
