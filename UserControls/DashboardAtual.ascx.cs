using BusinessData.BusinessLogic;
using BusinessData.DataAccess;
using BusinessData.Distribuicao.Entities;
using BusinessData.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Calendario = BusinessData.Entities.Calendario;
using Recurso = BusinessData.Entities.Recurso;

public partial class UserControls_DashboardAtual : System.Web.UI.UserControl
{

    public string ContainerCssClass { get; set; } // padding padrão para o container, pode ser sobrescrito ao usar o controle
    public bool ExibeRecursosRetirados { get; set; } // flag para exibir os recursos retirados neste momento (usada no painel de retiradas)
    public bool IgnoraReservas { get; set; } // flag para não exibir as reservas de lab (não são mais usadas) no dashboard

    private List<string> horarios;
    private List<TimeSpan> horariosTime;
    private SRRCDAO logDataDAO = new SRRCDAO();

    private DateTime dataHoraForcada;
    private bool forcaDataHora = false;
    private bool apenasAtual = false;
    private bool ocultaDescricaoCurta = false;

    private enum StatusRecurso
    {
        Disponivel, Retirado, SemInfo
    }

    private class RecursoItem
    {
        public string Horario;
        public string Predio;
        public string Nome;
        public string Abrev;
        public string AbrevPura;
        public char Tipo;
        public string DescricaoCurta;
        public string Descricao;
        public string Responsavel;
        public string ResponsavelCurto;
        public string Matricula;
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

        if (!string.IsNullOrEmpty(ContainerCssClass))
            container.Attributes["class"] = "container-fluid py-1 px-0" + ContainerCssClass;

        if (Request.QueryString["datahora"] != null)
        {
            dataHoraForcada = DateTime.Parse(Request.QueryString["datahora"]);
            forcaDataHora = true;
        }

        if (Request.QueryString["apenasAtual"] != null)
        {
            apenasAtual = true;
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
            ri.ResponsavelCurto,
            ri.DescricaoCurta,
            ri.Tipo,
            ri.Horario
        });

