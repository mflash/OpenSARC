using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.DataAccess;
using System.Security;
using BusinessData.Entities;

//using Microsoft.Practices.EnterpriseLibrary.Logging;
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
                //MembershipUser user = Membership.GetUser();
                //LogEntry log = new LogEntry();
                //log.Message = "Recurso: " + alocacao.Recurso.Categoria + " - " + alocacao.Recurso.Descricao + "; Id: "+ alocacao.Recurso.Id +"; Data: " + alocacao.Data.ToShortDateString() + "; Horário: " + alocacao.Horario + "; Usuário: " + user.UserName;
                //log.TimeStamp = DateTime.Now;
                //log.Severity = TraceEventType.Information;
                //log.Title = "Update Alocação";
                //log.MachineName = Dns.GetHostName();
                //Logger.Write(log);
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

        public void PreencheCalendarioDeAlocacoes(Entities.Calendario cal, Recurso rec, bool datasOrdenadas, bool completar=false)
        {
            if (!datasOrdenadas)
            {
                cal.Datas.Sort();
            }
            DateTime data = cal.InicioG1;
            Alocacao alocacao;

            Dictionary<DateTime, bool> aloc = new Dictionary<DateTime, bool>();

            Debug.WriteLine("\n****** Recurso: " + rec.Descricao);

            if (completar)
            {
                if (rec.EstaDisponivel == false)
                    return;
                foreach(Alocacao a in this.GetAlocacoesRecurso(cal, rec.Id))
                {
                    if (!aloc.ContainsKey(a.Data))
                    {
                        //Debug.WriteLine("Data ja existente: " + a.Data);
                        aloc.Add(a.Data, true);
                    }
                }
            }

            string[] listaHorarios = { "A", "B", "C", "D", "E", "X", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P" };

            while (data != cal.FimG2)
            {
                if(completar)
                {
                    if(aloc.ContainsKey(data))
                    {
                        data = data.AddDays(1);
                        continue;
                    }
                }
                foreach (string aux in listaHorarios)
                {
                    alocacao = Alocacao.newAlocacao(rec, data, aux, null, null);
                    Debug.WriteLine("   Nova: " + alocacao.Data + " " + alocacao.Horario + " " + alocacao.Recurso.Descricao);
                    this.InsereAlocacao(alocacao);
                }
                data = data.AddDays(1);

            }
        }

        public void preencheCalendario(Entities.Calendario cal, bool completar=false)
         {
             cal.Datas.Sort();
             RecursosBO controleRecursos = new RecursosBO();
             IList<Entities.Recurso> listaRecursos = controleRecursos.GetRecursos();
             
             foreach (Entities.Recurso recursoAux in listaRecursos)
             {
                 PreencheCalendarioDeAlocacoes(cal, recursoAux,true, completar);
             }
             Debug.WriteLine("FIM DA CRIACAO DE ALOCACOES");
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

        public List<Alocacao> GetAlocacoes(Calendario cal)
        {
            return dao.GetTodasAlocacoes(cal);
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

        public List<Alocacao> GetAlocacoesRecurso(Calendario cal, Guid recurso)
        {
            return dao.GetAlocacoesRecurso(cal, recurso);
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

        internal List<Alocacao> GetAlocacoes(DateTime data)
        {
            try
            {
                return dao.GetAlocacoes(data);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }
    }
}
