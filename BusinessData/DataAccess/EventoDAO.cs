using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using BusinessData.Entities;
using System.Web.Security;
using BusinessData.BusinessLogic;

namespace BusinessData.DataAccess
{
    internal class EventoDAO
    {
        private Database baseDados;

        public EventoDAO()
        {
            try
            {
                baseDados = DatabaseFactory.CreateDatabase("SARCcs");
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void InsereEvento(Entities.Evento evento)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("EventoInsere");
            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, evento.EventoId);
            baseDados.AddInParameter(cmd, "@AutorId", DbType.Guid, evento.AutorId.Id);
            baseDados.AddInParameter(cmd, "@Descricao", DbType.String, evento.Descricao);
            baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, evento.CalendarioId.Id);
            baseDados.AddInParameter(cmd, "@Responsavel", DbType.String, evento.Responsavel);
            baseDados.AddInParameter(cmd, "@Titulo", DbType.String, evento.Titulo);
            baseDados.AddInParameter(cmd, "@Unidade", DbType.String, evento.Unidade);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void RemoveEvento(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("EventoDelete");
            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void AtualizaEvento(Evento evento)
        {

            DbCommand cmd = baseDados.GetStoredProcCommand("EventoUpdate");
            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, evento.EventoId);
            baseDados.AddInParameter(cmd, "@Titulo", DbType.String, evento.Titulo);
            baseDados.AddInParameter(cmd, "@Responsavel", DbType.String, evento.Responsavel);
            baseDados.AddInParameter(cmd, "@Descricao", DbType.String, evento.Descricao);
            baseDados.AddInParameter(cmd, "@Unidade", DbType.String, evento.Unidade);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public Evento GetEvento(Guid? id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("EventoSelectById");
            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, id);

            ProfessorDAO professorDAO = new ProfessorDAO();
            CalendariosDAO calDAO = new CalendariosDAO();
            SecretarioDAO secretDAO = new SecretarioDAO();
            Evento aux = null;

            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();

                    PessoaBase autor;
                    PessoaFactory pF = PessoaFactory.GetInstance();
                    Guid? autorId = leitor["AutorId"] as Guid?;
                    if (autorId.HasValue)
                        autor = pF.CreatePessoa(leitor.GetGuid(leitor.GetOrdinal("AutorId")));
                    else autor = null;
                    Calendario calendario = calDAO.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));
                    aux = Evento.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")),
                                           autor,
                                           leitor.GetString(leitor.GetOrdinal("Descricao")),
                                           calendario,
                                           leitor.GetString(leitor.GetOrdinal("Responsavel")),
                                           leitor.GetString(leitor.GetOrdinal("Titulo")),
                                           leitor.GetString(leitor.GetOrdinal("Unidade")));
                }
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }

        public List<Evento> GetEventos()
        {
            List<Evento> listaAux;
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("EventosSelect");
                listaAux = new List<BusinessData.Entities.Evento>();
                Evento aux;

                CalendariosDAO calendarios = new CalendariosDAO();
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        PessoaBase autor = null;
                        autor = PessoaFactory.GetInstance().CreatePessoa(leitor.GetGuid(leitor.GetOrdinal("AutorId")));

                        Calendario calendario = calendarios.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                        aux = Evento.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")),
                                                    autor,
                                                    leitor.GetString(leitor.GetOrdinal("Descricao")),
                                                    calendario,
                                                    leitor.GetString(leitor.GetOrdinal("Responsavel")),
                                                    leitor.GetString(leitor.GetOrdinal("Titulo")),
                                                    leitor.GetString(leitor.GetOrdinal("Unidade")));
                        listaAux.Add(aux);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;
        }

        public List<Evento> GetEventos(Guid autorId, DateTime data, string hora)
        {
            List<Evento> listaAux;
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("EventoSelectByAutorDataHora");
                baseDados.AddInParameter(cmd, "@AutorId", DbType.Guid, autorId);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
                baseDados.AddInParameter(cmd, "@Hora", DbType.String, hora);

                CalendariosDAO calendarios = new CalendariosDAO();
                listaAux = new List<Evento>();
                Evento aux;
                PessoaBase autor = null;

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {

                        autor = PessoaFactory.GetInstance().CreatePessoa(leitor.GetGuid(leitor.GetOrdinal("AutorId")));

                        Calendario calendario = calendarios.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                        aux = Evento.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")),
                                                    autor,
                                                    leitor.GetString(leitor.GetOrdinal("Descricao")),
                                                    calendario,
                                                    leitor.GetString(leitor.GetOrdinal("Responsavel")),
                                                    leitor.GetString(leitor.GetOrdinal("Titulo")),
                                                    leitor.GetString(leitor.GetOrdinal("Unidade")));
                        listaAux.Add(aux);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;
        }

        public List<Evento> GetEventosByCal(Guid id)
        {
            List<Evento> listaAux;
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("EventoSelectByCal");
                baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, id);
                listaAux = new List<BusinessData.Entities.Evento>();
                Evento aux;
                CalendariosDAO calendarios = new CalendariosDAO();
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        PessoaBase autor = null;
                        autor = PessoaFactory.GetInstance().CreatePessoa(leitor.GetGuid(leitor.GetOrdinal("AutorId")));

                        Calendario calendario = calendarios.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                        aux = Evento.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")),
                                                    autor,
                                                    leitor.GetString(leitor.GetOrdinal("Descricao")),
                                                    calendario,
                                                    leitor.GetString(leitor.GetOrdinal("Responsavel")),
                                                    leitor.GetString(leitor.GetOrdinal("Titulo")),
                                                    leitor.GetString(leitor.GetOrdinal("Unidade")));
                        listaAux.Add(aux);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;
        }

        public List<Evento> GetEventos(Guid autorId, Calendario cal)
        {
            List<Evento> listaAux;
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("EventoSelectByAutor");
                baseDados.AddInParameter(cmd, "@AutorId", DbType.Guid, autorId);
                baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, DateTime.Now);

                CalendariosDAO calendarios = new CalendariosDAO();
                listaAux = new List<Evento>();
                Evento aux;
                PessoaBase autor = null;

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {

                        autor = PessoaFactory.GetInstance().CreatePessoa(leitor.GetGuid(leitor.GetOrdinal("AutorId")));

                        Calendario calendario = calendarios.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                        aux = Evento.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")),
                                                    autor,
                                                    leitor.GetString(leitor.GetOrdinal("Descricao")),
                                                    calendario,
                                                    leitor.GetString(leitor.GetOrdinal("Responsavel")),
                                                    leitor.GetString(leitor.GetOrdinal("Titulo")),
                                                    leitor.GetString(leitor.GetOrdinal("Unidade")));
                        listaAux.Add(aux);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;

        }

        public List<Evento> GetEventosNaoOcorridos(Guid calId)
        {
            List<Evento> listaAux;
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("EventoSelectNaoOcorridos");
                baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, calId);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, DateTime.Now);
                
                CalendariosDAO calendarios = new CalendariosDAO();
                listaAux = new List<Evento>();
                Evento aux;
                PessoaBase autor = null;

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        try
                        {
                            autor = PessoaFactory.GetInstance().CreatePessoa(leitor.GetGuid(leitor.GetOrdinal("AutorId")));

                            Calendario calendario = calendarios.GetCalendario(leitor.GetGuid(leitor.GetOrdinal("CalendarioId")));

                            aux = Evento.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")),
                                                        autor,
                                                        leitor.GetString(leitor.GetOrdinal("Descricao")),
                                                        calendario,
                                                        leitor.GetString(leitor.GetOrdinal("Responsavel")),
                                                        leitor.GetString(leitor.GetOrdinal("Titulo")),
                                                        leitor.GetString(leitor.GetOrdinal("Unidade")));
                            listaAux.Add(aux);
                        }
                        catch (NullReferenceException )
                        {
                            return listaAux;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;
        }
    }
}
