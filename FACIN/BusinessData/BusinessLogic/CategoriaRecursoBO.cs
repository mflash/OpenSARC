using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;
using BusinessData.Entities;

namespace BusinessData.BusinessLogic
{
    public class CategoriaRecursoBO
    {
        private DataAccess.CategoriaRecursoDAO dao;
        // FIXME: Alguma maneira melhor de fazer isto ???
        //private static List<CategoriaRecurso> listByUse = new BusinessData.DataAccess.CategoriaRecursoDAO().GetCategoriaRecursoSortedByUse();
        private Usuario usr;

        public CategoriaRecursoBO()
        {
            try
            {
                dao = new BusinessData.DataAccess.CategoriaRecursoDAO();
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateCategoriaRecurso(Entities.CategoriaRecurso categoriaRecurso)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.UpdateCategoriaRecurso(categoriaRecurso);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Recurso: " + categoriaRecurso.Descricao + "; Id: " +categoriaRecurso.Id +"; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Update Categoria de Recurso";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
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

        public void DeletaCategoriaRecurso(Guid id)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    CategoriaRecurso categoriaRecurso = dao.GetCategoriaRecurso(id);
                    dao.DeletaCategoriaRecurso(id);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Recurso: " + categoriaRecurso.Descricao + "; Id: " + categoriaRecurso.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Delete Categoria de Recurso";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
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

        public void InsereCategoriaRecurso(Entities.CategoriaRecurso categoriaRecurso)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereCategoriaRecurso(categoriaRecurso);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Recurso: " + categoriaRecurso.Descricao + "; Id: " + categoriaRecurso.Id.ToString() + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Insert Categoria de Recurso";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
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

        public Entities.CategoriaRecurso GetCategoriaRecursoById(Guid id)
        {
            try
            {
                return dao.GetCategoriaRecurso(id);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }

        }

        public List<Entities.CategoriaRecurso> GetCategoriaRecurso()
        {
            return dao.GetCategoriaRecurso();
        }

        public List<Entities.CategoriaRecurso> GetCategoriaRecursoSortedByUse()
        {
            return dao.GetCategoriaRecursoSortedByUse();// listByUse;
        }

    }
}
