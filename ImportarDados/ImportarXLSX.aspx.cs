using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToExcel;
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Security;
using System.Configuration;

public partial class ImportarDados_ImportarXLSX : System.Web.UI.Page
{
    private ExcelQueryFactory excel;
    private List<Professor> profs;
    private List<Disciplina> discs;
    private string codcurso;

    protected void Page_Load(object sender, EventArgs e)
    {
        profs = new List<Professor>();
        discs = new List<Disciplina>();
        //output.InnerHtml = "<h1>Teste</h1>";
        string cs = ConfigurationManager.ConnectionStrings["SARCFACINcs"].ConnectionString;
        if (cs.Contains("SARCDEV"))
        {
            butClearDB.Enabled = false;
            butImportXLSX.Enabled = false;
            output.InnerHtml += "<h2>Erro: não pode ser executado no BD da FACIN</h2>";
        }
        else if (cs.Contains("SARCEC"))
        {
            codcurso = "120L";
            butImportXLSX.Text = "Importar EC";
        }
        else if (cs.Contains("SARCGS"))
        {
            codcurso = "3630";
            butImportXLSX.Text = "Importar GS";
        }
    }

    protected void butImportXLSX_Click(object sender, EventArgs e)
    {
        string path = Server.MapPath("/ImportarDados/planilha_SARC.xlsx");
        excel = new ExcelQueryFactory(path);
        //foreach(var s in excel.GetColumnNames("profs"))
        //{
        //    ListBox1.Items.Add(s);
        //}
        Calendario calId = (Calendario)(Session["Calendario"]);

        output.InnerHtml += "<h2>Professores</h2>";
        //ListBox1.Items.Add("*** PROFESSORES ***");
        ImportarProfessores();
        output.InnerHtml += "<h2>Disciplinas</h2>";
        //ListBox1.Items.Add("*** DISCIPLINAS ***");
        ImportarDisciplinas(calId);
        output.InnerHtml += "<h2>Turmas</h2>";
        //ListBox1.Items.Add("*** TURMAS ***");
        ImportarTurmas(calId);
    }

