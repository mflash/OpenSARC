using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using BusinessData.Entities;
using System.Data.SqlClient;
using System.Web;

namespace BusinessData.DataAccess
{
    internal class AulasDAO
    {
        private Database baseDados;

        public AulasDAO()
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

        public void InsereAula(Aula aula)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("AulasInsere");
            baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, aula.Id);
            baseDados.AddInParameter(cmd, "@TurmaId", DbType.Guid, aula.TurmaId.Id);
            baseDados.AddInParameter(cmd, "@Hora", DbType.String, aula.Hora);
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, aula.Data);
            baseDados.AddInParameter(cmd, "@DescricaoAtividade", DbType.String, aula.DescricaoAtividade);
            baseDados.AddInParameter(cmd, "@CategoriaAtividadeId", DbType.Guid, aula.CategoriaAtividade.Id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void RemoveAula(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("AulasDelete");
            baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void AtualizaAula(Aula aula)
        {

            DbCommand cmd = baseDados.GetStoredProcCommand("AulasUpdate");

            baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, aula.Id);
            baseDados.AddInParameter(cmd, "@DescricaoAtividade", DbType.String, aula.DescricaoAtividade);
            baseDados.AddInParameter(cmd, "@CategoriaAtividadeId", DbType.Guid, aula.CategoriaAtividade.Id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public Aula GetAula(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("AulasSelectById");
            baseDados.AddInParameter(cmd, "@AulaId", DbType.Guid, id);
            Entities.Aula aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();
                    TurmaDAO turmas = new TurmaDAO();
                    Entities.Turma turma = turmas.GetTurma(leitor.GetGuid(leitor.GetOrdinal("TurmaId")));
                    CategoriaAtividadeDAO categoriaAtividades = new CategoriaAtividadeDAO();
                    Entities.CategoriaAtividade categoriaAtividade = categoriaAtividades.GetCategoriaAtividadeById(leitor.GetGuid(leitor.GetOrdinal("CategoriaAtividadeId")));
                    aux = Entities.Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")),
                                                turma,
                                                leitor.GetString(leitor.GetOrdinal("Hora")),
                                                leitor.GetDateTime(leitor.GetOrdinal("Data")),
                                                leitor.GetString(leitor.GetOrdinal("DescricaoAtividade")),
                                                categoriaAtividade);
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }

        public List<Aula> GetAulas()
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("AulasSelect");
            List<Entities.Aula> listaAux = new List<BusinessData.Entities.Aula>();
            Entities.Aula aux;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    DataAccess.TurmaDAO turmas = new TurmaDAO();
                    DataAccess.CategoriaAtividadeDAO categorias = new CategoriaAtividadeDAO();
                    while (leitor.Read())
                    {
                        Entities.Turma turma = turmas.GetTurma(leitor.GetGuid(leitor.GetOrdinal("TurmaId")));
                        Entities.CategoriaAtividade categoria = categorias.GetCategoriaAtividadeById(leitor.GetGuid(leitor.GetOrdinal("CategoriaAtividadeId")));

                        aux = Entities.Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")),
                                                    turma,
                                                    leitor.GetString(leitor.GetOrdinal("Hora")),
                                                    leitor.GetDateTime(leitor.GetOrdinal("Data")),
                                                    leitor.GetString(leitor.GetOrdinal("DescricaoAtividade")),
                                                    categoria);
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

        public List<Aula> GetAulas (Guid TurmaId)
        {
            try
            {

                DbCommand cmd = baseDados.GetStoredProcCommand("AulasSelectByTurma");
                baseDados.AddInParameter(cmd, "@TurmaId", DbType.Guid, TurmaId);

                CategoriaAtividadeDAO catDAO = new CategoriaAtividadeDAO();
                TurmaDAO turmaDAO = new TurmaDAO();
                Turma turma = turmaDAO.GetTurma(TurmaId);

                List<Aula> resultado = new List<Aula>();

                CategoriaAtividade cate = null;
                Aula aux = null;

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        cate = catDAO.GetCategoriaAtividadeById(leitor.GetGuid(leitor.GetOrdinal("CategoriaAtividadeId")));

                            aux = Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")),
                                               turma,
                                               leitor.GetString(leitor.GetOrdinal("Hora")),
                                               leitor.GetDateTime(leitor.GetOrdinal("Data")),
                                               leitor.GetString(leitor.GetOrdinal("DescricaoAtividade")),
                                               cate);
                            resultado.Add(aux);

                   }
                }
                
                return resultado;

            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }

        }

        public Aula GetAula(Guid turmaId, DateTime data, string hora)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("AulasSelectByTurmaDataHora");
            baseDados.AddInParameter(cmd, "@TurmaId", DbType.Guid, turmaId);
            baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
            baseDados.AddInParameter(cmd, "@Hora", DbType.String, hora);

            Aula aux = null;

            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();
            
                    TurmaDAO turmas = new TurmaDAO();
                    Turma turma = turmas.GetTurma(turmaId);
                    
                    CategoriaAtividadeDAO categoriaAtividades = new CategoriaAtividadeDAO();
                    CategoriaAtividade categoriaAtividade = categoriaAtividades.GetCategoriaAtividadeById(leitor.GetGuid(leitor.GetOrdinal("CategoriaAtividadeId")));
                    aux = Aula.GetAula(leitor.GetGuid(leitor.GetOrdinal("AulaId")),
                                       turma,
                                       leitor.GetString(leitor.GetOrdinal("Hora")),
                                       leitor.GetDateTime(leitor.GetOrdinal("Data")),
                                       leitor.GetString(leitor.GetOrdinal("DescricaoAtividade")),
                                       categoriaAtividade);
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return aux;
        }
    }
}
