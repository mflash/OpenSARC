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
    internal class CategoriaAtividadeDAO
    {
        private Database baseDados;

        // <summary>
        /// Atualiza uma Categoria Atividade
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="vinculo">Vinculo</param>
        public void UpdateCategoriaAtividade(CategoriaAtividade categoriaAtividade)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasAtividadeUpdate");
                baseDados.AddInParameter(cmd, "@Id", DbType.Guid, categoriaAtividade.Id);
                baseDados.AddInParameter(cmd, "@Descricao", DbType.String, categoriaAtividade.Descricao);
                baseDados.AddInParameter(cmd, "@Cor", DbType.String, categoriaAtividade.Cor.Name);

                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Categoria Atividade
        /// </summary>
        public CategoriaAtividadeDAO()
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
        /// Deleta uma categoria atividade
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        public void DeletaCategoriaAtividade(Guid cod)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasAtividadeDelete");
            baseDados.AddInParameter(cmd, "@CategoriasAtividadeId", DbType.Guid, cod);
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
        /// Insere uma Categoria Atividade
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="disciplina"></param>
        public void InsereCategoriaAtividade(CategoriaAtividade categoriaAtividade)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasAtividadeInsert");
            baseDados.AddInParameter(cmd, "@CategoriaAtividadeId", DbType.Guid, categoriaAtividade.Id);
            baseDados.AddInParameter(cmd, "@Descricao", DbType.String, categoriaAtividade.Descricao);
            baseDados.AddInParameter(cmd, "@Cor", DbType.String, categoriaAtividade.Cor.Name);
            
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
        /// Retorna a Categoria Ativade relativa ao Id especificado
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CategoriaAtividade GetCategoriaAtividadeById(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasAtividadeSelectById");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, id);

            CategoriaAtividade aux = null;
            Color cor;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    cor = Color.FromName(leitor.GetString(leitor.GetOrdinal("Cor")));
                    aux = CategoriaAtividade.GetCategoriaAtividade(leitor.GetGuid(leitor.GetOrdinal("CategoriaAtividadeId")),
                                                          leitor.GetString(leitor.GetOrdinal("Descricao")), cor);
                    
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
        /// Retorna todos as Categorias Atividade 
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns>Lista de Vinculos</returns>
        public List<CategoriaAtividade> GetCategoriaAtividade()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasAtividadeSelect");
            CategoriaAtividade aux; 
            List<CategoriaAtividade> listaAux = new List<CategoriaAtividade>();
            Color cor;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    
                    while (leitor.Read())
                    {
                        cor = Color.FromName(leitor.GetString(leitor.GetOrdinal("Cor")));
                        aux = CategoriaAtividade.GetCategoriaAtividade(leitor.GetGuid(leitor.GetOrdinal("CategoriaAtividadeId")),
                                                           leitor.GetString(leitor.GetOrdinal("Descricao")),cor);
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