    public void ImportarProfessores()
    {
        // Import profs from Excel data
        var data = from c in excel.Worksheet("profs")
                   select c;
        foreach (var s in data)
            profs.Add(Professor.NewProfessor(s[0], s[1], s[2]));

        bool existe = false;
        try
        {
            ProfessoresBO controleProfessores = new ProfessoresBO();
            IList<Professor> professoresCadastrados = (List<Professor>)controleProfessores.GetProfessores();
            IList<Professor> professoresImportados = profs;

            foreach (Professor profAtual in professoresImportados)
            {
                foreach (Professor profCadastrado in professoresCadastrados)
                {
                    if (profCadastrado.Equals(profAtual))
                    {
                        output.InnerHtml += "<font color=\"red\">"+profAtual.Nome + " já existe!</font><br>";
                        //ListBox1.Items.Add(profAtual.Nome + " já existe");
                        existe = true;
                        break;
                    }
                }
                if (!existe)
                {
                    output.InnerHtml += "Adicionando " + profAtual.Nome + " ("+
                    profAtual.Matricula+") - "+profAtual.Email+"<br>";
                    //ListBox1.Items.Add(profAtual.ToString());
                    controleProfessores.InsertPessoa(profAtual, "pergunta", profAtual.Matricula);
                }
                else
                {
                    existe = false;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    public void ImportarDisciplinas(Calendario cal)
    {
        // Import discs from Excel data
        var data = from c in excel.Worksheet("disciplinas")
                   select c;

        // All must be imported into this category
        CategoriaDisciplinaBO catDisBo = new CategoriaDisciplinaBO();
        var cat = (from c in catDisBo.GetCategoriaDisciplinas()
                                  where c.Descricao == "CategoriaDisciplinaImportação"
                                  select c).First();
        foreach (var s in data) {
            int cred = Int32.Parse(s[1]);
            bool g2 = s[3] == "Sim" ? true : false;
            discs.Add(Disciplina.GetDisciplina(s[0], cred, s[2], g2, cal, cat));
        }

        try
        {
            DisciplinasBO controleDisciplinas = new DisciplinasBO();
            IList<Disciplina> disciplinasCadastradas = controleDisciplinas.GetDisciplinas();
            IList<Disciplina> disciplinasImportadas = discs;
            List<Disciplina> disciplinasInCalendario = controleDisciplinas.GetDisciplinaInCalendario(cal.Id);

            //Dictionary<String, Disciplina> dic_disciplinasCadastradas = new Dictionary<string, Disciplina>();
            //foreach(Disciplina d in disciplinasCadastradas)
            //    dic_disciplinasCadastradas.Add(d.Cod, d);

            //Dictionary<String, Disciplina> dic_disciplinasInCalendario = new Dictionary<string, Disciplina>();
            //foreach (Disciplina d in disciplinasInCalendario)
            //    dic_disciplinasInCalendario.Add(d.Cod, d);

            int novas = 0;
            int novasCal = 0;

            foreach (Disciplina disciplinaAtual in disciplinasImportadas)
            {
                //                    if (!dic_disciplinasCadastradas.ContainsKey(disciplinaAtual.Cod))
                if (!disciplinasCadastradas.Contains(disciplinaAtual))
                {
                    output.InnerHtml += disciplinaAtual.ToString() + "<br>";
                    //ListBox1.Items.Add(disciplinaAtual.ToString());
                    //insere na tabela disciplinas e disciplinasincalendario
                    controleDisciplinas.InsereDisciplina(disciplinaAtual);
                    novas++;
                }
                else
                {
                    //                        if (!dic_disciplinasInCalendario.ContainsKey(disciplinaAtual.Cod))
                    if (!disciplinasInCalendario.Contains(disciplinaAtual))
                    {
                        //insere apenas na tabela disciplinasincalendario 
                        controleDisciplinas.InsereDisciplinaInCalendario(disciplinaAtual, cal.Id);
                        novasCal++;
                    }
                }
            }
            output.InnerHtml += "<h3>Novas: "+novas+", novas neste calendário: "+novasCal+"</h3>";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ImportarTurmas(Calendario calId)
    {
        ProfessoresBO profsBO = new ProfessoresBO();
        List<Professor> profs = profsBO.GetProfessores();

        DisciplinasBO discipBO = new DisciplinasBO();
        List<Disciplina> discs = discipBO.GetDisciplinaInCalendario(calId.Id);

        TurmaBO turmasBO = new TurmaBO();
        List<Turma> turmasCadastradas = turmasBO.GetTurmas(calId);
        List<Turma> turmasNovas = new List<Turma>();

        CursosBO cursosBO = new CursosBO();
        Curso curso = cursosBO.GetCursoByCodigo(codcurso);
        if (curso == null)
        {
            output.InnerHtml += "<font color=\"red\">Erro: curso " + codcurso + " inexistente!</font>";
            return;
        }
        output.InnerHtml += "<h2>" + curso.Nome + " (" + curso.Vinculo + ")</h2>";

        // Import turmas from Excel data
        var data = from c in excel.Worksheet("turmas")
                   select c;
        // cod, nro, horario, prof
        
        // Processa cada turma
        foreach(var s in data)
        {
            string cod = s[0];
            if (cod.Contains("-"))
                cod = cod.Substring(0, cod.IndexOf('-'));
            int nro = Int32.Parse(s[1]);
            string horario = s[2];
            string matr = s[3];

            Disciplina disc = (from d in discs
                               where d.Cod == cod
                               select d).FirstOrDefault();
            if (disc == null)
            {
                output.InnerHtml += "<font color=\"red\">Erro: disciplina " + cod + " inexistente!</font>";
                continue;
            }
            output.InnerHtml += "Turma: "+disc.CodCred+" - "+disc.Nome + " ("+nro+") - "+horario+" - "+matr+"<br>";

            Professor prof = (from p in profs
                             where p.Matricula == matr
                             select p).FirstOrDefault();
            if (prof == null)
            {
                output.InnerHtml += "<font color=\"red\">Professor " + matr + " não cadastrado!</font><br><br>";
            }

            Turma atual = (from t in turmasCadastradas
                           where t.Disciplina.Cod == cod && t.Numero == nro
                           select t).FirstOrDefault();
            if (atual == null)
            {
                Turma nova = Turma.NewTurma(nro, calId, disc, horario, prof, curso);
                turmasBO.InsereTurma(nova);
            }
            else
                output.InnerHtml += "<font color=\"red\">&nbsp;&nbsp;&nbsp;&nbsp;Turma já cadastrada!</font><br><br>";
        }
    }

    // Apaga todos os professores do Membership e do sistema
    protected void butClearDB_Click(object sender, EventArgs e)
    {
        foreach (MembershipUser u in Membership.GetAllUsers())
        {
            string role = Roles.GetRolesForUser(u.UserName).First();
            output.InnerHtml += u.UserName + " - " + u.Email;
            output.InnerHtml += " ("+role + ")<br>";
            if (role == "Professor")
                Membership.DeleteUser(u.UserName, true);
        }
    }

    // Apaga todos as turmas do calendario atual
    protected void butClearTurmas_Click(object sender, EventArgs e)
    {
        Calendario calId = (Calendario)(Session["Calendario"]);
        TurmaBO turmasBO = new TurmaBO();
        List<Turma> turmasCadastradas = turmasBO.GetTurmas(calId);        
        foreach (Turma tur in turmasCadastradas)
        {
            output.InnerHtml += "Apagando " + tur.Disciplina.CodCred + " - " + tur.Disciplina.Nome + " (" + tur.Numero + ")<br>";
            turmasBO.DeletaTurma(tur.Id);
        }
    }
}