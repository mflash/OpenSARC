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
        words = words.Replace("- Ec", "- EC");
        //words = Regex.Replace(words, "Em$", "EM");
        words = words.Replace(" Em ", " em ");
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

    private Professor FindProfEmail(string email)
    {
        return (from prof in profsCadastrados
                where prof.Email == email
                select prof).FirstOrDefault();
    }

    private Professor FindProfNome(string nome)
    {
        return (from prof in profsCadastrados
                where prof.Nome == nome
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
        else if (horas == "XZ")
            horas = "EX";
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
        else if (horas == "IJ")
            horas = "HI";

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
        // 4: teórica/prática
        // 5: turma
        // 6: horário (2LM 4LM)
        // 7: curso
        // 9: nome professor
        // 10: email professor
        // 11: matrícula professor
        // 12: prédio/bloco/sala 1
        // 13: horário 1
        // 14: prédio/bloco/sala 2
        // 15: horário 2
        // ... até 4
        const int CODIGO = 0;
        const int CREDITOS = 1;
        const int NOMEDISC = 3;
        const int TURMA = 5;
        const int HORARIO = 6;
        const int CURSO = 7;
        const int NOMEPROF = 9;
        const int EMAIL = 10;
        const int MATRICULA = 11;
        const int SALA1 = 12;
        const int SALA2 = 14;
        const int SALA3 = 16;
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        WebClient wc = new WebClient();
        String predio = txtPredio.Text;
        string data;
        try
        {
            String url = "http://www.politecnica.pucrs.br/academico/sarc/csv.php?GRP=&PREDIO=" + predio;
            data = wc.DownloadString(url);
        }
        catch(Exception ex)
        {
            output.InnerHtml = "Erro carregando dados: " + ex.Message;
            return;
        }
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
        Curso curso = null;

        int totalDiscNovas = 0;
        int totalDiscNovasCal = 0;
        int totalProfsNovos = 0;
        int totalTurmasNovas = 0;

        string novos = "";
        string semprof = "";
        string novasdisc = "";
        string novasturmas = "";

        string novos_emails = "";

        output.InnerHtml += "<table><tr><th>Codcred</th><th>Disciplina</th><th>Turma</th><th>Horário</th><th>Sala</th><th>Professor</th></tr>";
        
        bool first = true;
        bool simula = checkSimul.Checked;

        Dictionary<String, Curso> dicCursos = new Dictionary<string, Curso>();
        foreach(var item in cursosBO.GetCursos())
            dicCursos.Add(item.Codigo, item);

        foreach(var linha in linhas) {
            if (first)
            {
                first = false;
                continue;
            }

            //output.InnerHtml += "<pre>" + linha + "</pre><br>";
            if (linha.Trim() == String.Empty)
                break;
            var dados = linha.Split(';');
            int turma, cred;
            string cod = dados[CODIGO];
            if (predio == "15" && (!cod.StartsWith("46") && !cod.StartsWith("98")))
                continue; // Living: apenas turmas 46 ou 98 (informatica)
            int.TryParse(dados[CREDITOS], out cred);
            if (cred == 1)
                continue; // skip 1-credit modules
            string nomedisc = ToTitleCase(dados[NOMEDISC]);
            int.TryParse(dados[TURMA], out turma);
            string hora = dados[HORARIO].Replace('Y', 'X').Replace(" ", "");
            hora = FixTime(hora, cred);
            string nomeprof = ToTitleCase(dados[NOMEPROF]).Trim();
            if (nomeprof.Trim() == String.Empty)
                continue;
            string email = dados[EMAIL].Trim();
            if (email.StartsWith("professornovo"))
            {
                email = String.Format("professornovo{0}{1}@pucrs.br", nomeprof[0], nomeprof[nomeprof.Length-1]);
            }
            string matricula = dados[MATRICULA].Trim();
            if (matricula.StartsWith("0")) // matrícula antiga?
                matricula = "10" + matricula;

            string sala = dados[SALA1].Trim();
            if(dados[SALA2].Trim() != "-")
                sala += ", "+dados[SALA2].Trim();
            if(dados[SALA3].Trim() != "-")
                sala += ", "+dados[SALA3].Trim();

            try
            {
                //curso = cursosBO.GetCursoByCodigo(dados[CURSO]);
                curso = dicCursos[dados[CURSO]];
                if (curso == null)
                {
                    curso = dicCursos["EP"];
                    //curso = cursosBO.GetCursoByCodigo("EP");
                    //                Response.Write("ERROR! Null course!");
                }
            }
            catch (Exception ex)
            {
                curso = dicCursos["EP"];
                //curso = cursosBO.GetCursoByCodigo("EP");
            }

            Professor novoProf = null;
            string x = nomeprof;
            if (!profs.ContainsKey(x))
            {
                novoProf = FindProfEmail(email);
                //novoProf = FindProf(x);
                //novoProf = FindProfNome(x);
                if (novoProf == null && matricula != String.Empty)
                {
                    novoProf = Professor.NewProfessor(matricula, nomeprof, email);
                    novos += "<span style=\"color: red\">Novo: " + novoProf.Nome + " (" + novoProf.Email + ")</span><br>";
                    novos_emails += novoProf.Email + ", ";
                    if(!simula)
                        controleProfs.InsertPessoa(novoProf, "pergunta", novoProf.Matricula);
                    totalProfsNovos++;
                }
                if (matricula != String.Empty)
                    profs.Add(x, novoProf);
                else
                {
                    semprof += String.Format("<br>Sem professor associado: {0}-{1:D2} {2} ({3})",
                        cod, cred, nomedisc, turma);
                }
            }
            else
                novoProf = profs[x];

            Disciplina disc = FindDisc(cod);            
            if (disc != null)
            {
                // Disciplina já existe...
                nomedisc = disc.Nome;
                // Mas não existe no calendário?
                if (!disciplinasInCalendario.Contains(disc))
                {
                    // Insere apenas na tabela disciplinasincalendario 
                    if(!simula)
                        controleDiscs.InsereDisciplinaInCalendario(disc, cal.Id);
                    totalDiscNovasCal++;
                }
            }
            else
            {
                // Categoria pode ser "Prática", se for da Informática
                // ou "Teórica - Outras Unidades" se não for...
                CategoriaDisciplina cat2;
                if (cod.StartsWith("46"))
                    cat2 = cat32;
                else
                    cat2 = cat30;
                disc = Disciplina.GetDisciplina(cod, cred, nomedisc, true, cal, cat2);

                // Disciplina não existe ou é nova?
                if (!discs.ContainsKey(cod))
                {
                    // Disciplina nova, insere na tabela de disciplinas e
                    // disciplinasincalendario                    
                    if(!simula)
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
                    if(!simula)
                        turmasBO.InsereTurma(novaTurma, cal);
                    novasturmas += String.Format("<br>Nova turma: {0}-{1:D2} {2} ({4}) {5} - {3}",
                        disc.Cod, disc.Cred, disc.Nome, novoProf.Nome, turma, hora);
                    totalTurmasNovas++;
                }
                else
                {
                    if(!simula)
                    {
                        turmasBO.UpdateTurma(novaTurma);
                    }
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
        output.InnerHtml += "<br>" + novasturmas;
        output.InnerHtml += "<br><br>Total disciplinas novas: " + totalDiscNovas;
        output.InnerHtml += "<br>Novas neste calendário: " + totalDiscNovasCal;
        output.InnerHtml += "<br>Total profs. novos: " + totalProfsNovos;
        output.InnerHtml += "<br>Total turmas novas: " + totalTurmasNovas;
        output.InnerHtml += "<br>" + semprof;
        output.InnerHtml += "<br><br>Lista de emails novos: " + novos_emails;
    }
}