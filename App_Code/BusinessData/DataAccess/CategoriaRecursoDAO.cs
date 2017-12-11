using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;


namespace BusinessData.DataAccess
{
    internal class CategoriaRecursoDAO
    {
        private Database baseDados;

        public CategoriaRecursoDAO()
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

        public void UpdateCategoriaRecurso(Entities.CategoriaRecurso categoriaRecurso)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasRecursoUpdate");
            baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, categoriaRecurso.Id);
            baseDados.AddInParameter(cmd, "@Descricao", DbType.String, categoriaRecurso.Descricao);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void DeletaCategoriaRecurso(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasRecursoDelete");
            baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void InsereCategoriaRecurso(Entities.CategoriaRecurso categoriaRecurso)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasRecursoInsere");
            baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, categoriaRecurso.Id);
            baseDados.AddInParameter(cmd, "@Descricao", DbType.String, categoriaRecurso.Descricao);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public Entities.CategoriaRecurso GetCategoriaRecurso(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasRecursoSelectById");
            baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, id);

            Entities.CategoriaRecurso aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    aux = Entities.CategoriaRecurso.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaRecursoId")),
                                                      leitor.GetString(leitor.GetOrdinal("Descricao")));
                    //Debug.WriteLine("Entities.CategoriaRecurso.GetCategoriaRecurso: "+aux);
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

        public List<Entities.CategoriaRecurso> GetCategoriaRecursoSortedByUse()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasRecursoSelectSortedByUse");
            Entities.CategoriaRecurso aux;
            List<Entities.CategoriaRecurso> listaAux = new List<BusinessData.Entities.CategoriaRecurso>();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        aux = Entities.CategoriaRecurso.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaRecursoId")),
                                                                leitor.GetString(leitor.GetOrdinal("Descricao")));
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

        public List<Entities.CategoriaRecurso> GetCategoriaRecurso()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasRecursoSelect");
            Entities.CategoriaRecurso aux;
            List<Entities.CategoriaRecurso> listaAux = new List<BusinessData.Entities.CategoriaRecurso>();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        aux = Entities.CategoriaRecurso.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaRecursoId")),
                                                                leitor.GetString(leitor.GetOrdinal("Descricao")));
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
