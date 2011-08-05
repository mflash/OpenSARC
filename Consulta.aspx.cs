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

public partial class Alocacoes_Default : System.Web.UI.Page
{
    private CategoriaRecursoBO controladorCategorias;
    private RecursosBO controladorRecursos;
    private AlocacaoBO controladorAlocacoes;

    protected void Page_Load(object sender, EventArgs e)
    {
        controladorCategorias = new CategoriaRecursoBO();
        controladorRecursos = new RecursosBO();
        controladorAlocacoes = new AlocacaoBO();
		
		lblDataHora.Text = DateTime.Now.ToString();
      /*  if (!IsPostBack)
        {
            PopulaCategorias();
            PopulaProfessores();
            PopulaSecretarios();
        }		
		*/
    }

    protected void btnVisualizarAlocacoes_Click(object sender, EventArgs e)
    {
        lblStatus.Visible = true;
        VisualizarAlocacoesData();
    }
    
    protected void rblAlocacoes_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetaComponentes();
        lblOpcional.Visible = false;
        dgAlocacoes.Visible = false;
    }    
	
	private class AlocacaoTmp : Alocacao
	{
		private string delta;
		
		public AlocacaoTmp(BusinessData.Entities.Recurso r, DateTime d, string h, Aula a, Evento e) : base(r,d,h,a,e)
		{
		}
		
		public string Delta
		{
			get { return delta; }
			set { delta = value; }
		}
	}
	
    private void VisualizarAlocacoesData()
    {
        try
        {
            if (txtData.Text.Length != 0)
            {
				DateTime now = DateTime.Parse(txtData.Text);
                List<Alocacao> listaAlocacoes = controladorAlocacoes.GetAlocacoesByData(now, (BusinessData.Entities.Calendario)Session["Calendario"]);
				List<Alocacao> filtradaAtual = new List<Alocacao>();
                List<Alocacao> filtradaProx  = new List<Alocacao>();

				//string horarioAtual = String.Empty;

                List<string> horarios = new List<string>();
                List<TimeSpan> horariosTime = new List<TimeSpan>();
                foreach (string hor in Enum.GetNames(typeof(Horarios.HorariosPUCRS)))
                {
                    horariosTime.Add(Horarios.ParseToDateTime(hor).TimeOfDay);
                    horarios.Add(hor.ToString());
                }

                TimeSpan nowTime = DateTime.Now.TimeOfDay;
                //nowTime = nowTime.Add(new TimeSpan(2,0,0)); // para testar com outros horarios

                // Identifica o período de aula atual
                int pos;
				if(nowTime < horariosTime[0])
					pos = 0;
				else
                for(pos = 0; pos < horarios.Count-1; pos++)
                {
                    if (nowTime >= horariosTime[pos] && nowTime <= horariosTime[pos + 1])
                        break;
                }
				
                lblAtual.Text = "Horário atual: "+horarios[pos];//+" - "+nowTime.ToString();
				
				string horarioAtual = horarios[pos];
				string horarioProx  = String.Empty;
				if(pos < horarios.Count-1) // se nao estivermos ja no ultimo horario... 
				{
					horarioProx = horarios[pos+1];
					lblProximo.Text = "Proximo horario: "+horarioProx;
				}
					
				bool achei = false;
				
				foreach(Alocacao aloc in listaAlocacoes)
				{					
					if(aloc.Horario != horarioAtual && aloc.Horario != horarioProx && achei) // ja achou, ou seja, mudou o horario						
						break;
					if(aloc.Horario == horarioAtual)
					{
						//Alocacao nova = new Alocacao(aloc.Recurso,aloc.Data,aloc.Horario,aloc.Aula,aloc.Evento);
						//nova.Delta = dif.TotalMinutes.ToString();
						filtradaAtual.Add(aloc);
						achei = true; // indica que ja achou - quando o horario mudar, sai do foreach							
					}
					if(aloc.Horario == horarioProx)
					{
						//Alocacao nova = new Alocacao(aloc.Recurso,aloc.Data,aloc.Horario,aloc.Aula,aloc.Evento);
						filtradaProx.Add(aloc);
						achei = true;
					}
				}				
                if (filtradaAtual.Count != 0)
                {
                    dgAlocacoes.DataSource = filtradaAtual;
                    dgAlocacoes.Visible = true;
                    dgAlocacoes.DataBind();
                    lblStatus.Visible = false;
                }
                if(filtradaProx.Count != 0)
                {                    
                    dgAlocacoes2.DataSource = filtradaProx;
                    dgAlocacoes2.DataBind();
                    dgAlocacoes2.Visible = true;
                    lblStatus.Visible = false;
                }
                if(filtradaAtual.Count == 0 && filtradaProx.Count == 0)
                {
                    lblStatus.Text = "Não existem recursos alocados para hoje.";
                    lblStatus.Visible = true;
                    dgAlocacoes.Visible = false;
                }
            }
            else
            {
                lblStatus.Visible = true;
                FormatException excp = new FormatException();
                throw excp;
            }
        }
        catch (FormatException)
        {
            dgAlocacoes.Visible = false;
            lblStatus.Text = "Digite uma data válida!";
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
                lblDisc.Text = getNomeCurtoDisciplina(aloc.Aula.TurmaId.Disciplina.Nome) + " (" +aloc.Aula.TurmaId.Numero.ToString() + ")";
                lblResponsavel.Text = aloc.Aula.TurmaId.Professor.Nome;
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

    private void ResetaComponentes()
    {
        txtData.Text = "";
        lblStatus.Text = "";
        lblStatus.Visible = true;
        dgAlocacoes.Visible = false;
		dgAlocacoes2.Visible = false;        
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
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
                while(pos < pal.Length)
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
}
