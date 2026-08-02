using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using BusinessData.Entities;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;


namespace BusinessData.DataAccess
{
    public class RecursosDAO

    {
        private Database baseDados;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para Recursos
        /// </summary>
        public RecursosDAO()
        {
            try
            {
                String db = DatabaseSelect.Instance.DB;                
                baseDados = DatabaseFactory.CreateDatabase(db);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Atualiza um Recurso
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="recurso">Recurso</param>
        public void UpdateRecurso(Recurso recurso)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("RecursosUpdate");

                baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recurso.Id);
                baseDados.AddInParameter(cmd, "@CategoriaId", DbType.Guid, recurso.Categoria.Id);
                baseDados.AddInParameter(cmd, "@Descricao", DbType.String, recurso.Descricao);
                baseDados.AddInParameter(cmd, "@Vinculo", DbType.Guid, recurso.Vinculo.Id);
                baseDados.AddInParameter(cmd, "@EstaDisponivel", DbType.Boolean, recurso.EstaDisponivel);


                baseDados.ExecuteNonQuery(cmd);

                List<HorarioBloqueado> listaHB = this.GetHorarioBloqueadoByRecurso(recurso.Id);
                if (recurso.HorariosBloqueados != null)
                    foreach (HorarioBloqueado hb in recurso.HorariosBloqueados)
                    {
                        if (listaHB.Count != 0)
                        {
                            foreach (HorarioBloqueado hbTem in listaHB)
                            {
                                if ((hb.HoraInicio.Equals(hbTem.HoraInicio)) && (hb.HoraFim.Equals(hbTem.HoraFim)))
                                    this.UpdateHorarioBloqueado(hb, recurso);
                                else this.InsereHorarioBloqueado(hb, recurso);
                            }
                        }
                        else
                        {
                            this.InsereHorarioBloqueado(hb, recurso);
                        }
                    }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Deleta um Recurso
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        public void DeletaRecurso(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("RecursosDelete");
            
            baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, id);

            DeletaHorarioBloqueado(id);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Insere um Recurso
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="recurso"></param>
        public void InsereRecurso(Recurso recurso)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("RecursosInsere");
                baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recurso.Id);
                baseDados.AddInParameter(cmd, "@Descricao", DbType.String, recurso.Descricao);
                baseDados.AddInParameter(cmd, "@Abrev", DbType.String, recurso.Abrev);
                baseDados.AddInParameter(cmd, "@Tipo", DbType.AnsiStringFixedLength, recurso.Tipo);
                baseDados.AddInParameter(cmd, "@Vinculo", DbType.Guid, recurso.Vinculo.Id);
                baseDados.AddInParameter(cmd, "@EstaDisponivel", DbType.Boolean, recurso.EstaDisponivel);
                baseDados.AddInParameter(cmd, "@CategoriaId", DbType.Guid, recurso.Categoria.Id);


                baseDados.ExecuteNonQuery(cmd);
                if (recurso.HorariosBloqueados != null)
                    foreach (HorarioBloqueado hb in recurso.HorariosBloqueados)
                    {
                        InsereHorarioBloqueado(hb, recurso);
                    }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
        /// <summary>
        /// Retorna o Recurso relativo ao Id especificado
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Recurso GetRecurso(Guid id)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("RecursosSelectById");
            baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, id);

