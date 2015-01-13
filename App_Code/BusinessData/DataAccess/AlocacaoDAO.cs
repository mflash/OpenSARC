using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using BusinessData.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;

namespace BusinessData.DataAccess
{
    public class AlocacaoDAO
    {
        private Database baseDados;

        public AlocacaoDAO()
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

        public void InsereAlocacao(Alocacao alocacao)
        {

            if ((alocacao.Aula != null) && (alocacao.Evento != null))
                throw new Exception("Dados Inválidos!");
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoInsere");

                baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, alocacao.Recurso.Id);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, alocacao.Data);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, alocacao.Horario);
                if (alocacao.Aula != null)
                {
                    baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, alocacao.Aula.Id);
                    baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, DBNull.Value);
                }
                else if (alocacao.Evento != null)
                {
                    baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, DBNull.Value);
                    baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, alocacao.Evento.EventoId);
                }
                else
                {
                    baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, DBNull.Value);
                    baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, DBNull.Value);
                }
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }

        }

        public void DeletaAlocacao(Alocacao alocacao)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoDeleta");

                baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, alocacao.Recurso.Id);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, alocacao.Data);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, alocacao.Horario);

                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }

        }

        public void UpdateAlocacao(Alocacao alocacao)
        {
            // a, b, c, d, e, x, f, g, h, i, j, k, l, m, n, p

            //string aux = alocacao.Horario;
            //for (char c = aux[0]; c <= aux[aux.Length-1]; c++)
            foreach(char c in alocacao.Horario)
            {
                if ((alocacao.Aula != null) && (alocacao.Evento != null))
                    throw new Exception("Dados Inválidos!");
                if (c != 'o')
                {
                    try
                    {
                        DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoUpdate");

                        baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, alocacao.Recurso.Id);
                        baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, alocacao.Data);
                        baseDados.AddInParameter(cmd, "@Horario", DbType.String, c);
                        if (alocacao.Aula != null)
                        {
                            baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, alocacao.Aula.Id);
                            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, DBNull.Value);
                        }
                        else if (alocacao.Evento != null)
                        {
                            baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, DBNull.Value);
                            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, alocacao.Evento.EventoId);
                        }
                        else
                        {
                            baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, DBNull.Value);
                            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, DBNull.Value);
                        }

                        baseDados.ExecuteNonQuery(cmd);

                    }

                    catch (SqlException ex)
                    {
                        throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
                    }
                }
            }
        }

        public Alocacao GetAlocacao(Guid recursoid, DateTime data, string horario)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelect");
                baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recursoid);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, horario);

                Alocacao aux = null;

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();
                    
                    RecursosDAO recursoDao = new RecursosDAO();
                    Recurso recurso = recursoDao.GetRecurso(recursoid);

                    AulasDAO aulaDao = new AulasDAO();
                    Aula aula;

                    Guid? aulaId = leitor["AulaId"] as Guid?;
                    if (aulaId.HasValue)
                        aula = aulaDao.GetAula(aulaId.Value);
                    else aula = null;

                    EventoDAO eventoDao = new EventoDAO();
                    Evento evento;                  

                    Guid? eventoID = leitor["EventoId"] as Guid?;
                    if (eventoID.HasValue)
                        evento = eventoDao.GetEvento(eventoID.Value);
                    else evento = null;



                    aux = new Alocacao(recurso, leitor.GetDateTime(leitor.GetOrdinal("Data")), leitor.GetString(leitor.GetOrdinal("Horario")), aula, evento);
                }
                return aux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<Alocacao> GetRecursosAlocados(DateTime data, string hora)
        {
            try
            {
                List<Alocacao> aux = new List<Alocacao>();

                DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectAlocados");
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
                baseDados.AddInParameter(cmd, "@Hora", DbType.String, hora);

                RecursosDAO recursoDao = new RecursosDAO();
                EventoDAO eventoDao = new EventoDAO();
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        
                        Recurso recurso = recursoDao.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                        AulasDAO aulaDao = new AulasDAO();
                        Aula aula;
                        Evento evento;

                        Guid? aulaId = leitor["AulaId"] as Guid?;
                        if (aulaId.HasValue)
                            aula = aulaDao.GetAula(aulaId.Value);
                        else aula = null;

                        Guid? eventoID = leitor["EventoId"] as Guid?;
                        if (eventoID.HasValue)
                            evento = eventoDao.GetEvento(eventoID.Value);
                        else evento = null;

                        Alocacao aloc = new Alocacao(recurso,leitor.GetDateTime(leitor.GetOrdinal("Data")),hora, aula, evento);

                        aux.Add(aloc);
                    }
                }

                return aux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }

        }

        public List<Recurso> GetRecursoAlocadoByAula(DateTime data, string hora, Guid aulaId)
        {
            try
            {
                
                    DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectByAula");
                    baseDados.AddInParameter(cmd, "@Aula", DbType.Guid, aulaId);
                    baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);                   
                    baseDados.AddInParameter(cmd, "@Horario", DbType.String, hora);

                    List<Recurso> listaRecursos = new List<Recurso>();
                    Recurso aux;
                    RecursosDAO RecDAO = new RecursosDAO(); 
                    using (RefCountingDataReader leitor = (RefCountingDataReader) baseDados.ExecuteReader(cmd))
                    {
                        while (leitor.Read())
                        {
                            if (((SqlDataReader)leitor.InnerReader).HasRows)
                            {
                                aux = new Recurso();
                                CategoriaRecurso catRec = new CategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaId")), leitor.GetString(leitor.GetOrdinal("CatDescricao")));
                                aux.Categoria = catRec;
                                aux.Descricao = leitor.GetString(leitor.GetOrdinal("Descricao"));
                                aux.EstaDisponivel = leitor.GetBoolean(leitor.GetOrdinal("EstaDisponivel"));
                                List<HorarioBloqueado> listaHB = RecDAO.GetHorarioBloqueadoByRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));
                                aux.HorariosBloqueados = listaHB;
                                aux.Id = leitor.GetGuid(leitor.GetOrdinal("RecursoId"));
                                Faculdade facul = Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("FaculdadeId")), leitor.GetString(leitor.GetOrdinal("FaculdadeNome")));
                                aux.Vinculo = facul;

                                listaRecursos.Add(aux);
                            }
                        }
                    }

                    return listaRecursos;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<Alocacao> GetAlocacoes(Calendario cal, DateTime data, Professor prof)
        {
            List<Alocacao> lista = new List<Alocacao>();

            DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectByProfessor");
            baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, prof.Id);
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);

            AulasDAO aulaDAO = new AulasDAO();
            EventoDAO eventoDAO = new EventoDAO();
            RecursosDAO recDAO = new RecursosDAO();

            using (IDataReader leitor = baseDados.ExecuteReader(cmd))
            {
                Alocacao aloc;

                while (leitor.Read())
                {
                    Aula au = null;
                    Recurso rec = null;
                    Evento evento = null;

                    rec = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                    Guid? aulaId = leitor["AulaId"] as Guid?;
                    if (aulaId.HasValue)
                        au = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")));

                    Guid? eventoId = leitor["EventoId"] as Guid?;
                    if (eventoId.HasValue)
                        evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));

                    string hora = (string)leitor["Hora"];
                    
                    aloc = new Alocacao(rec, data, hora, au, evento);

                    lista.Add(aloc);
                }
            }
            return lista;
        }

        public List<Alocacao> GetAlocacoes(Calendario cal,DateTime data, Guid recursoId)
        {
            List<Alocacao> lista = new List<Alocacao>();

            DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectByRecurso");
            baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recursoId);
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);

            AulasDAO aulaDAO = new AulasDAO();
            EventoDAO eventoDAO = new EventoDAO();
            RecursosDAO recDAO = new RecursosDAO();

            using (IDataReader leitor = baseDados.ExecuteReader(cmd))
            {
                
                Alocacao aloc;

                while (leitor.Read())
                {
                    Aula au = null;
                    Recurso rec = null;
                    Evento evento = null;

                    rec = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                    Guid? aulaId = leitor["AulaId"] as Guid?;
                    if (aulaId.HasValue)
                        au = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")));

                    Guid? eventoId = leitor["EventoId"] as Guid?;
                    if (eventoId.HasValue)
                        evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));

                    string hora = (string)leitor["Hora"];

                    aloc = new Alocacao(rec, data, hora, au, evento);

                    lista.Add(aloc);
                }
            }
            return lista;
        }
        
        public List<Alocacao> GetAlocacoes(Calendario cal, DateTime data, Secretario secretario)
        {
            List<Alocacao> lista = new List<Alocacao>();

            DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectBySecretario");
            baseDados.AddInParameter(cmd, "@SecretarioId", DbType.Guid, secretario.Id);
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);

            AulasDAO aulaDAO = new AulasDAO();
            EventoDAO eventoDAO = new EventoDAO();
            RecursosDAO recDAO = new RecursosDAO();

            using (IDataReader leitor = baseDados.ExecuteReader(cmd))
            {

                Alocacao aloc;

                while (leitor.Read())
                {
                    Aula au = null;
                    Recurso rec = null;
                    Evento evento = null;

                    rec = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                    Guid? aulaId = leitor["AulaId"] as Guid?;
                    if (aulaId.HasValue)
                        au = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")));

                    Guid? eventoId = leitor["EventoId"] as Guid?;
                    if (eventoId.HasValue)
                        evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));

                    string hora = (string)leitor["Hora"];

                    aloc = new Alocacao(rec, data, hora, au, evento);

                    lista.Add(aloc);
                }
            }
            return lista;
        }

        public List<Alocacao> GetAlocacoesSemData(Calendario cal, Guid recursoId)
        {
            List<Alocacao> lista = new List<Alocacao>();
            
            DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectByRecursoSemData");
            baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recursoId);            
            baseDados.AddInParameter(cmd, "@DataInicio", DbType.DateTime, cal.InicioG1);
            baseDados.AddInParameter(cmd, "@DataFim", DbType.DateTime, cal.FimG2);

            AulasDAO aulaDAO = new AulasDAO();
            EventoDAO eventoDAO = new EventoDAO();
            RecursosDAO recDAO = new RecursosDAO();

            using (IDataReader leitor = baseDados.ExecuteReader(cmd))
            {

                Alocacao aloc;

                while (leitor.Read())
                {
                    Aula au = null;
                    Recurso rec = null;
                    Evento evento = null;
                    DateTime dateTime = new DateTime();

                    rec = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                    Guid? aulaId = leitor["AulaId"] as Guid?;
                    if (aulaId.HasValue)
                        au = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")));

                    Guid? eventoId = leitor["EventoId"] as Guid?;
                    if (eventoId.HasValue)
                        evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));

                    string hora = (string)leitor["Hora"];

                    dateTime = (DateTime)leitor["Data"];

                    aloc = new Alocacao(rec, dateTime, hora, au, evento);

                    lista.Add(aloc);
                }
            }
            return lista;
        }

        public List<Alocacao> GetAlocacoesSemData(Calendario cal, Professor prof)
        {
            List<Alocacao> lista = new List<Alocacao>();

            DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectByProfessorSemData");
            baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, prof.Id);
            baseDados.AddInParameter(cmd, "@DataInicio", DbType.DateTime, cal.InicioG1);
            baseDados.AddInParameter(cmd, "@DataFim", DbType.DateTime, cal.FimG2);

            AulasDAO aulaDAO = new AulasDAO();
            EventoDAO eventoDAO = new EventoDAO();
            RecursosDAO recDAO = new RecursosDAO();

            using (IDataReader leitor = baseDados.ExecuteReader(cmd))
            {

                Alocacao aloc;

                while (leitor.Read())
                {
                    Aula au = null;
                    Recurso rec = null;
                    Evento evento = null;
                    DateTime dateTime = new DateTime();

                    rec = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                    Guid? aulaId = leitor["AulaId"] as Guid?;
                    if (aulaId.HasValue)
                        au = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")));

                    Guid? eventoId = leitor["EventoId"] as Guid?;
                    if (eventoId.HasValue)
                        evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));

                    string hora = (string)leitor["Hora"];

                    dateTime = (DateTime)leitor["Data"];

                    aloc = new Alocacao(rec, dateTime, hora, au, evento);

                    lista.Add(aloc);
                }
            }
            return lista;
        }

        public List<Alocacao> GetAlocacoesSemData(Calendario cal, Secretario secretario)
        {
            List<Alocacao> lista = new List<Alocacao>();

            DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectByProfessorSemData");
            baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, secretario.Id);
            baseDados.AddInParameter(cmd, "@DataInicio", DbType.DateTime, cal.InicioG1);
            baseDados.AddInParameter(cmd, "@DataFim", DbType.DateTime, cal.FimG2);

            AulasDAO aulaDAO = new AulasDAO();
            EventoDAO eventoDAO = new EventoDAO();
            RecursosDAO recDAO = new RecursosDAO();

            using (IDataReader leitor = baseDados.ExecuteReader(cmd))
            {

                Alocacao aloc;

                while (leitor.Read())
                {
                    Aula au = null;
                    Recurso rec = null;
                    Evento evento = null;
                    DateTime dateTime = new DateTime();

                    rec = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                    Guid? aulaId = leitor["AulaId"] as Guid?;
                    if (aulaId.HasValue)
                        au = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")));

                    Guid? eventoId = leitor["EventoId"] as Guid?;
                    if (eventoId.HasValue)
                        evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));

                    string hora = (string)leitor["Hora"];

                    dateTime = (DateTime)leitor["Data"];

                    aloc = new Alocacao(rec, dateTime, hora, au, evento);

                    lista.Add(aloc);
                }
            }
            return lista;
        }

        public List<Alocacao> GetAlocacoesByData(DateTime data, Calendario cal)
        {
            List<Alocacao> lista = new List<Alocacao>();

            DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectByData");
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);

            AulasDAO aulaDAO = new AulasDAO();
            EventoDAO eventoDAO = new EventoDAO();
            RecursosDAO recDAO = new RecursosDAO();

            using (IDataReader leitor = baseDados.ExecuteReader(cmd))
            {
                Alocacao aloc;

                while (leitor.Read())
                {
                    Aula au = null;
                    Recurso rec = null;
                    Evento evento = null;

                    rec = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                    Guid? aulaId = leitor["AulaId"] as Guid?;
                    if (aulaId.HasValue)
                        au = aulaDAO.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")));
                    
                    Guid? eventoId = leitor["EventoId"] as Guid?;
                    if (eventoId.HasValue)
                        evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));
                    
                    string hora = (string)leitor["Hora"];

                    aloc = new Alocacao(rec, data,hora,au,evento);

                    lista.Add(aloc);
                }
            }
            return lista;
        }

        public List<Recurso> GetRecursoAlocadoByEvento(DateTime data, string hora, Guid eventoId)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("AlocacaoSelectByEvento");
                baseDados.AddInParameter(cmd, "@Evento", DbType.Guid, eventoId);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, hora);

                List<Recurso> listaRecursos = new List<Recurso>();
                Recurso aux;
                RecursosDAO RecDAO = new RecursosDAO();
                FaculdadesDAO faculDAO = new FaculdadesDAO();

                using (RefCountingDataReader leitor = (RefCountingDataReader)baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        if (((SqlDataReader)leitor.InnerReader).HasRows)
                        {
                            aux = new Recurso();
                            CategoriaRecurso catRec = new CategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaId")), leitor.GetString(leitor.GetOrdinal("CatDescricao")));
                            aux.Categoria = catRec;
                            aux.Descricao = leitor.GetString(leitor.GetOrdinal("Descricao"));
                            aux.EstaDisponivel = leitor.GetBoolean(leitor.GetOrdinal("EstaDisponivel"));
                            
                            List<HorarioBloqueado> listaHB = RecDAO.GetHorarioBloqueadoByRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));
                            aux.HorariosBloqueados = listaHB;
                            aux.Id = leitor.GetGuid(leitor.GetOrdinal("RecursoId"));

                            Faculdade facul = faculDAO.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("Vinculo")));
                            aux.Vinculo = facul;

                            listaRecursos.Add(aux);
                        }
                    }
                }

                return listaRecursos;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }


    }
    
}
