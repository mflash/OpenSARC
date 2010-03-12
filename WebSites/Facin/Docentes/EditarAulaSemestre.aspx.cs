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
    private int cont = 1;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack)
                return;

            if (Session["AppState"] != null && ((AppState)Session["AppState"]) == AppState.Admin)
            {
                Server.Transfer("~/Default/Erro.aspx?Erro=O sistema está bloqueado.");
            }
            else if (Session["AppState"] != null && (AppState)Session["AppState"] != AppState.AtivoSemestre)
                Server.Transfer("~/Default/Erro.aspx?Erro=O semestre ainda não foi iniciado.");
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
                    Disciplina d = listaAulas[0].TurmaId.Disciplina;
                    lblTitulo.Text = d.Cod + "-" + d.Cred + " " + d.Nome + ", turma " + listaAulas[0].TurmaId.Numero;

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
            Label lblHora = (Label)e.Item.FindControl("lblHora");
			
			Panel pnRecursos = (Panel)e.Item.FindControl("pnRecursos");
            HtmlTable tabRecursos = (HtmlTable)e.Item.FindControl("tabRecursos");
            int i = tabRecursos.Rows[0].Cells[0].Controls.Count;
            CheckBoxList cbRecursos = (CheckBoxList)tabRecursos.Rows[0].Cells[0].Controls[1];

            //CheckBoxList cbRecursos = (CheckBoxList) tabRecursos.FindControl("cbRecursos");

			//Label tmp2 = new Label();
			//tmp2.Text = "boo";
			//pnRecursos.Controls.Add(tmp2);
			//Label tmp3 = new Label();
			//tmp3.Text = "boo2";
			//pnRecursos.Controls.Add(tmp3);
			//pnRecursos.BackColor = Color.Red;
            Color cor = argb[0];

            txtDescricao.Attributes.Add("onkeyup", "setDirtyFlag()");

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
            DropDownList ddlOpcao1 = (DropDownList)e.Item.FindControl("ddlOpcao1");
            ddlOpcao1.DataValueField = "Id";
            ddlOpcao1.DataTextField = "Descricao";
            ddlOpcao1.DataSource = livres;
            ddlOpcao1.DataBind();


            ddlAtividade.DataValueField = "Id";
            ddlAtividade.DataTextField = "Descricao";
            ddlAtividade.DataSource = listaAtividades;
            ddlAtividade.DataBind();

            ddlAtividade.SelectedValue = categorias[0].ToString();

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
                        {
                            e.Item.BackColor = c.Cor;
                            lblCorDaData.Text = "True";
                            if (!c.DiaLetivo)
                            {
                                e.Item.Enabled = false;
                                txtDescricao.Text = c.Descricao;
                            }
                            else
                            {
                                lblDescData.Text = c.Descricao;
                                txtDescricao.Text = c.Descricao + "\n" + txtDescricao.Text;
                            }
                        }
                }
                else
                {
                    e.Item.BackColor = cor;
                    lblCorDaData.Text = "False";
                    lbl.Text = (cont++).ToString();
                }
            }

            AlocacaoBO alocBO = new AlocacaoBO();
            List<Recurso> recAlocados = alocBO.GetRecursoAlocadoByAula(dataAtual, lblHora.Text, new Guid(lblAulaId.Text));
			
            if (recAlocados.Count != 0)
            {
                //if(cbRecursos != null && cbRecursos.Items != null)
                cbRecursos.Items.Clear();
				foreach(Recurso r in recAlocados)
                //for (int i = 0; i < recAlocados.Count - 1; i++)
                {
                    //lblRecursosAlocados.Text += r.Descricao; //recAlocados[i].Descricao + ", ";
                    lblRecursosAlocadosId.Text += r.Id; //recAlocados[i].Id + ",";
					
					//Label tmpLbl = new Label();
					//tmpLbl.Text = r.Descricao; //recAlocados[i].Descricao;
					//pnRecursos.Controls.Add(tmpLbl);

                    cbRecursos.Items.Add(r.Descricao);

                    /*
                    HtmlTableRow row = new HtmlTableRow();
                    HtmlTableCell cell = new HtmlTableCell();                    
                    
                    CheckBox cbRec = new CheckBox();
                    cbRec.Text = r.Descricao;
                    cbRec.Checked = false;
                    cell.Controls.Add(cbRec);
                    row.Cells.Add(cell);
                    HtmlTableCell cell2 = new HtmlTableCell();
                    
                    //ImageButton butDelete = new ImageButton();                    
                    //butDelete.ImageUrl = "/_layouts/images/delete.gif";
                    //butDelete.ID = "imgButton1";
                    //butDelete.BorderColor = Color.Red;
                    //butDelete.Click +=new ImageClickEventHandler(butDelete_Click);
                    //butDelete.Command += new CommandEventHandler(butDelete_Command);
                    //butDelete.CommandArgument = "blablabla";

                    Button butDelete = new Button();
                    butDelete.Command += new CommandEventHandler(butDelete_Command);
                    butDelete.CommandName = "butDelete";
                    butDelete.Text = " X ";
                    cell2.Controls.Add(butDelete);
                    row.Cells.Add(cell2);
                    tabRecursos.Rows.Add(row);
                     */
                }
                //lblRecursosAlocados.Text += recAlocados[recAlocados.Count - 1].Descricao;
                //lblRecursosAlocadosId.Text += recAlocados[recAlocados.Count - 1].Id.ToString();
				//Label tmp0 = new Label();
				//tmp0.Text = recAlocados[recAlocados.Count-1].Descricao;
				//pnRecursos.Controls.Add(tmp0);
            }
            else lblRecursosAlocados.Text = "";

            categorias.RemoveAt(0);
            argb.RemoveAt(0);
        }

    }

    void butDelete_Command(object sender, CommandEventArgs e)
    {        
        //throw new NotImplementedException();
        ClientScript.RegisterClientScriptBlock(this.GetType(), "img", "<script type = 'text/javascript'>alert('ImageButton Clicked');</script>");       
    }

    void butDelete_Click(object sender, ImageClickEventArgs e)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "img", "<script type = 'text/javascript'>alert('ImageButton Clicked');</script>");       
    }

    protected void dgAulas_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        #region Select

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
            Session["DataAula"] = lblData.Text;
            Session["Horario"] = lblHora.Text;
            string id = lblaulaId.Text;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "onClick", "popitup('Recursos.aspx?AulaId=" + id + "',350,220);", true);

        }

        #endregion

        #region Trocar

        if (e.CommandName == "Trocar")
        {
            //salva dados digitados antes de selecionar os recursos
            Label lblData = (Label)e.Item.FindControl("lblData");
            Label lblHora = (Label)e.Item.FindControl("lblHora");
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
            DropDownList ddlAtividade = (DropDownList)e.Item.FindControl("ddlAtividade");
            Label lblCorDaData = (Label)e.Item.FindControl("lblCorDaData");
            Label lblDescData = (Label)e.Item.FindControl("lblDescData");
            Label lblaulaId = (Label)e.Item.FindControl("lblAulaId");
            Label lblRecursosAlocadosId = (Label)e.Item.FindControl("lblRecursosAlocadosId");

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

            Session["RecursosIds"] = lblRecursosAlocadosId.Text;

            // abre a popup de trocar de recursos
            Session["DataAula"] = lblData.Text;
            Session["Horario"] = lblHora.Text;
            string id = lblaulaId.Text;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
                @"popitup('TrocarRecurso.aspx?AulaId=" + id + "',400,300);", true);

        }

        #endregion

        #region Transferir

        if (e.CommandName == "Transferir")
        {
            //salva dados digitados antes de selecionar os recursos
            Label lblData = (Label)e.Item.FindControl("lblData");
            Label lblHora = (Label)e.Item.FindControl("lblHora");
            TextBox txtDescricao = (TextBox)e.Item.FindControl("txtDescricao");
            DropDownList ddlAtividade = (DropDownList)e.Item.FindControl("ddlAtividade");
            Label lblCorDaData = (Label)e.Item.FindControl("lblCorDaData");
            Label lblDescData = (Label)e.Item.FindControl("lblDescData");
            Label lblaulaId = (Label)e.Item.FindControl("lblAulaId");
            Label lblRecursosAlocadosId = (Label)e.Item.FindControl("lblRecursosAlocadosId");

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

            Session["RecursosIds"] = lblRecursosAlocadosId.Text;

            // abre a popup de transferir de recursos
            Session["DataAula"] = lblData.Text;
            Session["Horario"] = lblHora.Text;
            string id = lblaulaId.Text;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
                @"popitup('TransferirRecurso.aspx?AulaId=" + id + "',350,300);",
                true);

        }

        #endregion
    }

    protected void btnSalvarTudo_Click(object sender, EventArgs e)
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

            //FIXME: não deveria atualizar apenas descricao e categoria??
            aula = Aula.GetAula(idaula, turma, hora, data, descricao, categoria);

            aulaBo.UpdateAula(aula);

            txtDescricao.Text = lblDescData.Text + "\n" + descricao;
        }

        lblResultado.Text = "Alteração realizada com sucesso!";

        // TODO: alterar nome do botão.
        Button salvar = (Button)sender;
        salvar.Text = "Salvo";
        salvar.Enabled = false;

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
                @"releaseDirtyFlag();", true);

    }

    protected void btnExportarHTML_Click(object sender, EventArgs e)
    {
        ExportarHtml();
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
        TextBox ddlRecursos;
        foreach (DataGridItem item in dgAulas.Items)
        {
            dr = tabela.NewRow();
            lblAux = (Label)item.FindControl("lblAula");
            dr["#"] = lblAux.Text;

            lblAux = (Label)item.FindControl("lblData2");
            dr["Data Hora"] = lblAux.Text; 
            
            lblAux = (Label)item.FindControl("lblDia2");
            dr["Data Hora"] += " "+ lblAux.Text;

            lblAux = (Label)item.FindControl("lblHora");
            dr["Data Hora"] += lblAux.Text;

            txtDescricao = (TextBox)item.FindControl("txtDescricao");
            dr["Descrição"] = txtDescricao.Text;

            ddlAtividade = (DropDownList)item.FindControl("ddlAtividade");
            dr["Atividade"] = ddlAtividade.SelectedItem.Text;

            ddlRecursos = (TextBox)item.FindControl("lblRecursosAlocados");
            dr["Recursos"] = ddlRecursos.Text;

            dr["CorDaData"] = item.BackColor.Name;
            tabela.Rows.Add(dr);
        }
        Session["DownHtml"] = tabela;
        Response.Redirect("DownloadHtml.aspx");
    }

    protected void ddlOpcao1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FIXME: tratar possíveis problemas de conexão com o servidor e solicitação de recurso indisponível.
        DropDownList ddlOpcao1 = (DropDownList)sender;
        string recString = ddlOpcao1.SelectedValue;

        TableCell cell = (TableCell)ddlOpcao1.Parent;
        DataGridItem grid = (DataGridItem)cell.Parent;

        string dataString = ((Label)grid.FindControl("lblData")).Text;
        string horario = ((Label)grid.FindControl("lblHora")).Text;
        string aulaString = ((Label)grid.FindControl("lblAulaId")).Text;

        alocar(recString, dataString, horario, aulaString);

        Label lblRecursosAlocados = (Label)grid.FindControl("lblRecursosAlocados");
        Label lblRecursosAlocadosId = (Label)grid.FindControl("lblRecursosAlocadosId");
		
		Panel pnRecursos = (Panel)grid.FindControl("pnRecursos");			

        lblRecursosAlocados.Text = "";
        lblRecursosAlocadosId.Text = "";

        DateTime data = Convert.ToDateTime(dataString);

        AlocacaoBO alocBO = new AlocacaoBO();
        List<Recurso> recAlocados = alocBO.GetRecursoAlocadoByAula(data, horario, new Guid(aulaString));

        if (recAlocados.Count != 0)
        {
			foreach(Recurso r in recAlocados)
            //for (int i = 0; i < recAlocados.Count - 1; i++)
            {
                lblRecursosAlocados.Text += r.Descricao; //recAlocados[i].Descricao + ", ";
                lblRecursosAlocadosId.Text += r.Id; // recAlocados[i].Id + ",";
				
				Label tmpLbl = new Label();
				tmpLbl.Text = r.Descricao; //recAlocados[i].Descricao;
				pnRecursos.Controls.Add(tmpLbl);
            }
            //lblRecursosAlocados.Text += recAlocados[recAlocados.Count - 1].Descricao;
            //lblRecursosAlocadosId.Text += recAlocados[recAlocados.Count - 1].Id.ToString();
        }
        else lblRecursosAlocados.Text = "";

        ddlOpcao1.Items.Remove(ddlOpcao1.Items.FindByValue(ddlOpcao1.SelectedValue));
        ddlOpcao1.SelectedIndex = 0;
    }


    public void alocar(string recString, string dataString, string horario, string aulaString)
    {
        Guid recId = new Guid(recString);
        Guid aulaId = new Guid(aulaString);
        DateTime data = Convert.ToDateTime(dataString);

        //FIXME: pode ocorrer um problema se outro usuário selecionar o recurso antes...
        Aula aula = aulaBo.GetAulaById(aulaId);
        Recurso rec = recursosBO.GetRecursoById(recId);
        Alocacao aloc = new Alocacao(rec, data, horario, aula, null);
        alocBO.UpdateAlocacao(aloc);
    }

    // Deleta o(s) recurso(s) selecionado(s)
    protected void butDeletar_Click(object sender, ImageClickEventArgs e)
    {
        sender = sender;
    }
}