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
    public class TurmaBO
    {
        private DataAccess.TurmaDAO dao;
        private Usuario usr;

         public TurmaBO()
        {
            try
            {
                dao = new BusinessData.DataAccess.TurmaDAO();
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateTurma(Turma turma)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    dao.AtualizaTurma(turma);
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Turma: " + turma.Numero + "; Disciplina: " + turma.Disciplina + "; Semestre: " + turma.Calendario.Ano + "/" + turma.Calendario.Semestre + "; Id: "+ turma.Id +"; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Update Turma";
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

        public void DeletaTurma(Guid id)
        {
            if (usr.IsAdmin())
            {
                try
                {
                    Turma turma = dao.GetTurma(id);
                    dao.RemoveTurma(id);
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Turma: " + turma.Numero + "; Disciplina: " + turma.Disciplina + "; Semestre: " + turma.Calendario.Ano + "/" + turma.Calendario.Semestre + "; Id: " + turma.Id + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Delete Turma";
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
        public void InsereTurma(Turma turma, Calendario cal)
        {
            InsereTurma(turma);

            ConfigBO configuracoes = new ConfigBO();
            if (configuracoes.IsAulasDistribuidas(cal))
            {
                AulaBO controleAulas = new AulaBO();
                controleAulas.CriarAulas(cal,turma);
            }
        }

        public void InsereTurma(Turma turma)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereTurma(turma);
                    //MembershipUser user = Membership.GetUser();
                    //LogEntry log = new LogEntry();
                    //log.Message = "Turma: " + turma.Numero + "; Disciplina: " + turma.Disciplina + "; Semestre: " + turma.Calendario.Ano + "/" + turma.Calendario.Semestre + "; Id: " + turma.Id + "; Administrador: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Insert Turma";
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

        public Turma GetTurmaById(Guid id)
        {
            try
            {
                return dao.GetTurma(id);
            }
            catch (DataAccessException )
            {
                throw;
            }

        }

        public Turma GetTurmaById(Guid id, Calendario cal)
        {
            try
            {
                return dao.GetTurma(id,cal);
            }
            catch (DataAccessException )
            {
                throw;
            }

        }

        public List<Turma> GetTurmas()
        {
            return dao.GetTurmas();
        }
        public List<Turma> GetTurmas(Calendario cal, List<CategoriaDisciplina> categoriasDeDisciplina)
        {
            return dao.GetTurmas(cal, categoriasDeDisciplina);
        }
        public List<Turma> GetTurmas(Calendario cal)
        {
            return dao.GetTurmas(cal);
        }

        public List<Turma> GetTurmas(Calendario cal, Professor professor)
        {
            try
            {
                return dao.GetTurmas(cal,professor);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }

        public List<Turma> GetTurmas(Calendario cal, Guid professorId, DateTime data, string horario)
        {
            try
            {
                return dao.GetTurmas(cal, professorId, data, horario);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }

        public List<Turma> GetTurmas(Calendario cal, Guid professorid)
        {
            try
            {
                return dao.GetTurmas(cal, professorid);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }

    }
}
