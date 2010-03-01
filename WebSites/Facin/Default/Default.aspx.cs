// $Id$
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
using BusinessData.Distribuicao.Entities;
using System.Collections.Generic;

public partial class _Default : System.Web.UI.Page
{
	private List<string> horarios;   
	private List<TimeSpan> horariosTime;

    protected void Page_Load(object sender, EventArgs e)
    {
		//controladorAlocacoes = new AlocacaoBO();
		horarios = new List<string>();  
			horariosTime = new List<TimeSpan>();
			
			foreach (string hor in Enum.GetNames(typeof(Horarios.HorariosPUCRS)))
			{
				horariosTime.Add(Horarios.ParseToDateTime(hor).TimeOfDay);
				horarios.Add(hor.ToString());
			}
        if (!IsPostBack)
        {
            if (Roles.GetUsersInRole("Admin").Length == 0)
            {
                Server.Transfer("~/Default/CadastrarAdmin.aspx");
            }
            if (User.Identity.IsAuthenticated == true)
            {
                Server.Transfer("~/Default/PaginaInicial.aspx");
            }

            //ACESSOS
            Acesso a = new Acesso(Guid.NewGuid(), DateTime.Now);
            AcessosBO controladorAcessos = new AcessosBO();
            //controladorAcessos.InserirAcesso(a);
			
			
			Timer1_Tick(null, null);
        }        
        
        //lblDataHora.Text = DateTime.Now.ToString();
    }
    protected void loginEntrada_LoginError(object sender, EventArgs e)
    {
        MembershipUser usr = Membership.GetUser(loginEntrada.UserName);
        if (usr != null && (!usr.IsApproved || usr.IsLockedOut))
            {
                ScriptManager.RegisterClientScriptBlock(this,GetType(), "Conta Bloqueada","alert(' Sua conta está bloqueada. Contate o administrador do sistema para mais informações');", true);
            }
        
    }

    protected void loginEntrada_Authenticate(object sender, AuthenticateEventArgs e)
    {

    }

	private List<Alocacao> ProcuraProximoHorario(List<Alocacao> lista, ref int pos)
	{
		List<Alocacao> filtradaAtual = new List<Alocacao>();
		bool achei = false;
		// Procura o primeiro período com reservas
        while (filtradaAtual.Count == 0)
        {
			if(pos > horarios.Count - 1) // não há mais horários neste dia
				break;
            string horarioAtual = horarios[pos];
            foreach (Alocacao aloc in lista)
            {
                if (aloc.Horario != horarioAtual && achei) // ja achou, ou seja, mudou o horario						
                    break;
                if (aloc.Horario == horarioAtual)
                {
                    //Alocacao nova = new Alocacao(aloc.Recurso,aloc.Data,aloc.Horario,aloc.Aula,aloc.Evento);
                    //nova.Delta = dif.TotalMinutes.ToString();
                    filtradaAtual.Add(aloc);
                    achei = true; // indica que ja achou - quando o horario mudar, sai do foreach							
                }
            }
			pos++;
        }
		return filtradaAtual;
	}
	
    private void VisualizarAlocacoesData()
    {
		DateTime tmp = DateTime.Parse(lblDataHora.Text);		
        DateTime now = tmp.Date;
		TimeSpan nowTime = tmp.TimeOfDay;
		
		AlocacaoBO controladorAlocacoes = new AlocacaoBO();
        List<Alocacao> listaAlocacoes = controladorAlocacoes.GetAlocacoesByData(now, (BusinessData.Entities.Calendario)Session["Calendario"]);
        //TimeSpan nowTime = DateTime.Now.TimeOfDay;
        //nowTime = nowTime.Add(new TimeSpan(2,0,0)); // para testar com outros horarios
		//nowTime = nowTime.Subtract(new TimeSpan(0,12,0));

        // Identifica o período de aula atual
        int pos;
        if (nowTime < horariosTime[0])
            pos = 0;
        else
            for (pos = 0; pos < horarios.Count - 1; pos++)
            {
                if (nowTime >= horariosTime[pos] && nowTime < horariosTime[pos + 1])
                    break;
            }        
		
		List<Alocacao> filtradaAtual = ProcuraProximoHorario(listaAlocacoes, ref pos);
		if (filtradaAtual != null && filtradaAtual.Count != 0)
        {
			//Response.Write("pos="+pos+"<br/>");
			lblAtual.Text = horarios[pos-1];
            dgAlocacoes.DataSource = filtradaAtual;
            dgAlocacoes.Visible = true;
            dgAlocacoes.DataBind();
            lblStatus.Visible = false;
        }
		
		List<Alocacao> filtradaProx  = ProcuraProximoHorario(listaAlocacoes, ref pos);
		if (filtradaProx != null && filtradaProx.Count != 0)
        {
			//Response.Write("pos="+pos+"<br/>");
			lblProximo.Text = horarios[pos-1];
            dgAlocacoes2.DataSource = filtradaProx;
            dgAlocacoes2.DataBind();
            dgAlocacoes2.Visible = true;
            lblStatus.Visible = false;
        }

		//if (pos < horarios.Count - 1) // se nao estivermos ja no ultimo horario... 
        //    {
        // lblAtual.Text = "Horário atual: " + horarioAtual;//+" - "+nowTime.ToString();
		// lblProximo.Text = "Proximo horario: " + horarioProx;
        if (filtradaAtual.Count == 0 && filtradaProx.Count == 0)
        {
            lblStatus.Text = "Não existem recursos alocados para hoje.";
            lblStatus.Visible = true;
            dgAlocacoes.Visible = false;
        }                     
    }

