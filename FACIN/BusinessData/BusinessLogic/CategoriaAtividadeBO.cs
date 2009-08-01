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
    public class CategoriaAtividadeBO
    {
        private CategoriaAtividadeDAO dao;
        private Usuario usr;

        public CategoriaAtividadeBO()
        {
            try
            {
                dao = new CategoriaAtividadeDAO();
            }
            catch (DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateCategoriaAtividade(CategoriaAtividade categoriaAtividade)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.UpdateCategoriaAtividade(categoriaAtividade);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Atividade: " + categoriaAtividade.Descricao + "; Id: " + categoriaAtividade.Id +"; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Update Categoria de Atividade";
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

        public void DeleteCategoriaAtividade(Guid id)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    CategoriaAtividade categoriaAtividade = dao.GetCategoriaAtividadeById(id);
                    dao.DeletaCategoriaAtividade(id);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Atividade: " + categoriaAtividade.Descricao + "; Id: " + categoriaAtividade.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Delete Categoria de Atividade";
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

        public void InsereCategoriaAtividade(CategoriaAtividade categoriaAtividade)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereCategoriaAtividade(categoriaAtividade);
                    MembershipUser user = Membership.GetUser();
                    LogEntry log = new LogEntry();
                    log.Message = "Categoria de Atividade: " + categoriaAtividade.Descricao + "; Id: " + categoriaAtividade.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Insert Categoria de Atividade";
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

        public CategoriaAtividade GetCategoriaAtividadeById(Guid id)
        {
            try
            {
                return dao.GetCategoriaAtividadeById(id);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public List<CategoriaAtividade> GetCategoriaAtividade()
        {
            try
            {
                return dao.GetCategoriaAtividade();
            }
            catch (DataAccessException )
            {
                throw;
            }
        }
    }
}
