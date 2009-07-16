using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using BusinessData.Entities;

namespace BusinessData.DataAccess
{
    internal class SecretarioDAO
    {
        private Database _baseDados;

        public SecretarioDAO()
        {
            try
            {
                _baseDados = DatabaseFactory.CreateDatabase("SARCFACINcs");
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void DeleteSecretario(Guid id)
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("SecretariosDelete");
            _baseDados.AddInParameter(cmd, "@SecretarioId", DbType.Guid, id);
            try
            {
                _baseDados.ExecuteNonQuery(cmd); 
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void InsertSecretario(Secretario secretario)
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("SecretariosInsere");
            _baseDados.AddInParameter(cmd, "@UserId", DbType.Guid, secretario.Id);
            _baseDados.AddInParameter(cmd, "@Nome", DbType.String, secretario.Nome);
            try
            {
                _baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public Secretario GetSecretario(Guid id)
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("SecretariosSelectById");
            _baseDados.AddInParameter(cmd, "@UserId", DbType.Guid, id);

            Secretario aux = null;
            try
            {
                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    aux = Secretario.GetSecretario(leitor.GetGuid(leitor.GetOrdinal("Id")),
                                                   leitor.GetString(leitor.GetOrdinal("Matricula")),
                                                   leitor.GetString(leitor.GetOrdinal("Nome")),
                                                   leitor.GetString(leitor.GetOrdinal("Email")));

                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            catch (Exception)
            {
                return null;
            }
            return aux;
        }

        public List<Secretario> GetSecretarios()
        {
            DbCommand cmd = _baseDados.GetStoredProcCommand("SecretariosSelect");
            Secretario aux;
            List<Secretario> listaAux = new List<Secretario>();
            try
            {
                using (IDataReader leitor = _baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        aux = Secretario.GetSecretario(leitor.GetGuid(leitor.GetOrdinal("Id")),
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
