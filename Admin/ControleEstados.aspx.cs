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
using System.Threading;


public partial class Admin_ControleEstados : System.Web.UI.Page
{
    ConfigBO controladorConfig;
    ControladorDistribuirAulasBO controladorAulas;
    Calendario calAtual;
    BusinessData.Distribuicao.BusinessLogic.ControleDistribuicao cDistr;
    bool isAulasDistribuidas;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        cDistr = new BusinessData.Distribuicao.BusinessLogic.ControleDistribuicao();
        controladorConfig = new ConfigBO();
        controladorAulas = new ControladorDistribuirAulasBO();
        calAtual = (Calendario)Session["Calendario"];
        if (!IsPostBack)
        {
            AtualizaBotoes();
        }
		Calendario meucal = (Calendario)Session["Calendario"];
		Response.Write(meucal.Ano + "<br/>");
		Response.Write(meucal.Semestre + "<br/>");
    }

    protected void btnAbrirSolicitacaoRecursos_Click(object sender, EventArgs e)
    {
        
        Session["AppState"] = AppState.Requisicoes;
        
        controladorConfig.SetAppState(AppState.Requisicoes,calAtual);
        isAulasDistribuidas = controladorConfig.IsAulasDistribuidas(calAtual);
        if (!isAulasDistribuidas)
            controladorAulas.AbreSolicitacaoRecursos(calAtual);
        lblResultado.Text = "Solicitação de Recursos aberta.";
        AtualizaBotoes();
    }

    protected void btnFecharAcesso_Click(object sender, EventArgs e)
    {
        Session["AppState"] = AppState.Admin;
        controladorConfig.SetAppState(AppState.Admin, calAtual);
        lblResultado.Text = "Acesso fechado.";
        AtualizaBotoes();
    }


    private void AtualizaBotoes()
    {
        switch ((AppState)Session["AppState"])
        {
            case AppState.Admin:
                if (controladorConfig.IsRecursosDistribuidos(calAtual))
                {
                    btnAbrirSolicitacaoRecursos.Enabled = false;
                    btnIniciarSemestre.Text = "Liberar Acesso";
                    btnIniciarSemestre.Enabled = true;
                    checkSimula.Enabled = false; // já foram distribuídos, não pode mais simular
                }
                else
                {
                    btnAbrirSolicitacaoRecursos.Enabled = true;
                    btnIniciarSemestre.Text = "Iniciar Semestre";
                    btnIniciarSemestre.Enabled = true;
                    checkSimula.Enabled = true; // Ainda não foram distribuídos, é possível simular
                }
                btnFecharAcesso.Enabled = false;
                break;
            case AppState.Requisicoes:
                btnAbrirSolicitacaoRecursos.Enabled = false;
                btnFecharAcesso.Enabled = true;
                break;
            case AppState.AtivoSemestre:
                btnAbrirSolicitacaoRecursos.Enabled = false;
                btnFecharAcesso.Enabled = true;
                btnIniciarSemestre.Enabled = false;
                checkSimula.Enabled = false; // já foram distribuídos, não pode mais simular
                break;
        }
    }
    protected void btnIniciarSemestre_Click(object sender, EventArgs e)
    {
        bool simula = checkSimula.Checked;
        if(!controladorConfig.IsRecursosDistribuidos((Calendario)Session["Calendario"]))
        {
            if(!simula) btnIniciarSemestre.Enabled = false;
            lock (Session.SyncRoot)
            {
                Session["Complete"] = false;
            }
            IniciarThread(simula);
        }
        else
        {
            AlterarEstadoParaDistribuido();
            lblResultado.Text = "Acesso para professores liberado.";
        }
    }


    //Controle de Threads

    private void IniciarThread(bool simula)
    {
        //Thread td = new Thread(new ParameterizedThreadStart(DistribuirRecursos));
        //td.Priority = ThreadPriority.Lowest;
        //td.Start((Calendario)Session["Calendario"]);
		DistribuirRecursos((Calendario)Session["Calendario"], simula);
        Response.Redirect("Results.aspx");
    }

    private void DistribuirRecursos(object calendario, bool simula)
    {
        Calendario cal = (Calendario)calendario;
        cDistr.DistribuirRecursos(cal.Ano, cal.Semestre, simula);
        lock (Session.SyncRoot)
        {
            Session["Complete"] = true;
        }
        //lblResultado.Text = "Distribuição Concluída!";
        if(!simula) AlterarEstadoParaDistribuido();
    }

    private void AlterarEstadoParaDistribuido()
    {
        Session["AppState"] = AppState.AtivoSemestre;
        controladorConfig.SetAppState(AppState.AtivoSemestre, (Calendario)Session["Calendario"]);
        controladorConfig.setRecursosDistribuidos((Calendario)Session["Calendario"], true);        
        AtualizaBotoes();
    }
}
