using BusinessData.BusinessLogic;
using BusinessData.DataAccess;
using BusinessData.Entities;
using BusinessData.Distribuicao.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices.AccountManagement;

public partial class UserControls_DashboardRecursos : System.Web.UI.UserControl
{
    private List<string> horarios;
    private List<TimeSpan> horariosTime;
    private SRRCDAO logDataDAO = new SRRCDAO();

    private DateTime dataHoraForcada;
    private bool forcaDataHora = false;
    private bool ocultaDescricaoCurta = false;

    private enum StatusRecurso
    {
        Disponivel, Retirado, SemInfo
    }

    private class RecursoItem
    {
        public string HorarioAtual;
        public string NomeCompleto;
        public string NomeCurto;
        public char Tipo;
        public string DescricaoAtualCurta;
        public string DescricaoAtual;
        public string ResponsavelAtual;
        public string ResponsavelAtualCurto;
        public StatusRecurso Status;
        public LogData latest;
    }

    private Dictionary<char, string[]> dicIcones = new Dictionary<char, string[]>
    {
        { 'L', new string[] { "lab", "Labs", "#feff00" } },
        { 'A', new string[] { "auditorio", "Auditórios", "#FFEEDD" } },
        { 'N', new string[] { "notebook", "Notebooks", "#CEFD30" } },
        { 'K', new string[] { "cabo-vga", "Kits VGA", "#CA2098" } },
        { 'H', new string[] { "cabo-hdmi", "Kits HDMI", "#F97845" } },
        { 'S', new string[] { "speaker", "Caixas de Som", "#F05692" } }
    };

    private Dictionary<StatusRecurso, string> dicCoresStatus = new Dictionary<StatusRecurso, string>
    {
        { StatusRecurso.Disponivel, "emusoedisp" },
        { StatusRecurso.Retirado, "emusoereserv" }
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

        if (Request.QueryString["descricao"] != null && Request.QueryString["descricao"] == "0")
            ocultaDescricaoCurta = true;

        foreach (string hor in Enum.GetNames(typeof(Horarios.HorariosPUCRS)))
        {
            horariosTime.Add(Horarios.ParseToDateTime(hor).TimeOfDay);
            horarios.Add(hor.ToString());
        }

        if (!IsPostBack)
            Timer1_Tick(null, null);
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        VisualizarAlocacoesData();
    }

    private List<Alocacao> ProcuraProximoHorario(List<Alocacao> lista, ref int pos)
    {
        List<Alocacao> filtradaAtual = new List<Alocacao>();
        bool achei = false;
        while (filtradaAtual.Count == 0)
        {
            if (pos > horarios.Count - 1)
                break;
            string horarioAtual = horarios[pos];
            foreach (Alocacao aloc in lista)
            {
                if (aloc.Horario != horarioAtual && achei)
                    break;
                if (aloc.Horario == horarioAtual)
                {
                    filtradaAtual.Add(aloc);
                    achei = true;
                }
            }
            pos++;
        }
        return filtradaAtual;
    }

    private List<RecursoItem> GroupRecursos(List<RecursoItem> lista)
    {
        var grupos = new List<RecursoItem>();
        var agrupados = lista.GroupBy(ri => new
        {
            ri.ResponsavelAtual,
            ri.DescricaoAtual,
            ri.Tipo,
            ri.HorarioAtual
        });

        foreach (var grupo in agrupados)
        {
            RecursoItem base_ = grupo.First();
            base_.NomeCurto = string.Join("/", grupo.Select(ri => ri.NomeCurto));
            base_.NomeCompleto = grupo.First().NomeCompleto;
            if (grupo.Any(ri => ri.Status == StatusRecurso.Retirado))
                base_.Status = StatusRecurso.Retirado;
            else if (grupo.Any(ri => ri.Status == StatusRecurso.Disponivel))
                base_.Status = StatusRecurso.Disponivel;
            grupos.Add(base_);
        }
        return grupos;
    }

