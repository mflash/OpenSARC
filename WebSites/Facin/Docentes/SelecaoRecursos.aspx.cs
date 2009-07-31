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
using AjaxControlToolkit;
using BusinessData.Entities.Decorator;

public partial class Docentes_SelecaoRecursos : System.Web.UI.Page
{
    private List<DecoratorRequisicoes> listaRequisicoes;
    private Calendario calendarioAtual;
    private Aula aulaAtual;
    private bool limparSessao = false;

    private void PopulaDDLCategoriaRecursos()
    {
        CategoriaRecursoBO controleCategorias = new CategoriaRecursoBO();
        ddlCategoriaRecurso.DataSource = controleCategorias.GetCategoriaRecurso();
        ddlCategoriaRecurso.DataTextField = "Descricao";
        ddlCategoriaRecurso.DataValueField = "Id";
        ddlCategoriaRecurso.DataBind();
    }

    private void GetRequisicoes()
    {

        RequisicoesBO controleRequisicoes = new RequisicoesBO();
        IList<Requisicao> requisicoesExistentes = controleRequisicoes.GetRequisicoesPorAula(aulaAtual.Id, calendarioAtual);

        foreach (Requisicao req in requisicoesExistentes)
        {
            DecoratorRequisicoes.EstadoRequisicao estado = DecoratorRequisicoes.EstadoRequisicao.Original;
            listaRequisicoes.Add(new DecoratorRequisicoes(req, estado));

            if (req.Prioridade == Convert.ToInt32(ddlPrioridadeRequisicao.SelectedValue))
            {
                
                ddlCategoriaRecurso.SelectedValue = req.CategoriaRecurso.Id.ToString();
            }
            if (req.Prioridade > ddlPrioridadeRequisicao.Items.Count)
            {
                if (!estado.ToString().Equals("Removida"))
                    AdicionaOpcao();
            }
        }
        Session.Add("listaRequisicoes", listaRequisicoes);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Opcoes"] = 0;
            if (Session["Calendario"] == null)
            {
                Response.Redirect("../Default/SelecionarCalendario.aspx");
            }
            listaRequisicoes = new List<DecoratorRequisicoes>();
            if (Request.QueryString["AulaId"] != String.Empty)
            {
                try
                {
                    AulaBO controleAulas = new AulaBO();
                    aulaAtual = controleAulas.GetAulaById(new Guid(Request.QueryString["AulaId"]));
                    
                    Session.Add("aulaAtual", aulaAtual);
                }
                catch (Exception ex)
                {
                    Response.Redirect("../Docentes/SelecionaTurma.aspx");
                }
            }
             PopulaDDLCategoriaRecursos();
            GetRequisicoes();
            listaRequisicoes = Session["listaRequisicoes"] as List<DecoratorRequisicoes>;
            Session["Opcoes"] = listaRequisicoes.Count;
        }
        calendarioAtual = (Calendario)Session["Calendario"];
        listaRequisicoes = Session["listaRequisicoes"] as List<DecoratorRequisicoes>;
        aulaAtual = Session["aulaAtual"] as Aula;
        limparSessao = false;
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        RequisicoesBO req = new RequisicoesBO();
        foreach (DecoratorRequisicoes dec in listaRequisicoes)
        {
            switch (dec.EstadoAtual)
            {
                case DecoratorRequisicoes.EstadoRequisicao.Inserida:
                    req.InsereRequisicao(dec);
                    break;
                case DecoratorRequisicoes.EstadoRequisicao.Removida:
                    req.DeletaRequisicao(dec.IdRequisicao);
                    break;
                case DecoratorRequisicoes.EstadoRequisicao.Atualizada:
                    req.UpdateRequisicoes(dec);
                    break;
            }
        }
        limparSessao = true;
        FechaJanela();
    }

    protected void ddlRequisicoes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!btnConfirmar.Enabled && ddlCategoriaRecurso.SelectedIndex != 0)
            btnConfirmar.Enabled = true;
        foreach (DecoratorRequisicoes dec in listaRequisicoes)
        {
            if (dec.EstadoAtual != DecoratorRequisicoes.EstadoRequisicao.Removida)
            {
                if (dec.Prioridade == Convert.ToInt32(ddlPrioridadeRequisicao.SelectedValue))
                {
                    ddlCategoriaRecurso.SelectedValue = dec.CategoriaRecurso.Id.ToString();
                    break;
                }
                else ddlCategoriaRecurso.SelectedIndex = 0;
            }
            
        }
        lblStatus.Text = "";
    }

    protected void btnNovaOpcao_Click(object sender, EventArgs e)
    {
        int opcoes = (int)Session["Opcoes"];
        if (opcoes >= ddlPrioridadeRequisicao.Items.Count)
        {
            if (AdicionaOpcao())
            {
                ddlCategoriaRecurso.SelectedIndex = 0;
                ddlPrioridadeRequisicao.SelectedIndex = ddlPrioridadeRequisicao.Items.Count - 1;
            }
        }
        else
        {
            lblStatus.Text = "Nenhuma opção pode estar sem categoria.";
        }
    }

    private bool AdicionaOpcao()
    {
        int opcoes = (int)Session["Opcoes"];
        if (opcoes < ddlCategoriaRecurso.Items.Count)
        {
            int valor = ddlPrioridadeRequisicao.Items.Count + 1;
            ListItem lsti = new ListItem(valor + "ª Opção", valor.ToString());
            ddlPrioridadeRequisicao.Items.Add(lsti);
            lblStatus.Text = "";
            return true;
        }
        else
        {
            lblStatus.Text = "Não é possível adicinar mais opções.";
            return false;
        }
    }

    protected void ControleRecursos_Load(object sender, EventArgs e)
    {

    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        limparSessao = true;
        FechaJanela();
    }

    protected void btnAdicionar_Click(object sender, EventArgs e)
    {

    }

    private void BloqueiaAdicionar()
    {

    }

    protected void btnRemover_Click(object sender, EventArgs e)
    {
        DecoratorRequisicoes dec = listaRequisicoes.Find(ProcuraRecurso);
        if (dec != null)
        {
            //Compara se dec já está no banco ou não
            if (dec.EstadoOriginal != DecoratorRequisicoes.EstadoRequisicao.Original)
            {
                //se não está, dec é removido da lista e os próximos items tem a prioridade
                //diminuida em 1
                listaRequisicoes.Remove(dec);
                int aux = (int)Session["Opcoes"]; 
                aux--;
                Session["Opcoes"] = aux;
                for (int i = dec.Prioridade - 1; i < listaRequisicoes.Count; i++)
                {
                    listaRequisicoes[i].Prioridade--;
                }
            }
            else
            {
                //se sim seta o estado para removida, para remover no confirmar
                int aux = (int)Session["Opcoes"];
                aux--;
                Session["Opcoes"] = aux;
                dec.EstadoAtual = DecoratorRequisicoes.EstadoRequisicao.Removida;
                for (int i = dec.Prioridade; i < listaRequisicoes.Count; i++)
                {
                    if (listaRequisicoes[i].EstadoAtual != DecoratorRequisicoes.EstadoRequisicao.Removida)
                    {
                        listaRequisicoes[i].Prioridade--;
                        if (listaRequisicoes[i].EstadoOriginal != DecoratorRequisicoes.EstadoRequisicao.Inserida)
                            listaRequisicoes[i].EstadoAtual = DecoratorRequisicoes.EstadoRequisicao.Atualizada;
                    }
                }
            }
            //foreach para setar o item da ddl categoria recurso
            foreach (DecoratorRequisicoes decor in listaRequisicoes)
            {
                if (decor.EstadoAtual != DecoratorRequisicoes.EstadoRequisicao.Removida)
                {
                    ddlCategoriaRecurso.SelectedValue = decor.CategoriaRecurso.Id.ToString();
                    break;
                }
                ddlCategoriaRecurso.SelectedIndex = 0;
            }
            //retira uma opção da ddl prioridade requisicao
            if (ddlPrioridadeRequisicao.Items.Count != 1)
            {
                ddlPrioridadeRequisicao.Items.RemoveAt(ddlPrioridadeRequisicao.Items.Count - 1);
                ddlPrioridadeRequisicao.SelectedIndex = ddlPrioridadeRequisicao.SelectedIndex - 1;
            }
            else
                ddlCategoriaRecurso.SelectedIndex = 0;
            lblStatus.Text = "Recurso removido com sucesso.";
        }
        else lblStatus.Text = "Selecione um recurso cadastrado para ser removido.";

    }

    private bool ProcuraRecurso(DecoratorRequisicoes req)
    {
        if (!ddlCategoriaRecurso.SelectedItem.Text.Equals("Selecione"))
        {
            Guid id = new Guid(ddlCategoriaRecurso.SelectedValue);
            return req.CategoriaRecurso.Id.Equals(id) ? true : false;
        }
        return false;
    }

    public override void Dispose()
    {
        if (limparSessao)
        {
            Session.Remove("listaRequisicoes");
            Session.Remove("aulaAtual");
        }
        base.Dispose();
    }

    private void FechaJanela()
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "fecha", "window.close()", true);
    }

    protected void ddlCategoriaRecurso_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCategoriaRecurso.SelectedIndex != 0)
        {
            if (!btnConfirmar.Enabled)
                btnConfirmar.Enabled = true;
            bool jaInserida = false;
            lblStatus.Text = "";
            CategoriaRecursoBO controladorCategorias = new CategoriaRecursoBO();
            Guid id = new Guid(ddlCategoriaRecurso.SelectedValue);

            CategoriaRecurso categoria = controladorCategorias.GetCategoriaRecursoById(id);
            Requisicao req = Requisicao.NewRequisicao(aulaAtual, categoria, Convert.ToInt32(ddlPrioridadeRequisicao.SelectedValue));
            DecoratorRequisicoes recComEstado = new DecoratorRequisicoes(req, DecoratorRequisicoes.EstadoRequisicao.Inserida);
            //foreach para comparar se Recurso já existe na lista
            foreach (DecoratorRequisicoes dec in listaRequisicoes)
            {
                if (dec.CategoriaRecurso.Equals(recComEstado.CategoriaRecurso) && dec.EstadoAtual != DecoratorRequisicoes.EstadoRequisicao.Removida)
                {
                    lblStatus.Text = "Recurso já selecionado.";
                    if (listaRequisicoes.Count < ddlPrioridadeRequisicao.SelectedIndex + 1)
                        ddlCategoriaRecurso.SelectedIndex = 0;
                    else
                    {
                        ddlCategoriaRecurso.SelectedValue = listaRequisicoes[ddlPrioridadeRequisicao.SelectedIndex].CategoriaRecurso.Id.ToString();
                    }
                    jaInserida = true;
                }

            }
            if (!jaInserida)
            {
                foreach (DecoratorRequisicoes dec in listaRequisicoes)
                {
                    //Compara se estado atual da categoria é igual à removida
                    if (dec.EstadoAtual == DecoratorRequisicoes.EstadoRequisicao.Removida)
                    {
                        continue;
                    }
                    //Compara se a prioridade da categoria para inserir é igual a da categoria da lista e se a categoria da lista é diferente de Inserida
                    if (dec.Prioridade == recComEstado.Prioridade && dec.EstadoOriginal != DecoratorRequisicoes.EstadoRequisicao.Inserida)
                    {
                        dec.CategoriaRecurso = controladorCategorias.GetCategoriaRecursoById(id);
                        dec.EstadoAtual = DecoratorRequisicoes.EstadoRequisicao.Atualizada;
                        lblStatus.Text = "Categoria alterada para a opção.";
                        jaInserida = true;
                        break;
                    }
                    //Compara se a prioridade da categoria para inserir é igual a da categoria da lista e se a categoria da lista é igual a Inserida
                    if (dec.Prioridade == recComEstado.Prioridade && dec.EstadoOriginal == DecoratorRequisicoes.EstadoRequisicao.Inserida)
                    {
                        dec.CategoriaRecurso = controladorCategorias.GetCategoriaRecursoById(id);
                        lblStatus.Text = "Categoria alterada para a opção.";
                        jaInserida = true;
                        break;
                    }
                }
            }
            //Insere se a categoria não estiver na lista
            if (!jaInserida)
            {
                int aux = (int)Session["Opcoes"];
                aux++;
                Session["Opcoes"] = aux;
                listaRequisicoes.Add(recComEstado);
                lblStatus.Text = "Categoria selecionada para a opção.";
                
            }
        }
        else
        {
            //foreach para percorrer a lista para a ddl de categorias voltar para a categoria
            //atual quando selecionada a opção "Selecione"
            foreach (DecoratorRequisicoes dec in listaRequisicoes)
            {
                if (dec.Prioridade == (ddlPrioridadeRequisicao.SelectedIndex + 1))
                {
                    ddlCategoriaRecurso.SelectedValue = dec.CategoriaRecurso.Id.ToString();
                    break;
                }
            }
        }
    }
}
