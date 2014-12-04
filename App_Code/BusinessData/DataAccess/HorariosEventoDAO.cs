using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using BusinessData.Entities;

namespace BusinessData.DataAccess
{
    internal class HorariosEventoDAO
    {
        private Database baseDados;

        public HorariosEventoDAO()
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

        public void InsereHorariosEvento(Entities.HorariosEvento horariosEvento)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorariosEventoInsere");
            baseDados.AddInParameter(cmd, "@HorariosEventoId", DbType.Guid, horariosEvento.HorariosEventoId);
            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, horariosEvento.EventoId.EventoId);
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, horariosEvento.Data);
            baseDados.AddInParameter(cmd, "@HorarioInicio", DbType.String, horariosEvento.HorarioInicio);
            baseDados.AddInParameter(cmd, "@HorarioFim", DbType.String, horariosEvento.HorarioFim);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void RemoveHorariosEvento(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorariosEventoDelete");
            baseDados.AddInParameter(cmd, "@HorariosEventoId", DbType.Guid, id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public Entities.HorariosEvento GetHorariosEvento(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorariosEventoSelectById");
            baseDados.AddInParameter(cmd, "@HorariosEventoId", DbType.Guid, id);
            Entities.HorariosEvento aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();
                    EventoDAO eventoDAO = new EventoDAO();
                    Entities.Evento evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));
                    aux = Entities.HorariosEvento.GetHorariosEvento(leitor.GetGuid(leitor.GetOrdinal("HorariosEventoId")),
                                                evento,
                                                leitor.GetDateTime(leitor.GetOrdinal("Data")),
                                                leitor.GetString(leitor.GetOrdinal("HorarioInicio")),
                                                leitor.GetString(leitor.GetOrdinal("HorarioFim")));
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }

        public List<HorariosEvento> GetHorariosEventosById(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorariosEventosSelectById");
            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, id);
            List<Entities.HorariosEvento> listaAux = new List<BusinessData.Entities.HorariosEvento>();
            Entities.HorariosEvento aux;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    EventoDAO eventoDAO = new EventoDAO();
                    Entities.Evento evento;
                    while (leitor.Read())
                    {
                        try
                        {
                           evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));
                        }
                        catch (InvalidOperationException)
                        {
                            evento = null;
                        }
                        aux = Entities.HorariosEvento.GetHorariosEvento(leitor.GetGuid(leitor.GetOrdinal("HorariosEventoId")),
                                                    evento,
                                                    leitor.GetDateTime(leitor.GetOrdinal("Data")),
                                                    leitor.GetString(leitor.GetOrdinal("HorarioInicio")),
                                                    leitor.GetString(leitor.GetOrdinal("HorarioFim")));
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

        public List<HorariosEvento> GetHorariosEventos()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorariosEventosSelect");
            List<Entities.HorariosEvento> listaAux = new List<BusinessData.Entities.HorariosEvento>();
            Entities.HorariosEvento aux;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    EventoDAO eventoDAO = new EventoDAO();
                    Entities.Evento evento;
                    while (leitor.Read())
                    {
                        try
                        {
                           evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));
                        }
                        catch (InvalidOperationException)
                        {
                            evento = null;
                        }
                        aux = Entities.HorariosEvento.GetHorariosEvento(leitor.GetGuid(leitor.GetOrdinal("HorariosEventoId")),
                                                    evento,
                                                    leitor.GetDateTime(leitor.GetOrdinal("Data")),
                                                    leitor.GetString(leitor.GetOrdinal("HorarioInicio")),
                                                    leitor.GetString(leitor.GetOrdinal("HorarioFim")));
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

        public List<HorariosEvento> GetHorariosEventosByIdDetalhados(Guid eventoId)
        {
            try
            {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorariosEventosSelectById");
            baseDados.AddInParameter(cmd, "@EventoId", DbType.Guid, eventoId);

            List<HorariosEvento> listaAux = new List<HorariosEvento>();
            HorariosEvento aux;

//            SqlDataReader sqlReader;
 //           return listaAux;
                using (RefCountingDataReader leitor = ((RefCountingDataReader)baseDados.ExecuteReader(cmd)))
                {
                    EventoDAO eventoDAO = new EventoDAO();
                    Evento evento;
                    while (leitor.Read())
                    {
                        if (((SqlDataReader)leitor.InnerReader).HasRows)
                        {
                            try
                            {
                                evento = eventoDAO.GetEvento(leitor.GetGuid(leitor.GetOrdinal("EventoId")));
                            }
                            catch (InvalidOperationException)
                            {
                                evento = null;
                            }
                            aux = HorariosEvento.GetHorariosEvento(leitor.GetGuid(leitor.GetOrdinal("HorariosEventoId")),
                                                        evento,
                                                        leitor.GetDateTime(leitor.GetOrdinal("Data")),
                                                        leitor.GetString(leitor.GetOrdinal("HorarioInicio")),
                                                        leitor.GetString(leitor.GetOrdinal("HorarioFim")));
                            listaAux.Add(aux);
                        }
                    }
                }
                if (listaAux != null)
                {
                    List<HorariosEvento> listaFinal = new List<HorariosEvento>();
                    HorariosEvento heAux = null;
                    foreach (HorariosEvento he in listaAux)
                    {
                        // A princípio, este ajuste não é mais necessário
                        //if (he.HorarioInicio == "E ")
                        //    he.HorarioInicio = "EE";
                        //if (he.HorarioFim == "E ")
                        //    he.HorarioFim = "EE";
                        string[] lista = HorariosEvento.HorariosEntre(he.HorarioInicio, he.HorarioFim);
                        foreach (string s in lista)
                        {
                            heAux = (HorariosEvento)he.Clone();
                            heAux.HorarioInicio = s;
                            listaFinal.Add(heAux);
                        }
                    }
                    return listaFinal;
                }
                else
                    return listaAux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
    
    
    }
}