    private void VisualizarAlocacoesData()
    {
        DateTime now = forcaDataHora ? dataHoraForcada : DateTime.Now;
        DateTime hoje = now.Date;
        TimeSpan nowTime = now.TimeOfDay;

        AlocacaoBO controladorAlocacoes = new AlocacaoBO();
        List<Alocacao> listaAlocacoes = controladorAlocacoes.GetAlocacoesByDataFull(hoje, (BusinessData.Entities.Calendario)Session["Calendario"]);

        int pos;
        TimeSpan ts = nowTime;
        if (nowTime < horariosTime[0])
            pos = 0;
        else
            for (pos = 0; pos < horarios.Count - 1; pos++)
            {
                if (nowTime >= horariosTime[pos] && nowTime < horariosTime[pos + 1])
                {
                    ts = nowTime.Subtract(horariosTime[pos]);
                    Debug.WriteLine("Timedelta: " + ts);
                    if (ts.TotalMinutes > 45)
                        pos++;
                    break;
                }
            }

        if (pos == horarios.Count)
            pos--;

        int posAula = pos;

        TimeSpan deltaNow = nowTime.Subtract(horariosTime[posAula]);
        TimeSpan deltaProx = nowTime;
        Debug.WriteLine("deltaNow: " + deltaNow);
        if (posAula < horarios.Count - 1)
        {
            deltaProx = nowTime.Subtract(horariosTime[posAula + 1]);
            Debug.WriteLine("deltaProx: " + deltaProx);
        }

        List<Alocacao> filtradaAtual = ProcuraProximoHorario(listaAlocacoes, ref pos);
        List<Alocacao> filtradaProx = ProcuraProximoHorario(listaAlocacoes, ref pos);

        List<RecursoItem> listaRecursosAtual = new List<RecursoItem>();
        List<RecursoItem> listaRecursosProx = new List<RecursoItem>();

        foreach (List<Alocacao> lista in new List<List<Alocacao>> { filtradaAtual, filtradaProx })
        {
            foreach (Alocacao aloc in lista)
            {
                RecursoItem rec = new RecursoItem();
                if (aloc.Aula != null)
                {
                    rec.NomeCompleto = aloc.Recurso.Descricao;
                    rec.HorarioAtual = aloc.Horario;
                    rec.NomeCurto = aloc.Recurso.Abrev;
                    rec.Tipo = aloc.Recurso.Tipo;
                    rec.DescricaoAtual = aloc.Aula.TurmaId.Disciplina.Nome + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                    rec.DescricaoAtualCurta = getNomeCurtoDisciplina(aloc.Aula.TurmaId.Disciplina.Nome) + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                    rec.ResponsavelAtual = getNomeSobrenomeProfessor(aloc.Aula.TurmaId.Professor.Nome);
                    rec.ResponsavelAtualCurto = aloc.Aula.TurmaId.Professor.Curto != null
                        ? aloc.Aula.TurmaId.Professor.Curto
                        : getNomeCurtoProfessor(aloc.Aula.TurmaId.Professor.Nome);
                    string stat = logDataDAO.GetUltimoStatus(rec.NomeCompleto);
                    LogData latest = logDataDAO.FindLatestActivity(rec.NomeCompleto);
                    rec.latest = null;
                    if (stat.StartsWith("Retirado"))
                    {
                        rec.Status = StatusRecurso.Retirado;
                        rec.latest = latest;
                    }
                    else if (stat.StartsWith("Disponível"))
                        rec.Status = StatusRecurso.Disponivel;
                    else
                        rec.Status = StatusRecurso.SemInfo;
                    if (aloc.Aula.TurmaId.Notebook)
                    {
                        string sala = aloc.Aula.TurmaId.Sala.Replace("32/A/", "").Replace("15/A/", "");
                        rec.NomeCurto = rec.NomeCurto + "/ " + sala;
                    }
                }
                else if (aloc.Evento != null)
                {
                    rec.NomeCompleto = aloc.Recurso.Descricao;
                    rec.HorarioAtual = aloc.Horario;
                    rec.NomeCurto = aloc.Recurso.Abrev;
                    rec.Tipo = aloc.Recurso.Tipo;
                    rec.DescricaoAtual = aloc.Evento.Descricao;
                    rec.DescricaoAtualCurta = getNomeMaisOuMenosCurtoDisciplina(aloc.Evento.Titulo);
                    rec.ResponsavelAtual = getNomeSobrenomeProfessor(aloc.Evento.Responsavel).Trim();
                    if (rec.ResponsavelAtual.ToLower().StartsWith("prof."))
                        rec.ResponsavelAtual = aloc.Evento.Responsavel.Substring(5).Trim();
                    if (rec.ResponsavelAtual.ToLower().StartsWith("profa."))
                        rec.ResponsavelAtual = aloc.Evento.Responsavel.Substring(6).Trim();
                    rec.ResponsavelAtualCurto = getNomeCurtoProfessor(rec.ResponsavelAtual);
                    string stat = logDataDAO.GetUltimoStatus(rec.NomeCompleto);
                    LogData latest = logDataDAO.FindLatestActivity(rec.NomeCompleto);
                    rec.latest = null;
                    if (stat.StartsWith("Retirado"))
                    {
                        rec.Status = StatusRecurso.Retirado;
                        rec.latest = latest;
                    }
                    else if (stat.StartsWith("Disponível"))
                        rec.Status = StatusRecurso.Disponivel;
                    else
                        rec.Status = StatusRecurso.SemInfo;
                }
                if (lista == filtradaAtual)
                    listaRecursosAtual.Add(rec);
                else
                    listaRecursosProx.Add(rec);
            }
        }

        if (filtradaAtual.Count == 0 && filtradaProx.Count == 0)
        {
            container.InnerHtml = @"
        <div class='row'>
            <div class='category'></div>
            <div class='grid'>
                <div class='block new-category'><span>Não há recursos alocados para hoje</span></div>
            </div>
        </div>";
            return;
        }

        container.InnerHtml = "";

        listaRecursosAtual = GroupRecursos(listaRecursosAtual).OrderBy(ri => ri.ResponsavelAtualCurto).ThenBy(ri => ri.DescricaoAtual).ToList();
        listaRecursosProx = GroupRecursos(listaRecursosProx).OrderBy(ri => ri.ResponsavelAtualCurto).ThenBy(ri => ri.DescricaoAtual).ToList();

        string horarioAtual = "";
        foreach (var ri in listaRecursosAtual) { horarioAtual = ri.HorarioAtual; break; }

        int cont = 0;
        string block = "";
        TimeSpan noventa = TimeSpan.FromMinutes(90);
        foreach (List<RecursoItem> lista in new List<List<RecursoItem>> { listaRecursosAtual, listaRecursosProx })
        {
            cont += 1;
            if (lista.Count == 0)
                continue;
            horarioAtual = lista[0].HorarioAtual;

            string infoHorario = deltaNow.ToString();
            if (deltaNow.TotalMinutes > 0)
                infoHorario = "<i class=\"bi bi-hourglass-split\"></i>" + noventa.Subtract(deltaNow).ToString(@"hh\:mm");
            else if (deltaNow.TotalMinutes < 0)
                infoHorario = "<i class=\"bi bi-hourglass-split\"></i>" + deltaNow.ToString(@"hh\:mm");

            if (lista == listaRecursosProx && deltaProx != nowTime)
                infoHorario = "<i class=\"bi bi-hourglass-top\"></i>" + deltaProx.ToString(@"hh\:mm");

            block += "<div class=\"col-12 col-lg-6 schedule-col\">\n<div class=\"card shadow-sm schedule-card\">";
            block += string.Format("<div class=\"schedule-header text-success\">HORÁRIO {0} {1}</div>\n", horarioAtual, infoHorario);
            block += "<div class=\"list-group list-group-flush\">\n";

            foreach (RecursoItem ri in lista)
            {
                if (ri.NomeCompleto == null) continue;

                bool isRetiradoAtual = cont == 1 && ri.Status == StatusRecurso.Retirado;

                string responsavelAttr = ri.ResponsavelAtual != null
                    ? ri.ResponsavelAtual.Replace("\"", "&quot;").Trim()
                    : "";

                block += string.Format(
                    "<div class=\"list-group-item d-flex justify-content-between align-items-center nomedisc\" data-responsavel=\"{0}\">\n",
                    responsavelAttr);

                if (isRetiradoAtual)
                {
                    string textoPrimario = string.Format("{0} - {1}", ri.ResponsavelAtualCurto, ri.DescricaoAtualCurta)
                        .Replace("\"", "&quot;");
                    string responsavelRetirada = "";
                    if(ri.latest != null)
                    {
                        string horarioRetirada = ri.latest.Horario.ToString(@"dd/MM HH:mm");
                        if(ri.latest.Horario.Day == hoje.Day)
                            horarioRetirada = ri.latest.Horario.ToString(@"HH:mm");
                        responsavelRetirada = getNomeCurtoProfessor(ri.latest.Usuario) + " - " + horarioRetirada;
                    }
                    block += string.Format(
                        "<span class=\"text-alternating\" data-text-primary=\"{0}\" data-text-alt=\"&#9888; {1}\">{0}</span>\n<div class=\"resource-container\">\n",
                        textoPrimario, responsavelRetirada);
                }
                else
                {
                    block += string.Format("<span>{0} - {1}</span>\n<div class=\"resource-container\">\n",
                        ri.ResponsavelAtualCurto, ri.DescricaoAtualCurta);
                }

                string destaque = "";
                string destaqueText = "";
                string corBadge = "bg-dark";
                if (cont == 1 && ri.Status == StatusRecurso.Retirado)
                {
                    destaque = "badge-active";
                    destaqueText = "text-danger";
                }

                string recursoIcone = "";
                switch (ri.Tipo)
                {
                    case 'A': recursoIcone = "bi-easel2"; corBadge = "auditorio"; break;
                    case 'H': recursoIcone = "bi-hdmi"; corBadge = "cabo-hdmi"; break;
                    case 'K': recursoIcone = "bi-display"; corBadge = "cabo-vga"; break;
                    case 'L':
                    case 'D':
                        if (ri.NomeCurto.StartsWith("RN")) { recursoIcone = "bi-laptop"; corBadge = "notebook"; }
                        else { recursoIcone = "bi-pc-display"; corBadge = "lab"; }
                        break;
                    case 'N': recursoIcone = "bi-laptop"; corBadge = "notebook"; break;
                    case 'S': recursoIcone = "bi-speaker"; corBadge = "speaker"; break;
                    case 'X':
                        if (ri.NomeCurto.StartsWith("211")) { recursoIcone = "bi-pc-display"; corBadge = "lab"; }
                        break;
                }
                if (ri.NomeCurto.StartsWith("211"))
                    recursoIcone = "bi-pc-display";

                block += string.Format("<i class=\"bi {0} resource-icon {1}\"></i>", recursoIcone, destaqueText);
                block += string.Format("<span class=\"badge {1} resource-tag {2}\">{0}</span>\n</div>\n</div>", ri.NomeCurto, destaque, corBadge);
            }

            block += "</div>";
            block += "</div></div>";
        }

        container.InnerHtml = string.Format("<div class=\"row g-4 justify-content-center\">{0}</div>", block);
    }

