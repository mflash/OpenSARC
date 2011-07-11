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
    public class DisciplinasBO
    {
        private DisciplinasDAO dao;
        private Usuario usr;

        public DisciplinasBO()
        {
            try
            {
                dao = new DisciplinasDAO();
            }
            catch (DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateDisciplina(Disciplina disciplina)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.UpdateDisciplina(disciplina);
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Disciplina: " + disciplina.Nome + "; Id: " + disciplina.Cod  +"; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Update Disciplina";
                    //log.MachineName = Dns.GetHostName();
                    //Logger.Write(log);
                }
                catch (DataAccess.DataAccessException )
                {
                    throw;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public void DeletaDisciplina(String cod, Guid calId)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    Disciplina disciplina = dao.GetDisciplina(cod, calId);
                    dao.DeletaDisciplina(cod, calId);
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Disciplina: " + disciplina.Nome + "; Id: " + disciplina.Cod + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Delete Disciplina";
                    //log.MachineName = Dns.GetHostName();
                    //Logger.Write(log);
                }
                catch (DataAccess.DataAccessException )
                {
                    throw;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public void InsereDisciplina(Disciplina disciplina)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereDisciplina(disciplina);
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Disciplina: " + disciplina.Nome + "; Id: " + disciplina.Cod + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Insert Disciplina";
                    //log.MachineName = Dns.GetHostName();
                    //Logger.Write(log);
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

        public void InsereDisciplinaInCalendario(Disciplina disciplina, Guid calId)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereDisciplinaInCalendario(disciplina, calId);
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

        public List<Disciplina> GetDisciplinaInCalendario(Guid id)
        {
            try
            {
                return dao.GetDisciplinaInCalendario(id);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public Disciplina GetDisciplina(string cod,Calendario cal)
        {
            try
            {
                return dao.GetDisciplina(cod,cal);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }
     
        public Disciplina GetDisciplina(string cod, Guid calId)
        {
            try
            {
                return dao.GetDisciplina(cod, calId);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }

        public List<Disciplina> GetDisciplinas(Calendario calendario)
        {
            try
            {
                return dao.GetDisciplinas(calendario);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }
        
        public List<Disciplina> GetDisciplinas(Guid calendarioId)
        {
            try
            {
               return dao.GetDisciplinas(calendarioId);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public List<Disciplina> GetDisciplinas()
        {
            try
            {
                return dao.GetDisciplinas();
            }
            catch (DataAccessException )
            {
                throw;
            }
        }


    }
}
