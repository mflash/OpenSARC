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
using System.Threading.Tasks;
using BusinessData.Util;


public partial class Admin_ControleEstados : System.Web.UI.Page
{
    ConfigBO controladorConfig;
    ControladorDistribuirAulasBO controladorAulas;
    Calendario calAtual;
    BusinessData.Distribuicao.BusinessLogic.ControleDistribuicao cDistr;
    bool isAulasDistribuidas;
    String oldMsgProgresso;
   
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
		//Response.Write(meucal.Ano + "<br/>");
		//Response.Write(meucal.Semestre + "<br/>");
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
            timerProgresso.Enabled = true;
            IniciarThread(simula);
        }
        else
        {
            AlterarEstadoParaDistribuido();
            lblResultado.Text = "Acesso para professores liberado.";
        }
    }

    protected void timerProgresso_Tick(object sender, EventArgs e)
    {
        txtProgresso.Text = DebugLog.GetLog();
        if (DebugLog.IsDone())
        {
            // Inject JS redirect
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "redirectAfterTask",
                "window.location.href = 'Results.aspx';", // change to your target page
                true
            );
        }
    }

    //Controle de Threads

    private void IniciarThread(bool simula)
    {
        //Thread td = new Thread(new ParameterizedThreadStart(DistribuirRecursos));
        //td.Priority = ThreadPriority.Lowest;
        //td.Start((Calendario)Session["Calendario"]);
        Session["ProgressoDistribuicao"] = 0;
        Calendario calendario = (Calendario)Session["Calendario"];
        DebugLog.Clear();
        DebugLog.Write("Iniciando distribuição");
        Task.Run(() => DistribuirRecursos(calendario, simula));
        //		DistribuirRecursos((Calendario)Session["Calendario"], simula);
        //        Response.Redirect("Results.aspx");
    }

    private void DistribuirRecursosAsync(Calendario calendario, bool simula)
    {
        // Exemplo: progresso de 0 a 100
        for (int progresso = 0; progresso <= 100; progresso += 10)
        {
            // Chame o método real de distribuição aqui, passando progresso se possível
            // cDistr.DistribuirRecursos(calendario.Ano, calendario.Semestre, simula);

            // Salve o progresso na Session (ou outro local)
            //msgProgresso += "\nAsync update: " + progresso;
            Session["MsgProgresso"] = "msgProgresso";
            Session["ProgressoDistribuicao"] = progresso;

            // Simule tempo de processamento
            Thread.Sleep(500);
        }
        Session["Complete"] = true;
        //if (!simula) AlterarEstadoParaDistribuido();
    }

    private void DistribuirRecursos(object calendario, bool simula)
    {
        Calendario cal = (Calendario)calendario;
        cDistr.DistribuirRecursos(cal.Ano, cal.Semestre, simula, Session);
        lock (Session.SyncRoot)
        {
            Session["Complete"] = true;
        }
        //lblResultado.Text = "Distribuição Concluída!";
        if(!simula) AlterarEstadoParaDistribuido();
        DebugLog.MarkDone();
//        Response.Redirect("Results.aspx");
    }

    private void AlterarEstadoParaDistribuido()
    {
        Session["AppState"] = AppState.AtivoSemestre;
        controladorConfig.SetAppState(AppState.AtivoSemestre, (Calendario)Session["Calendario"]);
        controladorConfig.setRecursosDistribuidos((Calendario)Session["Calendario"], true);        
        AtualizaBotoes();
    }
}
