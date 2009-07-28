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
    public class FaculdadesBO
    {
        private DataAccess.FaculdadesDAO dao;
        private Usuario usr;

        public FaculdadesBO()
        {
            try
            {
                dao = new BusinessData.DataAccess.FaculdadesDAO();
            }
            catch (DataAccess.DataAccessException ex)
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateFaculdade(Entities.Faculdade vinculo)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.UpdateFaculdade(vinculo);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Faculdade: " + vinculo.Nome + "; Id: "+ vinculo.Id+ "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Update Faculdade";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
                }
                catch (DataAccess.DataAccessException ex)
                {
                    throw;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }
        
        public void DeletaFaculdade(Guid id)
        {
            if (usr.IsAdmin())
                {
                    try
                    {
                        Faculdade vinculo = dao.GetFaculdade(id);
                        dao.DeletaFaculdade(id);
                        MembershipUser user = Membership.GetUser();
                        LogEntry log = new LogEntry();
                        log.Message = "Faculdade: " + vinculo.Nome + "; Id: " + vinculo.Id + "; Administrador: " + user.UserName;
                        log.TimeStamp = DateTime.Now;
                        log.Severity = TraceEventType.Information;
                        log.Title = "Delete Faculdade";
                        log.MachineName = Dns.GetHostName();
                        Logger.Write(log);
                    }
                    catch (DataAccess.DataAccessException ex)
                    {
                        throw;
                    }
                }
                else
                {
                    throw new SecurityException("Acesso Negado.");
                }
        }
        
        public void InsereFaculdade(Entities.Faculdade vinculo)
        {
            
            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereFaculdade(vinculo);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Faculdade: " + vinculo.Nome + "; Id: " + vinculo.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Insert Faculdade";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
                }
                catch (DataAccess.DataAccessException ex)
                {
                    throw;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }
        
        public Entities.Faculdade GetFaculdadeById(Guid id)
        {
            try
            {
                return dao.GetFaculdade(id);
            }
            catch (DataAccess.DataAccessException ex)
            {
                throw;
            }
            
        }
        
        public List<Entities.Faculdade> GetFaculdades()
        {
            try
            {
                return dao.GetFaculdades();
            }
            catch (DataAccess.DataAccessException ex)
            {
                throw;
            }
        }
    }
}
