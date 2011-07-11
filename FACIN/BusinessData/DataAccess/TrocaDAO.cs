using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace BusinessData.DataAccess
{
    public class TrocaDAO
    {
        private Database baseDados;

        public TrocaDAO()
        {
            try
            {
                baseDados = DatabaseFactory.CreateDatabase("SARCFACINcs");
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void InsereTroca(Troca troca)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TrocasInsere");

                baseDados.AddInParameter(cmd, "@Id", DbType.Guid, troca.Id);
                baseDados.AddInParameter(cmd, "@IdRecAtual", DbType.Guid, troca.AlocacaoAtual.Recurso.Id);

                if (troca.AlocacaoAtual.Aula == null)
                {
                    baseDados.AddInParameter(cmd, "@IdEventoAtual", DbType.Guid, troca.AlocacaoAtual.Evento.EventoId);
                    baseDados.AddInParameter(cmd, "@IdAulaAtual", DbType.Guid, DBNull.Value);
                }
                else
                {
                    baseDados.AddInParameter(cmd, "@IdEventoAtual", DbType.Guid, DBNull.Value);
                    baseDados.AddInParameter(cmd, "@IdAulaAtual", DbType.Guid, troca.AlocacaoAtual.Aula.Id);
                }

                baseDados.AddInParameter(cmd, "@IdRecDesejado", DbType.Guid, troca.AlocacaoDesejada.Recurso.Id);

                if (troca.AlocacaoDesejada.Aula == null)
                {
                    baseDados.AddInParameter(cmd, "@IdEventoDesejado", DbType.Guid, troca.AlocacaoDesejada.Evento.EventoId);
                    baseDados.AddInParameter(cmd, "@IdAulaDesejada", DbType.Guid, DBNull.Value);
                }
                else
                {
                    baseDados.AddInParameter(cmd, "@IdEventoDesejado", DbType.Guid, DBNull.Value);
                    baseDados.AddInParameter(cmd, "@IdAulaDesejada", DbType.Guid, troca.AlocacaoDesejada.Aula.Id);
                }

                baseDados.AddInParameter(cmd, "@EstaPendente", DbType.Boolean, troca.EstaPendente);
                baseDados.AddInParameter(cmd, "@FoiAceita", DbType.Boolean, DBNull.Value);
                baseDados.AddInParameter(cmd, "@FoiVisualizada", DbType.Boolean, troca.FoiVisualizada);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, troca.Horario);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, troca.Data);

                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void UpDateTroca(Troca troca)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TrocasUpdate");

                baseDados.AddInParameter(cmd, "@Id", DbType.Guid, troca.Id);
                baseDados.AddInParameter(cmd, "@IdRecAtual", DbType.Guid, troca.AlocacaoAtual.Recurso.Id);
                if (troca.AlocacaoAtual.Aula == null)
                {
                    baseDados.AddInParameter(cmd, "@IdEventoAtual", DbType.Guid, troca.AlocacaoAtual.Evento.EventoId);
                    baseDados.AddInParameter(cmd, "@IdAulaAtual", DbType.Guid, DBNull.Value);
                }
                else
                {
                    baseDados.AddInParameter(cmd, "@IdEventoAtual", DbType.Guid, DBNull.Value);
                    baseDados.AddInParameter(cmd, "@IdAulaAtual", DbType.Guid, troca.AlocacaoAtual.Aula.Id);
                }
                baseDados.AddInParameter(cmd, "@IdRecDesejado", DbType.Guid, troca.AlocacaoDesejada.Recurso.Id);
                if (troca.AlocacaoDesejada.Aula == null)
                {
                    baseDados.AddInParameter(cmd, "@IdEventoDesejado", DbType.Guid, troca.AlocacaoDesejada.Evento.EventoId);
                    baseDados.AddInParameter(cmd, "@IdAulaDesejada", DbType.Guid, DBNull.Value);
                }
                else
                {
                    baseDados.AddInParameter(cmd, "@IdEventoDesejado", DbType.Guid, DBNull.Value);
                    baseDados.AddInParameter(cmd, "@IdAulaDesejada", DbType.Guid, troca.AlocacaoDesejada.Aula.Id);
                }
                baseDados.AddInParameter(cmd, "@EstaPendente", DbType.Boolean, troca.EstaPendente);
                baseDados.AddInParameter(cmd, "@FoiAceita", DbType.Boolean, troca.FoiAceita);
                baseDados.AddInParameter(cmd, "@FoiVisualizada", DbType.Boolean, troca.FoiVisualizada);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, troca.Horario);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, troca.Data);


                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public Troca GetJaPropos(Guid idAtual)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TrocasSelectJaPropos");

                baseDados.AddInParameter(cmd, "@Id", DbType.Guid, idAtual);

                Troca aux = null;
                AlocacaoDAO alocDAO = new AlocacaoDAO();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {

                    while (leitor.Read())
                    {
                        DateTime data = leitor.GetDateTime(leitor.GetOrdinal("Data"));
                        string horario = leitor.GetString(leitor.GetOrdinal("Horario"));
                        Guid id = leitor.GetGuid(leitor.GetOrdinal("Id"));
                        Alocacao alocAtual = alocDAO.GetAlocacao(leitor.GetGuid(leitor.GetOrdinal("IdRecAtual")), data, horario);
                        Alocacao alocDesejada = alocDAO.GetAlocacao(leitor.GetGuid(leitor.GetOrdinal("IdRecDesejado")), data, horario);
                        bool estaPendente = leitor.GetBoolean(leitor.GetOrdinal("EstaPendente"));
                        

                        bool? foiAceita = leitor["FoiAceita"] as bool?;
                        if (foiAceita.HasValue)
                            foiAceita = foiAceita.Value;
                        else foiAceita = null;

                        bool foiVisualizada = leitor.GetBoolean(leitor.GetOrdinal("FoiVisualizada"));
                        aux = new Troca(id, alocAtual, alocDesejada, foiAceita, estaPendente, foiVisualizada, horario, data);

                    }
                }


                return aux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<Troca> GetTrocasAulasByProfessor(Guid profId, Calendario cal)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TrocasAulasSelectByProfessor");

                baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, profId);
                baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);

                List<Troca> aux = new List<Troca>();
                Troca troca = null;
                AlocacaoDAO alocDAO = new AlocacaoDAO();
                RecursosDAO recDAO = new RecursosDAO();
                EventoDAO eventoDAO = new EventoDAO();
                AulasDAO aulaDAO = new AulasDAO();


                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {

                    while (leitor.Read())
                    {

                        Guid trocaId = leitor.GetGuid(leitor.GetOrdinal("Id"));

                        Recurso recAtual = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("IdRecAtual")));
                        Recurso recDesejado = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("IdRecDesejado")));

                        string horario = leitor.GetString(leitor.GetOrdinal("Horario"));
                        DateTime data = leitor.GetDateTime(leitor.GetOrdinal("Data"));

                        Aula aulaAtual = null;
                        Aula aulaDesejada = null;
                        Evento eventoAtual = null;
                        Evento eventoDesejado = null;

                        Guid? IdAulaAtual = leitor["IdAulaAtual"] as Guid?;
                        if (IdAulaAtual.HasValue)
                            aulaAtual = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("IdAulaAtual")));

                        Guid? IdAulaDesejada = leitor["IdAulaDesejada"] as Guid?;
                        if (IdAulaDesejada.HasValue)
                            aulaDesejada = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("IdAulaDesejada")));

                        Guid? IdEventoAtual = leitor["IdEventoAtual"] as Guid?;
                        if (IdEventoAtual.HasValue)
                            eventoAtual = eventoDAO.GetEvento(IdEventoAtual);

                        Guid? IdEventoDesejado = leitor["IdEventoDesejado"] as Guid?;
                        if (IdEventoDesejado.HasValue)
                            eventoDesejado = eventoDAO.GetEvento(IdEventoDesejado);

                        Alocacao alocAtual = new Alocacao(recAtual, data, horario, aulaAtual, eventoAtual);
                        Alocacao alocDesejada = new Alocacao(recDesejado, data, horario, aulaDesejada, eventoDesejado);

                        bool estaPendente = leitor.GetBoolean(leitor.GetOrdinal("EstaPendente"));

                        bool? foiAceita = leitor["FoiAceita"] as bool?;
                        if (foiAceita.HasValue)
                            foiAceita = foiAceita.Value;
                        else foiAceita = null;

                        bool foiVisualizada = leitor.GetBoolean(leitor.GetOrdinal("FoiVisualizada"));

                        troca = new Troca(trocaId, alocAtual, alocDesejada, foiAceita, estaPendente, foiVisualizada, horario, data);

                        aux.Add(troca);
                    }
                }


                return aux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<Troca> GetNaoVisualizadasByAula(Guid aulaId, DateTime data, string horario)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TrocasSelectNaoVisualizadasByAula");
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, horario);
                baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, aulaId);

                List<Troca> aux = new List<Troca>();
                Troca troca = null;

                AlocacaoDAO alocDAO = new AlocacaoDAO();
                RecursosDAO recDAO = new RecursosDAO();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {

                    while (leitor.Read())
                    {

                        Guid trocaId = leitor.GetGuid(leitor.GetOrdinal("Id"));

                        Recurso recAtual = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("IdRecAtual")));
                        Recurso recDesejado = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("IdRecDesejado")));

                        Alocacao alocAtual = alocDAO.GetAlocacao(leitor.GetGuid(leitor.GetOrdinal("IdRecAtual")), data, horario);
                        Alocacao alocDesejada = alocDAO.GetAlocacao(leitor.GetGuid(leitor.GetOrdinal("IdRecDesejado")), data, horario);

                        bool estaPendente = leitor.GetBoolean(leitor.GetOrdinal("EstaPendente"));

                        bool? foiAceita = leitor["FoiAceita"] as bool?;
                        if (foiAceita.HasValue)
                            foiAceita = foiAceita.Value;
                        else foiAceita = null;

                        bool foiVisualizada = leitor.GetBoolean(leitor.GetOrdinal("FoiVisualizada"));

                        troca = new Troca(trocaId, alocAtual, alocDesejada, foiAceita, estaPendente, foiVisualizada, horario, data);
                        aux.Add(troca);
                    }
                }


                return aux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<Troca> GetTrocasEventosByAutor(Guid autorid, Calendario cal)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TrocasEventosSelectByAutor");

                baseDados.AddInParameter(cmd, "@AutorId", DbType.Guid, autorid);
                baseDados.AddInParameter(cmd, "@CalendarioId", DbType.Guid, cal.Id);

                List<Troca> aux = new List<Troca>();
                Troca troca = null;
                AulasDAO aulaDAO = new AulasDAO();
                AlocacaoDAO alocDAO = new AlocacaoDAO();
                RecursosDAO recDAO = new RecursosDAO();
                EventoDAO eventoDAO = new EventoDAO();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {

                    while (leitor.Read())
                    {

                        Guid trocaId = leitor.GetGuid(leitor.GetOrdinal("Id"));

                        Recurso recAtual = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("IdRecAtual")));
                        Recurso recDesejado = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("IdRecDesejado")));

                        string horario = leitor.GetString(leitor.GetOrdinal("Horario"));
                        DateTime data = leitor.GetDateTime(leitor.GetOrdinal("Data"));

                        Aula aulaAtual = null;
                        Aula aulaDesejada = null;
                        Evento eventoAtual = null;
                        Evento eventoDesejado = null;

                        Guid? IdAulaAtual = leitor["IdAulaAtual"] as Guid?;
                        if (IdAulaAtual.HasValue)
                            aulaAtual = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("IdAulaAtual")));

                        Guid? IdAulaDesejada = leitor["IdAulaDesejada"] as Guid?;
                        if (IdAulaDesejada.HasValue)
                            aulaDesejada = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("IdAulaDesejada")));

                        Guid? IdEventoAtual = leitor["IdEventoAtual"] as Guid?;
                        if (IdEventoAtual.HasValue)
                            eventoAtual = eventoDAO.GetEvento(IdEventoAtual);

                        Guid? IdEventoDesejado = leitor["IdEventoDesejado"] as Guid?;
                        if (IdEventoDesejado.HasValue)
                            eventoDesejado = eventoDAO.GetEvento(IdEventoDesejado);

                        Alocacao alocAtual = new Alocacao(recAtual, data, horario, aulaAtual, eventoAtual);
                        Alocacao alocDesejada = new Alocacao(recDesejado, data, horario, aulaDesejada, eventoDesejado);

                        bool estaPendente = leitor.GetBoolean(leitor.GetOrdinal("EstaPendente"));

                        bool? foiAceita = leitor["FoiAceita"] as bool?;
                        if (foiAceita.HasValue)
                            foiAceita = foiAceita.Value;
                        else foiAceita = null;

                        bool foiVisualizada = leitor.GetBoolean(leitor.GetOrdinal("FoiVisualizada"));

                        troca = new Troca(trocaId, alocAtual, alocDesejada, foiAceita, estaPendente, foiVisualizada, horario, data);

                        aux.Add(troca);
                    }
                }


                return aux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<Troca> GetNaoVisualizadasByEvento(Guid eventoId, DateTime data, string horario)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TrocasSelectNaoVisualizadasByEvento");
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, horario);
                baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, eventoId);

                List<Troca> aux = new List<Troca>();
                Troca troca = null;

                AlocacaoDAO alocDAO = new AlocacaoDAO();
                RecursosDAO recDAO = new RecursosDAO();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {

                    while (leitor.Read())
                    {

                        Guid trocaId = leitor.GetGuid(leitor.GetOrdinal("Id"));

                        Recurso recAtual = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("IdRecAtual")));
                        Recurso recDesejado = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("IdRecDesejado")));

                        Alocacao alocAtual = alocDAO.GetAlocacao(leitor.GetGuid(leitor.GetOrdinal("IdRecAtual")), data, horario);
                        Alocacao alocDesejada = alocDAO.GetAlocacao(leitor.GetGuid(leitor.GetOrdinal("IdRecDesejado")), data, horario);

                        bool estaPendente = leitor.GetBoolean(leitor.GetOrdinal("EstaPendente"));

                        bool? foiAceita = leitor["FoiAceita"] as bool?;
                        if (foiAceita.HasValue)
                            foiAceita = foiAceita.Value;
                        else foiAceita = null;

                        bool foiVisualizada = leitor.GetBoolean(leitor.GetOrdinal("FoiVisualizada"));

                        troca = new Troca(trocaId, alocAtual, alocDesejada, foiAceita, estaPendente, foiVisualizada, horario, data);
                        aux.Add(troca);
                    }
                }


                return aux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
    }
}