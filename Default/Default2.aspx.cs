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
using System.Net;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Image = System.Web.UI.WebControls.Image;
using System.ServiceModel.Configuration;
using System.Linq;


public partial class _Default : System.Web.UI.Page
{
    private List<string> horarios;
    private List<TimeSpan> horariosTime;
    private SRRCDAO logDataDAO = new SRRCDAO();

    private DateTime dataHoraForcada;
    private bool forcaDataHora = false;
    private bool ocultaDescricaoCurta = false;

    private enum StatusRecurso
    {
        Disponivel, EmUsoEDisponivel, EmUsoEReservado, DisponivelEReservado
    }
    private class RecursoItem
    {
        public string HorarioAtual;
        public string HorarioProx;
        public string NomeCompleto;
        public string DescricaoAtualCurta;
        public string DescricaoAtual;
        public string DescricaoProxCurta;
        public string DescricaoProx;
        public string ResponsavelAtual;
        public string ResponsavelAtualCurto;
        public string ResponsavelProx;
        public string ResponsavelProxCurto;
        public StatusRecurso Status;
    }

    private Dictionary<char, string[]> dicIcones = new Dictionary<char, string[]>
        {
            { 'L', new string[] {"lab", "Labs", "#feff00" } },
            { 'A', new string[] { "auditorio", "Auditórios", "#FFEEDD" } },
            { 'N', new string[] { "notebook", "Notebooks", "#CEFD30" } },
            { 'K', new string[] { "cabo-vga", "Kits VGA", "#CA2098" } },
            { 'H', new string[] { "cabo-hdmi", "Kits HDMI", "#F97845" } },
            { 'S', new string[] { "speaker", "Caixas de Som", "#F05692" } }
        };

    private Dictionary<StatusRecurso, string> dicCoresStatus = new Dictionary<StatusRecurso, string>
    {
        { StatusRecurso.EmUsoEDisponivel, "emusoedisp" },
        { StatusRecurso.DisponivelEReservado, "dispereserv" },
        { StatusRecurso.EmUsoEReservado, "emusoereserv" }
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        horarios = new List<string>();
        horariosTime = new List<TimeSpan>();

        if (Request.QueryString["datahora"] != null)
        {
            dataHoraForcada = DateTime.Parse(Request.QueryString["datahora"]);
            forcaDataHora = true;
        }

        if (Request.QueryString["descricao"] != null)
        {
            if (Request.QueryString["descricao"] == "0")
                ocultaDescricaoCurta = true;
        }

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
            //Acesso a = new Acesso(Guid.NewGuid(), DateTime.Now);
            //AcessosBO controladorAcessos = new AcessosBO();
            //controladorAcessos.InserirAcesso(a);


            Timer1_Tick(null, null);
        }
        //dgAlocacoes.AlternatingItemStyle.BackColor = Color.Gainsboro;
        //dgAlocacoes.ItemStyle.BackColor = Color.White;
        //dgAlocacoes2.AlternatingItemStyle.BackColor= Color.Gainsboro;
        //dgAlocacoes2.ItemStyle.BackColor = Color.White;

