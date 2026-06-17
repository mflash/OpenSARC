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

public partial class UserControls_DashboardUnificado : System.Web.UI.UserControl
{

    public string ContainerCssClass { get; set; } // padding padrão para o container, pode ser sobrescrito ao usar o controle
    public bool ExibeRecursosRetirados { get; set; } // flag para exibir os recursos retirados neste momento (usada no painel de retiradas)

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

    private class HorarioItem
    {
        public string Horario;
        public string Descricao;
        public string DescricaoCurta;
        public string Responsavel;
        public string ResponsavelCurto;
    }

    private class RecursoItem
    {
        public char Tipo;
        public string NomeCurto;
        public string NomeCompleto;

        public HorarioItem atual;
        public HorarioItem prox;

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
    protected void Page_Load(object sender, EventArgs e)
    {
        horarios = new List<string>();
        horariosTime = new List<TimeSpan>();

        if (!string.IsNullOrEmpty(ContainerCssClass))
            container.Attributes["class"] = "container-fluid py-1 " + ContainerCssClass;

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

    private void VisualizarAlocacoesData()
    {
        DateTime now = forcaDataHora ? dataHoraForcada : DateTime.Now;
        DateTime hoje = now.Date;
        TimeSpan nowTime = now.TimeOfDay;

        lblDataHora.Text = now.ToString();

        RecursosBO recursosBO = new RecursosBO();
        AlocacaoBO controladorAlocacoes = new AlocacaoBO();
        ProfessoresBO professoresBO = new ProfessoresBO();
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

        Dictionary<string, RecursoItem> dicRecursos = new Dictionary<string, RecursoItem>();
        HashSet<String> recursosAlocadosAgora = new HashSet<string>();

        foreach (List<Alocacao> lista in new List<List<Alocacao>> { filtradaAtual, filtradaProx })
        {
            foreach (Alocacao aloc in lista)
            {
                char tipo = aloc.Recurso.Tipo;
                RecursoItem rec = null;
                if (dicRecursos.ContainsKey(aloc.Recurso.Abrev))
                {
                    rec = dicRecursos[aloc.Recurso.Abrev];
                }
                else
                {
                    rec = new RecursoItem();
                    rec.Tipo = aloc.Recurso.Tipo;
                    rec.NomeCurto = aloc.Recurso.Abrev;
                    rec.NomeCompleto = aloc.Recurso.Descricao;

                    rec.atual = new HorarioItem();
                    rec.prox = new HorarioItem();
                    dicRecursos[rec.NomeCurto] = rec;
                }

                // Determina se é horário atual ou seguinte
                HorarioItem current = null;
                if (lista == filtradaAtual)
                    current = rec.atual;
                else
                    current = rec.prox;

                current.Horario = aloc.Horario;

                // Recupera status do recurso
                string stat = logDataDAO.GetUltimoStatus(rec.NomeCompleto);
                LogData latest = logDataDAO.FindLatestActivity(rec.NomeCompleto);
                rec.latest = null;
                if (stat.StartsWith("Retirado"))
                {
                    rec.Status = StatusRecurso.Retirado;
                    rec.latest = latest;
                    // Marca como retirado neste momento
                    recursosAlocadosAgora.Add(rec.NomeCompleto);
                }
                else if (stat.StartsWith("Disponível"))
                    rec.Status = StatusRecurso.Disponivel;
                else
                    rec.Status = StatusRecurso.SemInfo;

                // Se for aula...
                if (aloc.Aula != null)
                {
                    current.Descricao = aloc.Aula.TurmaId.Disciplina.Nome + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                    current.DescricaoCurta = getNomeCurtoDisciplina(aloc.Aula.TurmaId.Disciplina.Nome) + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                    current.Responsavel = getNomeSobrenomeProfessor(aloc.Aula.TurmaId.Professor.Nome);
                    current.ResponsavelCurto = aloc.Aula.TurmaId.Professor.Curto != null
                        ? aloc.Aula.TurmaId.Professor.Curto
                        : getNomeCurtoProfessor(aloc.Aula.TurmaId.Professor.Nome);
                    if (aloc.Aula.TurmaId.Notebook)
                    {
                        string sala = aloc.Aula.TurmaId.Sala.Replace("32/A/", "").Replace("15/A/", "");
                        rec.NomeCurto = rec.NomeCurto + "/ " + sala;
                    }
                }
                else if (aloc.Evento != null)
                {
                    current.Descricao = aloc.Evento.Descricao;
                    current.DescricaoCurta = getNomeMaisOuMenosCurtoDisciplina(aloc.Evento.Titulo);
                    current.Responsavel = aloc.Evento.Responsavel.Trim();

                    if (aloc.Evento.AutorId != null)
                    {
                        Professor prof = (Professor)professoresBO.GetPessoaById(aloc.Evento.AutorId.Id);
                        if (prof != null)
                        { // É professor
                            current.Responsavel = getNomeSobrenomeProfessor(prof.Nome).Trim();
                            current.ResponsavelCurto = prof.Curto != null ? prof.Curto : getNomeCurtoProfessor(prof.Nome).Trim();
                        }
                    }
                    else
                    {
                        if (current.Responsavel.ToLower().StartsWith("prof."))
                            current.Responsavel = aloc.Evento.Responsavel.Substring(5).Trim();
                        if (current.Responsavel.ToLower().StartsWith("profa."))
                            current.Responsavel = aloc.Evento.Responsavel.Substring(6).Trim();
                        current.ResponsavelCurto = getNomeCurtoProfessor(current.Responsavel);
                        current.Responsavel = getNomeSobrenomeProfessor(current.Responsavel).Trim();
                    }
                }
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

        if (ExibeRecursosRetirados)
            foreach (BusinessData.Entities.Recurso r in recursosBO.GetRecursos())
            {
                LogData latest = logDataDAO.FindLatestActivity(r.Descricao);
                if (latest != null)
                {
                    if (latest.Acao == "RETIRADA") // && !recursosAlocadosAgora.Contains(r.Descricao))
                    {
                        Debug.WriteLine("Recurso retirado: " + r.Descricao + " - " + r.Abrev);
                        RecursoItem rec = null;
                        if (dicRecursos.ContainsKey(r.Abrev))
                        {
                            rec = dicRecursos[r.Abrev];
                        }
                        else
                        {
                            rec = new RecursoItem();
                            rec.Tipo = r.Tipo;
                            rec.NomeCurto = r.Abrev;
                            rec.NomeCompleto = r.Descricao;

                            rec.atual = new HorarioItem();
                            rec.prox = new HorarioItem();
                            dicRecursos[rec.NomeCurto] = rec;
                        }

                        string horarioRetirada = latest.Horario.ToString(@"dd/MM HH:mm");
                        if (latest.Horario.Day == hoje.Day)
                            horarioRetirada = latest.Horario.ToString(@"HH:mm");
                        rec.atual.ResponsavelCurto = getNomeCurtoProfessor(latest.Usuario);
                        rec.atual.Responsavel = getNomeCurtoProfessor(latest.Usuario);
                        rec.atual.DescricaoCurta = horarioRetirada;
                        rec.atual.Descricao = "RETIRADA";
                        rec.atual.Horario = horarioRetirada;
                        //rec.ResponsavelAtualCurto = "&#9888; Desconhecido";
                        rec.Status = StatusRecurso.Retirado;
                        rec.latest = latest;
                    }
                }
            }

        container.InnerHtml = "";

        List<RecursoItem> listaRecursos = dicRecursos.ToList()
            .Where(ri => ri.Value.atual.ResponsavelCurto != null)
            .OrderBy(ri => ri.Value.atual.ResponsavelCurto)
            .ThenBy(ri => ri.Value.NomeCurto)
            .Select(kv => kv.Value).ToList();

        string horarioAtual = "";
        string horarioProx = "";
        TimeSpan noventa = TimeSpan.FromMinutes(90);

        foreach (RecursoItem ri in listaRecursos)
        {
            if (horarioAtual == "" && ri.atual.Horario != null && ri.atual.Horario.Length == 2)
                horarioAtual = ri.atual.Horario;
            if (horarioProx == "" && ri.prox.Horario != null && ri.prox.Horario.Length == 2)
                horarioProx = ri.prox.Horario;
            if (horarioAtual != "" && horarioProx != "") break;
        }

        string infoHorario = deltaNow.ToString();
        if (deltaNow.TotalMinutes > 0)
            infoHorario = "<i class=\"bi bi-hourglass-split\"></i>" + noventa.Subtract(deltaNow).ToString(@"hh\:mm");
        else if (deltaNow.TotalMinutes < 0)
            infoHorario = "<i class=\"bi bi-hourglass-split\"></i>" + deltaNow.ToString(@"hh\:mm");

        string block = "<div class=\"card shadow-sm schedule-card\">\n";
        block += "<div class=\"schedule-header\">\n";
        block += "<div class=\"dashboard-header-grid\">\n";
        block += "<div></div>\n";
        block += string.Format("<div class=\"text-success\">HORÁRIO {0} {1}</div>\n", horarioAtual, infoHorario);
        block += string.Format("<div class=\"text-secondary\">PRÓXIMO {0}</div>\n", horarioProx);
        block += "</div>\n</div>\n";
        block += "<div class=\"list-group list-group-flush\">\n";

        foreach (RecursoItem ri in listaRecursos)
        {
            if (ri.NomeCompleto == null) continue;

            bool isRetirado = ri.Status == StatusRecurso.Retirado;

            string responsavelAttr = ri.atual.Responsavel != null
                ? ri.atual.Responsavel.Replace("\"", "&quot;").Trim()
                : "";

            string recursoIcone = "";
            string corBadge = "bg-dark";
            string destaque = isRetirado ? "badge-active" : "";
            string destaqueText = isRetirado ? "text-danger" : "";

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

            block += string.Format(
                "<div class=\"list-group-item dashboard-row nomedisc\" data-responsavel=\"{0}\">\n",
                responsavelAttr);

            // Coluna 1: badge do recurso
            block += "<div class=\"resource-container\">\n";
            block += string.Format("<i class=\"bi {0} resource-icon {1}\"></i>", recursoIcone, destaqueText);
            block += string.Format("<span class=\"badge {0} resource-tag {1}\">{2}</span>\n", destaque, corBadge, ri.NomeCurto);
            block += "</div>\n";

            // Coluna 2: horário atual
            block += "<div class=\"schedule-cell\">\n";
            if (ri.atual.DescricaoCurta != null)
            {
                if (isRetirado)
                {
                    string textoPrimario = string.Format("{0} - {1}", ri.atual.ResponsavelCurto, ri.atual.DescricaoCurta)
                        .Replace("\"", "&quot;");
                    string responsavelRetirada = "";
                    if (ri.latest != null)
                    {
                        string horarioRetirada = ri.latest.Horario.ToString(@"dd/MM HH:mm");
                        if (ri.latest.Horario.Day == hoje.Day)
                            horarioRetirada = ri.latest.Horario.ToString(@"HH:mm");
                        responsavelRetirada = getNomeCurtoProfessor(ri.latest.Usuario) + " - " + horarioRetirada;
                    }
                    block += string.Format(
                        "<span class=\"text-alternating\" style=\"font-family: 'bootstrap-icons'\" data-text-primary=\"{0}\" data-text-alt=\" \u27a0 {1}\">{0}</span>\n",
                        textoPrimario, responsavelRetirada);
                }
                else
                {
                    block += string.Format("<span>{0} - {1}</span>\n", ri.atual.ResponsavelCurto, ri.atual.DescricaoCurta);
                }
            }
            else
            {
                block += "<span class=\"text-muted\">—</span>\n";
            }
            block += "</div>\n";

            // Coluna 3: próximo horário
            block += "<div class=\"schedule-cell schedule-cell-prox\">\n";
            if (ri.prox.DescricaoCurta != null)
            {
                block += string.Format("<span>{0} - {1}</span>\n", ri.prox.ResponsavelCurto, ri.prox.DescricaoCurta);
            }
            else
            {
                block += "<span class=\"text-muted\">—</span>\n";
            }
            block += "</div>\n";

            block += "</div>\n";
        }

        block += "</div>\n</div>\n";

        container.InnerHtml = string.Format("<div class=\"row g-2 justify-content-center\"><div class=\"col-12\">{0}</div></div>", block);
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

    public string toCamelCase(string nome)
    {
        return nome[0] + nome.Substring(1).ToLower();
    }

    public string getNomeSobrenomeProfessor(string nome)
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
