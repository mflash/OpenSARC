using BusinessData.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace BusinessData.DataAccess
{
    internal class DisciplinasDAO
    {
        private Database baseDados;
        private Dictionary<Guid, CategoriaDisciplina> dicCategDisciplinas;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Disciplinas
        /// </summary>
        public DisciplinasDAO()
        {
            try
            {
                baseDados = DatabaseFactory.CreateDatabase("SARCFACINcs");
                CategoriaDisciplinaDAO catDiscipDAO = new CategoriaDisciplinaDAO();
                dicCategDisciplinas = new Dictionary<Guid, CategoriaDisciplina>();
                foreach (var item in catDiscipDAO.GetCategoriaDisciplinas())
                    dicCategDisciplinas.Add(item.Id, item);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Atualiza uma Disciplina
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="disciplina">Disciplina</param>
        public void UpdateDisciplina(Disciplina disciplina)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaUpdate");
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, disciplina.Cod);
            baseDados.AddInParameter(cmd, "@Cred", DbType.Int32, disciplina.Cred);
            baseDados.AddInParameter(cmd, "@Nome", DbType.String, disciplina.Nome);
            baseDados.AddInParameter(cmd, "@G2", DbType.Boolean, disciplina.G2);
            baseDados.AddInParameter(cmd, "@Categoria", DbType.Guid, disciplina.Categoria.Id);
            
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
        /// Deleta uma Disciplina
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        public void DeletaDisciplina(string cod, Guid calId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaDelete");
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, cod);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calId);
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
        /// Insere uma Disciplina
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="disciplina"></param>
        public void InsereDisciplina(Disciplina disciplina)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaInsere");
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, disciplina.Cod);
            baseDados.AddInParameter(cmd, "@Cred", DbType.Int32, disciplina.Cred);
            baseDados.AddInParameter(cmd, "@Nome", DbType.String, disciplina.Nome);
            baseDados.AddInParameter(cmd, "@G2", DbType.Boolean, disciplina.G2);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, disciplina.Calendario.Id);
            baseDados.AddInParameter(cmd, "@Categoria", DbType.Guid, disciplina.Categoria.Id);

            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number)+":"+disciplina.Cod, ex);
            }
        }

        public void InsereDisciplinaInCalendario(Disciplina disciplina, Guid calId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinasInCalendarioInsere   ");
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, disciplina.Cod);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calId);
            

            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number)+":"+disciplina.Cod, ex);
            }
        }

        public Disciplina GetDisciplina(string cod, Calendario cal, List<BusinessData.Entities.CategoriaDisciplina> categoriasDeDisciplina)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaSelectByCod");
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, cod);

            Entities.Disciplina aux = null;
            Entities.CategoriaDisciplina categoria = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    Guid categId = leitor.GetGuid(leitor.GetOrdinal("Categoria"));
                    foreach(CategoriaDisciplina categDisc in categoriasDeDisciplina)
                    {
                        if(categDisc.Id.Equals(categId))
                        {
                            categoria = categDisc;
                        }
                    }
                    if(categoria == null)
                    {
                        throw new Exception("Categoria de disciplina não encontrada");
                    }

                    aux = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")),
                                                            leitor.GetInt32(leitor.GetOrdinal("Cred")),
                                                            leitor.GetString(leitor.GetOrdinal("Nome")),
                                                            leitor.GetBoolean(leitor.GetOrdinal("G2")),
                                                            cal,
                                                            categoria
                                                            );
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;

        }
        /// <summary>
        /// Retorna a Disciplina relativo ao Id especificado
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Disciplina GetDisciplina(string cod, Calendario cal)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaSelectByCod");
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, cod);
            //CategoriaDisciplinaDAO catdiscipDAO = new CategoriaDisciplinaDAO();

            Entities.Disciplina aux = null;
            Entities.CategoriaDisciplina categoria = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    //categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("Categoria")));
                    categoria = dicCategDisciplinas[leitor.GetGuid(leitor.GetOrdinal("Categoria"))];

                    aux = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")),
                                                            leitor.GetInt32(leitor.GetOrdinal("Cred")),
                                                            leitor.GetString(leitor.GetOrdinal("Nome")),
                                                            leitor.GetBoolean(leitor.GetOrdinal("G2")),
                                                            cal,
                                                            categoria
                                                            );
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }
        /// <summary>
        /// Retorna a Disciplina relativo ao Id especificado.
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="cod"></param>
        /// <param name="calendarioId">
        /// Id do calendário atual, passado para ser possível retornar um objeto completo
        /// </param>
        /// <returns></returns>
        public Disciplina GetDisciplina(string cod, Guid calendarioId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaSelectByCod");
            baseDados.AddInParameter(cmd, "@DisciplinaCod", DbType.String, cod);

            Entities.Disciplina aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    CalendariosDAO calendariosDAO = new CalendariosDAO();
                    //CategoriaDisciplinaDAO catdiscipDAO = new CategoriaDisciplinaDAO();

                    Entities.Calendario calendario = calendariosDAO.GetCalendario(calendarioId);
                    Entities.CategoriaDisciplina categoria = dicCategDisciplinas[leitor.GetGuid(leitor.GetOrdinal("Categoria"))];
                    //Entities.CategoriaDisciplina categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("Categoria")));

                    aux = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")),
                                                            leitor.GetInt32(leitor.GetOrdinal("Cred")),
                                                            leitor.GetString(leitor.GetOrdinal("Nome")),
                                                            leitor.GetBoolean(leitor.GetOrdinal("G2")),
                                                            calendario,
                                                            categoria
                                                            );
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }

        /// <summary>
        /// Retorna todas as Disciplinas ligadas à um calendário
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns>Lista de Disciplinas</returns>
        public List<Disciplina> GetDisciplinas(Calendario calendario)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaSelect");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calendario.Id);
            //CategoriaDisciplinaDAO catdiscipDAO = new CategoriaDisciplinaDAO();

            Entities.Disciplina aux;
            List<Entities.Disciplina> listaAux = new List<Entities.Disciplina>();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    CalendariosDAO calendariosDAO = new CalendariosDAO();

                    while (leitor.Read())
                    {
                        //Entities.CategoriaDisciplina categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("Categoria")));
                        Entities.CategoriaDisciplina categoria = dicCategDisciplinas[leitor.GetGuid(leitor.GetOrdinal("Categoria"))];

                        aux = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")),
                                                                leitor.GetInt32(leitor.GetOrdinal("Cred")),
                                                                leitor.GetString(leitor.GetOrdinal("Nome")),
                                                                leitor.GetBoolean(leitor.GetOrdinal("G2")),
                                                                calendario,
                                                                categoria
                                                                );
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
 
        /// <summary>
        /// Retorna todas as Disciplinas ligadas à um calendário
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns>Lista de Disciplinas</returns>
        public List<Disciplina> GetDisciplinas(Guid calendarioId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaSelect");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calendarioId);

            Disciplina aux;
            List<Disciplina> listaAux = new List<Disciplina>();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    CalendariosDAO calendariosDAO = new CalendariosDAO();
                    //CategoriaDisciplinaDAO catdiscipDAO = new CategoriaDisciplinaDAO();

                    Calendario calendario = null;
                    CategoriaDisciplina categoria = null;

                    calendario = calendariosDAO.GetCalendario(calendarioId);

                    while (leitor.Read())
                    {
                        categoria = dicCategDisciplinas[leitor.GetGuid(leitor.GetOrdinal("Categoria"))];
                        //categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("Categoria")));

                        aux = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")),
                                                                leitor.GetInt32(leitor.GetOrdinal("Cred")),
                                                                leitor.GetString(leitor.GetOrdinal("Nome")),
                                                                leitor.GetBoolean(leitor.GetOrdinal("G2")),
                                                                calendario,
                                                                categoria
                                                                );
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
        
        public List<Disciplina> GetDisciplinas()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaSelectAll");

            Disciplina aux;

            List<Disciplina> listaAux = new List<Disciplina>();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    CalendariosDAO calendariosDAO = new CalendariosDAO();
                    //CategoriaDisciplinaDAO catdiscipDAO = new CategoriaDisciplinaDAO();

                    Calendario calendario = null;
                    CategoriaDisciplina categoria = null;

                    while (leitor.Read())
                    {
                        calendario = null;
                        //categoria = catdiscipDAO.GetCategoriaDisciplina(leitor.GetGuid(leitor.GetOrdinal("Categoria")));
                        categoria = dicCategDisciplinas[leitor.GetGuid(leitor.GetOrdinal("Categoria"))];

                        aux = Entities.Disciplina.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")),
                                                                leitor.GetInt32(leitor.GetOrdinal("Cred")),
                                                                leitor.GetString(leitor.GetOrdinal("Nome")),
                                                                leitor.GetBoolean(leitor.GetOrdinal("G2")),
                                                                calendario,
                                                                categoria
                                                                );
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

        public List<Disciplina> GetDisciplinaInCalendario(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DisciplinaInCalendarioSelect");

            Disciplina aux;
            List<Disciplina> listaAux = new List<Disciplina>();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    CalendariosDAO calendariosDAO = new CalendariosDAO();
                    Calendario calendario = calendariosDAO.GetCalendario(id);
                   
                    while (leitor.Read())
                    {
                        //calendario = calendariosDAO.GetCalendario(id);
                        aux = this.GetDisciplina(leitor.GetString(leitor.GetOrdinal("DisciplinaCod")), calendario);

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

        public int GetMapeamentoDisciplinasAtas(string codigo)
        {
            string ano = DateTime.Now.Year.ToString();
            int mapeamento = -1;

            string cs = ConfigurationManager.ConnectionStrings["OracleCS"].ConnectionString;
            using (Oracle.ManagedDataAccess.Client.OracleConnection oconn = new Oracle.ManagedDataAccess.Client.OracleConnection(cs))
            {
                try
                {
                    oconn.Open();
                    Oracle.ManagedDataAccess.Client.OracleCommand c = oconn.CreateCommand();
                    c.CommandText = "SELECT DISTINCT CDDISCIPL,CD_DISCIPLINA FROM GRANDEPORTE.EF_DISCIPLINA_TURMA edt where edt.cdano = '" + ano + "' and edt.cddiscipl = '"+codigo+"'";

                    Oracle.ManagedDataAccess.Client.OracleDataReader or = c.ExecuteReader();
                    while (or.Read())
                    {
                        //Response.Write(or.FieldCount + ": "+or.GetString(0) + " " + or.GetString(1) +"<br>");
                        mapeamento = or.GetInt32(1);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Erro ao conectar ao banco de dados: " + ex.Message);
                }
            }
            //System.Diagnostics.Debug.WriteLine("Codcreds para mapeamento: " + mapeamentoDisciplinas.Count);
            return mapeamento;
        }

    }
}