    public string getNomeCurtoDisciplina(string nome)
    {
        if (nome.Length <= 20)
            return nome;
        string curto = "";
        foreach (string pal in nome.Split())
        {
            string palCurta = pal;
            if (pal.Length > 6)
            {
                palCurta = pal.Substring(0, 4);
                int pos = 4;
                while (pos < pal.Length)
                {
                    palCurta += pal[pos];
                    if (pal[pos] == 'a' || pal[pos] == 'e' || pal[pos] == 'á' || pal[pos] == 'ê'
                       || pal[pos] == 'o' || pal[pos] == 'u')
                        pos++;
                    else break;
                }
                palCurta += ". ";
            }
            curto += palCurta + " ";
        }
        return curto;
    }

    public string getNomeMaisOuMenosCurtoDisciplina(string nome, int maxLen = 30)
    {
        if (nome.Length <= maxLen)
            return nome;

        var vogais = new System.Collections.Generic.HashSet<char>
        {
            'a', 'á', 'à', 'â', 'ã', 'e', 'é', 'ê', 'i', 'í', 'o', 'ó', 'ô', 'õ', 'u', 'ú'
        };
        var stopWords = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "da", "de", "do", "das", "dos", "à", "á", "e", "ao", "a", "para", "em", "na", "no", "-"
        };
        var numerals = new System.Collections.Generic.HashSet<string>
        {
            "I", "II", "III", "IV", "V", "VI", "VII", "VIII"
        };

