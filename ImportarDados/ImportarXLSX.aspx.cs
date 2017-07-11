using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToExcel;
using BusinessData.BusinessLogic;
using BusinessData.Entities;

public partial class ImportarDados_ImportarXLSX : System.Web.UI.Page
{
    private ExcelQueryFactory excel;
    private List<Professor> profs;
    private List<Disciplina> discs;

    protected void Page_Load(object sender, EventArgs e)
    {
        profs = new List<Professor>();
        discs = new List<Disciplina>();
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
        ListBox1.Items.Add("*** PROFESSORES ***");
        ImportarProfessores();
        ListBox1.Items.Add("*** DISCIPLINAS ***");
        ImportarDisciplinas(calId);
        ListBox1.Items.Add("*** TURMAS ***");
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
                        ListBox1.Items.Add(profAtual.Nome + " já existe");
                        existe = true;
                        break;
                    }
                }
                if (!existe)
                {
                    ListBox1.Items.Add(profAtual.ToString());
                    //                    controleProfessores.InsertPessoa(profAtual, "pergunta", profAtual.Matricula);
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

            foreach (Disciplina disciplinaAtual in disciplinasImportadas)
            {
                //                    if (!dic_disciplinasCadastradas.ContainsKey(disciplinaAtual.Cod))
                if (!disciplinasCadastradas.Contains(disciplinaAtual))
                {
                    ListBox1.Items.Add(disciplinaAtual.ToString());
                    //insere na tabela disciplinas e disciplinasincalendario
                    controleDisciplinas.InsereDisciplina(disciplinaAtual);
                }
                else
                {
                    //                        if (!dic_disciplinasInCalendario.ContainsKey(disciplinaAtual.Cod))
                    if (!disciplinasInCalendario.Contains(disciplinaAtual))
                    {
                        //insere apenas na tabela disciplinasincalendario 
                        controleDisciplinas.InsereDisciplinaInCalendario(disciplinaAtual, cal.Id);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ImportarTurmas(Calendario calId)
    {
        TurmaBO turmasBO = new TurmaBO();
        List<Turma> turmasCadastradas = turmasBO.GetTurmas(calId);

        // Import turmas from Excel data
        var data = from c in excel.Worksheet("turmas")
                   select c;
        // cod, nro, horario, prof
    }
}