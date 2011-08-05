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
    public class CalendariosDAO
    {
        private Database baseDados;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Calendarios
        /// </summary>
        public CalendariosDAO()
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
       
        /// <summary>
        /// Deleta um Calendario
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        public void DeletaCalendario(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CalendariosDeleta");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
           catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Insere um Calendario
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="calendario"></param>
        public void InsereCalendario(Calendario calendario)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CalendariosInsere");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calendario.Id);
            baseDados.AddInParameter(cmd, "@Semestre", DbType.Int32, calendario.Semestre);
            baseDados.AddInParameter(cmd, "@Ano", DbType.Int32, calendario.Ano);
            baseDados.AddInParameter(cmd, "@InicioG1", DbType.DateTime, calendario.InicioG1);
            baseDados.AddInParameter(cmd, "@InicioG2", DbType.DateTime, calendario.InicioG2);
            baseDados.AddInParameter(cmd, "@FimG2", DbType.DateTime, calendario.FimG2);

            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Retorna o Calendario relativo ao Id especificado
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Calendario GetCalendario(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CalendariosSelectById");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, id);

            Entities.Calendario aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    DatasDAO datasDao = new DatasDAO();
                    List<Entities.Data> datas = datasDao.GetDatasByCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                    aux = Entities.Calendario.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")),
                                                            leitor.GetInt32(leitor.GetOrdinal("Semestre")),
                                                            leitor.GetInt32(leitor.GetOrdinal("Ano")),
                                                            datas,
                                                            leitor.GetDateTime(leitor.GetOrdinal("InicioG1")),
                                                            leitor.GetDateTime(leitor.GetOrdinal("InicioG2")),
                                                            leitor.GetDateTime(leitor.GetOrdinal("FimG2")));
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }

        /// <summary>
        /// Retorna um calendário de acordo com ano/semestre
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns>Calendario</returns>
        public Calendario GetCalendarioByAnoSemestre(int ano, int semestre)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CalendarioSelectByAnoAndSemestre");
            baseDados.AddInParameter(cmd, "@Ano", DbType.Int32, ano);
            baseDados.AddInParameter(cmd, "@Semestre", DbType.Int32, semestre);

            Entities.Calendario aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    
                    leitor.Read();
                    

                    DatasDAO datasDao = new DatasDAO();
                    List<Entities.Data> datas;
                    try
                    {
                        datas = datasDao.GetDatasByCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                        aux = Calendario.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")),
                                                   semestre, ano, datas,
                                                   leitor.GetDateTime(leitor.GetOrdinal("InicioG1")),
                                                   leitor.GetDateTime(leitor.GetOrdinal("InicioG2")),
                                                   leitor.GetDateTime(leitor.GetOrdinal("FimG2")));
                    }
                    catch (InvalidOperationException)
                    {
                        return null;
                    }
                                                            
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }

        /// <summary>
        /// Retorna todos os Calendarios 
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns>Lista de Calendarios</returns>
        public List<Calendario> GetCalendarios()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CalendariosSelect");
            
            Calendario aux;
            List<Calendario> listaAux = new List<Calendario>();
            
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        DatasDAO datasDao = new DatasDAO();
                        List<Data> datas = datasDao.GetDatasByCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                        aux = Calendario.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")),
                                                                leitor.GetInt32(leitor.GetOrdinal("Semestre")),
                                                                leitor.GetInt32(leitor.GetOrdinal("Ano")),
                                                                datas,
                                                                leitor.GetDateTime(leitor.GetOrdinal("InicioG1")),
                                                                leitor.GetDateTime(leitor.GetOrdinal("InicioG2")),
                                                                leitor.GetDateTime(leitor.GetOrdinal("FimG2")));
                        listaAux.Add(aux);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            
            listaAux.Sort(new ComparadorCalendario());
            return listaAux;
        }

      

        /// <summary>
        /// Registra um professor para um semestre especificado
        /// </summary>
        /// <param name="prof">Professor</param>
        /// <param name="cal">Calendário</param>
        public void RegistraProfessor(Professor prof, Calendario cal)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("ProfessoresCalendarioInsere");
            baseDados.AddInParameter(cmd, "@UserId", DbType.Guid, prof.Id);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        /// <summary>
        /// Cancela o mapeamento de um professor para um semestre
        /// </summary>
        /// <param name="prof">Professor</param>
        /// <param name="cal">Calendario</param>
        public void CancelaProfessor(Professor prof, Calendario cal)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("ProfessoresCalendarioDelete");
            baseDados.AddInParameter(cmd, "@UserId", DbType.Guid, prof.Id);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        //Relacionamentos Disciplinas
        /// <summary>
        /// Registra uma disciplina para um calendario especificado
        /// </summary>
        /// <param name="disc">Disciplina a ser registrada</param>
        /// <param name="cal">Calendario</param>
        public void RegistraDisciplina(Disciplina disc, Calendario cal)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinasInCalendarioInsere");
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, disc.Cod);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        /// <summary>
        /// Cancela o mapeamento da disciplina para o semestre espeficicado
        /// </summary>
        /// <param name="disc">Disciplina</param>
        /// <param name="cal">Calendario</param>
        public void CancelaDisciplina(Disciplina disc, Calendario cal)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinasInCalendarioDelete");
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, disc.Cod);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void UpdateCalendario(Calendario calendario)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CalendariosUpdate");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calendario.Id);
            baseDados.AddInParameter(cmd, "@Semestre", DbType.Int32, calendario.Semestre);
            baseDados.AddInParameter(cmd, "@Ano", DbType.Int32, calendario.Ano);
            baseDados.AddInParameter(cmd, "@InicioG1", DbType.DateTime, calendario.InicioG1);
            baseDados.AddInParameter(cmd, "@InicioG2", DbType.DateTime, calendario.InicioG2);
            baseDados.AddInParameter(cmd, "@FimG2", DbType.DateTime, calendario.FimG2);

            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
    }
}