        //lblDataHora.Text = DateTime.Now.ToString();

    }
    protected void loginEntrada_LoginError(object sender, EventArgs e)
    {
        MembershipUser usr = Membership.GetUser(loginEntrada.UserName);
        if (usr != null && (!usr.IsApproved || usr.IsLockedOut))
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Conta Bloqueada", "alert(' Sua conta está bloqueada. Contate o administrador do sistema para mais informações');", true);
        }

    }

    protected bool LDAPAuth(String user, String pass)
    {
        bool result = false;
        string domain = ConfigurationManager.AppSettings["ldapDomain"];
        string serviceUser = ConfigurationManager.AppSettings["ldapServiceUser"];
        string servicePass = ConfigurationManager.AppSettings["ldapServicePasswd"];
        using (var context = new PrincipalContext(ContextType.Domain, domain, serviceUser, servicePass))
        {
            //Username and password for authentication.
            result = context.ValidateCredentials(user, pass);
        }
        //Debug.WriteLine("Auth: " + result);
        return result;
    }

    protected bool moodleAuth(String user, String pass, out string reason)
    {
        reason = "";
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var request = WebRequest.Create("https://moodle.pucrs.br/cead/sarcauth.php");
        // All teacher user ids start with "10"... but in Moodle we use the old format (without "10")
        //if (user.StartsWith("10"))
        //    user = user.Substring(2);
        var postdata = "user=" + user + "&pass=" + Uri.EscapeDataString(pass);
        var data = Encoding.ASCII.GetBytes(postdata);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        try
        {
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }
        catch (WebException e)
        {
            Debug.WriteLine(e.ToString());
            reason = e.Message;
            return false;
        }
        var response = request.GetResponse();
        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        Debug.WriteLine("Moodle:" + responseString);
        if (responseString == "FAIL\n")
            return false;
        return true;
    }

    protected void loginEntrada_Authenticate(object sender, AuthenticateEventArgs e)
    {
        string reason = "";
        if (Membership.ValidateUser(loginEntrada.UserName, loginEntrada.Password))
            e.Authenticated = true;
        else if (LDAPAuth(loginEntrada.UserName, loginEntrada.Password))
            e.Authenticated = true;
        //else if (moodleAuth(loginEntrada.UserName, loginEntrada.Password, out reason))
        //    e.Authenticated = true;
        else
            e.Authenticated = false;
        //lblDataHora.Text = reason;
        //if (reason != string.Empty)
        //    lblDataHora.ForeColor = System.Drawing.Color.Red;
    }

    protected void dgAlocacoes_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (logDataDAO == null) return;
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lblTurmaEvento = (Label)e.Item.FindControl("lblTurmaEvento");
            //Label lblDiscCod = (Label)e.Item.FindControl("lblDiscCod");
            Label lblDisc = (Label)e.Item.FindControl("lblDisc");
            Label lblResponsavel = (Label)e.Item.FindControl("lblResponsavel");
            Label lblCurso = (Label)e.Item.FindControl("lblCurso");
            Label lblRecurso = (Label)e.Item.FindControl("lblRecurso");
            Label lblStatus = (Label)e.Item.FindControl("lblEstado");
            Image imgIcon = (Image)e.Item.FindControl("imgIcon");

            Alocacao aloc = (Alocacao)e.Item.DataItem;

            char tipoRecurso = aloc.Recurso.Tipo;
            if (aloc.Aula != null)
            {
                //lblDiscCod.Text = aloc.Aula.TurmaId.Disciplina.Cod.ToString();                
                lblDisc.Text = getNomeCurtoDisciplina(aloc.Aula.TurmaId.Disciplina.Nome) + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                lblResponsavel.Text = getNomeCurtoProfessor(aloc.Aula.TurmaId.Professor.Nome);
                // lblCurso.Text = aloc.Aula.TurmaId.Curso.Nome;// + " - " + aloc.Delta;
            }
            else
            {
                lblDisc.Text = aloc.Evento.Titulo;
                //lblTurmaEvento.Text = aloc.Evento.Titulo;
                lblResponsavel.Text = aloc.Evento.AutorId.Nome;
            }
            lblStatus.Text = logDataDAO.GetUltimoStatus(lblRecurso.Text);
            if (lblStatus.Text == "Disponível")
                e.Item.ForeColor = Color.Green;
            else
                e.Item.ForeColor = Color.Red;
            Debug.WriteLine("Recurso: " + tipoRecurso);
            switch (tipoRecurso)
            {
                case 'L':
                    imgIcon.ImageUrl = "~/SRRC/img/lab.png";
                    break;
                case 'N':
                    imgIcon.ImageUrl = "~/SRRC/img/notebook.png";
                    break;
                case 'A':
                    imgIcon.ImageUrl = "~/SRRC/img/auditorio.png";
                    break;
                case 'H':
                    imgIcon.ImageUrl = "~/SRRC/img/cabo-hdmi.png";
                    break;
                case 'S':
                    imgIcon.ImageUrl = "~/SRRC/img/speaker.png";
                    break;
                case 'K':
                    imgIcon.ImageUrl = "~/SRRC/img/cabo-vga.png";
                    break;
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

    public string getNomeBemCurtoDisciplina(string nome)
    {
        //if (nome.Length <= 10)
        if (nome.Length <= 40)
            return nome;
        HashSet<String> stopWords = new HashSet<string> {
            "da", "de", "à", "á", "e", "ao", "a", "para", "em", "-"
        };
        HashSet<String> numerals = new HashSet<string>
        {
            "I", "II", "III", "IV", "V", "VI"
        };
        char[] vogais = { 'a', 'á', 'e', 'ê', 'i', 'o', 'u' };
        string curto = "";
        foreach (string pal in nome.Split())
        {
            if (stopWords.Contains(pal))
                continue;
            if (numerals.Contains(pal))
            {
                curto += " " + pal;
                return curto;
            }
            else
                curto += pal[0];
            if (curto.Length >= 3)
                break;
        }
        return curto;
    }

    public string getNomeMaisOuMenosCurtoDisciplina(string nome)
    {
        if (nome.Length <= 15)
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
        if (curto.Length >= 35)
            curto = curto.Substring(0, 33) + "\u2026";
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
            return nome.Length <= 10 ? nome : nome.Substring(0, 10);
        string ultNome = nomes[nomes.Length - 1];
        return nomes[0][0] + ". " + (ultNome.Length <= 8 ? ultNome : ultNome.Substring(0, 8) + ".");
        ;
    }

    public string getNomeSobrenomeProfessor(string nome)
    {
        string[] nomes = nome.Split();
        if (nomes.Length == 1)
            return nome;
        return nomes[0] + " " + nomes[nomes.Length - 1];
    }


    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //lblDataHora.Text = DateTime.Now.ToString();
        //lblDataHora.Text = DateTime.Now.Add(new TimeSpan(0, 0, 0, 0)).ToString();
        VisualizarAlocacoesData();
    }

    private List<Alocacao> ProcuraProximoHorario(List<Alocacao> lista, ref int pos)
    {
        List<Alocacao> filtradaAtual = new List<Alocacao>();
        bool achei = false;
        // Procura o primeiro período com reservas
        while (filtradaAtual.Count == 0)
        {
            if (pos > horarios.Count - 1) // não há mais horários neste dia
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

    private void CopiaSalaDupla(SortedDictionary<string, RecursoItem> dicRecurso, string chavedupla, string chave1, string chave2)
    {
        RecursoItem r = dicRecurso[chavedupla];
        if (r.DescricaoAtual != null)
        {
            if (!dicRecurso.ContainsKey(chave1))
            {
                RecursoItem r1 = new RecursoItem();
                dicRecurso[chave1] = r1;
            }
            dicRecurso[chave1].DescricaoAtual = r.DescricaoAtual;
            dicRecurso[chave1].DescricaoAtualCurta = r.DescricaoAtualCurta;
            dicRecurso[chave1].ResponsavelAtual = r.ResponsavelAtual;
            dicRecurso[chave1].ResponsavelAtualCurto = r.ResponsavelAtualCurto;
            dicRecurso[chave1].HorarioAtual = r.HorarioAtual;

            if (!dicRecurso.ContainsKey(chave2))
            {
                RecursoItem r2 = new RecursoItem();
                dicRecurso[chave2] = r2;
            }
            dicRecurso[chave2].DescricaoAtual = r.DescricaoAtual;
            dicRecurso[chave2].DescricaoAtualCurta = r.DescricaoAtualCurta;
            dicRecurso[chave2].ResponsavelAtual = r.ResponsavelAtual;
            dicRecurso[chave2].ResponsavelAtualCurto = r.ResponsavelAtualCurto;
            dicRecurso[chave2].HorarioAtual = r.HorarioAtual;
        }
        if (r.DescricaoProx != null)
        {
            if (!dicRecurso.ContainsKey(chave1))
            {
                RecursoItem r1 = new RecursoItem();
                dicRecurso[chave1] = r1;
            }
            dicRecurso[chave1].DescricaoProx = r.DescricaoProx;
            dicRecurso[chave1].DescricaoProxCurta = r.DescricaoProxCurta;
            dicRecurso[chave1].ResponsavelProx = r.ResponsavelProx;
            dicRecurso[chave1].ResponsavelProxCurto = r.ResponsavelProxCurto;
            dicRecurso[chave1].HorarioProx = r.HorarioProx;

            if (!dicRecurso.ContainsKey(chave2))
            {
                RecursoItem r2 = new RecursoItem();
                dicRecurso[chave2] = r2;
            }
            dicRecurso[chave2].DescricaoProx = r.DescricaoProx;
            dicRecurso[chave2].DescricaoProxCurta = r.DescricaoProxCurta;
            dicRecurso[chave2].ResponsavelProx = r.ResponsavelProx;
            dicRecurso[chave2].ResponsavelProxCurto = r.ResponsavelProxCurto;
            dicRecurso[chave2].HorarioProx = r.HorarioProx;
        }
    }

    private void VisualizarAlocacoesData()
    {
        DateTime now;
        if (forcaDataHora)
            now = dataHoraForcada;
        else
            now = DateTime.Now;
        DateTime hoje = now.Date;
        //        now = now.Subtract(TimeSpan.FromDays(1));
        TimeSpan nowTime = now.TimeOfDay; //.Add(TimeSpan.FromMinutes(60));

        AlocacaoBO controladorAlocacoes = new AlocacaoBO();
        List<Alocacao> listaAlocacoes = controladorAlocacoes.GetAlocacoesByData(hoje, (BusinessData.Entities.Calendario)Session["Calendario"]);
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
                if (nowTime >= horariosTime[pos] && nowTime < horariosTime[pos + 1]) {
                    TimeSpan ts = nowTime.Subtract(horariosTime[pos]);
                    Debug.WriteLine("Timedelta: " + ts);
                    if (ts.TotalMinutes > 30)
                        pos++;
                    break;
                }
            }
        if (pos == horarios.Count)
            pos--;
        lblDataHora.Text = now.Add(new TimeSpan(0, 0, 0, 0)).ToString() + " - " + horarios[pos];

        //pos = 5;
        List<Alocacao> filtradaAtual = ProcuraProximoHorario(listaAlocacoes, ref pos);
        //foreach (Alocacao aloc in listaAlocacoes)
        //{
        //    System.Diagnostics.Debug.WriteLine("ALOC: "+aloc.Horario+" "+aloc.Recurso.Descricao);
        //}
        SortedDictionary<char, SortedDictionary<string, RecursoItem>> dic = new SortedDictionary<char, SortedDictionary<string, RecursoItem>>();

        if (filtradaAtual != null && filtradaAtual.Count != 0)
        {
            foreach (Alocacao aloc in filtradaAtual)
            {
                SortedDictionary<string, RecursoItem> dicRecurso;
                char tipo = aloc.Recurso.Tipo;
                if (tipo == 'D' || tipo == 'X') // sala dupla é lab
                    tipo = 'L';
                if (!dic.ContainsKey(tipo))
                {
                    dicRecurso = new SortedDictionary<string, RecursoItem>();
                    dic[tipo] = dicRecurso;
                }
                else
                    dicRecurso = dic[tipo];

                RecursoItem rec = new RecursoItem();
                //                if (!dicRecurso.ContainsKey(aloc.Recurso.Abrev))
                //                    rec = new RecursoItem();
                //                else
                //                    rec = dicRecurso[aloc.Recurso.Abrev];
                rec.NomeCompleto = aloc.Recurso.Descricao;
                rec.HorarioAtual = aloc.Horario;

                if (aloc.Aula != null)
                {
                    rec.DescricaoAtual = aloc.Aula.TurmaId.Disciplina.Nome + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                    rec.DescricaoAtualCurta = getNomeMaisOuMenosCurtoDisciplina(aloc.Aula.TurmaId.Disciplina.Nome) + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                    rec.ResponsavelAtualCurto = getNomeSobrenomeProfessor(aloc.Aula.TurmaId.Professor.Nome);
                    rec.ResponsavelAtual = getNomeSobrenomeProfessor(aloc.Aula.TurmaId.Professor.Nome);
                }
                else
                {
                    rec.DescricaoAtual = aloc.Evento.Descricao;
                    rec.DescricaoAtualCurta = getNomeMaisOuMenosCurtoDisciplina(aloc.Evento.Titulo);
                    rec.ResponsavelAtual = getNomeSobrenomeProfessor(aloc.Evento.Responsavel);
                    rec.ResponsavelAtualCurto = getNomeSobrenomeProfessor(rec.ResponsavelAtual);
                }
                rec.Status = StatusRecurso.EmUsoEDisponivel;
                dicRecurso[aloc.Recurso.Abrev] = rec;
            }
        }

        List<Alocacao> filtradaProx = ProcuraProximoHorario(listaAlocacoes, ref pos);
        if (filtradaProx != null && filtradaProx.Count != 0)
        {
            foreach (Alocacao aloc in filtradaProx)
            {
                SortedDictionary<string, RecursoItem> dicRecurso;
                char tipo = aloc.Recurso.Tipo;
                if (tipo == 'D' || tipo == 'X') // sala dupla é lab
                    tipo = 'L';
                if (!dic.ContainsKey(tipo))
                {
                    dicRecurso = new SortedDictionary<string, RecursoItem>();
                    dic[tipo] = dicRecurso;
                }
                else
                    dicRecurso = dic[tipo];

                RecursoItem rec = new RecursoItem();
                if (!dicRecurso.ContainsKey(aloc.Recurso.Abrev))
                {
                    rec = new RecursoItem();
                    rec.HorarioProx = aloc.Horario;
                    rec.NomeCompleto = aloc.Recurso.Descricao;
                }
                else
                {
                    rec = dicRecurso[aloc.Recurso.Abrev];
                    rec.HorarioProx = aloc.Horario;
                }

                if (aloc.Aula != null)
                {
                    rec.DescricaoProx = aloc.Aula.TurmaId.Disciplina.Nome + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                    rec.DescricaoProxCurta = getNomeMaisOuMenosCurtoDisciplina(aloc.Aula.TurmaId.Disciplina.Nome) + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                    rec.ResponsavelProxCurto = getNomeSobrenomeProfessor(aloc.Aula.TurmaId.Professor.Nome);
                    rec.ResponsavelProx = getNomeSobrenomeProfessor(aloc.Aula.TurmaId.Professor.Nome);
                }
                else
                {
                    rec.DescricaoProx = aloc.Evento.Descricao;
                    rec.DescricaoProxCurta = getNomeMaisOuMenosCurtoDisciplina(aloc.Evento.Titulo);
                    rec.ResponsavelProx = getNomeSobrenomeProfessor(aloc.Evento.Responsavel);
                    rec.ResponsavelProxCurto = getNomeSobrenomeProfessor(aloc.Evento.Responsavel);
                }
                dicRecurso[aloc.Recurso.Abrev] = rec;
            }
        }

        // Se houver, copia reservas da 309/312 e 409/412 para 309 + 312 e 409 + 412
        if (dic.ContainsKey('L'))
        {
            SortedDictionary<string, RecursoItem> dicRecurso = dic['L'];
            if (dicRecurso.ContainsKey("309/312   "))
            {
                CopiaSalaDupla(dicRecurso, "309/312   ", "309       ", "312       ");
            }
            if (dicRecurso.ContainsKey("409/412   "))
            {
                CopiaSalaDupla(dicRecurso, "409/412   ", "409       ", "412       ");
            }
        }
        //if (pos < horarios.Count - 1) // se nao estivermos ja no ultimo horario... 
        //    {
        // lblAtual.Text = "Horário atual: " + horarioAtual;//+" - "+nowTime.ToString();
        // lblProximo.Text = "Proximo horario: " + horarioProx;
        if (filtradaAtual.Count == 0 && filtradaProx.Count == 0)
        {
            string newContent = @"
        <div class='row'>
            <div class='category'></div>
            <div class='grid'>
                <div class='block new-category'><span>Não há recursos alocados para hoje</span></div>
            </div>
        </div>";
            container.InnerHtml = newContent;
        }
        else
        {
            container.InnerHtml = "";
            char[] keySeq = { 'A', 'L', 'N', 'H', 'K', 'S' };
            foreach (var key in keySeq)
            {
                if (!dic.ContainsKey(key))
                    continue;
                SortedDictionary<string, RecursoItem> dicRecurso = dic[key];
                string row = string.Format(@"
                 <div class='row {0}'>
                   <div class='category'><img src='/srrc/img/{0}.png' alt=''><span>{1}</span></div>
                   <div class='grid'>
                   ", dicIcones[key][0], dicIcones[key][1]);
                bool usaDescricaoLonga = false;
                if (dicRecurso.Count <= 2)
                    usaDescricaoLonga = true;
                var sortedDicRecurso = dicRecurso.OrderBy(kvp1 => kvp1.Value.ResponsavelAtual == null)
                                                 .ThenBy(kvp2 => kvp2.Value.ResponsavelAtual)
                                                 .ThenBy(kvp3 => kvp3.Value.ResponsavelProx == null)
                                                 .ThenBy(kvp3 => kvp3.Value.ResponsavelProx)
                                                 .ToDictionary(kvp4 => kvp4.Key, kvp4 => kvp4.Value);
                foreach (var recKV in sortedDicRecurso)
                {
                    //Debug.WriteLine("[" + recKV.Key + "]");
                    RecursoItem recItem = recKV.Value;
                    if (recItem.DescricaoAtual == null && recItem.DescricaoProx == null)
                        recItem.Status = StatusRecurso.Disponivel;
                    else if (recItem.DescricaoAtual == null && recItem.DescricaoProx != null)
                        recItem.Status = StatusRecurso.DisponivelEReservado;
                    else if (recItem.DescricaoAtual != null && recItem.DescricaoProx == null)
                        recItem.Status = StatusRecurso.EmUsoEDisponivel;
                    else if (recItem.DescricaoAtual != null && recItem.DescricaoProx != null)
                        recItem.Status = StatusRecurso.EmUsoEReservado;
                    Debug.WriteLine(" " + recKV.Key + " " + recItem.HorarioAtual + ": " + recItem.DescricaoAtual + " (" + recItem.ResponsavelAtual + ") " + recItem.Status);
                    Debug.WriteLine(" " + recKV.Key + " " + recItem.HorarioProx + ": " + recItem.DescricaoProx + " (" + recItem.ResponsavelProx + ")");
                    string innerText = "";
                    string tooltip = "";
                    if (ocultaDescricaoCurta)
                    {
                        recItem.DescricaoAtualCurta = "";
                        recItem.DescricaoProxCurta = "";
                    }
                    else
                    {
                        recItem.ResponsavelAtualCurto += " \u00b7 ";
                        recItem.ResponsavelProxCurto += " \u00b7 ";
                        recItem.ResponsavelAtual += " \u00b7 ";
                        recItem.ResponsavelProx += " \u00b7 ";
                    }
                    switch (recItem.Status)
                    {
                        case StatusRecurso.Disponivel:
                            innerText = "";
                            break;
                        case StatusRecurso.DisponivelEReservado:
                            tooltip = recItem.HorarioProx + ": " + recItem.ResponsavelProx + recItem.DescricaoProx;
                            if (usaDescricaoLonga)
                                innerText = "<b>" + recItem.HorarioProx + "</b>: " + recItem.ResponsavelProx + recItem.DescricaoProx;
                            else
                                innerText = "<b>" + recItem.HorarioProx + "</b>: " + recItem.ResponsavelProxCurto + recItem.DescricaoProxCurta;
                            break;
                        case StatusRecurso.EmUsoEDisponivel:
                            tooltip = recItem.HorarioAtual + ": " + recItem.ResponsavelAtual + recItem.DescricaoAtual;
                            if (usaDescricaoLonga)
                                innerText = "<b>" + recItem.HorarioAtual + "</b>: " + recItem.ResponsavelAtual + recItem.DescricaoAtual;
                            else
                                innerText = "<b>" + recItem.HorarioAtual + "</b>: " + recItem.ResponsavelAtualCurto + recItem.DescricaoAtualCurta;
                            break;
                        case StatusRecurso.EmUsoEReservado:
                            if (usaDescricaoLonga)
                            {
                                innerText = "<b>" + recItem.HorarioAtual + "</b>: " + recItem.ResponsavelAtual + recItem.DescricaoAtual;
                                innerText += "<br><b>" + recItem.HorarioProx + "</b>: " + recItem.ResponsavelProx + recItem.DescricaoProx;
                            }
                            else
                            {
                                innerText = "<b>" + recItem.HorarioAtual + "</b>: " + recItem.ResponsavelAtualCurto + recItem.DescricaoAtualCurta;
                                innerText += "<br><b>" + recItem.HorarioProx + "</b>: " + recItem.ResponsavelProxCurto + recItem.DescricaoProxCurta;
                            }
                            tooltip = recItem.HorarioAtual + ": " + recItem.ResponsavelAtual + recItem.DescricaoAtual;
                            tooltip += "\n" + recItem.HorarioProx + ": " + recItem.ResponsavelProx + recItem.DescricaoProx;
                            break;
                    }
                    string retiradaStatus = "";
                    //Debug.WriteLine("NOME RECURSO: " + recItem.DescricaoAtual + " [" + recItem.NomeCompleto + "]");
                    if (recItem.NomeCompleto != null)
                        retiradaStatus = logDataDAO.GetUltimoStatus(recItem.NomeCompleto);
                    tooltip += "\n" + retiradaStatus;
                    string colorStatus = "retirado";
                    if (retiradaStatus == "Disponível")
                        colorStatus = "disponivel";
                    row += string.Format(@"
                    <div class='block {0} {1}'>{3} <span class='recurso'>{4}</span><div class='tooltip'>{2}</div></div>
                    ", dicCoresStatus[recItem.Status], colorStatus, tooltip, recKV.Key, innerText);
                }
                row += "</div></div>";
                container.InnerHtml += row;
            }
        }
    }
}
