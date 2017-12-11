using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Web;

namespace BusinessData.DataAccess
{
    public class ConfigDAO
    {

          private Database baseDados;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Vinculos
        /// </summary>
        public ConfigDAO()
        {
            try
            {
                baseDados = DatabaseFactory.CreateDatabase("SARCFACINcs");
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao conectar com o servidor.", ex);
            }
        }
            
        public AppState GetAppState(Calendario calId)
        {
                DbCommand cmd = baseDados.GetStoredProcCommand("ConfigSelectEstadoAtual");
                baseDados.AddInParameter(cmd, "@CalId", DbType.Guid, calId.Id);
                try
                {
                    int i = Convert.ToInt32(baseDados.ExecuteScalar(cmd));
                    return (AppState)i;
                }
                catch (Exception ex)
                {
                    throw new DataAccessException("Erro ao ler dados", ex);
                }
        }

        public void SetAppState(AppState novoEstado, Calendario calId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("ConfigSetEstadoAtual");
            baseDados.AddInParameter(cmd, "@CalId", DbType.Guid, calId.Id);
            baseDados.AddInParameter(cmd, "@NovoEstado", DbType.Int32, Convert.ToInt32(novoEstado));
            

            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados", ex);
            }
        }

        public bool IsRecursosDistribuidos(Calendario calId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("ConfigSelectRecursosDistribuidos");
            baseDados.AddInParameter(cmd, "@CalId", DbType.Guid, calId.Id);
            try
            {
                bool i = Convert.ToBoolean(baseDados.ExecuteScalar(cmd));
                return i;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados", ex);
            }
        }

        public bool IsAulasDistribuidas(Calendario calId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("ConfigSelectAulasDistribuidas");
            baseDados.AddInParameter(cmd, "@CalId", DbType.Guid, calId.Id);
            try
            {
                bool i = Convert.ToBoolean(baseDados.ExecuteScalar(cmd));
                return i;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados", ex);
            }
        }

        public void setAulasDistribuidas(Calendario calId, bool dist)
        {

            DbCommand cmd = baseDados.GetStoredProcCommand("ConfigSetAulasDistribuidas");
            baseDados.AddInParameter(cmd, "@CalId", DbType.Guid, calId.Id);
            baseDados.AddInParameter(cmd, "@AulasDistribuidas", DbType.Boolean, dist);
            try
            {
                baseDados.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados", ex);
            }
        }

        public void setRecursosDistribuidos(Calendario calId, bool dist)
        {

            DbCommand cmd = baseDados.GetStoredProcCommand("ConfigSetRecursosDistribuidos");
            baseDados.AddInParameter(cmd, "@CalId", DbType.Guid, calId.Id);
            baseDados.AddInParameter(cmd, "@RecursosDistribuidos", DbType.Boolean, dist);
            try
            {
                baseDados.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados", ex);
            }
        }

        public void createConfig(Calendario calId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("ConfigCreateConfig");
            baseDados.AddInParameter(cmd, "@CalId", DbType.Guid, calId.Id);
            baseDados.AddInParameter(cmd, "@Estado", DbType.Int32, 0);
            baseDados.AddInParameter(cmd, "@AulasDistribuidas", DbType.Boolean, false);
            baseDados.AddInParameter(cmd, "@RecursosDistribuidos", DbType.Boolean, false);

            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao ler dados", ex);
            }
        
        }

    }
}
