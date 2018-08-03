using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using BusinessData.Entities;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace BusinessData.DataAccess.SPA
{
    public class SPADAO
    {
        /// <summary>
        /// Acesso a base de dado. LowLevel DAO
        /// </summary>
        private Database baseDados;

        public SPADAO()
        {
            baseDados = DatabaseFactory.CreateDatabase("SPA");
        }
        /// <summary>
        /// Retorna todas as faculdades cadastradas no SPA
        /// </summary>
        /// <returns></returns>
        public IList<Faculdade> GetFaculdades()
        {
            DbCommand cmdSelect = baseDados.GetSqlStringCommand(QueryMap.Default.Faculdades);
            Entities.Faculdade faculdade = null;
            try
            {
                IList<Faculdade> listaAux = new List<Faculdade>();
                using (IDataReader leitor = baseDados.ExecuteReader(cmdSelect))
                {
                    while (leitor.Read())
                    {
                        string nome = leitor.GetValue(leitor.GetOrdinal("UNIDADE")).ToString();
                        faculdade = Entities.Faculdade.NewFaculdade(nome);
                        listaAux.Add(faculdade);
                    }
                }
                return listaAux;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados.", ex);
            }
        }

        /// <summary>
        /// Retorna todas as turmas cadastradas no SPA
        /// </summary>
        /// <returns></returns>
        public IList<Turma> GetTurmas(Guid calendarioId)
        {
            DbCommand cmdSelect = baseDados.GetSqlStringCommand(QueryMap.Default.Turmas);
            Entities.Turma turma = null;
            try
            {
                IList<Entities.Turma> listaAux = new List<Entities.Turma>();
                using (IDataReader leitor = baseDados.ExecuteReader(cmdSelect))
                {
                    CalendariosDAO caDAO = new CalendariosDAO();
                    DisciplinasDAO disciDAO = new DisciplinasDAO();
                    ProfessorDAO profDAO = new ProfessorDAO();
                    CursosDAO curDAO = new CursosDAO();
                    
                    //Calendario - pega o corrente, neste caso, foi passado por parâmetro,
                    //no futuro, será variável de sessão.
                    Calendario cal = caDAO.GetCalendario(calendarioId);
                    
                    while (leitor.Read())
                    {
                        Curso cur = null;
                        Disciplina discip = null;
                        Professor prof = null;

                        string numero = leitor.GetString(leitor.GetOrdinal("TURMA"));
                        int num = 1;
                        if(numero.Length >= 3)
                            num = Convert.ToInt32(numero.Substring(0, 3));
                        string datahora = leitor.GetValue(leitor.GetOrdinal("HORARIO")).ToString();
                        datahora = datahora.Replace("EF", "EX");
                        string codigoCurso;
                        string disciplinaCodigo;
                        string matriculaProfessor;

                        //Disciplina - Turmas.CODIGO
                        try
                        {
                            disciplinaCodigo = leitor.GetValue(leitor.GetOrdinal("CODIGO")).ToString();

                            discip = disciDAO.GetDisciplina(disciplinaCodigo, calendarioId);
                        }
                        catch (Exception )
                        {
                            CriaLOG(leitor);
                            continue;
                        }

                        //Matricula do professor - Turmas.PROFESSOR
                        try
                        {
                            matriculaProfessor = leitor.GetValue(leitor.GetOrdinal("PROFESSOR1")).ToString();

                            prof = profDAO.GetProfessorByMatricula(matriculaProfessor);
                        }
                        catch (Exception )
                        {
                            CriaLOG(leitor);
                            continue;
                        }
                        
                        //Código do curso - Turmas.CURSO
                        try
                        {
                            codigoCurso = leitor.GetValue(leitor.GetOrdinal("CURSO")).ToString();

                            cur = curDAO.GetCurso(codigoCurso);
                        }
                        
                        catch (Exception )
                        {
                            CriaLOG(leitor);
                            continue;
                        }

                        turma = Entities.Turma.NewTurma(num, cal, discip, datahora, prof, cur);
                        listaAux.Add(turma);
                    }
                }
                return listaAux;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados.", ex);
            }
        }

        /// <summary>
        /// Retorna todas as turmas que não há professor cadastrado
        /// </summary>
        /// <param name="calendarioId"></param>
        /// <returns></returns>
        public IList<TurmaSemProfessor> GetProfessorNone(Guid calendarioId)
        {
            DbCommand cmdSelect = baseDados.GetSqlStringCommand(QueryMap.Default.TurmasNone);
            Entities.TurmaSemProfessor turma = null;
            try
            {
                IList<Entities.TurmaSemProfessor> listaAux = new List<Entities.TurmaSemProfessor>();
                using (IDataReader leitor = baseDados.ExecuteReader(cmdSelect))
                {
                    CalendariosDAO caDAO = new CalendariosDAO();
                    DisciplinasDAO disciDAO = new DisciplinasDAO();
                    CursosDAO curDAO = new CursosDAO();

                    
                    Entities.Calendario cal = caDAO.GetCalendario(calendarioId);

                    while (leitor.Read())
                    {                        
                        //Disciplina - Turmas.CODIGO
                        string disciplinaCodigo = leitor.GetValue(leitor.GetOrdinal("CODIGO")).ToString();
                        Debug.WriteLine("Cod: "+disciplinaCodigo);
                        string numero = leitor.GetString(leitor.GetOrdinal("TURMA"));
                        Debug.WriteLine("Turma:"+ numero);
                        int num = Convert.ToInt32(numero.Substring(0, 3));

                        Entities.Disciplina discip = disciDAO.GetDisciplina(disciplinaCodigo, calendarioId);

                        string datahora = leitor.GetValue(leitor.GetOrdinal("HORARIO")).ToString();

                        //Código do curso - Turmas.CURSO
                        string codigoCurso = leitor.GetValue(leitor.GetOrdinal("CURSO")).ToString();

                        Entities.Curso cur = curDAO.GetCurso(codigoCurso);

                        turma = Entities.TurmaSemProfessor.NewTurma(num, cal, discip, datahora, cur);

                        listaAux.Add(turma);
                    }
                }
                return listaAux;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados.", ex);
            }
        }

        public void CriaLOG(IDataReader dr)
        {
            
            string path = ConfigurationManager.AppSettings["PahtLog"];
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (StreamWriter sr = File.CreateText(path + DateTime.Now.ToString("dd-mm-yyyy") + ".log"))
            {
                sr.WriteLine("Turma: " + dr.GetString(dr.GetOrdinal("TURMA")));
                sr.WriteLine("Horario: " + dr.GetValue(dr.GetOrdinal("HORARIO")).ToString());
                sr.WriteLine("Codigo: " + dr.GetValue(dr.GetOrdinal("CODIGO")).ToString());
                sr.WriteLine("Professor: " + dr.GetValue(dr.GetOrdinal("PROFESSOR1")).ToString());
                sr.WriteLine("Curso: " + dr.GetValue(dr.GetOrdinal("CURSO")).ToString());
                sr.WriteLine("");
            }
        }

        /// <summary>
        /// Retorna todas os cursos cadastrados no SPA
        /// </summary>
        /// <returns></returns>
        public IList<Entities.Curso> GetCursos()
        {
            DbCommand cmdSelect = baseDados.GetSqlStringCommand(QueryMap.Default.Cursos);
            Entities.Curso curso = null;
            try
            {
                IList<Entities.Curso> listaAux = new List<Entities.Curso>();
                using (IDataReader leitor = baseDados.ExecuteReader(cmdSelect))
                {
                    while (leitor.Read())
                    {
                        string nome = leitor.GetValue(leitor.GetOrdinal("NOME")).ToString();
                        Entities.Faculdade faculdade = Entities.Faculdade.NewFaculdade(leitor.GetValue(leitor.GetOrdinal("UNIDADE")).ToString());

                        FaculdadesDAO facul = new FaculdadesDAO();
                        List<Faculdade> lista = facul.GetFaculdades();

                        foreach (Faculdade f in lista){
                            if (faculdade.Nome.ToString().Equals(f.Nome.ToString()))
                            {
                                //Guid id = new Guid());
                                faculdade.Id = f.Id;
                            }
                        }
                        
                        
                        string codigo = leitor.GetValue(leitor.GetOrdinal("CODIGO")).ToString();
                        curso = Entities.Curso.NewCurso(codigo, nome, faculdade);
                        listaAux.Add(curso);
                    }
                }
                return listaAux;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados.", ex);
            }
        }

        /// <summary>
        /// Retorna todas os professores cadastrados no SPA
        /// </summary>
        /// <returns></returns>
        public IList<Entities.Professor> GetProfessores()
        {
            DbCommand cmdSelect = baseDados.GetSqlStringCommand(QueryMap.Default.Professores);
            Entities.Professor professor = null;
            try
            {
                IList<Entities.Professor> listaAux = new List<Entities.Professor>();
                using (IDataReader leitor = baseDados.ExecuteReader(cmdSelect))
                {
                    while (leitor.Read())
                    {
                        string nomeProfessor = leitor.GetValue(leitor.GetOrdinal("Professor")).ToString();
                        string matricula = leitor.GetValue(leitor.GetOrdinal("Matricula")).ToString();
                        string correioEletronico = leitor.GetValue(leitor.GetOrdinal("Correio Eletronico")).ToString();
                        professor = Entities.Professor.NewProfessor(matricula, nomeProfessor, correioEletronico);
                        listaAux.Add(professor);
                    }
                }
                return listaAux;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados.", ex);
            }
        }

        public IList<Disciplina> GetDisciplinas(Guid calendarioId)
        {
            try
            {
            DbCommand cmdSelect = baseDados.GetSqlStringCommand(QueryMap.Default.Disciplinas);
            Disciplina disciplina = null;
            CalendariosDAO calendariodao = new CalendariosDAO();
            CategoriaDisciplinaDAO catdisDAO = new CategoriaDisciplinaDAO();

            IList<Disciplina> listaAux = new List<Disciplina>();
                using (IDataReader leitor = baseDados.ExecuteReader(cmdSelect))
                {
                    while (leitor.Read())
                    {
                        string nome = leitor.GetValue(leitor.GetOrdinal("NOME")).ToString();
                        string cod = leitor.GetValue(leitor.GetOrdinal("CODIGO")).ToString();
                        int cred = Convert.ToInt32(leitor.GetValue(leitor.GetOrdinal("CREDITOS")).ToString());
                        bool g2 = true;

                        
                        List<CategoriaDisciplina> listCd = catdisDAO.GetCategoriaDisciplinas();
                        CategoriaDisciplina cd = null;

                        //Verifica se existe no banco uma Categoria "auxiliar" chamada 'CategoriaDisciplinaImportação'
                        //É esta que vai ser ligada às disciplinas importadas, para não haver conflito
                        //Se o banco estiver vazio por algum motivo, recria a tal categoria
                        foreach (CategoriaDisciplina cdis in listCd)
                        {
                            if (cdis.Descricao == "CategoriaDisciplinaImportação")
                            {
                                cd = cdis;
                                break;
                            }
                        }
                        if (cd == null)
                        {
                            cd = CategoriaDisciplina.NewCategoriaDisciplina("CategoriaDisciplinaImportação",
                                                                            new Dictionary<CategoriaRecurso,double>());
                            catdisDAO.InsereCategoriaDisciplina(cd);
                        }

                        disciplina = Disciplina.GetDisciplina(cod, cred, nome, g2, calendariodao.GetCalendario(calendarioId),cd);
                        listaAux.Add(disciplina);
                    }
                    
                }
                return listaAux;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados.", ex);
            }
        }
        
    }
}