    protected void dgAlocacoes_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lblTurmaEvento = (Label)e.Item.FindControl("lblTurmaEvento");
            //Label lblDiscCod = (Label)e.Item.FindControl("lblDiscCod");
            Label lblDisc = (Label)e.Item.FindControl("lblDisc");
            Label lblResponsavel = (Label)e.Item.FindControl("lblResponsavel");
            Label lblCurso = (Label)e.Item.FindControl("lblCurso");

            Alocacao aloc = (Alocacao)e.Item.DataItem;

            if (aloc.Aula != null)
            {
                //lblDiscCod.Text = aloc.Aula.TurmaId.Disciplina.Cod.ToString();                
                lblDisc.Text = getNomeCurtoDisciplina(aloc.Aula.TurmaId.Disciplina.Nome) + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                lblResponsavel.Text = getNomeCurtoProfessor(aloc.Aula.TurmaId.Professor.Nome);
                lblCurso.Text = aloc.Aula.TurmaId.Curso.Nome;// + " - " + aloc.Delta;
            }
            else
            {
                lblDisc.Text = aloc.Evento.Titulo;
                //lblTurmaEvento.Text = aloc.Evento.Titulo;
                lblResponsavel.Text = aloc.Evento.AutorId.Nome;
            }
        }
    }

    /*
     * Retorna um nome curto para a disciplina, se o tamanho passar de 20 caracteres     
     */
    public string getNomeCurtoDisciplina(string nome)
    {
        if (nome.Length <= 20)
            return nome;
        char[] vogais = { 'a', 'á', 'e', 'ê', 'i', 'o', 'u' };
        string curto = "";
        foreach (string pal in nome.Split())
        {
            // Se a palavra tiver menos de 7 caracteres (ex: "de", "para", "(SI)") usa como está
            string palCurta = pal;
            if (pal.Length > 7)
            {
                // Pega as 4 primeiras letras da palavra
                palCurta = pal.Substring(0, 4);
                // A partir da quarta letra, procura a primeira consoante
                int pos = 4;
                while (pos < pal.Length)
                {
                    palCurta += pal[pos];
                    if (pal[pos] == 'a' || pal[pos] == 'á' || pal[pos] == 'e' || pal[pos] == 'ê'
                       || pal[pos] == 'o' || pal[pos] == 'ó' || pal[pos] == 'u')
                        pos++;
                    else break;
                }
                // Se terminar com uma vogal, acrescenta mais uma letra
                //if (palCurta[2] == 'a' || palCurta[2] == 'á' || palCurta[2] == 'e' || palCurta[2] == 'i' || palCurta[2] == 'o'
                //    || palCurta[2] == 'u')
                //    palCurta = pal.Substring(0, 4);
                palCurta += ". ";
            }
            curto += palCurta + " ";
        }
        return curto;
    }

    /*
     * Retorna um nome curto para um professor, i.e. apenas nome e último sobrenome
     */
    public string getNomeCurtoProfessor(string nome)
    {
        string[] nomes = nome.Split();
        // Somente um nome ?
        if (nomes.Length == 1)
            return nome;
        return nomes[0] + " " + nomes[nomes.Length - 1];
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //lblDataHora.Text = DateTime.Now.ToString();
        lblDataHora.Text = DateTime.Now.Add(new TimeSpan(1, 2, 9, 0)).ToString();
        VisualizarAlocacoesData();
    }
}
