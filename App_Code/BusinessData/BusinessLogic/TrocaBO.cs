using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.DataAccess;
using BusinessData.Entities;

//using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;

namespace BusinessData.BusinessLogic
{
    public class TrocaBO
    {
        private TrocaDAO dao;
        private Usuario usr;

        public TrocaBO()
        {
            try
            {
                dao = new TrocaDAO();
            }
            catch (DataAccessException)
            {
                throw;
            }
            usr = new Usuario();
        }

        public void InsereTroca(Troca troca)
        {
            try
            {
                dao.InsereTroca(troca);

                //MembershipUser user = Membership.GetUser();
                //LogEntry log = new LogEntry();
                //log.Message = "; Id: " + troca.Id+ "; Usuário: " + user.UserName;
                //log.TimeStamp = DateTime.Now;
                //log.Severity = TraceEventType.Information;
                //log.Title = "Insert Troca";
                //log.MachineName = Dns.GetHostName();
                //Logger.Write(log);
            }
            catch (DataAccessException)
            {
                throw;
            }
        }
        
        public void UpDateTroca(Troca troca)
        {
            try
            {
                dao.UpDateTroca(troca);

                //MembershipUser user = Membership.GetUser();
                //LogEntry log = new LogEntry();
                //log.Message = "; Id: " + troca.Id + "; Usuário: " + user.UserName;
                //log.TimeStamp = DateTime.Now;
                //log.Severity = TraceEventType.Information;
                //log.Title = "Update Troca";
                //log.MachineName = Dns.GetHostName();
                //Logger.Write(log);
            }
            catch (DataAccessException)
            {
                throw;
            }
        }

        public Troca GetJaPropos(Guid idAtual)
        {
            try
            {
                return dao.GetJaPropos(idAtual);
            }
            catch (DataAccessException)
            {
                throw;
            }
        }

        public List<Troca> GetTrocasAulasByProfessor(Guid profId, Calendario cal)
        { 
            try
            {
                return dao.GetTrocasAulasByProfessor(profId, cal);
            }
            catch(DataAccessException)
            {
                throw;
            }
        }

        public List<Troca> GetNaoVisualizadasByAula(Guid aulaId, DateTime data, string horario)
        {
            try
            {
                return dao.GetNaoVisualizadasByAula(aulaId, data, horario);
            }
            catch (DataAccessException)
            {
                throw;
            }
        }

        public List<Troca> GetTrocasEventosByAutor(Guid autorid, Calendario cal)
        {
            try
            {
                return dao.GetTrocasEventosByAutor(autorid, cal);
            }
            catch (DataAccessException)
            {
                throw;
            }
        }

        public List<Troca> GetNaoVisualizadasByEvento(Guid eventoId, DateTime data, string horario)
        {
            try
            {
                return dao.GetNaoVisualizadasByEvento(eventoId, data, horario);
            }
            catch (DataAccessException)
            {
                throw;
            }
        }

    }
}
