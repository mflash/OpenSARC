using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using BusinessData.Entities;
using System.Drawing;
using System.Data.SqlClient;

namespace BusinessData.DataAccess
{
    internal class CategoriaDataDAO
    {
        private Database baseDados;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para CategoriaData
        /// </summary>
        public CategoriaDataDAO()
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
        /// Atualiza uma CategoriaData
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="CategoriaData">CategoriaData</param>
        public void UpdateCategoriaData(CategoriaData categoriaData)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDataUpdate");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, categoriaData.Id);
            baseDados.AddInParameter(cmd, "@Descricao", DbType.String, categoriaData.Descricao);
            baseDados.AddInParameter(cmd, "@Cor", DbType.String, categoriaData.Cor.Name);
            baseDados.AddInParameter(cmd, "@DiaLetivo", DbType.Boolean, categoriaData.DiaLetivo);

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
        /// Deleta um CategoriaData
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        public void DeletaCategoriaData(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDataDelete");
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
        /// Insere um CategoriaData
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="CategoriaData"></param>
        public void InsereCategoriaData(CategoriaData categoriaData)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDataInsert");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, categoriaData.Id);
            baseDados.AddInParameter(cmd, "@Descricao", DbType.String, categoriaData.Descricao);
            baseDados.AddInParameter(cmd, "@Cor", DbType.String, categoriaData.Cor.Name);
            baseDados.AddInParameter(cmd, "@DiaLetivo", DbType.Boolean, categoriaData.DiaLetivo);

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
        /// Retorna Categoria de Data relativa ao Id especificado
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CategoriaData GetCategoriaData(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDataSelectById");
            baseDados.AddInParameter(cmd, "@CategoriasDataId", DbType.Guid, id);

            CategoriaData aux = null;
            Color cor;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();
                    cor = Color.FromName(leitor.GetString(leitor.GetOrdinal("Cor")));
                    aux = CategoriaData.GetCategoriaData(leitor.GetGuid(leitor.GetOrdinal("CategoriasDataId")), 
                                                         leitor.GetString(leitor.GetOrdinal("Descricao")),
                                                         cor,
                                                         leitor.GetBoolean(leitor.GetOrdinal("DiaLetivo")));
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
        /// Retorna todas as ocorrências de CategoriaData
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns>Lista de CategoriaData</returns>
        public List<CategoriaData> GetCategoriaData()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDataSelect");
            CategoriaData aux;
            List<CategoriaData> listaAux = new List<CategoriaData>();
            Color cor;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        cor = Color.FromName(leitor.GetString(leitor.GetOrdinal("Cor")));
                        aux = CategoriaData.GetCategoriaData(leitor.GetGuid(leitor.GetOrdinal("CategoriasDataId")),
                                                             leitor.GetString(leitor.GetOrdinal("Descricao")),
                                                             cor,
                                                             leitor.GetBoolean(leitor.GetOrdinal("DiaLetivo")));
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
