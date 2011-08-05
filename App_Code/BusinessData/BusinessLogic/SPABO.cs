using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.DataAccess.SPA;
using BusinessData.Entities;
using System.Collections;
using System.Web.UI;

namespace BusinessData.BusinessLogic
{
    public class SPABO
    {
        private SPADAO acessoDados;
       
        public SPABO()
        {
            acessoDados = new SPADAO();
        }

        public IList<Faculdade> GetFaculdades()
        {
            return acessoDados.GetFaculdades();
        }

        public IList<Curso> GetCursos()
        {
            return acessoDados.GetCursos();
        }

        public IList<Professor> getProfessores()
        {
            return acessoDados.GetProfessores();
        }

        public IList<Turma> getTurmas(Guid calendarioId)
        {
            return acessoDados.GetTurmas(calendarioId);
        }

        public IList<TurmaSemProfessor> getTurmasSemProfessor(Guid calendarioId)
        {
            return acessoDados.GetProfessorNone(calendarioId);
        }

        public IList<Disciplina> GetDisciplinas(Guid calendarioId)
        {
            return acessoDados.GetDisciplinas(calendarioId);
        }

        public void ImportarFaculdades()
        {
            try
            {
                FaculdadesBO controleFaculdades = new FaculdadesBO();
                IList<Faculdade> faculdadesCadastradas = controleFaculdades.GetFaculdades();

                IList<Faculdade> faculdadesImportadas = this.GetFaculdades();
                foreach (Faculdade faculdadeAtual in faculdadesImportadas)
                {
                    if (!faculdadesCadastradas.Contains(faculdadeAtual))
                    {
                        controleFaculdades.InsereFaculdade(faculdadeAtual);
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
        }

        public void ImportarProfessores()
        {
            bool existe = false;
            try
            {
                ProfessoresBO controleProfessores = new ProfessoresBO();
                IList<Professor> professoresCadastrados = (List<Professor>)controleProfessores.GetProfessores();
                IList<Professor> professoresImportados = this.getProfessores();

                foreach (Professor profAtual in professoresImportados)
                {
                    foreach (Professor profCadastrado in professoresCadastrados)
                    {
                        if (profCadastrado.Equals(profAtual))
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (!existe)
                    {
                        controleProfessores.InsertPessoa(profAtual, "pergunta", profAtual.Matricula);
                    }
                    else
                    {
                        existe = false;
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
        }

        public void ImportarCursos()
        {
            try
            {
                CursosBO controleCursos = new CursosBO();
                IList<Curso> cursosCadastrados = controleCursos.GetCursos();
                IList<Curso> cursosImportados = this.GetCursos();

                foreach (Curso cursoAtual in cursosImportados)
                {
                    if (!cursosCadastrados.Contains(cursoAtual))
                    {
                        controleCursos.InsereCurso(cursoAtual);
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
        }
        /// <summary>
        /// Importa turmas da tabela Turmas
        /// </summary>
        /// <param name="calendarioId"></param>
        public void ImportarTurmas(Guid calendarioId)
        {
            try
            {
                TurmaBO controleTurmas = new TurmaBO();
                IList<Turma> turmasCadastradas = controleTurmas.GetTurmas();

                IList<Turma> turmasImportadas = this.getTurmas(calendarioId);
                foreach (Turma turmaAtual in turmasImportadas)
                {
                    if (!turmasCadastradas.Contains(turmaAtual))
                    {
                        controleTurmas.InsereTurma(turmaAtual);
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
        }

        /// <summary>
        /// Importa turmas para a tabela Turmas
        /// </summary>
        /// <param name="lista"></param>
        public void ImportaTurmas(IList<Turma> lista)
        {
            TurmaBO turmaBo = new TurmaBO();
            foreach (Turma t in lista)
            {
                turmaBo.InsereTurma(t);
            }
        }

        /// <summary>
        /// Importa professores sem nome da tabela Turmas
        /// </summary>
        /// <param name="calendarioId"></param>
        public void ImportarTurmasSemProfessor(Guid calendarioId)
        {
            try
            {
                TurmaBO controleTurmas = new TurmaBO();
                IList<Turma> turmasCadastradas = controleTurmas.GetTurmas();

                IList<TurmaSemProfessor> turmasImportadas = this.getTurmasSemProfessor(calendarioId);
                //foreach (Turma turmaAtual in turmasImportadas)
                //{
                //    if (!turmasCadastradas.Contains(turmaAtual))
                //    {
                //        controleTurmas.InsereTurma(turmaAtual);
                //    }
                //}
            }
            catch (Exception )
            {
                throw;
            }
        }

        public IList<Turma> turmaCalendario(Guid calendarioId)
        {
            IList<Turma> arrayTurmas = this.getTurmas(calendarioId);
            return arrayTurmas;
        }

        public void ImportarDiscplinas(Guid calendarioId)
        {
            try
            {
                DisciplinasBO controleDisciplinas = new DisciplinasBO();
                IList<Disciplina> disciplinasCadastradas = controleDisciplinas.GetDisciplinas();
                IList<Disciplina> disciplinasImportadas = this.GetDisciplinas(calendarioId);
                List<Disciplina> disciplinasInCalendario = controleDisciplinas.GetDisciplinaInCalendario(calendarioId);
                
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
                        //insere na tabela disciplinas e disciplinasincalendario
                        controleDisciplinas.InsereDisciplina(disciplinaAtual);
                    }
                    else
                    {
//                        if (!dic_disciplinasInCalendario.ContainsKey(disciplinaAtual.Cod))
                            if (!disciplinasInCalendario.Contains(disciplinaAtual))
                            {
                                //insere apenas na tabela disciplinasincalendario 
                                controleDisciplinas.InsereDisciplinaInCalendario(disciplinaAtual, calendarioId);
                            }
                    }          
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
