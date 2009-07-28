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
using System.Security;
using BusinessData.DataAccess;
using System.Collections.Generic;

public partial class Calendario_Cadastro : System.Web.UI.Page
{ 

    CategoriaDataBO categBO = new CategoriaDataBO();
    CalendariosBO calendariosBo = new CalendariosBO();
    DatasBO dBo = new DatasBO(); 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            populaCategoria();
            
            if (Request.QueryString["Calendario"] != null)
            {
                Guid idCal = new Guid(Request.QueryString["Calendario"]);
                try
                {
                    Calendario c = calendariosBo.GetCalendario(idCal);
                    txtAno.Text = c.Ano.ToString();
                    ddlSemestre.SelectedValue = c.Semestre.ToString();

                    Session["Semestre"] = c.Semestre;
                    Session["Ano"] = c.Ano;

                    AtualizaDatas();
                }
                catch (DataAccessException)
                {
                    Response.Redirect("~/Calendario/Cadastro.aspx");
                }
            }
        }
    }

    protected void populaCategoria()
    {
        try
        {
            ddlCategoria.DataSource = categBO.GetCategoriaDatas();
            ddlCategoria.DataTextField = "Descricao";
            ddlCategoria.DataValueField = "Id";
            ddlCategoria.DataBind();
        }
        catch (DataAccessException ex)
        {
            throw ex;
        }
    }

    protected void btnAdicionarData_Click(object sender, EventArgs e)
    {
        DateTime dtInicioG1 = Convert.ToDateTime(txtInicioG1.Text);
        DateTime dtInicioG2 = Convert.ToDateTime(txtInicioG2.Text);
        DateTime dtFimG2 = Convert.ToDateTime(txtFimG2.Text);
        int anoAux = Convert.ToInt32(txtAno.Text);



        int ano = (int)Session["Ano"];
        int semestre = (int)Session["Semestre"];

        Calendario c = null;
        // Se existir o calendário selecionado, pega por aqui
        try
        {
            c = calendariosBo.GetCalendarioByAnoSemestre(ano, semestre);

            if (c == null)
            {
                c = Calendario.NewCalendario(semestre, ano, Convert.ToDateTime(txtInicioG1.Text), Convert.ToDateTime(txtInicioG2.Text), Convert.ToDateTime(txtFimG2.Text));
                calendariosBo.InsereCalendario(c);
            }
        }
        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (SecurityException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

        // Faz a inserção propriamente dita
        try
        {
            CategoriaData categoriaData = categBO.GetCategoriaDataById(new Guid(ddlCategoria.SelectedValue));

            //Testa se a data a ser inserida é válida para o calendário vigente
            DateTime dataIn = clndario.SelectedDate;
            DateTime dataMinima = new DateTime();
            DateTime dataMaxima = new DateTime();

            dataMinima = Convert.ToDateTime(txtInicioG1.Text);
            dataMaxima = Convert.ToDateTime(txtFimG2.Text);

            //compara se a data eh valida
            if (dataIn >= dataMinima && dataIn <= dataMaxima)
            {
                Data d = Data.GetData(dataIn, categoriaData);
                dBo.InsereData(d, c);
                lblStatus.Text = "Data inserida com sucesso";
            }
            else
                throw new InvalidConstraintException("A data selecionada não existe no calendário vigente", null);

        }
        catch (InvalidConstraintException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (SecurityException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

        AtualizaDatas();
        lblStatus.Text = "Ano deve ser igual ao selecionado.";

    }

    protected void AtualizaDatas()
    {
        if (Session["Ano"] != null && Session["Semestre"] != null)
        {
            try
            {
                int ano = (int)Session["Ano"];
                int semestre = (int)Session["Semestre"];

                if (semestre == 1)
                    clndario.VisibleDate = new DateTime(ano, 3, 1);
                if (semestre == 2)
                    clndario.VisibleDate = new DateTime(ano, 7, 1);

                Calendario c = calendariosBo.GetCalendarioByAnoSemestre(ano, semestre);
                txtInicioG1.Text = c.InicioG1.ToShortDateString();
                txtInicioG2.Text = c.InicioG2.ToShortDateString();
                txtFimG2.Text = c.FimG2.ToShortDateString();
                lbDatas.DataSource = c.Datas;
                lbDatas.DataTextField = "PorExtenso";
                lbDatas.DataValueField = "date";
                lbDatas.DataBind();
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }
    }

    protected void btnExcluirData_Click(object sender, EventArgs e)
    {
        DateTime data = new DateTime();

        try
        {
            //como estamos usando um listbox, temos que "parsear" a porcaria da string
            string[] valores = lbDatas.SelectedValue.Split('/');
            data = new DateTime(Convert.ToInt32(valores[2].Substring(0, 4)), Convert.ToInt32(valores[1]), Convert.ToInt32(valores[0]));// = (DateTime)lbDatas.selected
        }
        catch
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + "Problema ao excluir");
        }
        try
        {
            DatasBO dBo = new DatasBO();
            Calendario c = calendariosBo.GetCalendarioByAnoSemestre((int)Session["Ano"], (int)Session["Semestre"]);

            dBo.DeletaData(c.Id, Data.GetData(data, null));
            lblStatus.Text = "Data excluida com sucesso";
            lblStatus.Visible = true;

            AtualizaDatas();
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dtInicioG1 = Convert.ToDateTime(txtInicioG1.Text);
            DateTime dtInicioG2 = Convert.ToDateTime(txtInicioG2.Text);
            DateTime dtFimG2 = Convert.ToDateTime(txtFimG2.Text);
            int ano = Convert.ToInt32(txtAno.Text);

            if ((dtInicioG1.Year == ano) && (dtInicioG2.Year == ano) && (dtFimG2.Year == ano))
            {
                if (((Convert.ToInt32(ddlSemestre.SelectedItem.Text) == 1) && (dtInicioG1.Month < 8) && (dtInicioG2.Month < 8) && (dtFimG2.Month < 8))
                    || ((Convert.ToInt32(ddlSemestre.SelectedItem.Text) == 2) && (dtInicioG1.Month > 7) && (dtInicioG2.Month > 7) && (dtFimG2.Month > 7)))
                {
                    if ((dtInicioG1.CompareTo(dtInicioG2) == -1) && (dtInicioG2.CompareTo(dtFimG2) == -1))
                    {
                        if ((txtAno.Text != "") && (ddlSemestre.SelectedIndex != 0))
                        {
                            int semestre = 0;

                            if (Session["Ano"] != null && Session["Semestre"] != null)
                            {
                                semestre = (int)Session["Semestre"];
                                ano = (int)Session["Ano"];
                            }
                            else
                            {
                                semestre = Convert.ToInt32(ddlSemestre.Text);
                                ano = Convert.ToInt32(txtAno.Text);
                            }

                            Calendario c = calendariosBo.GetCalendarioByAnoSemestre(ano, semestre);

                            if (c == null)
                            {
                                c = Calendario.NewCalendario(semestre, ano, dtInicioG1, dtInicioG2, dtFimG2);
                                calendariosBo.InsereCalendario(c);
                            }
                            else
                            {
                                c.InicioG1 = Convert.ToDateTime(txtInicioG1.Text);
                                c.InicioG2 = Convert.ToDateTime(txtInicioG2.Text);
                                c.FimG2 = Convert.ToDateTime(txtFimG2.Text);

                                calendariosBo.UpdateCalendario(c);
                            }

                            lblStatus.Text = "Calendário salvo com sucesso!";
                        }
                        else
                            lblStatus.Text = "Calendário não cadastrado";
                    }
                    else
                        lblStatus.Text = "Datas não existente no calendário selecionado.";
                }
                else
                    lblStatus.Text = "Mês não disponível para o semestre selecionado.";
            }
                else
                lblStatus.Text = "Ano deve ser igual ao selecionado.";
        }   
        catch (DataAccessException)
        {
            Response.Redirect("~/Calendario/Cadastro.aspx");
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void btnCarregaDatas_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(txtAno.Text) != 0)
            {
                if (Convert.ToInt32(txtAno.Text) >= 2000 && Convert.ToInt32(txtAno.Text) <= 4000)
                {
                    LimpaCampos();
                    int ano = Convert.ToInt32(txtAno.Text);
                    Session["Ano"] = ano;
                    int semestre = Convert.ToInt32(ddlSemestre.Text);
                    Session["Semestre"] = semestre;


                    Calendario c = calendariosBo.GetCalendarioByAnoSemestre(ano, semestre);
                    if (c != null)
                    {
                        AtualizaDatas();
                    }
                    else
                    {
                        if (semestre == 1)
                            clndario.VisibleDate = new DateTime(ano, 3, 1);
                        if (semestre == 2)
                            clndario.VisibleDate = new DateTime(ano, 7, 1);
                    }
                }
                else lblStatus.Text = "Ano deve ser um valor entre 2000 e 4000.";
            }
            else lblStatus.Text = "Ano deve ser diferente de 0 (zero).";
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void LimpaCampos()
    {
        txtInicioG1.Text = "";
        txtInicioG2.Text = "";
        txtFimG2.Text = "";
        lbDatas.Items.Clear();
        lblStatus.Text = "";
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}