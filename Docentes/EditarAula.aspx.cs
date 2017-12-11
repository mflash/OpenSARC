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
    CategoriaRecursoBO categoriaRecursoBo = new CategoriaRecursoBO();
    List<Guid> categorias = new List<Guid>();
    List<Color> argb = new List<Color>();
    List<CategoriaData> listCData = new List<CategoriaData>();
    List<CategoriaAtividade> listaAtividades = new List<CategoriaAtividade>();
    Calendario cal;
    private int cont = 1;
    Guid dummyGuid = new Guid();
	bool facin = true;
    bool semRecursos = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        string cs = ConfigurationManager.ConnectionStrings["SARCFACINcs"].ConnectionString;
        if (cs.Contains("SARCDEV"))
        {
            semRecursos = false;            
        }
        try
        {
            if (!IsPostBack)
            {
                if (Session["AppState"] != null && ((AppState)Session["AppState"]) == AppState.Admin)
                {
                    Server.Transfer("~/Default/Erro.aspx?Erro=O sistema está bloqueado.");
                }
                //else if ((AppState)Session["AppState"] != AppState.Requisicoes)
                //    Server.Transfer("~/Default/Erro.aspx?Erro=Os recursos já foram distribuídos.");
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

                        foreach (Aula a in listaAulas)
                        {
                            categorias.Add(a.CategoriaAtividade.Id);
                            argb.Add(a.CategoriaAtividade.Cor);
                        }
						
						Disciplina disc = listaAulas[0].TurmaId.Disciplina;
						CategoriaDisciplina cat = disc.Categoria;
						//lblTitulo.text += " " + cat.Descricao;
						
						// Mega gambiarra master extended++
						// TODO: retirar assim que possível!
						if(cat.Descricao.IndexOf("Outras Unidades") != -1)
							facin = false;
						lblTitulo.Text = listaAulas[0].TurmaId.Disciplina.NomeCodCred + " - Turma " + listaAulas[0].TurmaId.Numero;//+ " " + facin;
						Session["facin"] = facin;
						
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

    protected Data VerificaData(DateTime dt)
    {
        foreach (Data data in cal.Datas)
            if (dt == data.Date)
                return data;
        return null;
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
            Label lblRecursosSelecionados = (Label)e.Item.FindControl("lblRecursosSelecionados");
            Label lblAulaId = (Label)e.Item.FindControl("lblAulaId");
            Color cor = argb[0];

            txtDescricao.Attributes.Add("onkeyup", "setDirtyFlag()");

            Label lbl = (Label)e.Item.FindControl("lblAula");
            lbl.Text = "";

            listCData = cdataBo.GetCategoriaDatas();
            List<Requisicao> listReq = reqBo.GetRequisicoesPorAula(new Guid(lblAulaId.Text), cal);                        

            
            

            DateTime dataAtual = Convert.ToDateTime(lblData.Text);

            ddlAtividade.DataValueField = "Id";
            ddlAtividade.DataTextField = "Descricao";
            ddlAtividade.DataSource = listaAtividades;
            ddlAtividade.DataBind();

            ddlAtividade.SelectedValue = categorias[0].ToString();

            List<CategoriaRecurso> listCatRecursos = categoriaRecursoBo.GetCategoriaRecursoSortedByUse();
            // listCatRecursos.Sort();
            CategoriaRecurso dummy = new CategoriaRecurso(dummyGuid, "Selecionar...");
            listCatRecursos.Insert(0, dummy);

            string recursos = "";
            foreach (Requisicao r in listReq)
            {
                if (recursos != String.Empty) recursos += "<br/>";
                recursos += r.Prioridade + ": " + r.CategoriaRecurso.Descricao;
                listCatRecursos.Remove(listCatRecursos.Find(delegate(CategoriaRecurso cr)
                {
                    return cr.Descricao == r.CategoriaRecurso.Descricao;
                }
                ));             
            }

            DropDownList ddlCategoriaRecurso = (DropDownList)e.Item.FindControl("ddlRecurso");
            if (semRecursos)
            {
                dgAulas.Columns[8].Visible = false;
                dgAulas.Columns[9].Visible = false;
                dgAulas.Columns[10].Visible = false;
                //ddlCategoriaRecurso.Visible = false;
                //lblRecursosSelecionados.Visible = false;
            }
            else
            {
                ddlCategoriaRecurso.SelectedIndex = 0;
                ddlCategoriaRecurso.DataSource = listCatRecursos;
                ddlCategoriaRecurso.DataTextField = "Descricao";
                ddlCategoriaRecurso.DataValueField = "Id";
                ddlCategoriaRecurso.DataBind();
            }

//            ddlCategoriaRecurso.Items.Remove("Laboratório");

            lblRecursosSelecionados.Text = recursos;

            //Data data = null;
            //verifica as datas para pintar as linhas
            if ((dataAtual >= cal.InicioG2))
            {
                e.Item.BackColor = Color.LightGray;
            }
            else
            {
                Data data = VerificaData(dataAtual);
                if (data != null)
                {
                    foreach (CategoriaData c in listCData)
                        if (c.Id == data.Categoria.Id)
                            if (!c.DiaLetivo)
                            {
                                e.Item.BackColor = c.Cor;
                                e.Item.Enabled = false;
                                txtDescricao.Text = c.Descricao;
                                lblCorDaData.Text = "True";
								break;
                            }
                            else
                            {
								facin = (bool) Session["facin"];
								if(facin) {
									lblDescData.Text = c.Descricao;
									txtDescricao.Text = c.Descricao;// + " "+facin; // + " - " + txtDescricao.Text;
									//txtDescricao.Text = txtDescricao.Text;
									e.Item.BackColor = c.Cor;
									lblCorDaData.Text = "True";
								}
								else {
								    e.Item.BackColor = cor;								
									lblCorDaData.Text = "False";
								}
								lbl.Text = (cont++).ToString();
								break;
                            }
                }
                else
                {
                    e.Item.BackColor = cor;
                    lblCorDaData.Text = "False";
                    lbl.Text = (cont++).ToString();
                }
            }

            categorias.RemoveAt(0);
            argb.RemoveAt(0);
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
			//txtDescricao.Text = descricao;

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

                //txtDescricao.Text = lblDescData.Text + "\n" + descricao;
				txtDescricao.Text = descricao;
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

            //txtDescricao.Text = lblDescData.Text + "\n" + descricao;
			txtDescricao.Text = descricao;
        }

        lblResultado.Text = "Alteração realizada com sucesso!";
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
                @"releaseDirtyFlag();", true);
    }


    protected void btnExportarHTML_Click(object sender, EventArgs e)
    {
        ExportarHtml();
    }

    private void ExportarHtml()
    {
        DataTable tabela = new DataTable();

		/*
		tabela.Columns.Add("#");
		tabela.Columns.Add("Dia");
		tabela.Columns.Add("Data");
		tabela.Columns.Add("Hora");
		tabela.Columns.Add("Descrição");
		tabela.Columns.Add("Atividade");		
		tabela.Columns.Add("CorDaData");
		tabela.Columns.Add("Recursos");
		*/
				        
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
            dr["#"] = lblAux.Text;

			lblAux = (Label)item.FindControl("lblDia");
			dr["Dia"] = lblAux.Text;
			
			lblAux = (Label)item.FindControl("lblData");
			dr["Data"] = lblAux.Text;
			
			lblAux = (Label)item.FindControl("lblHora");
			dr["Hora"] = lblAux.Text;
			
            //lblAux = (Label)item.FindControl("lblData2");
            //dr["Data Hora"] = lblAux.Text;

            //lblAux = (Label)item.FindControl("lblDia2");
            //dr["Data Hora"] += " " + lblAux.Text;

            //lblAux = (Label)item.FindControl("lblHora");
            //dr["Data Hora"] += lblAux.Text;

            /*lblAux = (Label)item.FindControl("lblDia");
            dr["Dia"] = lblAux.Text;

            lblAux = (Label)item.FindControl("lblData");
            dr["Data"] = lblAux.Text;

            lblAux = (Label)item.FindControl("lblHora");
            dr["Hora"] = lblAux.Text;
            */

            txtDescricao = (TextBox)item.FindControl("txtDescricao");
            dr["Descrição"] = txtDescricao.Text;

            ddlAtividade = (DropDownList)item.FindControl("ddlAtividade");
            dr["Atividade"] = ddlAtividade.SelectedItem.Text;

            lblAux = (Label)item.FindControl("lblRecursosSelecionados");
            dr["Recursos"] = lblAux.Text;

            dr["CorDaData"] = item.BackColor.Name;
            tabela.Rows.Add(dr);
        }

        
        Session["DownHtml"] = tabela;
        Response.Redirect("DownloadHtml2.aspx");
    }

    protected void ddlRecurso_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlRecurso = (DropDownList) sender;
        string recString = ddlRecurso.SelectedValue;

        TableCell cell = (TableCell) ddlRecurso.Parent;
        DataGridItem gridItem = (DataGridItem) cell.Parent;

        // Salva dados digitados

        SalvarTodos();
