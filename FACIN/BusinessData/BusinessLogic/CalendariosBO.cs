using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using BusinessData.Entities;
using BusinessData.DataAccess;

//using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;

namespace BusinessData.BusinessLogic
{
    public class CalendariosBO
    {
        private DataAccess.CalendariosDAO dao;
        private Usuario usr;

        public CalendariosBO()
        {
            try
            {
                dao = new CalendariosDAO();
            }
            catch (DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateCalendario(Calendario calendario)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.UpdateCalendario(calendario);
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Calendario: " + calendario.Ano + "/" + calendario.Semestre + "; Id: "+calendario.Id +"; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Update Calendario";
                    //log.MachineName = Dns.GetHostName();
                    //Logger.Write(log);
                }
                catch (DataAccessException )
                {
                    throw;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public void DeletaCalendarioById(Guid cod)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    Calendario calendario = dao.GetCalendario(cod);
                    dao.DeletaCalendario(cod);
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Calendario: " + calendario.Ano + "/" + calendario.Semestre + "; Id: " + calendario.Id + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Delete Calendario";
                    //log.MachineName = Dns.GetHostName();
                    //Logger.Write(log);
                }
                catch (DataAccessException )
                {
                    throw;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public void InsereCalendario(Calendario calendario)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereCalendario(calendario);
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Calendario: " + calendario.Ano + "/" + calendario.Semestre + "; Id: " + calendario.Id + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Insert Calendario";
                    //log.MachineName = Dns.GetHostName();
                    //Logger.Write(log);
                }
                catch (DataAccessException )
                {
                    throw;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public List<Calendario> GetCalendarios()
        {
            try
            {
                return dao.GetCalendarios();
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public Calendario GetCalendario(Guid cod)
        {
            try
            {
                return dao.GetCalendario(cod);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public Calendario GetCalendarioByAnoSemestre(int ano, int semestre)
        {
            try
            {
                return dao.GetCalendarioByAnoSemestre(ano, semestre);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }
    }
}
