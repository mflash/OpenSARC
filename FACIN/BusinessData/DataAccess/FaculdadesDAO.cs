using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace BusinessData.DataAccess
{
    internal class FaculdadesDAO
    {
        private Database baseDados;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Vinculos
        /// </summary>
        public FaculdadesDAO()
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
        /// Atualiza um Vinculo
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="vinculo">Vinculo</param>
        public void UpdateFaculdade(Entities.Faculdade vinculo)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("FaculdadesUpdate");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, vinculo.Id);
            baseDados.AddInParameter(cmd, "@Nome", DbType.String, vinculo.Nome);
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
        /// Deleta um Vinculo
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        public void DeletaFaculdade(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("FaculdadesDelete");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, id);
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
        /// Insere um Vinculo
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="vinculo"></param>
        public void InsereFaculdade(Entities.Faculdade vinculo)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("FaculdadesInsere");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, vinculo.Id);
            baseDados.AddInParameter(cmd, "@Nome", DbType.String, vinculo.Nome);
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
        /// Retorna o Vinculo relativo ao Id especificado
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Faculdade GetFaculdade(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("FaculdadesSelectById");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, id);

            Entities.Faculdade aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    aux = Entities.Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("FaculdadeId")), 
                                                      leitor.GetString(leitor.GetOrdinal("Nome")));
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
        /// <summary>
        /// Retorna todos os Vinculos 
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns>Lista de Vinculos</returns>
        public List<Entities.Faculdade> GetFaculdades()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("FaculdadesSelect");
            Entities.Faculdade aux;
            List<Entities.Faculdade> listaAux = new List<BusinessData.Entities.Faculdade>();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        aux = Entities.Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("FaculdadeId")),
                                                           leitor.GetString(leitor.GetOrdinal("Nome")));
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
