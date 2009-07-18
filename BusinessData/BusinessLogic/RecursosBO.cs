using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using BusinessData.Entities;
using BusinessData.DataAccess;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;

namespace BusinessData.BusinessLogic
{
    public class RecursosBO
    {
        private DataAccess.RecursosDAO dao;
        private Usuario usr;

        public RecursosBO()
        {
            try
            {
                dao = new BusinessData.DataAccess.RecursosDAO();
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateRecurso(Recurso recurso)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.UpdateRecurso(recurso);

                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    LogEntry log = new LogEntry();
                    //monta log
                    log.Message = "Recurso: " + recurso.Descricao + "; Id: " + recurso.Id+"; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Update Recurso";
                    log.MachineName = Dns.GetHostName();
                    //guarda log no banco
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

        public void DeletaRecurso(Guid id)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    Recurso recurso = GetRecursoById(id);
                    dao.DeletaRecurso(id);

                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    LogEntry log = new LogEntry();
                    //monta log
                    log.Message = "Recurso: " + recurso.Descricao + "; Id: " + recurso.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Delete Recurso";
                    log.MachineName = Dns.GetHostName();
                    //guarda log no banco
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

        public void InsereRecurso(Recurso recurso, Calendario cal)
        {
            
            if (usr.IsAdmin())
            {
                ConfigBO configuracoes = new ConfigBO();
                try
                {
                    dao.InsereRecurso(recurso);

                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    LogEntry log = new LogEntry();
                    //monta log
                    log.Message = "Recurso: " + recurso.Descricao + "; Id: " + recurso.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Insert Recurso";
                    log.MachineName = Dns.GetHostName();
                    //guarda log no banco
                    Logger.Write(log);

                    if (configuracoes.IsAulasDistribuidas(cal))
                    {
                        AlocacaoBO aloc = new AlocacaoBO();
                        aloc.PreencheCalendarioDeAlocacoes(cal, recurso, false);
                    }
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

        public Recurso GetRecursoById(Guid id)
        {
            try
            {
                return dao.GetRecurso(id);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }

        }

        public List<Recurso> GetRecursos()
        {
            try
            {
                return dao.GetRecursos();
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }

        public List<Recurso> GetRecursosDisponiveis(DateTime data, string hora)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    return dao.GetRecursosDisponiveis(data,hora);
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

        public List<Recurso> GetRecursosAlocados()
        {
            if (usr.IsAdmin())
            {
                try
                {
                    return dao.GetRecursosAlocados();
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

        public void InsereHorarioBloqueado(HorarioBloqueado hb, Recurso recurso)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereHorarioBloqueado(hb, recurso);

                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    LogEntry log = new LogEntry();
                    //monta log
                    log.Message = "Recurso: " + recurso.Descricao + "; Id: " + recurso.Id + "; Horario de Inicio: " + hb.HoraInicio + "; Horario de Fim: " + hb.HoraFim + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Insert Horario Bloqueado de um Recurso";
                    log.MachineName = Dns.GetHostName();
                    //guarda log no banco
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

        public void DeletaHorarioBloqueado(Guid recursoId)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    Recurso recurso = GetRecursoById(recursoId);
                    dao.DeletaHorarioBloqueado(recursoId);

                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    LogEntry log = new LogEntry();
                    //monta log
                    log.Message = "Recurso: " + recurso.Descricao + "; Id: " + recurso.Id + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Delete todos Horarios Bloqueado de um Recurso";
                    log.MachineName = Dns.GetHostName();
                    //guarda log no banco
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

        public void DeletaHorarioBloqueado(Guid recursoId, HorarioBloqueado hb)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    Recurso recurso = GetRecursoById(recursoId);
                    dao.DeletaHorarioBloqueado(recursoId, hb);

                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    LogEntry log = new LogEntry();
                    //monta log
                    log.Message = "Recurso: " + recurso.Descricao + "; Id: " + recurso.Id + "; Horario de Inicio: " + hb.HoraInicio + "; Horario de Fim: " + hb.HoraFim + "; Administrador: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Delete Horario Bloqueado de um Recurso";
                    log.MachineName = Dns.GetHostName();
                    //guarda log no banco
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

        public List<HorarioBloqueado> GetHorarioBloqueadoByRecurso(Guid recursoId)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    return dao.GetHorarioBloqueadoByRecurso(recursoId);
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

        public List<Recurso> GetRecursosDisponiveis(DateTime data, string horarioPUCRS, Guid categoriaRecursoId)
        {
            try
            {
                return dao.GetRecursosDisponiveis(data, horarioPUCRS, categoriaRecursoId);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna todos os recursos da categoria especificada
        /// </summary>
        /// <param name="cat">Categoria de Recursos</param>
        /// <returns></returns>
        public List<Recurso> GetRecursosPorCategoria(CategoriaRecurso cat)
        {
            return dao.GetRecursosPorCategoria(cat);
        }
    }

}