        string[] palavras = nome.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string numeral = "";
        var partes = new List<string>();

        foreach (string pal in palavras)
        {
            if (numerals.Contains(pal)) { numeral = " " + pal; continue; }
            if (stopWords.Contains(pal) && partes.Count > 0) continue;
            if (pal.Length > 6)
            {
                string abrev = pal.Substring(0, 4);
                int pos = 4;
                while (pos < pal.Length)
                {
                    abrev += pal[pos];
                    if (vogais.Contains(char.ToLower(pal[pos]))) pos++;
                    else break;
                }
                partes.Add(abrev + ".");
            }
            else
            {
                partes.Add(pal);
            }
        }

        string curto = string.Join(" ", partes) + numeral;
        if (curto.Length > maxLen)
            curto = curto.Substring(0, maxLen - 1) + "\u2026";
        return curto;
    }

    public string getNomeCurtoProfessor(string nome)
    {
        string[] nomes = nome.Trim().Split();
        if (nomes.Length == 1)
            return nome.Length <= 10 ? nome : nome.Substring(0, 10);
        string ultNome = nomes[nomes.Length - 1];
        ultNome = ultNome[0] + ultNome.Substring(1).ToLower();
        return nomes[0][0] + ". " + (ultNome.Length <= 10 ? ultNome : ultNome.Substring(0, 10) + ".");
    }

    public static string toCamelCase(string nome)
    {
        return nome[0] + nome.Substring(1).ToLower();
    }

    public static string getNomeSobrenomeProfessor(string nome)
    {
        string[] nomes = nome.Split();
        if (nomes.Length == 1)
            return toCamelCase(nome);
        return toCamelCase(nomes[0]) + " " + toCamelCase(nomes[nomes.Length - 1]);
    }

    public void Refresh()
    {
        VisualizarAlocacoesData();
    }
}