//        SalvaDados(gridItem);
        
        // abre a popup de selecao de recursos
        //string id = lblaulaId.Text;
        //ScriptManager.RegisterClientScriptBlock(this, GetType(), "onClick", "popitup('SelecaoRecursos.aspx?AulaId=" + id + "');", true);

        Label lblaulaId = (Label) gridItem.FindControl("lblAulaId");
        Guid idAula = new Guid(lblaulaId.Text);
        Aula aulaAtual = aulaBo.GetAulaById(idAula);

        RequisicoesBO controleRequisicoes = new RequisicoesBO();
        IList<Requisicao> requisicoesExistentes = controleRequisicoes.GetRequisicoesPorAula(idAula, cal);
        int pri = 0;
        foreach (Requisicao req in requisicoesExistentes)
            if (req.Prioridade > pri)
                pri = req.Prioridade;

        CategoriaRecursoBO controladorCategorias = new CategoriaRecursoBO();
        Guid catId = new Guid(ddlRecurso.SelectedValue);
        CategoriaRecurso categoria = controladorCategorias.GetCategoriaRecursoById(catId);
        Requisicao novaReq = Requisicao.NewRequisicao(aulaAtual, categoria, pri+1); // teste! sempre prioridade + 1

        // Insere a nova requisição
        controleRequisicoes.InsereRequisicao(novaReq);
        requisicoesExistentes.Add(novaReq);

        // Atualiza label com os recursos selecionados
        Label lblRecursosSelecionados = (Label) gridItem.FindControl("lblRecursosSelecionados");
        string recursos = "";
        foreach (Requisicao r in requisicoesExistentes)
        {
            if (recursos != String.Empty) recursos += "<br/>";
            recursos += r.Prioridade + ": " + r.CategoriaRecurso.Descricao;
        }
        lblRecursosSelecionados.Text = recursos;

        // Remove a categoria selecionada do drop down list
        ddlRecurso.Items.Remove(ddlRecurso.Items.FindByValue(ddlRecurso.SelectedValue));
        ddlRecurso.SelectedIndex = 0;
    }

    // Troca de tipo de atividade, atualiza aula e cores na tela
    protected void ddlAtividade_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAtividade = (DropDownList)sender;
        string ativString = ddlAtividade.SelectedValue;

        TableCell cell = (TableCell)ddlAtividade.Parent;
        DataGridItem gridItem = (DataGridItem)cell.Parent;

        // Salva dados digitados

        SalvarTodos();
