using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessData.BusinessLogic;
using BusinessData.Entities;

public partial class DetalhesEvento : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;

        if (Request.QueryString["Evento"] == null)
            return;

        try
        {
            Guid idEvento = new Guid(Request.QueryString["Evento"]);
            EventoBO gerenciadorEventos = new EventoBO();
            Evento evento = gerenciadorEventos.GetEventoById(idEvento);
            lblTituloEvento.Text = evento.Titulo;
        }
        catch (ArgumentNullException)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=Evento Inválido.");
        }
        catch (FormatException)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=Evento Inválido.");
        }
        catch (OverflowException)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=Evento Inválido.");
        }
        catch (Exception ex)
        {
            Response.Redirect(
                "~/Default/Erro.aspx?Erro=Erro ao listar horários: " + ex.Message);
        }
    }

    private void BindGrid()
    {
        if (Request.QueryString["Evento"] == null)
            return;

        Guid idEvento = new Guid(Request.QueryString["Evento"]);
        HorariosEventoBO heBO = new HorariosEventoBO();

        List<HorariosEvento> horariosEvento =
            heBO.GetHorariosEventosByIdDetalhados(idEvento);

        horariosEvento.Sort();
        dgHorariosEvento.DataSource = horariosEvento;
        dgHorariosEvento.DataBind();
    }

    protected void dgHorariosEvento_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        AlocacaoBO alocBO = new AlocacaoBO();

        Label lblEventoId = (Label)e.Item.FindControl("lblEventoId");
        Label lblData = (Label)e.Item.FindControl("lblData");
        Label lblHorario = (Label)e.Item.FindControl("lblHorario");

        List<HorariosEvento> horariosEvento = (List<HorariosEvento>)dgHorariosEvento.DataSource;
        lblHorario.Text = horariosEvento[e.Item.ItemIndex].HorarioInicio;

        DateTime data = Convert.ToDateTime(lblData.Text);

        List<Recurso> recAlocados = alocBO.GetRecursoAlocadoByEvento(
            data,
            horariosEvento[e.Item.ItemIndex].HorarioInicio,
            new Guid(lblEventoId.Text));

        CheckBoxList cbRecursos = (CheckBoxList)e.Item.FindControl("cbRecursos");
        if (cbRecursos != null)
        {
            cbRecursos.Items.Clear();
            foreach (Recurso r in recAlocados)
                cbRecursos.Items.Add(new ListItem(r.Descricao, r.Id.ToString()));
        }

        // garante que o DDL tenha ao menos um item e seja renderizado/clicável
        DropDownList ddl = (DropDownList)e.Item.FindControl("ddlDisponiveis");

        if (ddl != null)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("-- Selecione um recurso --", ""));

            string valorSelecionado = Request.Form[ddl.UniqueID];

            if (!string.IsNullOrEmpty(valorSelecionado))
            {
                ddl.Items.Add(new ListItem(string.Empty, valorSelecionado));
                ddl.SelectedValue = valorSelecionado;
            }
        }
    }

    protected void ddlDisponiveis_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        DataGridItem item = (DataGridItem)ddl.NamingContainer;

        string valorSelecionado = Request.Form[ddl.UniqueID];

        if (string.IsNullOrEmpty(valorSelecionado))
            valorSelecionado = hdnRecursoSelecionado.Value;

        Guid recursoId;

        if (!Guid.TryParse(valorSelecionado, out recursoId))
        {
            lblResultado.Text = "Nenhum recurso foi selecionado.";
            lblResultado.CssClass =
                "d-block mb-3 fw-semibold text-danger";
            return;
        }

        Label lblEventoId = (Label)item.FindControl("lblEventoId");
        Label lblData = (Label)item.FindControl("lblData");
        Label lblHorario = (Label)item.FindControl("lblHorario");

        try
        {
            DateTime data = Convert.ToDateTime(lblData.Text);
            string horario = lblHorario.Text;
            Guid eventoId = new Guid(lblEventoId.Text);

            RecursosBO recBO = new RecursosBO();
            Recurso recurso = recBO.GetRecursoById(recursoId);

            EventoBO eventoBO = new EventoBO();
            Evento evento = eventoBO.GetEventoById(eventoId);

            AlocacaoBO alocBO = new AlocacaoBO();
            Alocacao alocacao =
                Alocacao.newAlocacao(recurso, data, horario, null, evento);

            alocBO.UpdateAlocacao(alocacao);

            lblResultado.Text = "Recurso alocado com sucesso.";
            lblResultado.CssClass =
                "d-block mb-3 fw-semibold text-success";
        }
        catch (Exception ex)
        {
            lblResultado.Text = "Erro ao alocar recurso: " + ex.Message;
            lblResultado.CssClass =
                "d-block mb-3 fw-semibold text-danger";
        }

        RebindGrid();
    }

    protected void butDeletar_Click(object sender, EventArgs e)
    {
        DataGridItem item =
            (DataGridItem)((LinkButton)sender).NamingContainer;

        Label lblData = (Label)item.FindControl("lblData");
        Label lblHorario = (Label)item.FindControl("lblHorario");
        CheckBoxList cbRecursos =
            (CheckBoxList)item.FindControl("cbRecursos");

        string recursoIdStr =
            Request.Form[hdnRecursoSelecionado.UniqueID];

        Guid recursoId;

        if (!Guid.TryParse(recursoIdStr, out recursoId))
        {
            recursoId = Guid.Empty;

            foreach (ListItem recursoItem in cbRecursos.Items)
            {
                if (recursoItem.Selected &&
                    Guid.TryParse(recursoItem.Value, out recursoId))
                {
                    break;
                }
            }
        }

        if (recursoId == Guid.Empty)
        {
            lblResultado.Text =
                "O recurso selecionado possui um identificador inválido.";
            lblResultado.CssClass =
                "d-block mb-3 fw-semibold text-danger";
            return;
        }

        try
        {
            DateTime data = Convert.ToDateTime(lblData.Text);
            string horario = lblHorario.Text;

            RecursosBO recursosBO = new RecursosBO();
            Recurso recurso = recursosBO.GetRecursoById(recursoId);

            Alocacao alocacao =
                new Alocacao(recurso, data, horario, null, null);

            AlocacaoBO alocBO = new AlocacaoBO();
            alocBO.UpdateAlocacao(alocacao);

            lblResultado.Text = "Recurso liberado com sucesso.";
            lblResultado.CssClass =
                "d-block mb-3 fw-semibold text-success";
        }
        catch (Exception ex)
        {
            lblResultado.Text =
                "Erro ao liberar recurso: " + ex.Message;
            lblResultado.CssClass =
                "d-block mb-3 fw-semibold text-danger";
        }

        RebindGrid();
    }

    protected void butTransferir_Click(object sender, EventArgs e)
    {
        DataGridItem item = (DataGridItem)((LinkButton)sender).NamingContainer;
        Label lblEventoId = (Label)item.FindControl("lblEventoId");
        Label lblData = (Label)item.FindControl("lblData");
        Label lblHorario = (Label)item.FindControl("lblHorario");
        CheckBoxList cbRecursos = (CheckBoxList)item.FindControl("cbRecursos");

        string recursoIdStr = null;
        foreach (ListItem li in cbRecursos.Items)
            if (li.Selected) { recursoIdStr = li.Value; break; }

        Session["Data"] = lblData.Text;
        Session["Horario"] = lblHorario.Text;
        Session["RecursosIds"] = recursoIdStr ?? string.Empty;

        string id = lblEventoId.Text;
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
            @"window.open('TransferirRecurso.aspx?EventoId=" + id + "', '','width=350, height=220, menubar=no, resizable=no');", true);
    }

    protected void butTrocar_Click(object sender, EventArgs e)
    {
        DataGridItem item = (DataGridItem)((LinkButton)sender).NamingContainer;
        Label lblEventoId = (Label)item.FindControl("lblEventoId");
        Label lblData = (Label)item.FindControl("lblData");
        Label lblHorario = (Label)item.FindControl("lblHorario");
        CheckBoxList cbRecursos = (CheckBoxList)item.FindControl("cbRecursos");

        string recursoIdStr = null;
        foreach (ListItem li in cbRecursos.Items)
            if (li.Selected) { recursoIdStr = li.Value; break; }

        Session["Data"] = lblData.Text;
        Session["Horario"] = lblHorario.Text;
        Session["RecursosIds"] = recursoIdStr ?? string.Empty;

        string id = lblEventoId.Text;
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
            @"window.open('TrocarRecurso.aspx?EventoId=" + id + "', '','width=400, height=220, menubar=no, resizable=no');", true);
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }

    [WebMethod]
    public static object ObterRecursosDisponiveis(string data, string hora)
    {
        DateTime dataConvertida = Convert.ToDateTime(data);
        RecursosBO recBO = new RecursosBO();
        List<Recurso> recursos = recBO.GetRecursosDisponiveis(dataConvertida, hora);
        return recursos
            .Where(r => r.Abrev.Trim() != "310" && r.Abrev.Trim() != "410" && r.Abrev.Trim() != "301"
                    && !r.Abrev.StartsWith("RN") && !r.Abrev.StartsWith("NB") && !r.Abrev.StartsWith("NR"))
            .OrderBy(r => r.Descricao)
            .Select(r => new { Id = r.Id, Descricao = r.Descricao })
            .ToList<object>();
    }

    private void RebindGrid()
    {
        BindGrid();

        ScriptManager.RegisterStartupScript(
            UpdatePanel1,
            UpdatePanel1.GetType(),
            "carregarRecursosDepoisDoRebind",
            "setTimeout(carregarTodosOsRecursos, 100);",
            true);
    }
}