            Recurso aux = null;
            try
            {
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    leitor.Read();
                    List<HorarioBloqueado> listaHB = this.GetHorarioBloqueadoByRecurso(id);
                    FaculdadesDAO vinculosDAO = new FaculdadesDAO();
                    CategoriaRecursoDAO categoriarecursoDAO = new CategoriaRecursoDAO();

                    // TODO
                    Guid block1 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia1"));
                    Guid block2 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia2"));

                    aux = Recurso.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")),
                                             leitor.GetString(leitor.GetOrdinal("Descricao")),
                                             leitor.GetString(leitor.GetOrdinal("Abrev")),
                                             leitor.GetString(leitor.GetOrdinal("Tipo"))[0],
                                             vinculosDAO.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("FaculdadeId"))),
                                             categoriarecursoDAO.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaRecursoId"))),
                                             leitor.GetBoolean(leitor.GetOrdinal("EstaDisponivel")),
                                             block1, block2,
                                             listaHB);
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            catch (Exception )
            {
                return null;
            }
            return aux;
        }

        public List<Recurso> GetRecursosDisponiveis(DateTime data, string horarioPUCRS, Guid categoriaRecursoId)
        {
            //Debug.WriteLine(data.ToShortDateString() + " - " + horarioPUCRS + " " + categoriaRecursoId);
            DbCommand cmd = baseDados.GetStoredProcCommand("GetRecursosDisponiveisDataHorarioCategoria");
            baseDados.AddInParameter(cmd,"@Data",DbType.DateTime,data);
            baseDados.AddInParameter(cmd,"@HOrario",DbType.String,horarioPUCRS);
            baseDados.AddInParameter(cmd,"@CategoriaRecurso",DbType.Guid,categoriaRecursoId);

            Recurso aux;
            List<Recurso> listaRecursos = new List<Recurso>();
            List<Recurso> alocados = GetRecursosAlocados(data, horarioPUCRS);
            try
            {
                using(IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    // Fetch all blocked schedules in one query instead of one per resource.
                    Dictionary<Guid, List<HorarioBloqueado>> todosHB = GetAllHorariosBloqueados();

                    while(leitor.Read())
                    {
                        Guid recursoId = leitor.GetGuid(leitor.GetOrdinal("RecursoId"));
                        bool block = false;
                        foreach (Recurso alocado in alocados)
                            if (alocado.Bloqueia1 == recursoId || alocado.Bloqueia2 == recursoId)
                            {
                                block = true;
                                break;
                            }
                        if (block) continue;

                        aux = new Recurso();
                        CategoriaRecurso catRec = new CategoriaRecurso(categoriaRecursoId, leitor.GetString(leitor.GetOrdinal("CategoriaRecursoDescricao")));
                        Faculdade facul = Faculdade.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("FaculdadeId")), leitor.GetString(leitor.GetOrdinal("FaculdadeNome")));                       
                        aux.Categoria = catRec;
                        aux.Descricao = leitor.GetString(leitor.GetOrdinal("RecursoDescricao"));
                        aux.EstaDisponivel = leitor.GetBoolean(leitor.GetOrdinal("RecursoEstaDisponivel"));
                        List<HorarioBloqueado> listaHB;
                        todosHB.TryGetValue(recursoId, out listaHB);
                        aux.HorariosBloqueados = listaHB ?? new List<HorarioBloqueado>();
                        aux.Abrev = leitor.GetString(leitor.GetOrdinal("Abrev")).Trim();
                        aux.Id = recursoId;
                        aux.Vinculo = facul;

                        listaRecursos.Add(aux);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaRecursos;
        }
        /// <summary>
        /// Retorna todos os Rescursos 
        /// throws BusinessData.BusinessLogic.DataAccess.DataAccessExceptiom
        /// </summary>
        /// <returns>Lista de Recursos</returns>
        public List<Recurso> GetRecursos()
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("RecursosSelect");

                Recurso aux;
                FaculdadesDAO vinculosDAO = new FaculdadesDAO();
                CategoriaRecursoDAO categoriarecursoDAO = new CategoriaRecursoDAO();
                List<Recurso> listaAux = new List<Recurso>();
                Guid recursoId;

                // Fetch all blocked schedules in one query instead of one per resource.
                Dictionary<Guid, List<HorarioBloqueado>> todosHB = GetAllHorariosBloqueados();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {                        
                        Guid block1 = new Guid();
                        Guid block2 = new Guid();
                        if (leitor["Bloqueia1"].GetType() != typeof(DBNull))
                            block1 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia1"));

                        if (leitor["Bloqueia2"].GetType() != typeof(DBNull))
                            block2 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia2"));

                        recursoId = leitor.GetGuid(leitor.GetOrdinal("RecursoId"));
                        List<HorarioBloqueado> listaHB;
                        todosHB.TryGetValue(recursoId, out listaHB);
                        listaHB = listaHB ?? new List<HorarioBloqueado>();
                        aux = Recurso.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")),
                                                 leitor.GetString(leitor.GetOrdinal("Descricao")).Trim(),
                                                 leitor.GetString(leitor.GetOrdinal("Abrev")).Trim(),
                                                 leitor.GetString(leitor.GetOrdinal("Tipo"))[0],
                                                 vinculosDAO.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("FaculdadeId"))),
                                                 categoriarecursoDAO.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaRecursoId"))),
                                                 leitor.GetBoolean(leitor.GetOrdinal("EstaDisponivel")),
                                                 block1, block2,
                                                 listaHB);
                        listaAux.Add(aux);
                    }
                }
                return listaAux;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }

            
        }

        /// <summary>
        /// Retorna todos os recursos da categoria especificada
        /// </summary>
        /// <param name="cat">Categoria de Recursos</param>
        /// <returns></returns>
        public List<Recurso> GetRecursosPorCategoria(CategoriaRecurso cat)
        {
            
            DbCommand cmd = baseDados.GetStoredProcCommand("RecursosSelectByCategoria");
            baseDados.AddInParameter(cmd, "@CategoriaRecursoId", DbType.Guid, cat.Id);
            List<Recurso> listaAux = new List<Recurso>();
            List<HorarioBloqueado> listaHB = new List<HorarioBloqueado>();
            Guid recursoId;
            Recurso aux;
            try
            {
                FaculdadesDAO vinculosDAO = new FaculdadesDAO();
                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        recursoId = leitor.GetGuid(leitor.GetOrdinal("RecursoId"));
                        listaHB = this.GetHorarioBloqueadoByRecurso(recursoId);

                        // TODO
                        Guid block1 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia1"));
                        Guid block2 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia2"));

                        aux = Recurso.GetRecurso(leitor.GetGuid(leitor.GetOrdinal("RecursoId")),
                                                 leitor.GetString(leitor.GetOrdinal("Descricao")),
                                                 leitor.GetString(leitor.GetOrdinal("Abrev")),
                                                 leitor.GetString(leitor.GetOrdinal("Tipo"))[0],
                                                 vinculosDAO.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("Vinculo"))),
                                                 cat,
                                                 leitor.GetBoolean(leitor.GetOrdinal("EstaDisponivel")),
                                                 block1, block2,
                                                 listaHB);
                        listaAux.Add(aux);
                    }
                }
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
            return listaAux;
        }

        public List<Recurso> GetRecursosDisponiveis(DateTime data, string hora)
        {
            List<Recurso> alocados = GetRecursosAlocados(data, hora);
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("RecursosSelectDisponiveis");
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, hora);

                List<Recurso> resultado = new List<Recurso>();
                Recurso aux = null;
                FaculdadesDAO faculDao = new FaculdadesDAO();
                CategoriaRecursoDAO categoriaDao = new CategoriaRecursoDAO();

                // Fetch all blocked schedules in one query instead of one per resource.
                Dictionary<Guid, List<HorarioBloqueado>> todosHB = GetAllHorariosBloqueados();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        Guid recursoId = leitor.GetGuid(leitor.GetOrdinal("RecursoId"));
                        bool block = false;
                        foreach (Recurso alocado in alocados)
                            if (alocado.Bloqueia1 == recursoId || alocado.Bloqueia2 == recursoId)
                            {
                                block = true;
                                break;
                            }
                        if (block) continue;

                        List<HorarioBloqueado> listaHB;
                        todosHB.TryGetValue(recursoId, out listaHB);
                        listaHB = listaHB ?? new List<HorarioBloqueado>();

                        Faculdade facul = faculDao.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("Vinculo")));
                        CategoriaRecurso categoria = categoriaDao.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaId")));

                        string descricao = leitor.GetString(leitor.GetOrdinal("Descricao"));
                        bool disponivel = leitor.GetBoolean(leitor.GetOrdinal("EstaDisponivel"));
                        string abrev = leitor.GetString(leitor.GetOrdinal("Abrev"));
                        char tipo = leitor.GetString(leitor.GetOrdinal("Tipo"))[0];

                        Guid block1 = new Guid();
                        Guid block2 = new Guid();
                        if(leitor["Bloqueia1"].GetType() != typeof(DBNull))
                            block1 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia1"));
                        if (leitor["Bloqueia2"].GetType() != typeof(DBNull))
                            block2 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia2"));

                        aux = Recurso.GetRecurso(recursoId, descricao, abrev, tipo, facul, categoria, disponivel, block1, block2, listaHB);
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

        public List<Recurso> GetRecursosAlocados(DateTime data, string hora)
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("RecursosSelectAlocadosByDataHora");
                baseDados.AddInParameter(cmd, "@Data", DbType.DateTime, data);
                baseDados.AddInParameter(cmd, "@Horario", DbType.String, hora);

                List<Recurso> resultado = new List<Recurso>();
                Recurso aux = null;
                FaculdadesDAO faculDao = new FaculdadesDAO();
                CategoriaRecursoDAO categoriaDao = new CategoriaRecursoDAO();

                // Fetch all blocked schedules in one query instead of one per resource.
                Dictionary<Guid, List<HorarioBloqueado>> todosHB = GetAllHorariosBloqueados();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        Guid recursoId = leitor.GetGuid(leitor.GetOrdinal("RecursoId"));

                        List<HorarioBloqueado> listaHB;
                        todosHB.TryGetValue(recursoId, out listaHB);
                        listaHB = listaHB ?? new List<HorarioBloqueado>();

                        Faculdade facul = faculDao.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("Vinculo")));
                        CategoriaRecurso categoria = categoriaDao.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaId")));

                        string descricao = leitor.GetString(leitor.GetOrdinal("Descricao"));
                        bool disponivel = leitor.GetBoolean(leitor.GetOrdinal("EstaDisponivel"));
                        string abrev = leitor.GetString(leitor.GetOrdinal("Abrev"));
                        char tipo = leitor.GetString(leitor.GetOrdinal("Tipo"))[0];

                        Guid block1 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia1"));
                        Guid block2 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia2"));

                        aux = Recurso.GetRecurso(recursoId, descricao, abrev, tipo, facul, categoria, disponivel, block1, block2, listaHB);
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

        public List<Recurso> GetRecursosAlocados()
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("RecursosSelectAlocados");

                List<Recurso> resultado = new List<Recurso>();
                Recurso aux = null;
                FaculdadesDAO faculDao = new FaculdadesDAO();
                CategoriaRecursoDAO categoriaDao = new CategoriaRecursoDAO();

                // Fetch all blocked schedules in one query instead of one per resource.
                Dictionary<Guid, List<HorarioBloqueado>> todosHB = GetAllHorariosBloqueados();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        Guid recursoId = leitor.GetGuid(leitor.GetOrdinal("RecursoId"));
                        List<HorarioBloqueado> listaHB;
                        todosHB.TryGetValue(recursoId, out listaHB);
                        listaHB = listaHB ?? new List<HorarioBloqueado>();
                        Faculdade facul = faculDao.GetFaculdade(leitor.GetGuid(leitor.GetOrdinal("Vinculo")));
                        CategoriaRecurso categoria = categoriaDao.GetCategoriaRecurso(leitor.GetGuid(leitor.GetOrdinal("CategoriaId")));
                        string descricao = leitor.GetString(leitor.GetOrdinal("Descricao"));
                        bool disponivel = leitor.GetBoolean(leitor.GetOrdinal("EstaDisponivel"));
                        string abrev = leitor.GetString(leitor.GetOrdinal("Abrev"));
                        char tipo = leitor.GetString(leitor.GetOrdinal("Tipo"))[0];

                        Guid block1 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia1"));
                        Guid block2 = leitor.GetGuid(leitor.GetOrdinal("Bloqueia2"));

                        aux = Recurso.GetRecurso(recursoId, descricao, abrev, tipo, facul, categoria, disponivel, block1, block2, listaHB);
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

        public void InsereHorarioBloqueado(HorarioBloqueado hb, Recurso recurso)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorarioBloqueadoInsere");
            baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recurso.Id);
            baseDados.AddInParameter(cmd, "@HorarioInicio", DbType.String, hb.HoraInicio);
            baseDados.AddInParameter(cmd, "@HorarioFim", DbType.String, hb.HoraFim);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void DeletaHorarioBloqueado(Guid recursoId)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorarioBloqueadoDeleta");
            baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recursoId);
            
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void DeletaHorarioBloqueado(Guid recursoId, HorarioBloqueado hb)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorarioBloqueadoDeletaById");
            baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recursoId);
            baseDados.AddInParameter(cmd, "@HorarioInicio", DbType.String, hb.HoraInicio);
            baseDados.AddInParameter(cmd, "@HorarioFim", DbType.String, hb.HoraFim);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public void UpdateHorarioBloqueado(HorarioBloqueado hb, Recurso recurso)
        {
            DbCommand cmd = baseDados.GetStoredProcCommand("HorarioBloqueadoUpdate");
            baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recurso.Id);
            baseDados.AddInParameter(cmd, "@HorarioInicio", DbType.String, hb.HoraInicio);
            baseDados.AddInParameter(cmd, "@HorarioFim", DbType.String, hb.HoraFim);
            try
            {
                baseDados.ExecuteNonQuery(cmd);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public List<HorarioBloqueado> GetHorarioBloqueadoByRecurso(Guid recursoId)
        {
            try 
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("HorarioBloqueadoGetByRecurso");
                baseDados.AddInParameter(cmd, "@RecursoId", DbType.Guid, recursoId);

                HorarioBloqueado aux;
                List<HorarioBloqueado> listaHB = new List<HorarioBloqueado>();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        aux = new HorarioBloqueado(leitor.GetString(leitor.GetOrdinal("HorarioInicio")),
                                                   leitor.GetString(leitor.GetOrdinal("HorarioFim")));
                        listaHB.Add(aux);
                    }
                }
                return listaHB;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        /// <summary>
        /// Fetches all HorarioBloqueado rows in a single query and groups them by RecursoId.
        /// Use this dictionary in place of per-resource calls to GetHorarioBloqueadoByRecurso
        /// when building lists of multiple resources, to avoid N+1 database round-trips.
        /// </summary>
        public Dictionary<Guid, List<HorarioBloqueado>> GetAllHorariosBloqueados()
        {
            try
            {
                DbCommand cmd = baseDados.GetStoredProcCommand("HorarioBloqueadoGetAll");
                Dictionary<Guid, List<HorarioBloqueado>> dict = new Dictionary<Guid, List<HorarioBloqueado>>();

                using (IDataReader leitor = baseDados.ExecuteReader(cmd))
                {
                    while (leitor.Read())
                    {
                        Guid recursoId = leitor.GetGuid(leitor.GetOrdinal("RecursoId"));
                        HorarioBloqueado hb = new HorarioBloqueado(
                            leitor.GetString(leitor.GetOrdinal("HorarioInicio")),
                            leitor.GetString(leitor.GetOrdinal("HorarioFim")));

                        List<HorarioBloqueado> lista;
                        if (!dict.TryGetValue(recursoId, out lista))
                        {
                            lista = new List<HorarioBloqueado>();
                            dict[recursoId] = lista;
                        }
                        lista.Add(hb);
                    }
                }
                return dict;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }
    }
}