//        SalvaDados(gridItem);
    }

    // Salva os dados da linha corrente (chamados pelos eventos de select das drop down lists, etc)
    private void SalvaDados(DataGridItem gridItem)
    {
        // Salva dados digitados

        Label lblData = (Label)gridItem.FindControl("lblData");
        Label lblHora = (Label)gridItem.FindControl("lblHora");
        TextBox txtDescricao = (TextBox)gridItem.FindControl("txtDescricao");

        DropDownList ddlAtividade = (DropDownList)gridItem.FindControl("ddlAtividade");
        Label lblCorDaData = (Label)gridItem.FindControl("lblCorDaData");
        Label lblDescData = (Label)gridItem.FindControl("lblDescData");
        Label lblaulaId = (Label)gridItem.FindControl("lblAulaId");

        Guid idaula = new Guid(lblaulaId.Text);
        Guid idturma = (Guid)Session["TurmaId"];
        Turma turma = turmaBo.GetTurmaById(idturma);

        string hora = lblHora.Text;
        DateTime data = Convert.ToDateTime(lblData.Text);

        string aux = txtDescricao.Text;
        string descricao = aux.Substring(aux.IndexOf('\n') + 1);

        Guid idcategoria = new Guid(ddlAtividade.SelectedValue);
        CategoriaAtividade categoria = categoriaBo.GetCategoriaAtividadeById(idcategoria);

        if (gridItem.BackColor != Color.LightGray && lblCorDaData.Text.Equals("False"))
            gridItem.BackColor = categoria.Cor;

        Aula aula = Aula.GetAula(idaula, turma, hora, data, descricao, categoria);
        
        aulaBo.UpdateAula(aula);        

        //txtDescricao.Text = lblDescData.Text + "\n" + descricao;
		txtDescricao.Text = descricao;
    }

}
