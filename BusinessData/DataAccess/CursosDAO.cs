using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace BusinessData.DataAccess
{
    internal class CursosDAO
    {
        private Database baseDados;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Vinculos
        /// </summary>
        public CursosDAO()
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
        public void UpdateCurso(Entities.Curso curso)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CursosUpdate");
            baseDados.AddInParameter(cmd, "@Codigo", DbType.String, curso.Codigo);
            baseDados.AddInParameter(cmd, "@Nome", DbType.String, curso.Nome);
            baseDados.AddInParameter(cmd, "@FaculdadeId", DbType.Guid, curso.Vinculo.Id);
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
        public void DeletaCurso(string cod)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CursosDelete");
            baseDados.AddInParameter(cmd, "@Codigo", DbType.String, cod);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException( ErroMessages.GetErrorMessage(ex.Number),ex); 
            }
            
        }
        /// <summary>
        /// Insere um Vinculo
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="vinculo"></param>
        public void InsereCurso(Entities.Curso curso)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CursosInsere");
            baseDados.AddInParameter(cmd, "@Codigo", DbType.String, curso.Codigo);
            baseDados.AddInParameter(cmd, "@FaculdadeId", DbType.Guid, curso.Vinculo.Id);
            baseDados.AddInParameter(cmd, "@Nome", DbType.String, curso.Nome);
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
        public Entities.Curso GetCurso(string codigo)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CursosSelectByCodigo");
            baseDados.AddInParameter(cmd, "@Codigo", DbType.String, codigo);

            Entities.Curso aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    FaculdadesDAO faculs = new FaculdadesDAO();
                    Entities.Faculdade facul = faculs.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("FaculdadeId")));

                    aux = Entities.Curso.GetCurso(leitor.GetString(leitor.GetOrdinal("Codigo")),
                                                    leitor.GetString(leitor.GetOrdinal("Nome")), facul);
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
        /// <returns>Lista de Vinculos</returns>
        public List<Entities.Curso> GetCursos()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CursosSelect");
            Entities.Curso aux;
            List<Entities.Curso> listaAux = new List<BusinessData.Entities.Curso>();
            Entities.Faculdade faculdade = null;
            try
            {
                FaculdadesDAO faculdades = new FaculdadesDAO();
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                         faculdade = faculdades.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("FaculdadeId")));
        
                         aux = Entities.Curso.GetCurso(leitor.GetString(leitor.GetOrdinal("Codigo")),
                                                           leitor.GetString(leitor.GetOrdinal("Nome")),
                                                           faculdade);
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
