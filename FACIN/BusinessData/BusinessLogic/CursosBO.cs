using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using BusinessData.DataAccess;
using BusinessData.Entities;
//Log
//using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;

namespace BusinessData.BusinessLogic
{
    public class CursosBO
    {
        private CursosDAO dao;
        private Usuario usr;
               
         public CursosBO()
        {
            try
            {
                dao = new CursosDAO();
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
            
        }

        public void UpdateCurso(Curso curso)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.UpdateCurso(curso);

                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    //LogEntry log = new LogEntry();
                    //monta log
                    //log.Message = "Curso: " + curso.Nome + "; Id: " + curso.Codigo + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Update Curso";
                    //log.MachineName = Dns.GetHostName();
                    //guarda log no banco
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

        public void DeletaCurso(string cod)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    Curso curso = dao.GetCurso(cod);
                    dao.DeletaCurso(cod);

                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    //LogEntry log = new LogEntry();
                    //monta log
                    //log.Message = "Curso: " + curso.Nome + "; Id: " + curso.Codigo + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Delete Curso";
                    //log.MachineName = Dns.GetHostName();
                    //guarda log no banco
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

        public void InsereCurso(Curso curso)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereCurso(curso);

                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    //LogEntry log = new LogEntry();
                    //monta log
                    //log.Message = "Curso: " + curso.Nome + "; Id: " + curso.Codigo + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Insert Curso";
                    //log.MachineName = Dns.GetHostName();
                    //guarda log no banco
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

        public Curso GetCursoByCodigo(string cod)
        {
            try
            {
                return dao.GetCurso(cod);
            }
            catch (DataAccessException )
            {
                throw;
            }

        }

        public List<Curso> GetCursos()
        {
            return dao.GetCursos();
        }
    }
}
