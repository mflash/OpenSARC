using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.DataAccess;
using System.Security;
using BusinessData.Entities;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;

namespace BusinessData.BusinessLogic
{
    public class AlocacaoBO
    {
        private AlocacaoDAO dao;
        private Usuario user;

        public AlocacaoBO()
        {
            user = new Usuario();
            dao = new AlocacaoDAO();
        }
    
        public void InsereAlocacao(Alocacao alocacao)
        {
            if (user.IsAdmin())
            {
                try
                {
                    dao.InsereAlocacao(alocacao);
                }
                catch (DataAccessException ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public void DeletaAlocacao(Alocacao alocacao)
        {
            if (user.IsAdmin())
            {
                try
                {
                    dao.DeletaAlocacao(alocacao);
                }
                catch (DataAccessException ex)
                {
                    throw ex;
                }
            }
            else throw new SecurityException("Acesso Negado.");

        }

        public void UpdateAlocacao(Alocacao alocacao)
        {  
            try
            {
                dao.UpdateAlocacao(alocacao);
                MembershipUser user = Membership.GetUser();
                LogEntry log = new LogEntry();
                log.Message = "Recurso: " + alocacao.Recurso.Categoria + " - " + alocacao.Recurso.Descricao + "; Id: "+ alocacao.Recurso.Id +"; Data: " + alocacao.Data.ToShortDateString() + "; Horário: " + alocacao.Horario + "; Usuário: " + user.UserName;
                log.TimeStamp = DateTime.Now;
                log.Severity = TraceEventType.Information;
                log.Title = "Update Alocação";
                log.MachineName = Dns.GetHostName();
                Logger.Write(log);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }            
        }

        public Alocacao GetAlocacao(Guid recursoId, DateTime data, string horario)
        {  
            try
            {
                return dao.GetAlocacao(recursoId, data, horario);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }   
        }

        public void PreencheCalendarioDeAlocacoes(Entities.Calendario cal, Recurso rec, bool datasOrdenadas)
        {
            if (!datasOrdenadas)
            {
                cal.Datas.Sort();
            }
            DateTime data = cal.InicioG1;
            Alocacao alocacao;

            string[] listaHorarios = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P" };

            while (data != cal.FimG2)
            {
                foreach (string aux in listaHorarios)
                {
                    alocacao = Alocacao.newAlocacao(rec, data, aux, null, null);
                    this.InsereAlocacao(alocacao);
                }
                data = data.AddDays(1);

            }
        }

        public void preencheCalendario(Entities.Calendario cal)
         {
             cal.Datas.Sort();
             RecursosBO controleRecursos = new RecursosBO();
             IList<Entities.Recurso> listaRecursos = controleRecursos.GetRecursos();
             
             foreach (Entities.Recurso recursoAux in listaRecursos)
             {
                 PreencheCalendarioDeAlocacoes(cal, recursoAux,true);
             }
         }

        public void esvaziaCalendario()
         {

         }

        public List<Recurso> GetRecursoAlocadoByAula(DateTime data, string hora, Guid aulaId)
         { 
                try
                {
                     return dao.GetRecursoAlocadoByAula(data, hora, aulaId);
                }
                catch (DataAccessException ex)
                {
                     throw ex;
                }
         }

        public List<Alocacao> GetRecursosAlocados(DateTime data, string hora)
         {
             try
             {
                 return dao.GetRecursosAlocados(data, hora);
             }
             catch (DataAccessException ex)
             {
                 throw ex;
             }
         }

        public List<Alocacao> GetAlocacoes(Calendario cal, DateTime data, Professor prof)
         {
             return dao.GetAlocacoes(cal, data, prof);
         }

        public List<Alocacao> GetAlocacoes(Calendario cal, DateTime data, Guid recursoId)
         {
             return dao.GetAlocacoes(cal, data, recursoId);
         }

        public List<Alocacao> GetAlocacoes(Calendario cal, DateTime data, Secretario secretario)
        {
            return dao.GetAlocacoes(cal, data, secretario);
        }

        public List<Alocacao> GetAlocacoesSemData(Calendario cal, Guid recursoId)
        {
            return dao.GetAlocacoesSemData(cal, recursoId);
        }

        public List<Alocacao> GetAlocacoesSemData(Calendario cal, Professor prof)
        {
            return dao.GetAlocacoesSemData(cal, prof);
        }

        public List<Alocacao> GetAlocacoesSemData(Calendario cal, Secretario secretario)
        {
            return dao.GetAlocacoesSemData(cal, secretario);
        }

        public List<Recurso> GetRecursoAlocadoByEvento(DateTime data, string hora, Guid eventoId)
        {
            try
            {
                return dao.GetRecursoAlocadoByEvento(data, hora, eventoId);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }

        public List<Alocacao> GetAlocacoesByData(DateTime data, Calendario cal)
        {
            try
            {
                return dao.GetAlocacoesByData(data, cal);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }
    }
}