        foreach (var grupo in agrupados)
        {
            RecursoItem base_ = grupo.First();
            base_.Abrev = string.Join("/", grupo.Select(ri => ri.Abrev));
            base_.Nome = grupo.First().Nome;
            if (grupo.Any(ri => ri.Status == StatusRecurso.Retirado))
                base_.Status = StatusRecurso.Retirado;
            else if (grupo.Any(ri => ri.Status == StatusRecurso.Disponivel))
                base_.Status = StatusRecurso.Disponivel;
            grupos.Add(base_);
        }
        return grupos; // TODO: corrigir agrupamento - por enquanto retorna a lista original
    }

    private void VisualizarAlocacoesData()
    {
        DateTime now = forcaDataHora ? dataHoraForcada : DateTime.Now;
        //now = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0);
        DateTime hoje = now.Date;
        TimeSpan nowTime = now.TimeOfDay;
        int diaSemNum = now.DayOfWeek == DayOfWeek.Sunday ? 1 : (int)now.DayOfWeek + 1;

        lblDataHora.Text = now.ToString();

        RecursosBO recursosBO = new RecursosBO();
        AlocacaoBO controladorAlocacoes = new AlocacaoBO();
        ProfessoresBO professoresBO = new ProfessoresBO();
        CalendariosBO calendariosBO = new CalendariosBO();
        TurmaBO turmasBO = new TurmaBO();

        List<Calendario> calendarios = calendariosBO.GetCalendarios();
        Calendario cal = calendarios[calendarios.Count - 1];

        Dictionary<string, List<Turma>> dicTurmas = new Dictionary<string, List<Turma>>();

        List<Turma> turmas = turmasBO.GetTurmas(cal);
        foreach (var turma in turmas)
        {
            string[] horariosTurma = Enumerable.Range(0, turma.DataHora.Length / 3)
                .Select(i => turma.DataHora.Substring(i * 3, 3))
                .ToArray();
            foreach (var horario in horariosTurma)
            {
                if (dicTurmas.ContainsKey(horario))
                    dicTurmas[horario].Add(turma);
                else
                    dicTurmas[horario] = new List<Turma> { turma };
            }
        }

        Dictionary<string, Professor> dicProfs = new Dictionary<string, Professor>();
        foreach (var prof in professoresBO.GetProfessores())
        {
            dicProfs.Add(prof.Matricula, prof);
        }
        List<Alocacao> listaAlocacoes = controladorAlocacoes.GetAlocacoesByDataFull(hoje, cal);

        Dictionary<string, string> dicRecursos = new Dictionary<string, string>();
        recursosBO.GetRecursos().ForEach(r => dicRecursos[r.Abrev] = r.Descricao);

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
                    if (ts.TotalMinutes > 80)
                        pos++;
                    break;
                }
            }

        if (pos == horarios.Count)
            pos--;

        int posAula = pos;
        int posAulaProx = pos + 1;

        TimeSpan deltaNow = nowTime.Subtract(horariosTime[posAula]);
        TimeSpan deltaProx = nowTime;
        Debug.WriteLine("deltaNow: " + deltaNow);
        if (posAula < horarios.Count - 1)
        {
            deltaProx = nowTime.Subtract(horariosTime[posAula + 1]);
            Debug.WriteLine("deltaProx: " + deltaProx);
        }
        else
        {
            posAulaProx--;
        }

        List<Alocacao> filtradaAtual = ProcuraProximoHorario(listaAlocacoes, ref pos);
        Debug.WriteLine("Atual: " + horarios[posAula]);
        List<Alocacao> filtradaProx = ProcuraProximoHorario(listaAlocacoes, ref pos);
        Debug.WriteLine("Prox : " + horarios[posAulaProx]);

        List<RecursoItem> listaRecursosAtual = new List<RecursoItem>();
        List<RecursoItem> listaRecursosProx = new List<RecursoItem>();

        Dictionary<string, RecursoItem> dicRecursosAtual = new Dictionary<string, RecursoItem>();
        Dictionary<string, RecursoItem> dicRecursosProx = new Dictionary<string, RecursoItem>();

        string atual = filtradaAtual.Count > 0 ? filtradaAtual[0].Horario : horarios[posAula];
        string prox = filtradaProx.Count > 0 ? filtradaProx[0].Horario : horarios[posAulaProx];
        atual = diaSemNum + atual;
        prox = diaSemNum + prox;

        if (!dicTurmas.ContainsKey(atual))
            dicTurmas.Add(atual, new List<Turma>());

        if (!dicTurmas.ContainsKey(prox))
            dicTurmas.Add(prox, new List<Turma>());

        HashSet<String> recursosAlocadosAgora = new HashSet<string>();

        foreach (List<Turma> turmasAux in new List<List<Turma>> { dicTurmas[atual], dicTurmas[prox] })
        {
            foreach (Turma t in turmasAux)
            {
                RecursoItem rec = new RecursoItem();
                string sala = t.Sala;
                if (sala.StartsWith("32"))
                {
                    sala = sala.Replace("32/A", "32");
                    rec.Predio = "32";
                }
                if (sala.StartsWith("15"))
                {
                    sala = sala.Replace("15/A", "15");
                    rec.Predio = "15";
                }
                if (sala.StartsWith("30/"))
                {
                    string[] dados = sala.Split('/');
                    sala = dados[2];
                    rec.Predio = dados[0] + "/" + dados[1];
                }
                rec.Abrev = sala;
                rec.AbrevPura = sala;
                rec.Nome = sala;
                if (turmasAux == dicTurmas[atual])
                    rec.Horario = atual.Substring(1);
                else
                    rec.Horario = prox.Substring(1);
                rec.Abrev = sala;
                rec.Tipo = 'N';
                if (rec.Predio == "15")
                    rec.Tipo = 'A';
                rec.Descricao = t.Disciplina.Nome + " (" + t.Numero.ToString() + ")";
                rec.DescricaoCurta = getNomeCurtoDisciplina(t.Disciplina.Nome) + " (" + t.Numero.ToString() + ")";
                rec.Responsavel = getNomeSobrenomeProfessor(t.Professor.Nome);
                rec.Matricula = t.Professor.Matricula;
                rec.ResponsavelCurto = t.Professor.Curto != null
                    ? t.Professor.Curto
                    : getNomeCurtoProfessor(t.Professor.Nome);
                rec.Status = StatusRecurso.Disponivel;
                Debug.WriteLine(t.DataHora + ": " + t.Numero + " - " + t.Disciplina.Nome + " - " + t.Professor.Nome + " - " + t.Sala);

                if (turmasAux == dicTurmas[atual])
                {
                    Debug.WriteLine("Atual: " + rec.AbrevPura + " - " + rec.Abrev + " - " + rec.Status);
                    if (!dicRecursosAtual.ContainsKey(rec.AbrevPura))
                        dicRecursosAtual.Add(rec.AbrevPura, rec);
                    if (rec.Status == StatusRecurso.Retirado)
                        recursosAlocadosAgora.Add(rec.AbrevPura);
                }
                else
                {
                    Debug.WriteLine("Prox: " + rec.AbrevPura + " - " + rec.Abrev + " - " + rec.Status);
                    if (!dicRecursosProx.ContainsKey(rec.AbrevPura))
                        dicRecursosProx.Add(rec.AbrevPura, rec);
                }
            }
        }

        if (filtradaAtual.Count == 0 && filtradaProx.Count == 0 && dicTurmas[atual].Count == 0 && dicTurmas[prox].Count == 0)
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

        if (!IgnoraReservas)
            foreach (List<Alocacao> lista in new List<List<Alocacao>> { filtradaAtual, filtradaProx })
            {
                foreach (Alocacao aloc in lista)
                {
                    RecursoItem rec = new RecursoItem();
                    if (aloc.Aula != null)
                    {
                        rec.Nome = aloc.Recurso.Descricao;
                        rec.Horario = aloc.Horario;
                        rec.Abrev = rec.AbrevPura = aloc.Recurso.Abrev;
                        rec.Tipo = aloc.Recurso.Tipo;
                        rec.Descricao = aloc.Aula.TurmaId.Disciplina.Nome + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                        rec.DescricaoCurta = getNomeCurtoDisciplina(aloc.Aula.TurmaId.Disciplina.Nome) + " (" + aloc.Aula.TurmaId.Numero.ToString() + ")";
                        rec.Responsavel = getNomeSobrenomeProfessor(aloc.Aula.TurmaId.Professor.Nome);
                        rec.Matricula = aloc.Aula.TurmaId.Professor.Matricula;
                        rec.ResponsavelCurto = aloc.Aula.TurmaId.Professor.Curto != null
                            ? aloc.Aula.TurmaId.Professor.Curto
                            : getNomeCurtoProfessor(aloc.Aula.TurmaId.Professor.Nome);

                        if (aloc.Aula.TurmaId.Notebook)
                        {
                            string sala = aloc.Aula.TurmaId.Sala.Replace("32/A/", "").Replace("15/A/", "");
                            rec.Abrev = rec.Abrev + "/" + sala;
                            rec.AbrevPura = sala;
                            if (dicRecursos.ContainsKey(sala))
                                rec.Nome = dicRecursos[sala];
                            else
                                Debug.WriteLine("ERRO: " + sala + " não encontrada");
                        }
                    }
                    else if (aloc.Evento != null)
                    {
                        rec.Nome = aloc.Recurso.Descricao;
                        rec.Horario = aloc.Horario;
                        rec.Abrev = rec.AbrevPura = aloc.Recurso.Abrev;
                        rec.Tipo = aloc.Recurso.Tipo;

                        rec.Descricao = aloc.Evento.Descricao;
                        rec.DescricaoCurta = getNomeMaisOuMenosCurtoDisciplina(aloc.Evento.Titulo);

                        rec.Responsavel = aloc.Evento.Responsavel.Trim();

                        if (aloc.Evento.AutorId != null)
                        {
                            Professor pes = (Professor)professoresBO.GetPessoaById(aloc.Evento.AutorId.Id);
                            if (pes != null)
                            { // É professor
                                Professor prof = pes as Professor;
                                rec.Responsavel = getNomeSobrenomeProfessor(prof.Nome).Trim();
                                rec.ResponsavelCurto = prof.Curto != null ? prof.Curto : getNomeCurtoProfessor(prof.Nome).Trim();
                                rec.Matricula = prof.Matricula;
                            }
                        }
                        else
                        {
                            if (rec.Responsavel.ToLower().StartsWith("prof."))
                                rec.Responsavel = aloc.Evento.Responsavel.Substring(5).Trim();
                            if (rec.Responsavel.ToLower().StartsWith("profa."))
                                rec.Responsavel = aloc.Evento.Responsavel.Substring(6).Trim();
                            rec.ResponsavelCurto = getNomeCurtoProfessor(rec.Responsavel);
                            rec.Responsavel = getNomeSobrenomeProfessor(rec.Responsavel).Trim();
                        }
                    }

                    string stat = logDataDAO.GetUltimoStatus(rec.Nome);
                    LogData latest = logDataDAO.FindLatestActivity(rec.Nome);
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

                    if (lista == filtradaAtual)
                    {
                        Debug.WriteLine("Atual: " + rec.AbrevPura + " - " + rec.Abrev + " - " + rec.Status);
                        if (!dicRecursosAtual.ContainsKey(rec.AbrevPura))
                            dicRecursosAtual.Add(rec.AbrevPura, rec);
                        if (rec.Status == StatusRecurso.Retirado)
                            recursosAlocadosAgora.Add(rec.AbrevPura);
                    }
                    else
                    {
                        Debug.WriteLine("Prox: " + rec.AbrevPura + " - " + rec.Abrev + " - " + rec.Status);
                        if (!dicRecursosProx.ContainsKey(rec.AbrevPura))
                            dicRecursosProx.Add(rec.AbrevPura, rec);
                    }

                }
            }


        if (ExibeRecursosRetirados)
        {
            Dictionary<string, Tuple<Recurso, LogData>> retiradas = new Dictionary<string, Tuple<Recurso, LogData>>();
            foreach (BusinessData.Entities.Recurso r in recursosBO.GetRecursos())
            {
                LogData latest = logDataDAO.FindLatestActivity(r.Descricao);
                if (latest != null && latest.Acao == "RETIRADA" && !recursosAlocadosAgora.Contains(r.Abrev))
                    retiradas.Add(r.Abrev, new Tuple<Recurso, LogData>(r, latest));
            }
            foreach (string key in retiradas.Keys)
            {
                Recurso r = retiradas[key].Item1;
                LogData latest = retiradas[key].Item2;
                RecursoItem rec = null;

                /*
                // Verifica se já existe uma reserva para a mesma dupla de salas
                if ((r.Abrev == "309" || r.Abrev == "312") && dicRecursosAtual.ContainsKey("309/312"))
                {
                    string user309 = retiradas["309"].Item2.Matricula;
                    string user312 = retiradas["312"].Item2.Matricula;
                    string user309312 = dicRecursosAtual["309/312"].Matricula;
                    Debug.WriteLine("Sala 309: " + user309 + " - Sala 312: " + user312);
                    Debug.WriteLine("309/312: " + user309312);
                    if (user309 != null && user309 == user309312)
                    {
                        // Copia dos dados da 309/312 para o recursoItem da 309
                        rec = new RecursoItem();
                        rec.latest = retiradas["309"].Item2;
                        rec.Abrev = rec.AbrevPura = "309";
                        rec.Status = StatusRecurso.Retirado;
                        rec.Tipo = 'L';
                        rec.Responsavel = dicRecursosAtual["309/312"].Responsavel;
                        rec.ResponsavelCurto = dicRecursosAtual["309/312"].Responsavel;
                        rec.Matricula = user309;
                        rec.Descricao = dicRecursosAtual["309/312"].Descricao;
                        rec.DescricaoCurta = dicRecursosAtual["309/312"].DescricaoCurta;
                        string horarioRetir = retiradas["309"].Item2.Horario.ToString(@"dd/MM HH:mm");
                        if (latest.Horario.Day == hoje.Day)
                            horarioRetir = rec.latest.Horario.ToString(@"HH:mm");
                        rec.Horario = horarioRetir;
                        rec.Nome = "Laboratório - 309";
                        // Remove a 309/312 da lista
                        dicRecursosAtual.Remove("309/312");
                    }
                    if (user312 != null && user312 == user309312)
                    {
                        // Copia dos dados da 309/312 para o recursoItem da 312
                        rec = new RecursoItem();
                        rec.latest = retiradas["312"].Item2;
                        rec.Abrev = rec.AbrevPura = "312";
                        rec.Status = StatusRecurso.Retirado;
                        rec.Tipo = 'L';
                        rec.Responsavel = dicRecursosAtual["309/312"].Responsavel;
                        rec.ResponsavelCurto = dicRecursosAtual["309/312"].Responsavel;
                        rec.Matricula = user312;
                        rec.Descricao = dicRecursosAtual["309/312"].Descricao;
                        rec.DescricaoCurta = dicRecursosAtual["309/312"].DescricaoCurta;
                        string horarioRetir = retiradas["309"].Item2.Horario.ToString(@"dd/MM HH:mm");
                        if (latest.Horario.Day == hoje.Day)
                            horarioRetir = rec.latest.Horario.ToString(@"HH:mm");
                        rec.Horario = horarioRetir;
                        rec.Nome = "Laboratório - 312";
                        dicRecursosAtual.Add(rec.AbrevPura, rec);
                        // Remove a 309/312 da lista
                        dicRecursosAtual.Remove("309/312");
                    }
                }
                if ((r.Abrev == "409" || r.Abrev == "412") && dicRecursosAtual.ContainsKey("409/412"))
                {

                }
                */
                //else if(r.Abrev != "309" && r.Abrev != "312" && r.Abrev != "409" && r.Abrev != "412")
                {
                    // Reserva 309/312: Michael
                    // Michael: retirou 309
                    // Agustini: retirou 312
                    // Agustini sem reserva

                    rec = new RecursoItem();
                    rec.Nome = r.Descricao;
                    rec.Abrev = rec.AbrevPura = r.Abrev;
                    rec.Tipo = r.Tipo;

                    string horarioRetirada = latest.Horario.ToString(@"dd/MM HH:mm");
                    if (latest.Horario.Day == hoje.Day)
                        horarioRetirada = latest.Horario.ToString(@"HH:mm");
                    string responsCurto = getNomeCurtoProfessor(latest.Usuario);
                    if (latest.TipoUsuario == "P")
                    {
                        if (latest.Matricula != null)
                        {
                            if (dicProfs.ContainsKey(latest.Matricula))
                                responsCurto = dicProfs[latest.Matricula].Curto;
                        }
                    }
                    rec.ResponsavelCurto = responsCurto;
                    rec.Responsavel = responsCurto;
                    rec.DescricaoCurta = "Retirado: " + responsCurto + " - " + horarioRetirada;
                    rec.Descricao = "RETIRADA";
                    rec.Horario = horarioRetirada;
                    //rec.ResponsavelAtualCurto = "&#9888; Desconhecido";
                    rec.Status = StatusRecurso.Retirado;
                    rec.latest = latest;
                    //if (!dicRecursosAtual.ContainsKey(rec.AbrevPura))
                    dicRecursosAtual[rec.AbrevPura] = rec;
                }
            }
        }

        container.InnerHtml = "";

        listaRecursosAtual = dicRecursosAtual.Values.ToList();
        listaRecursosProx = dicRecursosProx.Values.ToList();

        listaRecursosAtual = GroupRecursos(listaRecursosAtual).OrderBy(ri => ri.ResponsavelCurto).ThenBy(ri => ri.Descricao).ToList();

        if (apenasAtual || now.Hour >= 21)
        {
            int quebra = 12;
            if (listaRecursosAtual.Count > quebra)
            {
                listaRecursosProx = listaRecursosAtual.GetRange(quebra, listaRecursosAtual.Count - quebra);
                listaRecursosAtual = listaRecursosAtual.GetRange(0, quebra);
            }
        }
        else
        {
            listaRecursosProx = GroupRecursos(listaRecursosProx).OrderBy(ri => ri.ResponsavelCurto).ThenBy(ri => ri.Descricao).ToList();
        }

        string horarioAtual = "";
        //foreach (var ri in listaRecursosAtual) { horarioAtual = ri.HorarioAtual; break; }

        int cont = 0;
        string block = "";
        TimeSpan noventa = TimeSpan.FromMinutes(90);
        foreach (List<RecursoItem> lista in new List<List<RecursoItem>> { listaRecursosAtual, listaRecursosProx })
        {
            cont += 1;
            if (lista.Count == 0)
                continue;
            for (int p = 0; p < lista.Count; p++)
            {
                // Obtém o horário a partir do primeiro elemento válido (AB,  CD, ...)
                if (lista[p].Horario.Length == 2)
                {
                    horarioAtual = lista[p].Horario;
                    break;
                }
            }

            string infoHorario = deltaNow.ToString();
            if (deltaNow.TotalMinutes > 0)
                infoHorario = "<i class=\"bi bi-hourglass-split\"></i>" + noventa.Subtract(deltaNow).ToString(@"hh\:mm");
            else if (deltaNow.TotalMinutes < 0)
                infoHorario = "<i class=\"bi bi-hourglass-split\"></i>" + deltaNow.ToString(@"hh\:mm");

            if (!apenasAtual && lista == listaRecursosProx && deltaProx != nowTime)
                infoHorario = "<i class=\"bi bi-hourglass-top\"></i>" + deltaProx.ToString(@"hh\:mm");

            block += "<div class=\"col-12 col-lg-6 schedule-col\">\n<div class=\"card shadow-sm schedule-card\">";
            block += string.Format("<div class=\"schedule-header text-success\">HORÁRIO {0} {1}</div>\n", horarioAtual, infoHorario);
            block += "<div class=\"list-group list-group-flush\">\n";

            foreach (RecursoItem ri in lista)
            {
                if (ri.Nome == null) continue;

                string destaque = "";
                string destaqueText = "";
                string corBadge = "speaker";
                if ((cont == 1 || (apenasAtual || now.Hour >= 21)) && ri.Status == StatusRecurso.Retirado)
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
                        if (ri.Abrev.StartsWith("RN")) { recursoIcone = "bi-laptop"; corBadge = "notebook"; }
                        else { recursoIcone = "bi-pc-display"; corBadge = "lab"; }
                        break;
                    case 'N': recursoIcone = "bi-laptop"; corBadge = "notebook"; break;
                    case 'S': recursoIcone = "bi-speaker"; corBadge = "speaker"; break;
                    case 'X':
                        if (ri.Abrev.StartsWith("211")) { recursoIcone = "bi-pc-display"; corBadge = "lab"; }
                        break;
                }
                if (ri.Abrev.StartsWith("211"))
                    recursoIcone = "bi-pc-display";

                string apelido = ri.ResponsavelCurto; // ObterIniciais(ri.ResponsavelCurto);
                string classeTamanho = apelido.Length > 8 ? " long-text" : "";
                string htmlIniciais = string.Format("<span class=\"user-initials {2} {1}\">{0}</span>", apelido, classeTamanho, corBadge);

                bool isRetiradoAtual = cont == 1 && ri.Status == StatusRecurso.Retirado;
                if ((apenasAtual || now.Hour >= 21) && ri.Status == StatusRecurso.Retirado)
                    isRetiradoAtual = true;

                string responsavelAttr = ri.Responsavel != null
                    ? ri.Responsavel.Replace("\"", "&quot;").Trim()
                    : "";

                block += string.Format(
                    "<div class=\"list-group-item d-flex justify-content-between align-items-center nomedisc\" data-responsavel=\"{0}\">\n",
                    responsavelAttr);

                if (isRetiradoAtual)
                {
                    // string textoPrimario = string.Format("{0} - {1}", ri.ResponsavelCurto, ri.DescricaoCurta).Replace("\"", "&quot;");
                    string textoPrimario = string.Format("{0}", ri.DescricaoCurta).Replace("\"", "&quot;");
                    string responsavelRetirada = "";
                    if (ri.latest != null)
                    {
                        string horarioRetirada = ri.latest.Horario.ToString(@"dd/MM HH:mm");
                        if (ri.latest.Horario.Day == now.Day)
                            horarioRetirada = ri.latest.Horario.ToString(@"HH:mm");
                        responsavelRetirada = "Retirado: " + ri.ResponsavelCurto + " - " + horarioRetirada;
                        //responsavelRetirada = "Retirado: " + horarioRetirada;
                    }
                    block += string.Format(
                        "<div class=\"d-flex align-items-center\">{2}<span class=\"text-alternating\" data-text-primary=\"{0}\" data-text-alt=\"{1}\">{0}</span></div>\n<div class=\"resource-container\">\n",
                        textoPrimario, responsavelRetirada, htmlIniciais);
                    //////block += string.Format(
                    ////    "<span class=\"text-alternating\" data-text-primary=\"{0}\" data-text-alt=\"{1}\">{0}</span>\n<div class=\"resource-container\">\n",
                    //    textoPrimario, responsavelRetirada);
                }
                else
                {
                    block += string.Format(
                        "<div class=\"d-flex align-items-center\">{1}<span>{0}</span></div>\n<div class=\"resource-container\">\n",
                        ri.DescricaoCurta, htmlIniciais);

                    //block += string.Format(
                    //    "<div class=\"d-flex align-items-center\">{2}<span>{0} - {1}</span></div>\n<div class=\"resource-container\">\n",
                    //    ri.ResponsavelCurto, ri.DescricaoCurta, htmlIniciais);

                    ////block += string.Format("<span>{0} - {1}</span>\n<div class=\"resource-container\">\n",
                    //    ri.ResponsavelCurto, ri.DescricaoCurta);
                }

                string predioHtml = !string.IsNullOrEmpty(ri.Predio)
                    ? string.Format("<span class=\"resource-predio badge bg-secondary\">{0}</span>", ri.Predio)
                    : "";
                //                block += string.Format("{0}<span class=\"badge {1} resource-tag {2}\">{3}</span>\n</div>\n</div>", predioHtml, destaque, corBadge, ri.Abrev);
                block += string.Format("<i class=\"bi {0} resource-icon {1}\"></i>", recursoIcone, destaqueText);
                block += string.Format("<span class=\"badge {1} resource-tag {2}\">{0}</span>\n</div>\n</div>", ri.Abrev, destaque, corBadge);
            }

            block += "</div>";
            block += "</div></div>";
        }

        container.InnerHtml = string.Format("<div class=\"row gx-0 gy-4 justify-content-center\">{0}</div>", block);
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

    private string ObterIniciais(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) return "";

        // Remove os prefixos para não gerar iniciais incorretas (ex: "P" para Prof.)
        string nomeLimpo = nome.Replace("Prof. ", "").Replace("Profa. ", "").Trim();
        string[] partes = nomeLimpo.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (partes.Length == 0) return "";
        if (partes.Length == 1) return partes[0].Length >= 2 ? partes[0].Substring(0, 2) : partes[0];

        string inicial1 = partes[0].Substring(0, 1);
        string inicial2 = partes[partes.Length - 1].Substring(0, 1);

        return inicial1 + inicial2;
    }

    public void Refresh()
    {
        VisualizarAlocacoesData();
    }
}
