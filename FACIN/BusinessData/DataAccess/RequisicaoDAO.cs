using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using BusinessData.Entities;
using System.Drawing;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace BusinessData.DataAccess
{
    internal class RequisicaoDAO
    {
        CategoriaDisciplinaDAO catdiscipDAO = new CategoriaDisciplinaDAO();

        private Database _baseDados;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Professore
        /// </summary>
        public RequisicaoDAO()
        {
            try
            {
                _baseDados = DatabaseFactory.CreateDatabase("SARCFACINcs");
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Deleta uma Requisicao
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        public void DeleteRequisicao(Guid id)
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicoesDelete");
            _baseDados.AddInParameter(cmd, "@RequisicaoId", DbType.Guid, id);
            try
            {
                _baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Insere uma Requisicao
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="professor"></param>
        public void InsertRequisicao(IRequisicao requisicao)
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicoesInsert");
            _baseDados.AddInParameter(cmd, "@RequisicaoId", DbType.Guid, requisicao.IdRequisicao);
            _baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, requisicao.Aula.Id);
            _baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, requisicao.CategoriaRecurso.Id);
            _baseDados.AddInParameter(cmd, "@Prioridade", DbType.Int32, requisicao.Prioridade);

            try
            {
                _baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Retorna a Requisiçao relativa ao Id especificado
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Requisicao> GetRequisicaoByAulaAndPrioridade(Guid aulaId, Entities.Calendario cal, int prioridade)
        {
            BusinessData.Entities.Aula aula;
            BusinessData.Entities.CategoriaAtividade categoriaAtividade;
            BusinessData.Entities.Professor professor;
            BusinessData.Entities.Faculdade faculdade;
            BusinessData.Entities.Curso curso;
            BusinessData.Entities.Disciplina disciplina;
            BusinessData.Entities.CategoriaRecurso categoriaRecurso;
            BusinessData.Entities.Turma turma;
            BusinessData.Entities.CategoriaDisciplina categoria;

            DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicoesByAulaAndPrioridade");
            _baseDados.AddInParameter(cmd, "@IdAula", DbType.Guid, aulaId);
            _baseDados.AddInParameter(cmd, "@Prioridade", DbType.Int32, prioridade);
            Entities.Requisicao aux = null;
            try
            {
                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    List<Requisicao> listaAux = new List<Requisicao>();
                    while (leitor.Read())
                    {

                        faculdade = Entities.Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("idFaculdadeCurso")),
                                                                    leitor.GetString(leitor.GetOrdinal("nomeFaculdade")));

                        curso = Entities.Curso.GetCurso(leitor.GetString(leitor.GetOrdinal("cursoTurma")),
                                                        leitor.GetString(leitor.GetOrdinal("nomeCurso")), faculdade);

                        professor = Entities.Professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("idProfessorTurma")),
                                                                    leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                                    leitor.GetString(leitor.GetOrdinal("Nome")),
                                                                    leitor.GetString(leitor.GetOrdinal("Email")));

                        categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("categoriaDisciplina")));

                        disciplina = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("codDisciplinaTurma")),
                                                                       leitor.GetInt32(leitor.GetOrdinal("credDisciplina")),
                                                                       leitor.GetString(leitor.GetOrdinal("nomeDisciplina")),
                                                                       leitor.GetBoolean(leitor.GetOrdinal("g2Disciplina")),
                                                                       cal,
                                                                       categoria);

                        categoriaAtividade = Entities.CategoriaAtividade.GetCategoriaAtividade(leitor.GetGuid(leitor.GetOrdinal("idCategoriaAtividadeAula")),
                                                                                               leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                                                   Color.FromArgb(leitor.GetInt32(leitor.GetOrdinal("Cor"))));

                        turma = Entities.Turma.GetTurma(leitor.GetGuid(leitor.GetOrdinal("idTurmaAula")),
                                                           leitor.GetInt32(leitor.GetOrdinal("numeroTurma")),
                                                           cal, disciplina, leitor.GetString(leitor.GetOrdinal("dataHoraTurma")),
                                                           professor, curso);

                        aula = Entities.Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("idRequisicaoAula")), (BusinessData.Entities.Turma)turma, leitor.GetString(leitor.GetOrdinal("horarioAula")),
                                                     leitor.GetDateTime(leitor.GetOrdinal("dataAula")), leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                     categoriaAtividade);


                        categoriaRecurso = Entities.CategoriaRecurso.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("idCategoriaRecurso")),
                                                                                        leitor.GetString(leitor.GetOrdinal("descricaoCategoriaRecurso")));


                        aux = Entities.Requisicao.GetRequisicao(aula, leitor.GetGuid(leitor.GetOrdinal("idRequisicao")), categoriaRecurso, leitor.GetInt32(leitor.GetOrdinal("prioridadeRequisicao")), leitor.GetBoolean(leitor.GetOrdinal("estaAtendido")));
                        listaAux.Add(aux);
                    }
                    return listaAux;
                }
            }


            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }

        }
        /// <summary>
        /// Retorna a primeira requisicao que atenda os parametros especificados
        /// </summary>
        /// <param name="aulaId"></param>
        /// <param name="cal"></param>
        /// <param name="prioridade"></param>
        /// <returns></returns>
        public Requisicao GetRequisicaoByAulaPrioridadeAndCategoria(Guid aulaId, Entities.Calendario cal, int prioridade, Guid catrecursoId)
        {
            BusinessData.Entities.Aula aula;
            BusinessData.Entities.CategoriaAtividade categoriaAtividade;
            BusinessData.Entities.Professor professor;
            BusinessData.Entities.Faculdade faculdade;
            BusinessData.Entities.Curso curso;
            BusinessData.Entities.Disciplina disciplina;
            BusinessData.Entities.CategoriaDisciplina categoria;
            BusinessData.Entities.CategoriaRecurso categoriaRecurso;
            BusinessData.Entities.Turma turma;

            DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicaoByAulaPrioridadeAndCategoria");
            _baseDados.AddInParameter(cmd, "@IdAula", DbType.Guid, aulaId);
            _baseDados.AddInParameter(cmd, "@Prioridade", DbType.Int32, prioridade);
            _baseDados.AddInParameter(cmd, "@IdCategoriaRecurso", DbType.Guid, catrecursoId);
            Entities.Requisicao aux = null;
            try
            {
                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    try
                    {
                        leitor.Read();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    faculdade = Entities.Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("idFaculdadeCurso")),
                                                                leitor.GetString(leitor.GetOrdinal("nomeFaculdade")));

                    curso = Entities.Curso.GetCurso(leitor.GetString(leitor.GetOrdinal("cursoTurma")),
                                                    leitor.GetString(leitor.GetOrdinal("nomeCurso")), faculdade);

                    professor = Entities.Professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("idProfessorTurma")),
                                                                leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                                leitor.GetString(leitor.GetOrdinal("Nome")),
                                                                leitor.GetString(leitor.GetOrdinal("Email")));

                    categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("categoriaDisciplina")));

                    disciplina = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("codDisciplinaTurma")),
                                                                   leitor.GetInt32(leitor.GetOrdinal("credDisciplina")),
                                                                   leitor.GetString(leitor.GetOrdinal("nomeDisciplina")),
                                                                   leitor.GetBoolean(leitor.GetOrdinal("g2Disciplina")),
                                                                   cal,
                                                                   categoria);

                    categoriaAtividade = Entities.CategoriaAtividade.GetCategoriaAtividade(leitor.GetGuid(leitor.GetOrdinal("idCategoriaAtividadeAula")),
                                                                                           leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                                               Color.FromArgb(leitor.GetInt32(leitor.GetOrdinal("Cor"))));

                    turma = Entities.Turma.GetTurma(leitor.GetGuid(leitor.GetOrdinal("idTurmaAula")),
                                                       leitor.GetInt32(leitor.GetOrdinal("numeroTurma")),
                                                       cal, disciplina, leitor.GetString(leitor.GetOrdinal("dataHoraTurma")),
                                                       professor, curso);

                    aula = Entities.Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("idRequisicaoAula")), (BusinessData.Entities.Turma)turma, leitor.GetString(leitor.GetOrdinal("horarioAula")),
                                                 leitor.GetDateTime(leitor.GetOrdinal("dataAula")), leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                 categoriaAtividade);


                    categoriaRecurso = Entities.CategoriaRecurso.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("idCategoriaRecurso")),
                                                                                    leitor.GetString(leitor.GetOrdinal("descricaoCategoriaRecurso")));


                    aux = Entities.Requisicao.GetRequisicao(aula, leitor.GetGuid(leitor.GetOrdinal("idRequisicao")), categoriaRecurso, leitor.GetInt32(leitor.GetOrdinal("prioridadeRequisicao")), leitor.GetBoolean(leitor.GetOrdinal("requisicaoEstaAtendida")));

                }
                return aux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }

        }
        /// <summary>
        /// Lista todas as requisições de recursos feitas por uma turma em uma aula especificada
        /// </summary>
        /// <param name="au">Aula que se deseja obter os recursos requisitados</param>
        /// <param name="cal">Calendario atual</param>
        /// <returns></returns>
        public List<Requisicao> GetRequisicoesPorAula(Guid? aulaId, Calendario cal)
        {
            BusinessData.Entities.Aula aula;
            BusinessData.Entities.CategoriaAtividade categoriaAtividade;
            BusinessData.Entities.Professor professor;
            BusinessData.Entities.Faculdade faculdade;
            BusinessData.Entities.Curso curso;
            BusinessData.Entities.Disciplina disciplina;
            BusinessData.Entities.CategoriaDisciplina categoria;
            BusinessData.Entities.CategoriaRecurso categoriaRecurso;
            BusinessData.Entities.Turma turma;

            DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicoesSelectByAula");
            _baseDados.AddInParameter(cmd, "@IdAula", DbType.Guid, aulaId);
            Entities.Requisicao aux = null;
            try
            {
                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    List<Requisicao> listaAux = new List<Requisicao>();
                    while (leitor.Read())
                    {

                        faculdade = Entities.Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("idFaculdadeCurso")),
                                                                    leitor.GetString(leitor.GetOrdinal("nomeFaculdade")));

                        curso = Entities.Curso.GetCurso(leitor.GetString(leitor.GetOrdinal("cursoTurma")),
                                                        leitor.GetString(leitor.GetOrdinal("nomeCurso")), faculdade);

                        professor = Entities.Professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("idProfessorTurma")),
                                                                    leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                                    leitor.GetString(leitor.GetOrdinal("Nome")),
                                                                    leitor.GetString(leitor.GetOrdinal("Email")));

                        categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("categoriaDisciplina")));

                        disciplina = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("codDisciplinaTurma")),
                                                                       leitor.GetInt32(leitor.GetOrdinal("credDisciplina")),
                                                                       leitor.GetString(leitor.GetOrdinal("nomeDisciplina")),
                                                                       leitor.GetBoolean(leitor.GetOrdinal("g2Disciplina")),
                                                                       cal,
                                                                       categoria);

                        categoriaAtividade = Entities.CategoriaAtividade.GetCategoriaAtividade(leitor.GetGuid(leitor.GetOrdinal("idCategoriaAtividadeAula")),
                                                                                               leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                                                   Color.FromName(leitor.GetString(leitor.GetOrdinal("Cor"))));

                        turma = Entities.Turma.GetTurma(leitor.GetGuid(leitor.GetOrdinal("idTurmaAula")),
                                                           leitor.GetInt32(leitor.GetOrdinal("numeroTurma")),
                                                           cal, disciplina, leitor.GetString(leitor.GetOrdinal("dataHoraTurma")),
                                                           professor, curso);

                        aula = Entities.Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("idRequisicaoAula")), (BusinessData.Entities.Turma)turma, leitor.GetString(leitor.GetOrdinal("horarioAula")),
                                                     leitor.GetDateTime(leitor.GetOrdinal("dataAula")), leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                     categoriaAtividade);


                        categoriaRecurso = Entities.CategoriaRecurso.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("idCategoriaRecurso")),
                                                                                        leitor.GetString(leitor.GetOrdinal("descricaoCategoriaRecurso")));


                        aux = Entities.Requisicao.GetRequisicao(aula, leitor.GetGuid(leitor.GetOrdinal("idRequisicao")), categoriaRecurso, leitor.GetInt32(leitor.GetOrdinal("prioridadeRequisicao")), leitor.GetBoolean(leitor.GetOrdinal("requisicaoEstaAtendida")));
                        listaAux.Add(aux);
                    }
                    return listaAux;
                }
            }


            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>RequisicoesSelectByCalendarioAndCategoriaRecurso
        /// Retorna a Requisiçao relativa ao Id especificado
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Requisicao GetRequisicaoByTurma(Entities.Turma turma, Entities.Calendario cal)
        {
            BusinessData.Entities.Aula aula;
            BusinessData.Entities.CategoriaAtividade categoriaAtividade;
            BusinessData.Entities.Professor professor;
            BusinessData.Entities.Faculdade faculdade;
            BusinessData.Entities.Curso curso;
            BusinessData.Entities.Disciplina disciplina;
            BusinessData.Entities.CategoriaDisciplina categoria;
            BusinessData.Entities.CategoriaRecurso categoriaRecurso;

            DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicoesByTurma");
            _baseDados.AddInParameter(cmd, "@IdTurma", DbType.Guid, turma.Id);

            Entities.Requisicao aux = null;
            try
            {
                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    try
                    {
                        leitor.Read();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    faculdade = Entities.Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("idFaculdadeCurso")),
                                                                leitor.GetString(leitor.GetOrdinal("nomeFaculdade")));

                    curso = Entities.Curso.GetCurso(leitor.GetString(leitor.GetOrdinal("cursoTurma")),
                                                    leitor.GetString(leitor.GetOrdinal("nomeCurso")), faculdade);

                    professor = Entities.Professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("idProfessorTurma")),
                                                                leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                                leitor.GetString(leitor.GetOrdinal("nomeProfessor")),
                                                                leitor.GetString(leitor.GetOrdinal("Email")));

                    categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("categoriaDisciplina")));

                    disciplina = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("odDisciplinaTurma")),
                                                                   leitor.GetInt32(leitor.GetOrdinal("credDisciplina")),
                                                                   leitor.GetString(leitor.GetOrdinal("nomeDisciplina")),
                                                                   leitor.GetBoolean(leitor.GetOrdinal("g2Disciplina")),
                                                                   cal,
                                                                   categoria);

                    categoriaAtividade = CategoriaAtividade.GetCategoriaAtividade(leitor.GetGuid(leitor.GetOrdinal("idCategoriaAtividadeAula")),
                                                                                  leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                                                   Color.FromArgb(leitor.GetInt32(leitor.GetOrdinal("Cor"))));

                    turma = Entities.Turma.GetTurma(leitor.GetGuid(leitor.GetOrdinal("idTurmaAula")),
                                                       leitor.GetInt32(leitor.GetOrdinal("numeroTurma")),
                                                       cal, disciplina, leitor.GetString(leitor.GetOrdinal("dataHoraTurma")),
                                                       professor, curso);

                    aula = Entities.Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("idRequisicaoAula")), (BusinessData.Entities.Turma)turma, leitor.GetString(leitor.GetOrdinal("horarioAula")),
                                                 leitor.GetDateTime(leitor.GetOrdinal("dataAula")), leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                 categoriaAtividade);


                    categoriaRecurso = Entities.CategoriaRecurso.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("idCategoriaRecurso")),
                                                                                    leitor.GetString(leitor.GetOrdinal("descricaoCategoriaRecurso")));


                    aux = Entities.Requisicao.GetRequisicao(aula, leitor.GetGuid(leitor.GetOrdinal("idRequisicao")), categoriaRecurso, leitor.GetInt32(leitor.GetOrdinal("prioridadeRequisicao")), leitor.GetBoolean(leitor.GetOrdinal("requisicaoEstaAtendida")));

                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;

        }

        public IList<Requisicao> GetRequisicoesByCalendario(Calendario cal)
        {
            BusinessData.Entities.Aula aula;
            BusinessData.Entities.CategoriaAtividade categoriaAtividade;
            BusinessData.Entities.Professor professor;
            BusinessData.Entities.Faculdade faculdade;
            BusinessData.Entities.Curso curso;
            BusinessData.Entities.Disciplina disciplina;
            BusinessData.Entities.CategoriaDisciplina categoria;
            BusinessData.Entities.Turma turma;
            BusinessData.Entities.CategoriaRecurso catRecurso;

            DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicoesSelectByCalendario");
            _baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);

            Entities.Requisicao aux = null;
            List<Requisicao> listaAux = new List<Requisicao>();
            try
            {

                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        try
                        {
                            faculdade = Entities.Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("idFaculdadeCurso")),
                                                                    leitor.GetString(leitor.GetOrdinal("nomeFaculdade")));

                            curso = Entities.Curso.GetCurso(leitor.GetString(leitor.GetOrdinal("cursoTurma")),
                                                        leitor.GetString(leitor.GetOrdinal("nomeCurso")), faculdade);

                            professor = Entities.Professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("idProfessorTurma")),
                                                                    leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                                    leitor.GetString(leitor.GetOrdinal("Nome")),
                                                                    leitor.GetString(leitor.GetOrdinal("Email")));

                            categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("categoriaDisciplina")));

                            disciplina = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("codDisciplinaTurma")),
                                                                       leitor.GetInt32(leitor.GetOrdinal("credDisciplina")),
                                                                       leitor.GetString(leitor.GetOrdinal("nomeDisciplina")),
                                                                       leitor.GetBoolean(leitor.GetOrdinal("g2Disciplina")),
                                                                       cal,
                                                                       categoria);

                            categoriaAtividade = Entities.CategoriaAtividade.GetCategoriaAtividade(leitor.GetGuid(leitor.GetOrdinal("idCategoriaAtividadeAula")),
                                                                                               leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                                                   Color.FromName(leitor.GetString(leitor.GetOrdinal("Cor"))));

                            turma = Entities.Turma.GetTurma(leitor.GetGuid(leitor.GetOrdinal("idTurmaAula")),
                                                           leitor.GetInt32(leitor.GetOrdinal("numeroTurma")),
                                                           cal, disciplina, leitor.GetString(leitor.GetOrdinal("dataHoraTurma")),
                                                           professor, curso);

                            aula = Entities.Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("idRequisicaoAula")), (BusinessData.Entities.Turma)turma, leitor.GetString(leitor.GetOrdinal("horarioAula")),
                                                     leitor.GetDateTime(leitor.GetOrdinal("dataAula")), leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                     categoriaAtividade);

                            catRecurso = Entities.CategoriaRecurso.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("idCategoriaRecurso")),
                                leitor.GetString(leitor.GetOrdinal("descricaoCategoriaRecurso")));

                            aux = Entities.Requisicao.GetRequisicao(aula, leitor.GetGuid(leitor.GetOrdinal("idRequisicao")), catRecurso, leitor.GetInt32(leitor.GetOrdinal("prioridadeRequisicao")), leitor.GetBoolean(leitor.GetOrdinal("requisicaoEstaAtendida")));
                            listaAux.Add(aux);
                        }
                        catch (InvalidOperationException)
                        {
                            return new List<Requisicao>();
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;
        }

        public IList<Requisicao> GetRequisicoesByCalendario(Calendario cal, CategoriaRecurso cat)
        {
            BusinessData.Entities.Aula aula;
            BusinessData.Entities.CategoriaAtividade categoriaAtividade;
            BusinessData.Entities.Professor professor;
            BusinessData.Entities.Faculdade faculdade;
            BusinessData.Entities.Curso curso;
            BusinessData.Entities.Disciplina disciplina;
            BusinessData.Entities.CategoriaDisciplina categoria;
            BusinessData.Entities.Turma turma;

            DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicoesSelectByCalendarioAndCategoriaRecurso");
            _baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, cat.Id);
            _baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);

            Entities.Requisicao aux = null;
            List<Requisicao> listaAux = new List<Requisicao>();
            try
            {

                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        try
                        {
                            faculdade = Entities.Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("idFaculdadeCurso")),
                                                                    leitor.GetString(leitor.GetOrdinal("nomeFaculdade")));

                            curso = Entities.Curso.GetCurso(leitor.GetString(leitor.GetOrdinal("cursoTurma")),
                                                        leitor.GetString(leitor.GetOrdinal("nomeCurso")), faculdade);

                            professor = Entities.Professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("idProfessorTurma")),
                                                                    leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                                    leitor.GetString(leitor.GetOrdinal("Nome")),
                                                                    leitor.GetString(leitor.GetOrdinal("Email")));

                            categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("categoriaDisciplina")));

                            disciplina = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("codDisciplinaTurma")),
                                                                       leitor.GetInt32(leitor.GetOrdinal("credDisciplina")),
                                                                       leitor.GetString(leitor.GetOrdinal("nomeDisciplina")),
                                                                       leitor.GetBoolean(leitor.GetOrdinal("g2Disciplina")),
                                                                       cal,
                                                                       categoria);

                            categoriaAtividade = Entities.CategoriaAtividade.GetCategoriaAtividade(leitor.GetGuid(leitor.GetOrdinal("idCategoriaAtividadeAula")),
                                                                                               leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                                                   Color.FromName(leitor.GetString(leitor.GetOrdinal("Cor"))));

                            turma = Entities.Turma.GetTurma(leitor.GetGuid(leitor.GetOrdinal("idTurmaAula")),
                                                           leitor.GetInt32(leitor.GetOrdinal("numeroTurma")),
                                                           cal, disciplina, leitor.GetString(leitor.GetOrdinal("dataHoraTurma")),
                                                           professor, curso);

                            aula = Entities.Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("idRequisicaoAula")), (BusinessData.Entities.Turma)turma, leitor.GetString(leitor.GetOrdinal("horarioAula")),
                                                     leitor.GetDateTime(leitor.GetOrdinal("dataAula")), leitor.GetString(leitor.GetOrdinal("descricaoAtividadeAula")),
                                                     categoriaAtividade);


                            aux = Entities.Requisicao.GetRequisicao(aula, leitor.GetGuid(leitor.GetOrdinal("idRequisicao")), cat, leitor.GetInt32(leitor.GetOrdinal("prioridadeRequisicao")), leitor.GetBoolean(leitor.GetOrdinal("requisicaoEstaAtendida")));
                            listaAux.Add(aux);
                        }
                        catch (InvalidOperationException)
                        {
                            return new List<Requisicao>();
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;
        }

        public void SetAtendida(Guid reqId)
        {
            try
            {
                DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicoesSetAtendida");
                _baseDados.AddInParameter(cmd, "@RequisicaoId", DbType.Guid, reqId);
                _baseDados.AddInParameter(cmd, "@EstaAtendida", DbType.Boolean, true);

                _baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
                    
        }

        public void UpdateRequisicoes(IRequisicao req)
        {
            try
            {
                DbCommand cmd = _baseDados.GetStoredProcCommand("RequisicoesUpdate");
                _baseDados.AddInParameter(cmd, "@RequisicaoId", DbType.Guid, req.IdRequisicao);
                _baseDados.AddInParameter(cmd, "@Prioridade", DbType.Int32, Convert.ToInt32(req.Prioridade));
                _baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, req.CategoriaRecurso.Id);
                _baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
    }
}

