using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using BusinessData.Entities;
using System.Data.SqlClient;

namespace BusinessData.DataAccess
{
    internal class TurmaDAO
    {
        private Database baseDados;
       

        public TurmaDAO()
        {
            try
            {
                baseDados = DatabaseFactory.CreateDatabase("SARCFACINcs");
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void InsereTurma(Turma turma)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("TurmasInsere");
            baseDados.AddInParameter(cmd, "@TurmaId", DbType.Guid, turma.Id);
            baseDados.AddInParameter(cmd, "@Numero", DbType.Int32, turma.Numero);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, turma.Calendario.Id);
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, turma.Disciplina.Cod);
            baseDados.AddInParameter(cmd, "@DataHora", DbType.String, turma.DataHora);
            baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, turma.Professor.Id);
            baseDados.AddInParameter(cmd, "@Curso", DbType.String, turma.Curso.Codigo);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void RemoveTurma(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("TurmasDelete");
            baseDados.AddInParameter(cmd, "@TurmaId", DbType.Guid, id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void AtualizaTurma(Turma turma)
        {

            DbCommand cmd = baseDados.GetStoredProcCommand("TurmasUpdate");

            baseDados.AddInParameter(cmd, "@TurmaId", DbType.Guid, turma.Id);
            baseDados.AddInParameter(cmd, "@Numero", DbType.Int32, turma.Numero);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, turma.Calendario.Id);
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, turma.Disciplina.Cod);
            baseDados.AddInParameter(cmd, "@DataHora", DbType.String, turma.DataHora);
            baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, turma.Professor.Id);
            baseDados.AddInParameter(cmd, "@Curso", DbType.String, turma.Curso.Codigo);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public Turma GetTurma(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("TurmasSelectById");
            baseDados.AddInParameter(cmd, "@TurmaId", DbType.Guid, id);

            Entities.Turma aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();
                    CalendariosDAO calendarios = new CalendariosDAO();
                    Entities.Calendario cal = calendarios.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                    DisciplinasDAO disciplinas = new DisciplinasDAO();
                    Entities.Disciplina disc = disciplinas.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")), leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                    ProfessorDAO professor = new ProfessorDAO();
                    Entities.Professor prof = professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("ProfessorId")));

                    CursosDAO cursos = new CursosDAO();
                    Entities.Curso curso = cursos.GetCurso(leitor.GetString(leitor.GetOrdinal("Curso")));

                    aux = Entities.Turma.GetTurma(
                                                   leitor.GetGuid(leitor.GetOrdinal("TurmaId")),
                                                   leitor.GetInt32(leitor.GetOrdinal("Numero")),
                                                   cal, disc,
                                                   leitor.GetString(leitor.GetOrdinal("DataHora")), prof,
                                                   curso);
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }

        public Turma GetTurma(Guid id, Calendario cal)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("TurmasSelectById");
            baseDados.AddInParameter(cmd, "@TurmaId", DbType.Guid, id);

            Entities.Turma aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    DisciplinasDAO disciplinas = new DisciplinasDAO();
                    Entities.Disciplina disc = disciplinas.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")), cal);

                    ProfessorDAO professor = new ProfessorDAO();
                    Entities.Professor prof = professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("ProfessorId")));

                    CursosDAO cursos = new CursosDAO();
                    Entities.Curso curso = cursos.GetCurso(leitor.GetString(leitor.GetOrdinal("Curso")));

                    aux = Entities.Turma.GetTurma(
                                                   leitor.GetGuid(leitor.GetOrdinal("TurmaId")),
                                                   leitor.GetInt32(leitor.GetOrdinal("Numero")),
                                                   cal, disc,
                                                   leitor.GetString(leitor.GetOrdinal("DataHora")), prof,
                                                   curso);
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            catch (Exception )
            {
                return null;
            }
            return aux;
        }

        public List<Turma> GetTurmas()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("TurmasSelectAll");
            List<Entities.Turma> listaAux = new List<BusinessData.Entities.Turma>();
            Entities.Turma aux;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    DataAccess.CalendariosDAO calendarios = new CalendariosDAO();
                    DataAccess.DisciplinasDAO disciplinas = new DisciplinasDAO();
                    DataAccess.ProfessorDAO professor = new ProfessorDAO();
                    DataAccess.CursosDAO cursos = new CursosDAO();
                    while (leitor.Read())
                    {
                        Entities.Calendario cal = calendarios.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));
                        Entities.Disciplina disc = disciplinas.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")), leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));
                        Entities.Professor prof = professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("ProfessorId")));
                        Entities.Curso curso = cursos.GetCurso(leitor.GetString(leitor.GetOrdinal("Curso")));

                        aux = Entities.Turma.GetTurma( leitor.GetGuid(leitor.GetOrdinal("TurmaId")),
                                                       leitor.GetInt32(leitor.GetOrdinal("Numero")),
                                                       cal, disc,
                                                       leitor.GetString(leitor.GetOrdinal("DataHora")),
                                                       prof, curso);
                        listaAux.Add(aux);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;
        }
        
        public List<Turma> GetTurmas(Calendario cal, List<CategoriaDisciplina> categoriasDeDisciplina)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("TurmasSelect");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
            List<Entities.Turma> listaAux = new List<BusinessData.Entities.Turma>();
            Entities.Turma aux;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    DataAccess.DisciplinasDAO disciplinas = new DisciplinasDAO();
                    Entities.Disciplina disc = null;

                    DataAccess.ProfessorDAO professor = new ProfessorDAO();
                    Entities.Professor prof = null;

                    CursosDAO cursos = new CursosDAO();
                    Entities.Curso curso = null;



                    while (leitor.Read())
                    {
                        disc = disciplinas.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")), cal,categoriasDeDisciplina);
                        prof = professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("ProfessorId")));
                        curso = cursos.GetCurso(leitor.GetString(leitor.GetOrdinal("Curso")));

                        aux = Entities.Turma.GetTurma(
                                                       leitor.GetGuid(leitor.GetOrdinal("TurmaId")),
                                                       leitor.GetInt32(leitor.GetOrdinal("Numero")),
                                                       cal, disc,
                                                       leitor.GetString(leitor.GetOrdinal("DataHora")),
                                                       prof, curso);
                        listaAux.Add(aux);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux; 
        }
        
        public List<Turma> GetTurmas(Calendario cal)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("TurmasSelect");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
            List<Entities.Turma> listaAux = new List<BusinessData.Entities.Turma>();
            Entities.Turma aux;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    DataAccess.DisciplinasDAO disciplinas = new DisciplinasDAO();
                    Entities.Disciplina disc = null;  

                    DataAccess.ProfessorDAO professor = new ProfessorDAO();
                    Entities.Professor prof = null;

                    CursosDAO cursos = new CursosDAO();
                    Entities.Curso curso = null; 



                    while (leitor.Read())
                    {
                        disc = disciplinas.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")), cal);
                        prof = professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("ProfessorId")));
                        curso = cursos.GetCurso(leitor.GetString(leitor.GetOrdinal("Curso"))); 

                        aux = Entities.Turma.GetTurma(
                                                       leitor.GetGuid(leitor.GetOrdinal("TurmaId")),
                                                       leitor.GetInt32(leitor.GetOrdinal("Numero")),
                                                       cal, disc,
                                                       leitor.GetString(leitor.GetOrdinal("DataHora")),
                                                       prof, curso);
                        listaAux.Add(aux);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;
        }

        public List<Turma> GetTurmas(Calendario cal, Professor professor)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TurmasSelectByProfessor");
                baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
                baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, professor.Id);

                CursosDAO cursoDAO = new CursosDAO();

                List<Turma> resultado = new List<Turma>();

                Turma aux = null;
                Disciplina disc = null;
                Curso curso = null;

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    CategoriaDisciplinaDAO catdiscipDAO = new CategoriaDisciplinaDAO();

                    while (leitor.Read())
                    {
                        curso = cursoDAO.GetCurso(leitor.GetString(leitor.GetOrdinal("Curso")));

                        Entities.CategoriaDisciplina categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("categoriaDisciplina")));

                        disc = Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")),
                                                        leitor.GetInt32(leitor.GetOrdinal("Cred")),
                                                        leitor.GetString(leitor.GetOrdinal("NomeDisciplina")),
                                                        leitor.GetBoolean(leitor.GetOrdinal("G2")),
                                                        cal,
                                                        categoria);

                        aux = Turma.GetTurma(leitor.GetGuid(leitor.GetOrdinal("TurmaId")),
                                             leitor.GetInt32(leitor.GetOrdinal("Numero")),
                                             cal,
                                             disc,
                                             leitor.GetString(leitor.GetOrdinal("DataHora")),
                                             professor,
                                             curso);
                        resultado.Add(aux);

                    }
                }


                return resultado;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<Turma> GetTurmas(Calendario cal, Guid professorId, DateTime data, string horario)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TurmasSelectByProfessorDataHorario");
                baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
                baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, professorId);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, horario);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);

                CursosDAO cursoDAO = new CursosDAO();

                List<Turma> resultado = new List<Turma>();

                Turma aux = null;
                Disciplina disc = null;
                Curso curso = null;

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    CategoriaDisciplinaDAO catdiscipDAO = new CategoriaDisciplinaDAO();
                    ProfessorDAO profDAO = new ProfessorDAO();

                    while (leitor.Read())
                    {
                        Professor professor = profDAO.GetProfessor(professorId);
                        curso = cursoDAO.GetCurso(leitor.GetString(leitor.GetOrdinal("Curso")));

                        CategoriaDisciplina categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("categoriaDisciplina")));

                        disc = Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")),
                                                        leitor.GetInt32(leitor.GetOrdinal("Cred")),
                                                        leitor.GetString(leitor.GetOrdinal("NomeDisciplina")),
                                                        leitor.GetBoolean(leitor.GetOrdinal("G2")),
                                                        cal,
                                                        categoria);

                        aux = Turma.GetTurma(leitor.GetGuid(leitor.GetOrdinal("TurmaId")),
                                             leitor.GetInt32(leitor.GetOrdinal("Numero")),
                                             cal,
                                             disc,
                                             leitor.GetString(leitor.GetOrdinal("DataHora")),
                                             professor,
                                             curso);
                        resultado.Add(aux);

                    }
                }


                return resultado;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<Turma> GetTurmas(Calendario cal, Guid professorid)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TurmasSelectByProfessor");
                baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
                baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, professorid);

                CursosDAO cursoDAO = new CursosDAO();
                ProfessorDAO profDAO = new ProfessorDAO();

                List<Turma> resultado = new List<Turma>();

                Turma aux = null;
                Disciplina disc = null;
                Curso curso = null;

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    CategoriaDisciplinaDAO catdiscipDAO = new CategoriaDisciplinaDAO();

                    while (leitor.Read())
                    {
                        curso = cursoDAO.GetCurso(leitor.GetString(leitor.GetOrdinal("Curso")));

                        Entities.CategoriaDisciplina categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("categoriaDisciplina")));

                        disc = Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")),
                                                        leitor.GetInt32(leitor.GetOrdinal("Cred")),
                                                        leitor.GetString(leitor.GetOrdinal("NomeDisciplina")),
                                                        leitor.GetBoolean(leitor.GetOrdinal("G2")),
                                                        cal,
                                                        categoria);

                        Professor professor = profDAO.GetProfessor(professorid);

                        aux = Turma.GetTurma(leitor.GetGuid(leitor.GetOrdinal("TurmaId")),
                                             leitor.GetInt32(leitor.GetOrdinal("Numero")),
                                             cal,
                                             disc,
                                             leitor.GetString(leitor.GetOrdinal("DataHora")),
                                             professor,
                                             curso);
                        resultado.Add(aux);

                    }
                }


                return resultado;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
    }
}

