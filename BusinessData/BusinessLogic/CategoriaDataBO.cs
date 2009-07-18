using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;
using BusinessData.Entities;
using BusinessData.DataAccess;

namespace BusinessData.BusinessLogic
{
    public class CategoriaDataBO
    {
        private CategoriaDataDAO dao;
        private Usuario usr;

        public CategoriaDataBO()
        {
            try
            {
                dao = new CategoriaDataDAO();
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            usr = new Usuario();
        }

        public void UpdateCategoriaData(CategoriaData categoriaData)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.UpdateCategoriaData(categoriaData);

                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Data: " + categoriaData.Descricao + "; Id: " + categoriaData.Id +"; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Update Categoria de Data";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
                }
                catch (DataAccess.DataAccessException ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public void DeleteCategoriaData(Guid id)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    CategoriaData categoriaData = dao.GetCategoriaData(id);
                    dao.DeletaCategoriaData(id);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Data: " + categoriaData.Descricao  +"; Id: " + categoriaData.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Delete Categoria de Data";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
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

        public void InsereCategoriaData(CategoriaData categoriaData)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereCategoriaData(categoriaData);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Data: " + categoriaData.Descricao + "; Id: " + categoriaData.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Insert Categoria de Data";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
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

        public CategoriaData GetCategoriaDataById(Guid id)
        {
            try
            {
                return dao.GetCategoriaData(id);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }

        public List<CategoriaData> GetCategoriaDatas()
        {
            try
            {
                return dao.GetCategoriaData();
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }
    }
}
