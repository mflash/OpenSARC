using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using BusinessData.Entities;

namespace BusinessData.DataAccess
{
    internal class ProfessorDAO   
    {
        private Database _baseDados;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Professores
        /// </summary>
        public ProfessorDAO()
        {
            try
            {
                _baseDados = DatabaseFactory.CreateDatabase("SARCcs");
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        /// <summary>
        /// Deleta um Professor
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        public void DeletePofessor(Guid id)
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("ProfessoresDelete");
            _baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, id);
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
        /// Insere um Professor
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="professor"></param>
        public void InsertProfessor(Professor professor)
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("ProfessoresInsere");
            _baseDados.AddInParameter(cmd, "@UserId", DbType.Guid, professor.Id);
            _baseDados.AddInParameter(cmd, "@Nome", DbType.String, professor.Nome);
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
        /// Retorna o Professor relativo ao Id especificado
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Professor GetProfessor(Guid id)
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("ProfessoresSelectById");
            _baseDados.AddInParameter(cmd, "@UserId", DbType.Guid, id);

            Entities.Professor aux = null;
            try
            {
                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    aux = Entities.Professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("Id")),
                                                          leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                          leitor.GetString(leitor.GetOrdinal("Nome")),
                                                          leitor.GetString(leitor.GetOrdinal("Email")));

                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            catch (Exception exc)
            {
                return null;
            }
            return aux;
        }

        /// <summary>
        /// Retorna o Professor relativo à Matrícula especificada
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns></returns>
        public Professor GetProfessorByMatricula(string matricula)
        {
             DbCommand cmd = _baseDados.GetStoredProcCommand("ProfessoresSelectByMatricula");
            _baseDados.AddInParameter(cmd, "@Matricula", DbType.String, matricula);

            Entities.Professor aux = null;
            try
            {
                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                        leitor.Read();

                        aux = Entities.Professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("Id")),
                                                              leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                              leitor.GetString(leitor.GetOrdinal("Nome")),
                                                              leitor.GetString(leitor.GetOrdinal("Email")));
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }

        /// <summary>
        /// Retorna todos os Vinculos 
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns></returns>
        public List<Professor> GetProfessores()
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("ProfessoresSelect");
            Professor aux;
            List<Professor> listaAux = new List<Professor>();
            try
            {
                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        aux = Professor.GetProfessor(leitor.GetGuid(leitor.GetOrdinal("Id")),
                                                          leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                          leitor.GetString(leitor.GetOrdinal("Nome")),
                                                          leitor.GetString(leitor.GetOrdinal("Email")));
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

        
    }
}
