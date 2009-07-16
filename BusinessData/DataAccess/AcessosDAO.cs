using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace BusinessData.DataAccess
{
    public class AcessosDAO
    {
        private Database baseDados;

        public AcessosDAO()
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

        public void Add(Acesso a)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("AcessosInsere");
            baseDados.AddInParameter(cmd, "@Id", DbType.Guid, a.Id);
            baseDados.AddInParameter(cmd, "@Horario", DbType.DateTime,a.Horario);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public int GetNumeroDeAcessosPorData(FaixaDeAcesso faixa, DateTime data)
        {
            string dataI = data.ToString("yyyy-MM-dd");
            DbCommand cmd = baseDados.GetStoredProcCommand("GetNumeroDeAcessosPorData");

            string horarioInicial = GetDBString(faixa.HorarioInicial);
            string horarioFinal = GetDBString(faixa.HorarioFinal);

            baseDados.AddInParameter(cmd, "@HorarioInicial", DbType.String, horarioInicial);
            baseDados.AddInParameter(cmd, "@HorarioFinal", DbType.String, horarioFinal);
            baseDados.AddInParameter(cmd, "@Data", DbType.String, dataI);
            
            try
            {
                return (int)baseDados.ExecuteScalar(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }            
        }

        public int GetNumeroDeAcessosPorPeriodo(FaixaDeAcesso faixa, DateTime dataInicial, DateTime dataFinal)
        {
            string dataI = dataInicial.ToString("yyyy-MM-dd");
            string dataF = dataFinal.ToString("yyyy-MM-dd");

            DbCommand cmd = baseDados.GetStoredProcCommand("GetNumeroDeAcessosPorPeriodo");

            string horarioInicial = GetDBString(faixa.HorarioInicial);
            string horarioFinal = GetDBString(faixa.HorarioFinal);

            baseDados.AddInParameter(cmd, "@HorarioInicial", DbType.String, horarioInicial);
            baseDados.AddInParameter(cmd, "@HorarioFinal", DbType.String, horarioFinal);
            baseDados.AddInParameter(cmd, "@DataInicial", DbType.String, dataI);
            baseDados.AddInParameter(cmd, "@DataFinal", DbType.String, dataF);

            try
            {
                return (int)baseDados.ExecuteScalar(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }            
        }

        public int GetNumeroDeAcessosPorMes(FaixaDeAcesso faixa,int mes, int ano)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("GetNumeroDeAcessosPorMes");

            string horarioInicial = GetDBString(faixa.HorarioInicial);
            string horarioFinal = GetDBString(faixa.HorarioFinal);

            baseDados.AddInParameter(cmd, "@HorarioInicial", DbType.String, horarioInicial);
            baseDados.AddInParameter(cmd, "@HorarioFinal", DbType.String, horarioFinal);
            baseDados.AddInParameter(cmd, "@Mes", DbType.Int32, mes);
            baseDados.AddInParameter(cmd, "@Ano", DbType.Int32, ano);

            try
            {
                return (int)baseDados.ExecuteScalar(cmd);
                
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }            
        }

        public int GetNumeroDeAcessosPorAno(FaixaDeAcesso faixa, int ano)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("GetNumeroDeAcessosPorAno");
            
            string horarioInicial = GetDBString(faixa.HorarioInicial);
            string horarioFinal = GetDBString(faixa.HorarioFinal);
            
            baseDados.AddInParameter(cmd, "@HorarioInicial", DbType.String,horarioInicial);
            baseDados.AddInParameter(cmd, "@HorarioFinal", DbType.String, horarioFinal);
            baseDados.AddInParameter(cmd, "@Ano", DbType.Int32, ano);
 
            try
            {
                return (int)baseDados.ExecuteScalar(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }            
        }

        private string GetDBString(int horario)
        {
            if (horario == 24)
                return "23:59";
            StringBuilder sb = new StringBuilder();
            sb.Append(horario);
            sb.Append(":00");

            return sb.ToString();
        }
    }
}
