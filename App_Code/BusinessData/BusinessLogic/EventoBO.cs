using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using BusinessData.Util;
using BusinessData.Entities;
using BusinessData.DataAccess;
//Log
//using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;

namespace BusinessData.BusinessLogic
{
    public class EventoBO
    {
        private DataAccess.EventoDAO dao;
        private Usuario usr;

        public EventoBO()
        {
            try
            {
                dao = new BusinessData.DataAccess.EventoDAO();
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateEvento(Entities.Evento evento)
        {
            try
            {
                dao.AtualizaEvento(evento);
                //instancia o usuario logado
                MembershipUser user = Membership.GetUser();
                //instancia o log
                //LogEntry log = new LogEntry();
                //monta log
                //log.Message = "Evento: " + evento.Titulo + "; Id: " + evento.EventoId + "; Usuário: " + user.UserName;
                //log.TimeStamp = DateTime.Now;
                //log.Severity = TraceEventType.Information;
                //log.Title = "Update Evento";
                //log.MachineName = Dns.GetHostName();
                //guarda log no banco
                //Logger.Write(log);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }

        public void DeletaEvento(Guid id)
        {
            try
            {
                Evento evento = dao.GetEvento(id);
                dao.RemoveEvento(id);
                //instancia o usuario logado
                MembershipUser user = Membership.GetUser();
                //instancia o log
                //LogEntry log = new LogEntry();
                //monta log
                //log.Message = "Evento: " + evento.Titulo + "; Id: " + evento.EventoId + "; Usuário: " + user.UserName;
                //log.TimeStamp = DateTime.Now;
                //log.Severity = TraceEventType.Information;
                //log.Title = "Delete Evento";
                //log.MachineName = Dns.GetHostName();
                //guarda log no banco
                //Logger.Write(log);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }

        public void InsereEvento(Entities.Evento evento)
        {
            try
            {
                dao.InsereEvento(evento);
                //instancia o usuario logado
                MembershipUser user = Membership.GetUser();
                //instancia o log
                //LogEntry log = new LogEntry();
                //monta log
                //log.Message = "Evento: " + evento.Titulo + "; Id: " + evento.EventoId + "; Usuário: " + user.UserName;
                //log.TimeStamp = DateTime.Now;
                //log.Severity = TraceEventType.Information;
                //log.Title = "Insert Evento";
                //log.MachineName = Dns.GetHostName();
                //guarda log no banco
                //Logger.Write(log);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }

        public Evento GetEventoById(Guid id)
        {
            try
            {
                return dao.GetEvento(id);
            }
            catch (DataAccessException )
            {
                throw;
            }

        }

        public List<Evento> GetEventos()
        {
            try
            {
                return dao.GetEventos();
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public List<Evento> GetEventos(Guid autorId, DateTime data, string hora)
        {
            try
            {
                return dao.GetEventos(autorId, data, hora);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public List<Evento> GetEventosByCal(Guid id)
        {
            try
            {
                return dao.GetEventosByCal(id);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public List<Evento> GetEventos(Guid autorId, Calendario cal)
        {
            try
            {
                return dao.GetEventos(autorId, cal);
            }
            catch (DataAccessException )
            {
                throw;
            }

        }

        public List<Evento> GetEventosNaoOcorridos(Guid calId)
        {
            try
            {
                return dao.GetEventosNaoOcorridos(calId);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }
    }
}
