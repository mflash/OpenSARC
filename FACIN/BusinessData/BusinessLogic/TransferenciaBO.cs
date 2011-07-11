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
    public class TransferenciaBO
    {
        private TransferenciaDAO dao;
        private Usuario usr;

        public TransferenciaBO()
        {
            try
            {
                dao = new TransferenciaDAO();
            }
            catch (DataAccessException)
            {
                throw;
            }
            usr = new Usuario();
        }

        public void InsereTransferencia(Transferencia trans)
        {
            try
            {
                dao.InsereTransferencia(trans);

                //MembershipUser user = Membership.GetUser();
                //LogEntry log = new LogEntry();
                //log.Message = "; Id: " + trans.Id + "; Usuário: " + user.UserName;
                //log.TimeStamp = DateTime.Now;
                //log.Severity = TraceEventType.Information;
                //log.Title = "Insert Transferência";
                //log.MachineName = Dns.GetHostName();
                //Logger.Write(log);
            }
            catch (DataAccessException)
            {
                throw;
            }
        }

        public List<Transferencia> GetTransferencias(Guid profId, Calendario cal)
        {
            try
            {
                return dao.GetTransferencias(profId, cal);
            }
            catch (DataAccessException)
            {
                throw;
            }
        
        }

        public Transferencia GetTransferencia(Guid id, Calendario cal)
        {
            try
            {
                return dao.GetTransferencia(id, cal);
            }
            catch (DataAccessException)
            {
                throw;
            }
        
        }

        public void TransferenciaUpdate(Transferencia trans)
        {
            try
            {
                dao.TransferenciaUpdate(trans);

                //MembershipUser user = Membership.GetUser();
                //LogEntry log = new LogEntry();
                //log.Message = "; Id: " + trans.Id + "; Usuário: " + user.UserName;
                //log.TimeStamp = DateTime.Now;
                //log.Severity = TraceEventType.Information;
                //log.Title = "Update Transferência";
                //log.MachineName = Dns.GetHostName();
                //Logger.Write(log);
            }
            catch (DataAccessException)
            {
                throw;
            }
        
        }

        public List<PessoaBase> GetResponsaveisByDataHora(string horario, DateTime data, Guid profId)
        {
            try
            {
                return dao.GetResponsaveisByDataHora(horario, data, profId);
            }
            catch (DataAccessException)
            {
                throw;
            }

        }
    }
}
