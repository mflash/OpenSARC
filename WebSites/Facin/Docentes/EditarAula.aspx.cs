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
using System.Drawing;
using System.IO;


public partial class Docentes_EditarAula : System.Web.UI.Page
{
    AulaBO aulaBo = new AulaBO();
    TurmaBO turmaBo = new TurmaBO();
    CategoriaDataBO cdataBo = new CategoriaDataBO();
    RequisicoesBO reqBo = new RequisicoesBO();
    CategoriaAtividadeBO categoriaBo = new CategoriaAtividadeBO();
    List<Guid> categorias = new List<Guid>();
    List<Color> argb = new List<Color>();
    List<CategoriaData> listCData = new List<CategoriaData>();
    List<CategoriaAtividade> listaAtividades = new List<CategoriaAtividade>();
    Calendario cal;
    private int cont = 1;


    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                if (Session["AppState"] != null && ((AppState)Session["AppState"]) == AppState.Admin)
                {
                    Server.Transfer("~/Default/Erro.aspx?Erro=O sistema está bloqueado.");
                }
                else if ((AppState)Session["AppState"] != AppState.Requisicoes)
                    Server.Transfer("~/Default/Erro.aspx?Erro=Os recursos já foram distribuídos.");
                else
                {
                    if (Session["Calendario"] == null)
                    {
                        Response.Redirect("../Default/SelecionarCalendario.aspx");
                    }
                    Guid idturma = new Guid();
                    if (Request.QueryString["GUID"] != null)
                    {
                        try
                        {
                            idturma = new Guid(Request.QueryString["GUID"]);
                        }
                        catch (FormatException)
                        {
                            Response.Redirect("~/Default/Erro.aspx?Erro=Codigo de turma inválido!");
                        }
                        Session["TurmaId"] = idturma;
                        cal = (Calendario)Session["Calendario"];

                        CategoriaAtividadeBO cateBO = new CategoriaAtividadeBO();
                        listaAtividades = cateBO.GetCategoriaAtividade();
                        AulaBO AulaBO = new AulaBO();
                        List<Aula> listaAulas = null;
                        try
                        {
                            listaAulas = AulaBO.GetAulas(idturma);
                        }
                        catch (Exception)
                        {
                            Response.Redirect("~/Default/Erro.aspx?Erro=Codigo de turma inválido!");
                        }

                        lblTitulo.Text = listaAulas[0].TurmaId.Disciplina.Nome + " Turma-" + listaAulas[0].TurmaId.Numero;

                        foreach (Aula a in listaAulas)
                        {
                            categorias.Add(a.CategoriaAtividade.Id);
                            argb.Add(a.CategoriaAtividade.Cor);
                        }

                        dgAulas.DataSource = listaAulas;
                        
                        dgAulas.DataBind();
                    }
                }
            }
        }
        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }

    protected bool VerificaData(DateTime dt)
    {
        bool achou = false;
        int i = 0;

        while ((achou == false) && (i < cal.Datas.Count))
        {
            if (dt == cal.Datas[i].Date)
                achou = true;
            i++;
        }

        return achou;

    }

    protected void dgAulas_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            DropDownList ddlAtividade = (DropDownList)e.Item.FindControl("ddlAtividade");
            Label lblData = (Label)e.Item.FindControl("lblData");
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
            Label lblDescData = (Label)e.Item.FindControl("lblDescData");
            Label lblCorDaData = (Label)e.Item.FindControl("lblCorDaData");
            Label lblRecurosSelecionados = (Label)e.Item.FindControl("lblRecurosSelecionados");
            Label lblAulaId = (Label)e.Item.FindControl("lblAulaId");
            Color cor = argb[0];

            listCData = cdataBo.GetCategoriaDatas();
            List<Requisicao> listReq = reqBo.GetRequisicoesPorAula(new Guid(lblAulaId.Text), cal);

            string recuros = "";

            foreach (Requisicao r in listReq)
                recuros += r.CategoriaRecurso.Descricao + ", ";

            lblRecurosSelecionados.Text = recuros;

            DateTime dataAtual = Convert.ToDateTime(lblData.Text);

            ddlAtividade.DataValueField = "Id";
            ddlAtividade.DataTextField = "Descricao";
            ddlAtividade.DataSource = listaAtividades;
            ddlAtividade.DataBind();

            ddlAtividade.SelectedValue = categorias[0].ToString();

            Data data = null;
            //verifica as data para pintar as linhas
            if ((dataAtual >= cal.InicioG2))
            {
                e.Item.BackColor = Color.LightGray;
            }
            else if (VerificaData(dataAtual))
            {
                foreach (Data d in cal.Datas)
                    if (d.Date == dataAtual)
                        data = d;
                foreach (CategoriaData c in listCData)
                    if (c.Id == data.Categoria.Id)
                        if (!c.DiaLetivo)
                        {
                            e.Item.BackColor = c.Cor;
                            e.Item.Enabled = false;
                            txtDescricao.Text = c.Descricao;
                            lblCorDaData.Text = "True";
                        }
                        else
                        {
                            lblDescData.Text = c.Descricao;
                            txtDescricao.Text = c.Descricao + "\n" + txtDescricao.Text;
                            e.Item.BackColor = c.Cor;
                            lblCorDaData.Text = "True";
                        }
            }
            else
            {
                e.Item.BackColor = cor;
                lblCorDaData.Text = "False";
            }

            categorias.RemoveAt(0);
            argb.RemoveAt(0);

            Label lbl = (Label)e.Item.FindControl("lblAula");
            lbl.Text = (cont++).ToString();
        }

    }

    protected void dgAulas_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            //salva dados digitados antes de selecionar os recursos
            Label lblData = (Label)e.Item.FindControl("lblData");
            Label lblHora = (Label)e.Item.FindControl("lblHora");
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
            DropDownList ddlAtividade = (DropDownList)e.Item.FindControl("ddlAtividade");
            Label lblCorDaData = (Label)e.Item.FindControl("lblCorDaData");
            Label lblDescData = (Label)e.Item.FindControl("lblDescData");
            Label lblaulaId = (Label)e.Item.FindControl("lblAulaId");

            Guid idaula = new Guid(lblaulaId.Text);
            Guid idturma = (Guid)Session["TurmaId"];
            Turma turma = turmaBo.GetTurmaById(idturma);

            string hora = lblHora.Text;
            DateTime data = Convert.ToDateTime(lblData.Text);

            string aux = txtDescricao.Text;
            string descricao = aux.Substring(aux.IndexOf('\n') + 1);

            Guid idcategoria = new Guid(ddlAtividade.SelectedValue);
            CategoriaAtividade categoria = categoriaBo.GetCategoriaAtividadeById(idcategoria);

            if (e.Item.BackColor != Color.LightGray && lblCorDaData.Text.Equals("False"))
                e.Item.BackColor = categoria.Cor;


            Aula aula = Aula.GetAula(idaula, turma, hora, data, descricao, categoria);

            aulaBo.UpdateAula(aula);

            txtDescricao.Text = lblDescData.Text + "\n" + descricao;

            // abre a popup de selecao de recursos
            string id = lblaulaId.Text;
           
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "onClick", "popitup('SelecaoRecursos.aspx?AulaId=" + id + "');", true);
        }
        if (e.CommandName == "Salvar")
        {

            try
            {
                
                Label lblaulaId = (Label)e.Item.FindControl("lblAulaId");
                Label lblData = (Label)e.Item.FindControl("lblData");
                Label lblHora = (Label)e.Item.FindControl("lblHora");
                TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
                DropDownList ddlAtividade = (DropDownList)e.Item.FindControl("ddlAtividade");
                Label lblCorDaData = (Label)e.Item.FindControl("lblCorDaData");
                Label lblDescData = (Label)e.Item.FindControl("lblDescData");

                Guid idaula = new Guid(lblaulaId.Text);
                Guid idturma = (Guid)Session["TurmaId"];
                Turma turma = turmaBo.GetTurmaById(idturma);

                string hora = lblHora.Text;
                DateTime data = Convert.ToDateTime(lblData.Text);

                string aux = txtDescricao.Text;
                string descricao = aux.Substring(aux.IndexOf('\n') + 1);

                Guid idcategoria = new Guid(ddlAtividade.SelectedValue);
                CategoriaAtividade categoria = categoriaBo.GetCategoriaAtividadeById(idcategoria);

                if (e.Item.BackColor != Color.LightGray && lblCorDaData.Text.Equals("False"))
                    e.Item.BackColor = categoria.Cor;


                Aula aula = Aula.GetAula(idaula, turma, hora, data, descricao, categoria);

                aulaBo.UpdateAula(aula);

                txtDescricao.Text = lblDescData.Text + "\n" + descricao;
                lblResultado.Text = "Alteração realizada com sucesso!";

            }
            catch (Exception ex)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
            }
        }
    }

    protected void btnSalvarTudo_Click(object sender, EventArgs e)
    {
        SalvarTodos();        
    }

    protected void SalvarTodos()
    {
        DataGridItemCollection t = dgAulas.Items;
        Label lblAulaId;
        Label lblAula;
        Label lblData;
        Label lblHora;
        Label lblCorDaData;
        TextBox txtDescricao;
        Label lblDescData;
        DropDownList ddlAtividade;
        string hora;
        string aux;
        string descricao;
        DateTime data;
        Guid idcategoria;
        Guid idaula;
        CategoriaAtividade categoria;
        Aula aula;

        Guid idturma = (Guid)Session["TurmaId"];
        Turma turma = turmaBo.GetTurmaById(idturma);

        for (int i = 0; i < t.Count; i++)
        {
            lblAulaId = (Label)t[i].FindControl("lblAulaId");
            lblAula = (Label)t[i].FindControl("lblAula");
            lblData = (Label)t[i].FindControl("lblData");
            lblHora = (Label)t[i].FindControl("lblHora");
            txtDescricao = (TextBox)t[i].FindControl("txtDescricao");
            ddlAtividade = (DropDownList)t[i].FindControl("ddlAtividade");
            lblCorDaData = (Label)t[i].FindControl("lblCorDaData");
            lblDescData = (Label)t[i].FindControl("lblDescData");

            idaula = new Guid(lblAulaId.Text);
            hora = lblHora.Text;
            data = Convert.ToDateTime(lblData.Text);
            aux = txtDescricao.Text;
            descricao = aux.Substring(aux.IndexOf('\n') + 1);

            idcategoria = new Guid(ddlAtividade.SelectedValue);
            categoria = categoriaBo.GetCategoriaAtividadeById(idcategoria);

            if (t[i].BackColor != Color.LightGray && lblCorDaData.Text.Equals("False"))
                t[i].BackColor = categoria.Cor;


            aula = Aula.GetAula(idaula, turma, hora, data, descricao, categoria);

            aulaBo.UpdateAula(aula);

            txtDescricao.Text = lblDescData.Text + "\n" + descricao;
        }

        lblResultado.Text = "Alteração realizada com sucesso!";
    }


    protected void btnExportarHTML_Click(object sender, EventArgs e)
    {
        ExportarHtml();
    }

    private void ExportarHtml()
    {
        DataTable tabela = new DataTable();

        foreach (DataGridColumn coluna in dgAulas.Columns)
        {
            tabela.Columns.Add(coluna.HeaderText);
        }
        tabela.Columns.Add("Recursos");

        DataRow dr;
        Label lblAux; 
        TextBox txtDescricao;
        DropDownList ddlAtividade;
        foreach(DataGridItem item in dgAulas.Items)
        {
            dr = tabela.NewRow();
            lblAux = (Label)item.FindControl("lblAula");
            dr["Aula"] = lblAux.Text;

            lblAux = (Label)item.FindControl("lblDia");
            dr["Dia"] = lblAux.Text;

            lblAux = (Label)item.FindControl("lblData");
            dr["Data"] = lblAux.Text;

            lblAux = (Label)item.FindControl("lblHora");
            dr["Hora"] = lblAux.Text;

            txtDescricao = (TextBox)item.FindControl("txtDescricao");
            dr["Descrição"] = txtDescricao.Text;

            ddlAtividade = (DropDownList)item.FindControl("ddlAtividade");
            dr["Atividade"] = ddlAtividade.SelectedItem.Text;

            lblAux = (Label)item.FindControl("lblRecurosSelecionados");
            dr["Recursos"] = lblAux.Text;

            dr["CorDaData"] = item.BackColor.Name;
            tabela.Rows.Add(dr);
        }

        
        Session["DownHtml"] = tabela;
        Response.Redirect("DownloadHtml.aspx");
    }

}
    
    
