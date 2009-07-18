using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using BusinessData.Entities;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using BusinessData.BusinessLogic;

namespace BusinessData.DataAccess
{
    public class TransferenciaDAO
    {
        private Database baseDados;

        public TransferenciaDAO()
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

        public void InsereTransferencia(Transferencia trans)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TransfereciaInsert");

                baseDados.AddInParameter(cmd, "@Id", DbType.Guid, trans.Id);
                baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, trans.Recurso.Id);
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, trans.Data);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, trans.Horario);

                if (trans.TurmaRecebeu != null)
                baseDados.AddInParameter(cmd, "@TurmaRecebeu", DbType.Guid, trans.TurmaRecebeu.Id);
                else
                baseDados.AddInParameter(cmd, "@TurmaRecebeu", DbType.Guid, DBNull.Value);

                if (trans.TurmaTransferiu != null)
                baseDados.AddInParameter(cmd, "@TurmaTransferiu", DbType.Guid, trans.TurmaTransferiu.Id);
                else
                baseDados.AddInParameter(cmd, "@TurmaTransferiu", DbType.Guid, DBNull.Value);

                if (trans.EventoRecebeu != null)
                baseDados.AddInParameter(cmd, "@EventoRecebeu", DbType.Guid, trans.EventoRecebeu.EventoId);
                else
                baseDados.AddInParameter(cmd, "@EventoRecebeu", DbType.Guid, DBNull.Value);

                if (trans.EventoTransferiu != null)
                baseDados.AddInParameter(cmd, "@EventoTransferiu", DbType.Guid, trans.EventoTransferiu.EventoId);
                else
                baseDados.AddInParameter(cmd, "@EventoTransferiu", DbType.Guid, DBNull.Value);

                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<Transferencia> GetTransferencias(Guid profId, Calendario cal)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TranferenciaSelectByProfessor");

                baseDados.AddInParameter(cmd, "@ProfessorId", DbType.Guid, profId);

                List<Transferencia> aux = new List<Transferencia>();
                Transferencia trans = null;
                TurmaDAO turmaDAO = new TurmaDAO();
                RecursosDAO recDAO = new RecursosDAO();
                EventoDAO eventoDAO = new EventoDAO();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {

                    while (leitor.Read())
                    {

                        Guid transId = leitor.GetGuid(leitor.GetOrdinal("Id"));

                        Recurso rec = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                        string hora = leitor.GetString(leitor.GetOrdinal("Horario"));
                        DateTime data = leitor.GetDateTime(leitor.GetOrdinal("Data"));
                        
                        Turma turmaRecebeu;
                        Turma turmaTransferiu;
                        Evento eventoRecebeu; 
                        Evento eventoTransferiu;

                        Guid? turmaRecId = leitor["TurmaRecebeu"] as Guid?;
                        if (turmaRecId.HasValue)
                            turmaRecebeu = turmaDAO.GetTurma(turmaRecId.Value,cal);
                        else turmaRecebeu = null;

                        Guid? turmaTransId = leitor["TurmaTransferiu"] as Guid?;
                        if (turmaTransId.HasValue)
                            turmaTransferiu = turmaDAO.GetTurma(turmaTransId.Value,cal);
                        else turmaTransferiu = null;
 
                        Guid? eventoRecId = leitor["EventoRecebeu"] as Guid?;
                        if (eventoRecId.HasValue)
                            eventoRecebeu = eventoDAO.GetEvento(eventoRecId.Value);
                        else eventoRecebeu = null;

                        Guid? eventoTransId = leitor["EventoTransferiu"] as Guid?;
                        if (eventoTransId.HasValue)
                            eventoTransferiu = eventoDAO.GetEvento(eventoTransId.Value);
                        else eventoTransferiu = null;

                        bool foiVisualizada = leitor.GetBoolean(leitor.GetOrdinal("FoiVisualizada"));
                        trans = new Transferencia(transId, rec, data, hora, turmaRecebeu, turmaTransferiu, foiVisualizada, eventoRecebeu, eventoTransferiu);

                        aux.Add(trans);
                    }
                }


                return aux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public Transferencia GetTransferencia(Guid id, Calendario cal)
        { 
        try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TransferenciaSelectById");

                baseDados.AddInParameter(cmd, "@Id", DbType.Guid, id);

                
                Transferencia trans = null;
                TurmaDAO turmaDAO = new TurmaDAO();
                RecursosDAO recDAO = new RecursosDAO();
                EventoDAO eventoDAO = new EventoDAO();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();
                    Guid transId = leitor.GetGuid(leitor.GetOrdinal("Id"));

                    Recurso rec = recDAO.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")));

                    string hora = leitor.GetString(leitor.GetOrdinal("Horario"));
                    DateTime data = leitor.GetDateTime(leitor.GetOrdinal("Data"));

                    Turma turmaRecebeu = null;
                    Turma turmaTransferiu = null;
                    Evento eventoRecebeu = null;
                    Evento eventoTransferiu = null;

                    Guid? turmaRecId = leitor["TurmaRecebeu"] as Guid?;
                    if (turmaRecId.HasValue)
                        turmaRecebeu = turmaDAO.GetTurma(turmaRecId.Value, cal);
                    else turmaRecebeu = null;

                    Guid? turmaTransId = leitor["TurmaTransferiu"] as Guid?;
                    if (turmaTransId.HasValue)
                        turmaTransferiu = turmaDAO.GetTurma(turmaTransId.Value, cal);
                    else eventoTransferiu = null;

                    Guid? eventoRecId = leitor["EventoRecebeu"] as Guid?;
                    if (eventoRecId.HasValue)
                        eventoRecebeu = eventoDAO.GetEvento(eventoRecId.Value);
                    else eventoRecebeu = null;

                    Guid? eventoTransId = leitor["EventoTransferiu"] as Guid?;
                    if (eventoTransId.HasValue)
                        eventoTransferiu = eventoDAO.GetEvento(eventoTransId.Value);
                    else eventoTransferiu = null;

                    bool foiVisualizada = leitor.GetBoolean(leitor.GetOrdinal("FoiVisualizada"));

                    trans = new Transferencia(transId, rec, data, hora, turmaRecebeu, turmaTransferiu, foiVisualizada, eventoRecebeu, eventoTransferiu);


                }
                return trans;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        
        public void TransferenciaUpdate(Transferencia trans)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("TransferenciaUpdate");

                baseDados.AddInParameter(cmd, "@Id", DbType.Guid, trans.Id);
                baseDados.AddInParameter(cmd, "@FoiVisualizada", DbType.Boolean, trans.FoiVisualizada);


                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<PessoaBase> GetResponsaveisByDataHora(string horario, DateTime data, Guid requerente)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("GetResponsaveisByDataHora");
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
            baseDados.AddInParameter(cmd, "@Horario", DbType.String, horario);

            PessoaBase aux;
            List<PessoaBase> listaAux = new List<PessoaBase>();
            PessoaFactory pFac = PessoaFactory.GetInstance();
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        aux = pFac.CreatePessoa(leitor.GetGuid(leitor.GetOrdinal("Id")));
                      
                        
                            if (aux.Id != requerente)
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
    }

}