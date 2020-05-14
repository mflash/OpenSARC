using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessData.Entities;
using BusinessData.BusinessLogic;
using System.Collections.Generic;
using BusinessData.DataAccess;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Security;
using System.Net.Mail;
using System.Configuration;
using System.Web.Security;
using System.Diagnostics;
using System.Text.RegularExpressions;


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
    RecursosBO recursosBO = new RecursosBO();
    Guid dummyGuid = new Guid();
    AlocacaoBO alocBO = new AlocacaoBO();
    Calendario cal;
    private int cont = 1;  // contador de aulas (SEM feriados)
    private int cont2 = 2; // contador de aulas (incluindo feriados)
	bool facin = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack)
                return;

            if (Session["AppState"] != null && ((AppState)Session["AppState"]) == AppState.Admin)
            {
                Server.Transfer("~/Default/Erro.aspx?Erro=O sistema est� bloqueado.");
            }
            else if (Session["AppState"] != null && (AppState)Session["AppState"] != AppState.AtivoSemestre)
                Server.Transfer("~/Default/Erro.aspx?Erro=O semestre ainda n�o foi iniciado.");
            else
            {
                if (Session["Calendario"] == null)
                {
                    Response.Redirect("../Default/SelecionarCalendario.aspx");
                }
                //FIXME: falta um else aqui?
                Guid idturma = new Guid();
                if (Request.QueryString["GUID"] != null)
                {
                    try
                    {
                        idturma = new Guid(Request.QueryString["GUID"]);
                    }
                    catch (FormatException)
                    {
                        Response.Redirect("~/Default/Erro.aspx?Erro=Codigo de turma inv�lido!");
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
                        Response.Redirect("~/Default/Erro.aspx?Erro=Codigo de turma inv�lido!");
                    }
                    Disciplina d = listaAulas[0].TurmaId.Disciplina;
					CategoriaDisciplina cat = d.Categoria;
					
					// Mega gambiarra master extended++
					// TODO: retirar assim que poss�vel!
					if(cat.Descricao.IndexOf("Outras Unidades") != -1)
							facin = false;
					Session["facin"] = facin;

                    lblTitulo.Text = d.Cod + "-" + d.Cred + " " + d.Nome + ", turma " + listaAulas[0].TurmaId.Numero + " - " + Regex.Replace(listaAulas[0].TurmaId.Sala, "32/A", "32");//" "+facin;                    

                    int horasRelogioEsperadas = d.Cred * 15;
                    int durPeriodo = 45;
/*                    if (listaAulas[0].Hora == "JK" || listaAulas[0].Hora == "LM" || listaAulas[0].Hora == "NP"
                       || listaAulas[1].Hora == "JK" || listaAulas[1].Hora == "LM" || listaAulas[1].Hora == "NP"
                       || listaAulas[1].Hora == "JK" || listaAulas[2].Hora == "LM" || listaAulas[2].Hora == "NP")
                        durPeriodo = 45;*/

                    int totalAulas = 0;
                    bool emG2 = false;
                    bool haG2 = false;
                    int totalFeriados = 0;
                    foreach (Aula a in listaAulas)
                    {
                        categorias.Add(a.CategoriaAtividade.Id);
                        argb.Add(a.CategoriaAtividade.Cor);
                        if (a.Data >= cal.InicioG2)
                        {
                            //if (a.CategoriaAtividade.Descricao == "Prova de G2")                            
                            //Debug.WriteLine("EM G2");
                            emG2 = true;
                        }
                        if (a.DescricaoAtividade.StartsWith("Feriado") || a.DescricaoAtividade.StartsWith("Suspens�o"))
                            totalFeriados++;
                        if (a.CategoriaAtividade.Descricao == "Prova de G2")
                            haG2 = true;
                        if (!a.DescricaoAtividade.StartsWith("Feriado") && !a.DescricaoAtividade.StartsWith("Suspens�o")
                            && a.CategoriaAtividade.Descricao != "Prova de G2" && !emG2)
                            totalAulas++;                                                
                    }
                    // Contando mais uma aula por causa da G2 que pulamos antes
                    //if(haG2)
                    //    totalAulas++;
                    int totalEfetivo = totalAulas * 2 * durPeriodo / 60;
                    int complementares = horasRelogioEsperadas - totalEfetivo;
                    if (complementares < 0) complementares = 0;
                    //lblHoras.Text = "Dura��o do per�odo: " + durPeriodo + " - Horas esperadas: " + horasRelogioEsperadas + " - Horas efetivas: " + totalEfetivo
                    //    + " - <b>Previs�o de horas extraclasse: " + (horasRelogioEsperadas - totalEfetivo) + "</B>";
                    
//                    int minutosEsperados = horasRelogioEsperadas * 60;
                    int minutosFeriado = durPeriodo * totalFeriados * 2;
                    int minutosEsperados = durPeriodo * 2 * 18 * d.Cred/2;
                    int horasMinistradas = (minutosEsperados - minutosFeriado) / 60;
                    int extraClasse = horasRelogioEsperadas - horasMinistradas;
                    Debug.WriteLine("Minutos feriado: " + minutosFeriado);
                    Debug.WriteLine("Minutos esperados: " + minutosEsperados);                    

                    //lblHoras.Text = "Dura��o do per�odo: " + durPeriodo + " - Horas esperadas: " + horasRelogioEsperadas + " - Horas efetivas: " + horasMinistradas
                    //    + " - <b>Previs�o de horas extraclasse: " + extraClasse + "</B>";

                    lblHoras.Text = "- Horas esperadas: " + horasRelogioEsperadas + " - Horas efetivas: " + totalEfetivo
                        + " - <b>Previs�o de horas para TDE: " + complementares + "</B>";

                    dgAulas.DataSource = listaAulas;
                    dgAulas.DataBind();


                    if (Session["blocks"] == null)
                    {
                        //BusinessData.BusinessLogic.RecursosBO recursosBO = new BusinessData.BusinessLogic.RecursosBO();
                        // Monta dicion�rio com bloqueio de recursos devido a uso de outros
                        Dictionary<Guid, BusinessData.Entities.Recurso> todos = new Dictionary<Guid, BusinessData.Entities.Recurso>();
                        Dictionary<Guid, Tuple<Guid, Guid>> blocks = new Dictionary<Guid, Tuple<Guid, Guid>>();
                        List<BusinessData.Entities.Recurso> listRec = recursosBO.GetRecursos();
                        foreach (BusinessData.Entities.Recurso r in listRec)
                            todos.Add(r.Id, r);
                        foreach (BusinessData.Entities.Recurso r in listRec)
                        {
                            if (r.Bloqueia1 != Guid.Empty || r.Bloqueia2 != Guid.Empty)
                            {
                                //System.Diagnostics.Debug.WriteLine("block: " + r.Id + " -> " + r.Bloqueia1 + ", " + r.Bloqueia2);
                                blocks.Add(r.Id, new Tuple<Guid, Guid>(r.Bloqueia1, r.Bloqueia2));
                            }
                        }
                        Session["blocks"] = blocks;
                    }

                    // Gera link para HTML
                    Link1.NavigateUrl = "~/Default/Export.aspx?id=" + idturma + "&ano=" + cal.Ano + "&sem=" + cal.Semestre;
                    string navlink ="/Default/ExportIcal.aspx?id=" + idturma + "&ano=" + cal.Ano + "&sem=" + cal.Semestre;
                    Link2.NavigateUrl = "https://www.google.com/calendar/render?cid=" +
                        Server.UrlEncode("http://"+Request.Url.Host+navlink);
                    Link3.NavigateUrl = "webcal://" + Request.Url.Host + navlink;
                    Link4.NavigateUrl = "~" + navlink;

                    // Monta dicion�rio com bloqueio de recursos devido a uso de outros
                    // Movido para Global.asax (Application_Start)
                    //Dictionary<Guid, Tuple<Guid,Guid>> blocks = new Dictionary<Guid, Tuple<Guid,Guid>>();
                    //List<Recurso> listRec = recursosBO.GetRecursos();
                    //foreach (Recurso r in listRec) {
                    //    if(r.Bloqueia1 != Guid.Empty || r.Bloqueia2 != Guid.Empty)
                    //    {
                    //        //System.Diagnostics.Debug.WriteLine("block: " + r.Id + " -> " + r.Bloqueia1 + ", " + r.Bloqueia2);
                    //        blocks.Add(r.Id, new Tuple<Guid,Guid>(r.Bloqueia1, r.Bloqueia2));
                    //    }
                    //}
                    //Session["blocks"] = blocks;
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
        foreach(Data data in cal.Datas)
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
            Label lblRecursosAlocados = (Label)e.Item.FindControl("lblRecursosAlocados");
            //lblRecursosAlocados.ReadOnly = true;
            Label lblRecursosAlocadosId = (Label)e.Item.FindControl("lblRecursosAlocadosId");
            Label lblAulaId = (Label)e.Item.FindControl("lblAulaId");
            Label lblAula = (Label)e.Item.FindControl("lblAula");
            Label lblHora = (Label)e.Item.FindControl("lblHora");
			
			Panel pnRecursos = (Panel)e.Item.FindControl("pnRecursos");
            HtmlTable tabRecursos = (HtmlTable)e.Item.FindControl("tabRecursos");
            int i = tabRecursos.Rows[0].Cells[0].Controls.Count;
            CheckBoxList cbRecursos = (CheckBoxList)tabRecursos.Rows[0].Cells[0].Controls[1];

            ImageButton butDel = (ImageButton)e.Item.FindControl("butDeletar");
            ImageButton butTransf = (ImageButton)e.Item.FindControl("butTransferir");
            ImageButton butTrocar = (ImageButton)e.Item.FindControl("butTrocar");

            //CheckBoxList cbRecursos = (CheckBoxList) tabRecursos.FindControl("cbRecursos");

			//Label tmp2 = new Label();
			//tmp2.Text = "boo";
			//pnRecursos.Controls.Add(tmp2);
			//Label tmp3 = new Label();
			//tmp3.Text = "boo2";
			//pnRecursos.Controls.Add(tmp3);
			//pnRecursos.BackColor = Color.Red;
            Color cor = argb[0];

            //txtDescricao.Attributes.Add("onkeyup", "setDirtyFlag()");
            //string call = "testAlert(this," + lblAula.Text + ")";
            //txtDescricao.Attributes.Add("onkeyup", call);
            //txtDescricao.Attributes.Add("onkeyup", "this.className='changed'");

            Label lbl = (Label)e.Item.FindControl("lblAula");
            lbl.Text = "";

            listCData = cdataBo.GetCategoriaDatas();

            DateTime dataAtual = Convert.ToDateTime(lblData.Text);

            List<Recurso> livres = recursosBO.GetRecursosDisponiveis(dataAtual, lblHora.Text);
            livres.Sort();
            Recurso dummy = new Recurso();
            dummy.Descricao = "Selecionar...";
            dummy.Id = dummyGuid;
            livres.Insert(0, dummy);
            DropDownList ddlDisponiveis = (DropDownList)e.Item.FindControl("ddlDisponiveis");
            ddlDisponiveis.DataValueField = "Id";
            ddlDisponiveis.DataTextField = "Descricao";
            ddlDisponiveis.DataSource = livres;
            ddlDisponiveis.DataBind();

            ddlAtividade.DataValueField = "Id";
            ddlAtividade.DataTextField = "Descricao";
            ddlAtividade.DataSource = listaAtividades;
            ddlAtividade.DataBind();

            ddlAtividade.SelectedValue = categorias[0].ToString();

            //Data data = null;
            //verifica as datas para pintar as linhas

            // Associa a chamada da fun�ao Javascript para setar a dirty flag + trocar cor                    
            string num = cont2.ToString();
            if (cont2++ < 10)
                num = "0" + num;
            string call = "testAlert(this,'" + num + "')";
            txtDescricao.Attributes.Add("onkeyup", call);

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
                        {                      
                            if (!c.DiaLetivo)
                            {
								e.Item.BackColor = c.Cor;
								e.Item.Enabled = false;
								lblCorDaData.Text = "True";                                
                                txtDescricao.Text = c.Descricao + (txtDescricao.Text != "Feriado" ? " (era "+txtDescricao.Text+")" : "");
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
                            /*else
                            {
                                lblDescData.Text = c.Descricao;
                                txtDescricao.Text = c.Descricao + "\n" + txtDescricao.Text;
                            }*/
                        }
                }
                else
                {
                    e.Item.BackColor = cor;
                    lblCorDaData.Text = "False";
                    lbl.Text = (cont++).ToString();
                    // Associa a chamada da fun�ao Javascript para setar a dirty flag + trocar cor                    
                    /*string num = cont.ToString();
                    if (cont < 10)
                        num = "0" + num;
                    string call = "testAlert(this,'" + num + "')";
                    txtDescricao.Attributes.Add("onkeyup", call);
                     */
                }
            }

            AtualizaComponentes(e.Item, lblData.Text, lblHora.Text, lblAulaId.Text);

            /*
            */

            categorias.RemoveAt(0);
            argb.RemoveAt(0);
        }

    }

    protected void btnExportarHTML_Click(object sender, EventArgs e)
    {
        AtualizaTodaGrade();
        ExportarHtml();
    }
	
	protected void butTransferir_Click(object sender, EventArgs e)
	{
		ImageButton but = (ImageButton) sender;
		DataGridItem grid = (DataGridItem) but.Parent.Parent.Parent.Parent.Parent.Parent;
		
		Label lblData = (Label) grid.FindControl("lblData");
        Label lblHora = (Label) grid.FindControl("lblHora");
		Label lblaulaId = (Label) grid.FindControl("lblAulaId");
		
		// Salva a grade antes de selecionar os recursos
		//AtualizaTodaGrade();
		List<string> recursos = new List<string>();
		CheckBoxList cbRecursos = (CheckBoxList) grid.FindControl("cbRecursos");
		foreach(ListItem rec in cbRecursos.Items)
			recursos.Add(rec.Value);
		Session["RecursosIds"] = recursos;
		
		// abre a popup de trocar recursos
        Session["DataAula"] = lblData.Text;
        Session["Horario"] = lblHora.Text;
        string id = lblaulaId.Text;
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
            @"popitup('TransferirRecurso.aspx?AulaId=" + id + "',400,300);", true);
	}
	
	protected void butTrocar_Click(object sender, EventArgs e)
	{
		ImageButton but = (ImageButton) sender;
		DataGridItem grid = (DataGridItem) but.Parent.Parent.Parent.Parent.Parent.Parent;
		
		Label lblData = (Label) grid.FindControl("lblData");
		Label lblHora = (Label) grid.FindControl("lblHora");
		Label lblaulaId = (Label) grid.FindControl("lblAulaId");
		
		List<string> recursos = new List<string>();
		CheckBoxList cbRecursos = (CheckBoxList) grid.FindControl("cbRecursos");
		foreach(ListItem rec in cbRecursos.Items)
			recursos.Add(rec.Value);			
        Session["RecursosIds"] = recursos;
	
        // abre a popup de transferir recursos
        Session["DataAula"] = lblData.Text;
        Session["Horario"] = lblHora.Text;
        string id = lblaulaId.Text;
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
            @"popitup('TrocarRecurso.aspx?AulaId=" + id + "',350,300);",
                true);   

		// Salva a grade antes de selecionar os recursos
		AtualizaTodaGrade();		
    }

	// Salva o conteudo das linhas alteradas no BD	
	private void AtualizaTodaGrade()
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
        CheckBox cbChanged;
		ImageButton butConfirm;
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

		int totalLinhas = 0;
        for (int i = 0; i < t.Count; i++)
        {
            cbChanged = (CheckBox)t[i].FindControl("cbChanged");
			butConfirm = (ImageButton) t[i].FindControl("butConfirm");			
            // Se a linha n�o foi modificada, pula ela			
			
			// NAO FUNCIONA!
			//if(butConfirm.ImageUrl == "~/_layouts/images/STARgray.gif")
			//	continue;
			
            if (!cbChanged.Checked)
                continue;
            cbChanged.Checked = false;
			
			// NAO FUNCIONA!
			//if (!butConfirm.Enabled)
			//	continue;
			
			butConfirm.Enabled = false;
			butConfirm.ImageUrl = "~/_layouts/images/STARgray.gif";
			
			totalLinhas++;
            
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

            //FIXME: n�o deveria atualizar apenas descricao e categoria??
            aula = Aula.GetAula(idaula, turma, hora, data, descricao, categoria);

            aulaBo.UpdateAula(aula);

            txtDescricao.Text = lblDescData.Text + "\n" + descricao;
        }	
        lblResultado.Text = "Altera��o realizada com sucesso ("+totalLinhas.ToString()+" linhas)";

        // TODO: alterar nome do bot�o.
		btnSalvarTudo.Text = "Salvo";
		btnSalvarTudo.Enabled = false;
        //Button salvar = (Button)sender;
        //salvar.Text = "Salvo";
        //salvar.Enabled = false;

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
                @"releaseDirtyFlag();", true);
	}
	
    // Salva os dados de todas as aulas modificadas
    protected void btnSalvarTudo_Click(object sender, EventArgs e)
    {
		AtualizaTodaGrade();
    }

    protected void ExportarHtml()
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
        CheckBoxList cbRecursos;
        foreach (DataGridItem item in dgAulas.Items)
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
/*
            lblAux = (Label)item.FindControl("lblData2");
            dr["Data Hora"] = lblAux.Text; 
            
            lblAux = (Label)item.FindControl("lblDia2");
            dr["Data Hora"] += " "+ lblAux.Text;

            lblAux = (Label)item.FindControl("lblHora");
            dr["Data Hora"] += lblAux.Text;
            */
            txtDescricao = (TextBox)item.FindControl("txtDescricao");
            dr["Descri��o"] = txtDescricao.Text;

            ddlAtividade = (DropDownList)item.FindControl("ddlAtividade");
            dr["Atividade"] = ddlAtividade.SelectedItem.Text;

            cbRecursos = (CheckBoxList)item.FindControl("cbRecursos");
            string aux = "";
            foreach (ListItem rec in cbRecursos.Items)
                if (aux != String.Empty)
                    aux += "<br/>" + rec.Text;
                else aux = rec.Text;

            dr["Recursos"] = aux;

            dr["CorDaData"] = item.BackColor.Name;
            tabela.Rows.Add(dr);
        }
        Session["DownHtml"] = tabela;
        Response.Redirect("DownloadHtml2.aspx");
    }

    // Atualiza o dropdownlist de sele��o de recursos e o checkbox list dos selecionados,
    // para uma determinada linha da tabela
    private void AtualizaComponentes(DataGridItem grid, String dataString, String horario, String aulaString)
    {
        CheckBoxList cbRecursos = (CheckBoxList)grid.FindControl("cbRecursos");
        ImageButton butDel = (ImageButton)grid.FindControl("butDeletar");
        ImageButton butTransf = (ImageButton)grid.FindControl("butTransferir");
        ImageButton butTrocar = (ImageButton)grid.FindControl("butTrocar");

        DateTime data = Convert.ToDateTime(dataString);

        AlocacaoBO alocBO = new AlocacaoBO();
        List<Recurso> recAlocados = alocBO.GetRecursoAlocadoByAula(data, horario, new Guid(aulaString));

        cbRecursos.Items.Clear();
        if (recAlocados.Count != 0)
        {
            // Habilita bot�es
            butDel.Visible = true;
            butTransf.Visible = true;
            butTrocar.Visible = true;
            foreach (Recurso r in recAlocados)
                cbRecursos.Items.Add(new ListItem(r.Descricao, r.Id.ToString()));
        }
        else
        {
            // Desabilita bot�es
            butDel.Visible = false;
            butTransf.Visible = false;
            butTrocar.Visible = false;
        }
    }

    // Callback do dropdownlist de atividade: troca o tipo de atividade (aula, prova, etc)
	protected void ddlAtividade_SelectedIndexChanged(object sender, EventArgs e)
	{
        DropDownList ddlAtividade = (DropDownList)sender;
//        TableCell cell = (TableCell)ddlDisponiveis.Parent;
        DataGridItem grid = (DataGridItem)ddlAtividade.Parent.Parent;
		ImageButton butConfirm = (ImageButton) grid.FindControl("butConfirm");
		CheckBox cbChanged = (CheckBox) grid.FindControl("cbChanged");
		cbChanged.Checked = true; // marca para alteracao
		butConfirm.Enabled = true;		
		AtualizaTodaGrade();
	}
	
	// Atualiza uma unica linha da grade (sem uso no momento)
	private void AtualizaLinhaGrade(DataGridItem grid)
	{
        // Obtem os dados digitados nesta linha
        Label lblData = (Label) grid.FindControl("lblData");
        Label lblHora = (Label) grid.FindControl("lblHora");
        TextBox txtDescricao = (TextBox) grid.FindControl("txtDescricao");
        DropDownList ddlAtividade = (DropDownList) grid.FindControl("ddlAtividade");
        Label lblCorDaData = (Label) grid.FindControl("lblCorDaData");
        Label lblDescData = (Label) grid.FindControl("lblDescData");
        Label lblaulaId = (Label) grid.FindControl("lblAulaId");
        Label lblRecursosAlocadosId = (Label) grid.FindControl("lblRecursosAlocadosId");

        Guid idaula = new Guid(lblaulaId.Text);
        Guid idturma = (Guid)Session["TurmaId"];
        Turma turma = turmaBo.GetTurmaById(idturma);

        string hora = lblHora.Text;
        DateTime data = Convert.ToDateTime(lblData.Text);

        string aux = txtDescricao.Text;
        string descricao = aux.Substring(aux.IndexOf('\n') + 1);

        Guid idcategoria = new Guid(ddlAtividade.SelectedValue);
        CategoriaAtividade categoria = categoriaBo.GetCategoriaAtividadeById(idcategoria);

        if (grid.BackColor != Color.LightGray && lblCorDaData.Text.Equals("False"))
            grid.BackColor = categoria.Cor;

        Aula aula = Aula.GetAula(idaula, turma, hora, data, descricao, categoria);

        aulaBo.UpdateAula(aula);

        txtDescricao.Text = lblDescData.Text + "\n" + descricao;

	}
	
    // Callback do dropdownlist de sele��o: aloca um recurso e atualiza os componentes na tela
    protected void ddlDisponiveis_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FIXME: tratar poss�veis problemas de conex�o com o servidor e solicita��o de recurso indispon�vel.
        DropDownList ddlDisponiveis = (DropDownList)sender;
        string recString = ddlDisponiveis.SelectedValue;

        TableCell cell = (TableCell)ddlDisponiveis.Parent;
        DataGridItem grid = (DataGridItem)cell.Parent;

        string dataString = ((Label)grid.FindControl("lblData")).Text;
        string horario = ((Label)grid.FindControl("lblHora")).Text;
        string aulaString = ((Label)grid.FindControl("lblAulaId")).Text;

        alocar(recString, dataString, horario, aulaString);

        AtualizaComponentes(grid, dataString, horario, aulaString);

        Dictionary<Guid, Tuple<Guid, Guid>> blocks = (Dictionary<Guid, Tuple<Guid, Guid>>)Session["blocks"];
        Tuple<Guid, Guid> bloqueados = null;
        Guid key = new Guid(ddlDisponiveis.SelectedValue);
        // Remove recurso do dropdown de sele��o
        ddlDisponiveis.Items.Remove(ddlDisponiveis.Items.FindByValue(ddlDisponiveis.SelectedValue));

        // Se esse recurso bloqueia algum(s) outro(s), retira do dropdown tambem
        if (blocks.ContainsKey(key))
        {
            bloqueados = blocks[key];
            if (bloqueados.Item1 != Guid.Empty)
                ddlDisponiveis.Items.Remove(ddlDisponiveis.Items.FindByValue(bloqueados.Item1.ToString()));
            if (bloqueados.Item2 != Guid.Empty)
                ddlDisponiveis.Items.Remove(ddlDisponiveis.Items.FindByValue(bloqueados.Item2.ToString()));
        }
        ddlDisponiveis.SelectedIndex = 0;
				
		// E atualiza o BD com as alteracoes na grade
		AtualizaTodaGrade();
    }


    public void alocar(string recString, string dataString, string horario, string aulaString)
    {
        Guid recId = new Guid(recString);
        Guid aulaId = new Guid(aulaString);
        DateTime data = Convert.ToDateTime(dataString);

        //FIXME: pode ocorrer um problema se outro usu�rio selecionar o recurso antes...
        Aula aula = aulaBo.GetAulaById(aulaId);        

        Recurso rec = recursosBO.GetRecursoById(recId);
        System.Diagnostics.Debug.WriteLine("Alocando: " + rec.Descricao);
        Alocacao aloc = new Alocacao(rec, data, horario, aula, null);
        alocBO.UpdateAlocacao(aloc);

        /* Nao e' mais necessario: o DAO foi alterado para verificar isso (GetRecursosDisponiveis)
         *
        // Verifica se algum outro recurso depende deste, em caso positivo aloca tamb�m
        Dictionary<Guid, Tuple<Guid, Guid>> blocks = (Dictionary<Guid, Tuple<Guid, Guid>>) Session["blocks"];
        Tuple<Guid, Guid> bloqueados = blocks[recId];
        if(bloqueados.Item1 != Guid.Empty)
        {
            rec = recursosBO.GetRecursoById(bloqueados.Item1);
            System.Diagnostics.Debug.WriteLine("Bloq. dependente: " + rec.Descricao);
            aloc = new Alocacao(rec, data, horario, aula, null);
            alocBO.UpdateAlocacao(aloc);            
        }
        if (bloqueados.Item2 != Guid.Empty)
        {
            rec = recursosBO.GetRecursoById(bloqueados.Item2);
            System.Diagnostics.Debug.WriteLine("Bloq. dependente: " + rec.Descricao);
            aloc = new Alocacao(rec, data, horario, aula, null);
            alocBO.UpdateAlocacao(aloc);            
        }
         */
    }

    // Deleta o(s) recurso(s) selecionado(s)
    protected void butDeletar_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton butDel = (ImageButton)sender;
        
        // O checkbox list est� dentro da c�lula da tabela...
        HtmlTableCell cell = (HtmlTableCell) butDel.Parent;
        CheckBoxList cbList = (CheckBoxList)cell.FindControl("cbRecursos");

        // Para chegar no DataGridItem correspondente... bleargh!
        DataGridItem grid = (DataGridItem)cell.Parent.Parent.Parent.Parent.Parent;

        DropDownList ddlDisponiveis = (DropDownList)grid.FindControl("ddlDisponiveis");
        string dataString = ((Label)grid.FindControl("lblData")).Text;
        DateTime data = Convert.ToDateTime(dataString);
        string horario = ((Label)grid.FindControl("lblHora")).Text;
        string aulaString = ((Label)grid.FindControl("lblAulaId")).Text;

        Guid aulaId = new Guid(aulaString);
        Aula aula = aulaBo.GetAulaById(aulaId);

        // Varre o checkbox list do fim para o in�cio,
        // e remove todos os recursos selecionados (da tela e do BD)
        List<Recurso> listaRecLib = new List<Recurso>();
        for(int r=cbList.Items.Count-1; r>=0; r--)
        {
            ListItem recurso = cbList.Items[r];
            if (recurso.Selected)
            {
                Guid recId = new Guid(recurso.Value);
                Recurso rec = recursosBO.GetRecursoById(recId);
                Alocacao aloc = new Alocacao(rec, data, horario, null, null);
                alocBO.UpdateAlocacao(aloc);
                cbList.Items.RemoveAt(r);
                // TODO: melhorar isso - s� envia email se forem recursos com a palavra "lab" em algum lugar
                if(rec.Categoria.Descricao.ToLower().Contains("lab"))
                    listaRecLib.Add(rec);
            }
        }

        //ImageButton butDel = (ImageButton)grid.FindControl("butDeletar");
        ImageButton butTransf = (ImageButton)grid.FindControl("butTransferir");
        ImageButton butTrocar = (ImageButton)grid.FindControl("butTrocar");

        // Se nenhum restou, esconde bot�es
        if (cbList.Items.Count == 0)
        {
            butDel.Visible = false;
            butTransf.Visible = false;
            butTrocar.Visible = false;
        }

        // Recria o dropdownlist de recursos dispon�veis
        //ddlDisponiveis.Items.Clear();
        List<Recurso> livres = recursosBO.GetRecursosDisponiveis(data, horario);
        livres.Sort();
        Recurso dummy = new Recurso();
        dummy.Descricao = "Selecionar...";
        dummy.Id = dummyGuid;
        livres.Insert(0, dummy);
        
        ddlDisponiveis.DataValueField = "Id";
        ddlDisponiveis.DataTextField = "Descricao";
        ddlDisponiveis.DataSource = livres;
        ddlDisponiveis.DataBind();
		
		AtualizaTodaGrade();
        if(listaRecLib.Count > 0)
            EnviarEmailLiberacao("professores@inf.pucrs.br", listaRecLib, data, horario);
    }

    private void EnviarEmailLiberacao(string pessoa, List<Recurso> liberados , DateTime data, String horario)
    {
//        string para = ConfigurationManager.AppSettings["MailMessageSecretaria"];
        string from = ConfigurationManager.AppSettings["MailMessageFrom"];
        MailMessage email = new MailMessage(from, pessoa);
        email.Subject = "OpenSARC: Libera��o de recurso(s) no dia " + data.ToShortDateString() + ", hor�rio " + horario;
        email.Body = "Sistema de Aloca��o de Recursos Computacionais - FACIN \n\n" +
               "Um ou mais recursos foram liberados por " + Membership.GetUser().Email + "." +
               "\nDia: " + data.ToShortDateString() + ", hor�rio " + horario + "\n" +
               "\nRecurso(s):";
        foreach (Recurso r in liberados)
        {
            email.Body += "\n" + r.Descricao;
        }
        SmtpClient client = new SmtpClient();
        client.Send(email);
    }
}
