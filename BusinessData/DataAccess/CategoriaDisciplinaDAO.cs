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
    internal class CategoriaDisciplinaDAO
    {
        private Database baseDados;

        public CategoriaDisciplinaDAO()
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
        /// Faz a atualização de uma CategoriaDisciplina
        /// </summary>
        /// <param name="categoriaDisciplina"></param>
        public void UpdateCategoriaDisciplina(Entities.CategoriaDisciplina categoriaDisciplina)
        {
            //Primeiro atualiza o banco CategoriaDisciplina, que contém apenas ID e DESCRIÇÃO
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDisciplinaUpdate");
            baseDados.AddInParameter(cmd, "@CategoriaDisciplinaId", DbType.Guid, categoriaDisciplina.Id);
            baseDados.AddInParameter(cmd, "@Descricao", DbType.String, categoriaDisciplina.Descricao);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }

            //Instancia uma CategoriaDisciplina com os dados atuais, que virão a ser modificados, para verificar
            //se não foram adicionadas "CategoriaRecurso"s ao sistema que não existiam antes
            Entities.CategoriaDisciplina catOLD = this.GetCategoriaDisciplina(categoriaDisciplina.Id);

            bool existia = false;


            //Para cada CategoriaRecurso atual, verifica se ela já existia no antigo
            foreach (KeyValuePair<CategoriaRecurso, double> kvp in categoriaDisciplina.Prioridades)
            {
                foreach (KeyValuePair<CategoriaRecurso, double> kvpOLD in catOLD.Prioridades)
                {
                    existia = false;
                    //Se existia já no antigo, apenas seta um flag para fazer o update
                    //(setar flag aqui, pois está fazendo verificação um a um)
                    if (kvp.Key.Equals(kvpOLD.Key))
                    {
                        existia = true;
                        break;
                    }
                }
                //Se jah existia, Update
                if (existia)
                {
                    cmd = baseDados.GetStoredProcCommand("CategoriasDisciplinaInCatRecursoPrioridadeUpdate");
                    baseDados.AddInParameter(cmd, "@CategoriaDisciplinaId", DbType.Guid, categoriaDisciplina.Id);
                    baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, kvp.Key.Id);
                    baseDados.AddInParameter(cmd, "@Prioridade", DbType.Double, kvp.Value);
                    try
                    {
                        baseDados.ExecuteNonQuery(cmd);
                    }
                    catch (SqlException ex)
                    {
                        throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
                    }
                }
                //Caso contrário, Insert
                else
                {
                    cmd = baseDados.GetStoredProcCommand("CategoriasDisciplinaInCatRecursoPrioridadeInsere");
                    baseDados.AddInParameter(cmd, "@CategoriaDisciplinaId", DbType.Guid, categoriaDisciplina.Id);
                    baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, kvp.Key.Id);
                    baseDados.AddInParameter(cmd, "@Prioridade", DbType.Double, kvp.Value);
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

        public void DeletaCategoriaDisciplina(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDisciplinaDelete");
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

        public void InsereCategoriaDisciplina(Entities.CategoriaDisciplina categoriaDisciplina)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDisciplinaInsere");
            baseDados.AddInParameter(cmd, "@CategoriaDisciplinaId", DbType.Guid, categoriaDisciplina.Id);
            baseDados.AddInParameter(cmd, "@Descricao", DbType.String, categoriaDisciplina.Descricao);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }

            foreach (KeyValuePair<CategoriaRecurso, double> kvp in categoriaDisciplina.Prioridades)
            {
                cmd = baseDados.GetStoredProcCommand("CategoriasDisciplinaInCatRecursoPrioridadeInsere");
                baseDados.AddInParameter(cmd, "@CategoriaDisciplinaId", DbType.Guid, categoriaDisciplina.Id);
                baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, kvp.Key.Id);
                baseDados.AddInParameter(cmd, "@Prioridade", DbType.Double, kvp.Value);
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

        public Entities.CategoriaDisciplina GetCategoriaDisciplina(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDisciplinaSelectById");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, id);

            Entities.CategoriaDisciplina aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();
                    CategoriaRecursoDAO crDao = new CategoriaRecursoDAO();

                    Guid ideh = leitor.GetGuid(leitor.GetOrdinal("DisciplinaId"));
                    string nome = leitor.GetString(leitor.GetOrdinal("Nome"));
                    
                    Dictionary<CategoriaRecurso, double> prioridades = new Dictionary<CategoriaRecurso, double>();
                    DbCommand cmd2 = baseDados.GetStoredProcCommand("CategoriasDisciplinaSelectPrioridadesById");
                    baseDados.AddInParameter(cmd2, "@Id", DbType.Guid, ideh);
                    try
                    {
                        using (IDataReader leitor2 = baseDados.ExecuteReader(cmd2))
                        {
                            while (leitor2.Read())
                            {
                                CategoriaRecurso cat = crDao.GetCategoriaRecurso(leitor2.GetGuid(leitor2.GetOrdinal("CatRecursoId")));
                                double value = leitor2.GetDouble(leitor2.GetOrdinal("prioridade"));
                                prioridades.Add(cat, value);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
                    }


                    aux = Entities.CategoriaDisciplina.GetCategoriaDisciplina(id, nome, prioridades);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados.", ex);
            }

            return aux;
        }

        public List<Entities.CategoriaDisciplina> GetCategoriaDisciplinas()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("CategoriasDisciplinaSelect");
            Entities.CategoriaDisciplina aux;
            List<Entities.CategoriaDisciplina> listaAux = new List<BusinessData.Entities.CategoriaDisciplina>();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    CategoriaRecursoDAO crDao = new CategoriaRecursoDAO();
                    while (leitor.Read())
                    {
                        Guid id = leitor.GetGuid(leitor.GetOrdinal("DisciplinaId"));
                        string nome = leitor.GetString(leitor.GetOrdinal("Nome"));
                        
                        Dictionary<CategoriaRecurso, double> prioridades = new Dictionary<CategoriaRecurso, double>();
                        DbCommand cmd2 = baseDados.GetStoredProcCommand("CategoriasDisciplinaSelectPrioridadesById");
                        baseDados.AddInParameter(cmd2, "@Id", DbType.Guid, id);
                        try
                        {
                            using (IDataReader leitor2 = baseDados.ExecuteReader(cmd2))
                            {
                                while (leitor2.Read())
                                {
                                    CategoriaRecurso cat = crDao.GetCategoriaRecurso(leitor2.GetGuid(leitor2.GetOrdinal("CatRecursoId")));
                                    double value = leitor2.GetDouble(leitor2.GetOrdinal("prioridade"));
                                    prioridades.Add(cat, value);
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
                        }


                        aux = Entities.CategoriaDisciplina.GetCategoriaDisciplina(id, nome, prioridades);
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
