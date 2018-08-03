using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ImportarDados_ImportarAcad : System.Web.UI.Page
{
    private ProfessoresBO controleProfs;
    private CategoriaDisciplinaBO catDisBo;
    private DisciplinasBO controleDiscs;
    private TurmaBO turmasBO;
    private CursosBO cursosBO;

    private Dictionary<String, Professor> profs;
    private Dictionary<String, Disciplina> discs;

    private IList<Professor> profsCadastrados;
    private IList<Disciplina> disciplinasCadastradas;

    protected void Page_Load(object sender, EventArgs e)
    {
        controleProfs = new ProfessoresBO();
        profs = new Dictionary<string, Professor>();
        discs = new Dictionary<string, Disciplina>();
        catDisBo = new CategoriaDisciplinaBO();
        controleDiscs = new DisciplinasBO();
        turmasBO = new TurmaBO();
        cursosBO = new CursosBO();
    }

    private string ToTitleCase(string words)
    {
        TextInfo textInfo = new CultureInfo("pt-BR", false).TextInfo;

        words = textInfo.ToTitleCase(words.ToLower());
        
        words = Regex.Replace(words, @" (D)([eao]|[ao]s) ", @" d$2 ");
        words = words.Replace(" Iv", " IV");
        words = words.Replace(" Iii", " III");
        words = words.Replace(" Ii", " II");
        words = Regex.Replace(words, " Si$", " SI");
        words = Regex.Replace(words, " Es$", " ES");
        words = words.Replace(" Cc", " CC");
        words = words.Replace(" Ec", " EC");
        words = words.Replace(" Ti", " TI");
        words = words.Replace(" Na ", " na ");
        words = words.Replace(" No ", " no ");
        words = words.Replace(" E ", " e ");
        words = words.Replace(" A ", " a ");
        words = words.Replace(" Para ", " para ");
        /**/
        return words;
    }

    private Professor FindProf(string matr)
    {
        return (from prof in profsCadastrados
                where prof.Matricula == matr
                select prof).FirstOrDefault();
    }

    private Disciplina FindDisc(string codigo)
    {
        return (from d in disciplinasCadastradas
                where d.Cod == codigo
                select d).FirstOrDefault();
    }

    private string FixTime(string horario, int cred)
    {
        string dias = Regex.Replace(horario, "[A-Z]", "");
        string horas = Regex.Replace(horario, "[0-9]", "");

        int i = 0;
        if (horas == "ABC")
            horas = "ABCD";
        else if (horas == "BCD")
            horas = "ABCD";
        else if (horas == "CDE")
            horas = "CDEX";
        else if (horas == "FGH")
            horas = "FGHI";
        else if (horas == "GHI")
            horas = "FGHI";
        else if (horas == "HIJ")
            horas = "HIJK";
        else if (horas == "IJK")
            horas = "HIJK";
        else if (horas == "JKL")
            horas = "JKLM";
        else if (horas == "LMN")
            horas = "LMNP";
        else if (horas == "MNP")
            horas = "LMNP";

        string hora_s = "";
        var lista = horas.GroupBy(_ => i++ / 2).Select(g => String.Join("", g)).ToList();
        int pos = 0;
        foreach (var hora in lista)
        {
            hora_s += dias[pos] + hora;
            if (pos < dias.Length-1) pos++;
        }

        horario = dias + " - " + hora_s;
        return hora_s;
    }

    protected void butImportAcad_Click(object sender, EventArgs e)
    {
        // 46501;4;4650104;ALGORITMOS AVANCADOS;127;2LM 4LM;NÃO INFORMADO;MICHAEL DA COSTA MORA;michael.mora@pucrs.br;049214;-;-;-;-;-;-;-;-;
        // 0: código
        // 1: créditos
        // 3: nome disciplina
        // 4: turma
        // 5: horário (2LM 4LM)
        // 7: nome professor
        // 8: email professor
        // 9: matrícula professor
        // 10: prédio/bloco/sala 1
        // 11: horário 1
        // 12: prédio/bloco/sala 2
        // 13: horário 2
        // ... até 4
        WebClient wc = new WebClient();
        string data = wc.DownloadString("http://www.politecnica.pucrs.br/academico/sarc/csv.php?GRP=&PREDIO=32");
        string[] linhas = data.Split('\n');

        /**/
        profsCadastrados = (List<Professor>) controleProfs.GetProfessores();
        
        var cat32 = (from c in catDisBo.GetCategoriaDisciplinas()
                   where c.Descricao == "Prática"
                   select c).First();

        var cat30 = (from c in catDisBo.GetCategoriaDisciplinas()
                     where c.Descricao == "Teórica - Outras Unidades"
                     select c).First();

        if(cat32 == null || cat30 == null) {
            Response.Write("ERROR! Null category! "+cat32+" "+cat30);
        }

        Calendario cal = (Calendario)(Session["Calendario"]);

        disciplinasCadastradas = controleDiscs.GetDisciplinas();
        List<Disciplina> disciplinasInCalendario = controleDiscs.GetDisciplinaInCalendario(cal.Id);

        List<Turma> turmasCadastradas = turmasBO.GetTurmas(cal);
        Dictionary<string, Turma> turmasNovas = new Dictionary<string, Turma>();

        // Popula o dicionário com todas as turmas já existentes no calendário
        foreach (Turma turma in turmasCadastradas)
            turmasNovas.Add(turma.ToString(), turma);

        // Todas as turmas são vinculadas ao curso "Escola Politécnica Importação", pois não há
        // essa informação no CSV
        Curso curso = cursosBO.GetCursoByCodigo("EP");
        if (curso == null)
        {
            Response.Write("ERROR! Null course!");
        }

        int totalDiscNovas = 0;
        int totalDiscNovasCal = 0;
        int totalProfsNovos = 0;
        int totalTurmasNovas = 0;

        string novos = "";
        string semprof = "";
        string novasdisc = "";

        string novos_emails = "";

        output.InnerHtml += "<table><tr><th>Codcred</th><th>Disciplina</th><th>Turma</th><th>Horário</th><th>Sala</th><th>Professor</th></tr>";
        
        bool first = true;
        foreach(var linha in linhas) {
            if (first)
            {
                first = false;
                continue;
            }

            var dados = linha.Split(';');
            int turma, cred;
            string cod = dados[0];
            int.TryParse(dados[1], out cred);
            if (cred == 1)
                continue; // skip 1-credit modules
            string nomedisc = ToTitleCase(dados[3]);
            int.TryParse(dados[4], out turma);
            string hora = dados[5].Replace('Y', 'X').Replace(" ", "");
            hora = FixTime(hora, cred);
            string nomeprof = ToTitleCase(dados[7]).Trim();
            string email = dados[8].Trim();
            string matricula = dados[9].Trim();

            string sala = dados[10].Trim();
            if(dados[12].Trim() != "-")
                sala += ", "+dados[12].Trim();
            if(dados[14].Trim() != "-")
                sala += ", "+dados[14].Trim();

            Professor novoProf = null;
            if (!profs.ContainsKey(matricula))
            {
                novoProf = FindProf(matricula);
                if (novoProf == null && matricula != String.Empty)
                {
                    novoProf = Professor.NewProfessor(matricula, nomeprof, email);
                    novos += "<span style=\"color: red\">Novo: " + novoProf.Nome + " (" + novoProf.Email + ")</span><br>";
                    novos_emails += novoProf.Email + ", ";
                    controleProfs.InsertPessoa(novoProf, "pergunta", novoProf.Matricula);
                    totalProfsNovos++;
                }
                if (matricula != String.Empty)
                    profs.Add(matricula, novoProf);
                else
                {
                    semprof += String.Format("<br>Sem professor associado: {0}-{1:D2} {2} ({3})",
                        cod, cred, nomedisc, turma);
                }
            }
            else
                novoProf = profs[matricula];

            Disciplina disc = FindDisc(cod);
            if (disc != null)
            {
                nomedisc = disc.Nome;
                if (!disciplinasInCalendario.Contains(disc))
                {
                    // Insere apenas na tabela disciplinasincalendario 
                    controleDiscs.InsereDisciplinaInCalendario(disc, cal.Id);
                    totalDiscNovasCal++;
                }
            }
            else
            {
                if (!discs.ContainsKey(cod))
                {
                    // Disciplina nova, insere na tabela de disciplinas e
                    // disciplinasincalendario

                    // Categoria pode ser "Prática", se for da Informática
                    // ou "Teórica - Outras Unidades" se não for...
                    CategoriaDisciplina cat2;
                    if (cod.StartsWith("46"))
                        cat2 = cat32;
                    else
                        cat2 = cat30;
                    disc = Disciplina.GetDisciplina(cod, cred, nomedisc, true, cal, cat2);
                    controleDiscs.InsereDisciplina(disc);
                    novasdisc += String.Format("<br>Nova disciplina: {0}-{1:D2} {2}",
                        cod, cred, nomedisc);
                    totalDiscNovas++;
                }
            }
            if (!discs.ContainsKey(cod))
                discs.Add(cod, disc);

            // Adiciona turma (se ainda não existir no calendário)
            string style = "";
            if (novoProf != null)
            {
                Turma novaTurma = Turma.NewTurma(turma, cal, disc, hora, novoProf, curso, sala);
                if (!turmasNovas.ContainsKey(novaTurma.ToString()))
                {
                    turmasBO.InsereTurma(novaTurma);
                    totalTurmasNovas++;
                }
            }
            else
                style = "style=\"background-color: #ffb3b3\"";

            output.InnerHtml += String.Format("<tr {0}><td>{1}-{2:D2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>",
                style, cod, cred, nomedisc, turma, hora, sala, nomeprof);
            //output.InnerHtml += nomedisc+" "+nomeprof+"<br>";
        }
        output.InnerHtml += "</table>";
        output.InnerHtml += "<br>" + novos;
        output.InnerHtml += "<br>" + novasdisc;
        output.InnerHtml += "<br><br>Total disciplinas novas: " + totalDiscNovas;
        output.InnerHtml += "<br>Novas neste calendário: " + totalDiscNovasCal;
        output.InnerHtml += "<br>Total profs. novos: " + totalProfsNovos;
        output.InnerHtml += "<br>Total turmas novas: " + totalTurmasNovas;
        output.InnerHtml += "<br>" + semprof;
        output.InnerHtml += "<br><br>Lista de emails novos: " + novos_emails;
    }
}