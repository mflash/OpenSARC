using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace BusinessData.DataAccess
{
    internal class DatasDAO
    {
        private Database baseDados;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Datas
        /// </summary>
        public DatasDAO()
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
        /// Deleta uma Data
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        public void DeletaData(Guid calendarioId, Entities.Data data)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DatasSemestreRemove");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calendarioId);
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data.Date);
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
        /// Insere uma Data
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="data"></param>
        public void InsereData(Entities.Data data, Guid calendarioId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DatasSemestreInsere");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calendarioId);
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data.Date);
            baseDados.AddInParameter(cmd, "@CategoriaData", DbType.Guid, data.Categoria.Id);
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
        /// Retorna todos os Datas 
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns>Lista de Datas</returns>
        public List<Entities.Data> GetDatasByCalendario(Guid calendarioId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("DatasSemestreSelectByCalendario");
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calendarioId);

            Entities.Data aux;
            List<Entities.Data> listaAux = new List<BusinessData.Entities.Data>();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {

                    CategoriaDataDAO categoriaDataDAO = new CategoriaDataDAO();
                    Entities.CategoriaData categoriaData = null;
                    try
                    {
                        while (leitor.Read())
                        {
                            categoriaData = categoriaDataDAO.GetCategoriaData(leitor.GetGuid(leitor.GetOrdinal("CategoriasDataId")));

                            aux = Entities.Data.GetData(leitor.GetDateTime(leitor.GetOrdinal("Data")),
                                                        categoriaData);
                            listaAux.Add(aux);
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        return listaAux;
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
