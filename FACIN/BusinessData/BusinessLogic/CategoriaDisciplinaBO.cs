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
    public class CategoriaDisciplinaBO
    {
        private CategoriaDisciplinaDAO dao;
        private Usuario usr;

        public CategoriaDisciplinaBO()
        {
            try
            {
                dao = new CategoriaDisciplinaDAO();
            }
            catch (DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateCategoriaDisciplina(CategoriaDisciplina categoriaDisciplina)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.UpdateCategoriaDisciplina(categoriaDisciplina);

                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Disciplina: " + categoriaDisciplina.Descricao + "; Id: " + categoriaDisciplina.Id +"; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Update Categoria de Disciplina";
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

        public void DeletaCategoriaDisciplina(Guid id)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    CategoriaDisciplina categoriaDisciplina = dao.GetCategoriaDisciplina(id);
                    dao.DeletaCategoriaDisciplina(id);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Disciplina: " + categoriaDisciplina.Descricao + "; Id: " + categoriaDisciplina.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Delete Categoria de Disciplina";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
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

        public void InsereCategoriaDisciplina(CategoriaDisciplina categoriaDisciplina)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereCategoriaDisciplina(categoriaDisciplina);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Disciplina: " + categoriaDisciplina.Descricao + "; Id: " + categoriaDisciplina.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Insert Categoria de Disciplina";
                    log.MachineName = Dns.GetHostName();
                    Logger.Write(log);
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

        public CategoriaDisciplina GetCategoriaDisciplina(Guid id)
        {
            try
            {
                return dao.GetCategoriaDisciplina(id);
            }
            catch (DataAccessException )
            {
                throw;
            }

        }

        public List<CategoriaDisciplina> GetCategoriaDisciplinas()
        {
            return dao.GetCategoriaDisciplinas();
        }


    }
}
