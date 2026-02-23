using System;
using System.Collections.Generic;
using BusinessData.Entities;
using System.Data.SqlClient;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.ServiceModel.Description;

namespace BusinessData.DataAccess
{
    public class SRRCDAO
    {
        private static SqlConnection conn = null;
        private static OracleConnection oconn = null;

        /// <summary>
        /// Cria um novo Objeto de Acesso a Dados para LogData
        /// </summary>
        public SRRCDAO()
        {
        }

        public SqlConnection ConnectSqlServer()
        {
            if (conn != null) return conn;
            try
            {
                var constr = ConfigurationManager.ConnectionStrings["SARCFACINcs"];
                conn = new SqlConnection(constr.ConnectionString);
                return conn;
                //conn.Open();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException(ErroMessages.GetErrorMessage(ex.Number), ex);
            }
        }

        public OracleConnection ConnectOracle()
        {
            if (oconn != null) return oconn;
            string cs = ConfigurationManager.ConnectionStrings["OracleCS"].ConnectionString;
            oconn = new Oracle.ManagedDataAccess.Client.OracleConnection(cs);
            return oconn;
            /*            {
                            oconn.Open();
                            Oracle.ManagedDataAccess.Client.OracleCommand c = oconn.CreateCommand();
                            c.CommandText = "SELECT DISTINCT CDDISCIPL,CD_DISCIPLINA FROM GRANDEPORTE.EF_DISCIPLINA_TURMA edt where edt.cdano = '" + ano + "' ORDER BY CDDISCIPL";

                            Oracle.ManagedDataAccess.Client.OracleDataReader or = c.ExecuteReader();
                            while (or.Read())
                            {
                                //Response.Write(or.FieldCount + ": "+or.GetString(0) + " " + or.GetString(1) +"<br>");
                                mapeamentoDisciplinas[or.GetString(0)] = or.GetInt32(1);
                            }
                        }*/
        }

        public string GetUltimoStatus(string recurso)
        {
            if (recurso.StartsWith("LAPRO"))
                return "Disponível";

            SqlConnection c = ConnectSqlServer();
            c.Open();
            SqlCommand cmd = c.CreateCommand();
            if (recurso.IndexOf('/') != -1) // caso seja lab duplo, nao ha entrada no log (apenas para um deles, geralmente o primeiro)
            {
                recurso = recurso.Substring(0, recurso.IndexOf("/"));
                string[] aux = recurso.Split();
                recurso = aux[0] + " - " + aux[1];
            }
            cmd.CommandText = "select horario, acao, usuario from LogData where recurso = '" + recurso + "' order by horario desc";
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                c.Close();
                return "Sem informações";
            }
            dr.Read();
            string acao = dr["acao"] as string;
            DateTime horario = dr.GetDateTime(0);
            dr.Close();
            c.Close();
            string timespec = "";
            TimeSpan delta = DateTime.Now - horario;
            if (delta.TotalHours < 12)
                timespec = "(" + horario.ToShortTimeString() + ")";
            else if (delta.TotalHours >= 12 && delta.TotalHours < 48)
                timespec = "(ontem " + horario.ToShortTimeString() + ")";
            else
                timespec = "(" + delta.Days + " dias)";
            //            timespec += " ("+horario+")";
            if (acao == "E") return "Disponível";// +timespec;
            return "Retirado " + timespec;
        }

        public Usuario FindProf(String id)
        {
            Usuario u = null;
            OracleConnection c = ConnectOracle();
            String sql = "select pr.cdunid, un.nounid, pr.cdmatricula, pr.nmprofessor from GRANDEPORTE.VW_PROFESSORES_MOODLE pr " +
                             "inner join grandeporte.unidade un " +
                             "on pr.cdunid = un.cdunid " +
                             "where pr.cdmatricula = ";
            string mat = "10" + id.Substring(2, id.Length - 2);
            sql += mat;
            c.Open();
            OracleCommand cmd = c.CreateCommand();

            cmd.CommandText = sql;

            OracleDataReader or = cmd.ExecuteReader();
            if (or.HasRows)
            {
                or.Read();
                string cdunid = or.GetString(0);
                string nounid = or.GetString(1);
                string matr = or.GetString(2);
                string nome = or.GetString(3);
                u = new Usuario(matr, nome, nounid, Usuario.TipoUsuario.PROFESSOR);
            }
            c.Close();
            /*            catch (SQLException ex)
                        {
                            System.out.println("findProf: error connecting to DB!");
                            System.out.println(ex.getMessage());
                            System.exit(1);
                        }*/
            return u;
        }


        public Usuario FindFunc(String id)
        {
            Usuario u = null;
            SqlConnection c = ConnectSqlServer();
            c.Open();
            String sql = "select userid, username from Secretaria " +
                         "where userid = '"+id+"'";
//            sql += id;
            SqlCommand cmd = c.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                dr.Close();
                c.Close();
                return null;
            }
            dr.Read();
            string nome = dr["username"] as string;
            string nounid = "ESCOLA POLITÉCNICA";
            string matr = id;
            u = new Usuario(matr, nome, nounid, Usuario.TipoUsuario.FUNCIONARIO);
            dr.Close();
            c.Close();
            return u;
        }

        public Usuario FindStudentUndergrad(String id)
        {
            Usuario u = null;
            OracleConnection c = ConnectOracle();
            String sql = "select cdmatricula, noaluno, nmcurso, nmunidade " +
                     "from GRANDEPORTE.VW_ALUNOS_MOODLE al " +
                     "where cdmatricula = '";
            string mat = id.Substring(0, id.Length - 2);
            sql += mat+"'";
            c.Open();
            OracleCommand cmd = c.CreateCommand();

            cmd.CommandText = sql;

            OracleDataReader or = cmd.ExecuteReader();
            if (or.HasRows)
            {
                or.Read();
                string cdunid = or.GetString(2);
                string nounid = or.GetString(3);
                string matr = or.GetString(0);
                string nome = or.GetString(1);
                u = new Usuario(matr, nome, nounid, Usuario.TipoUsuario.ALUNO_GRAD);
            }
            c.Close();
            /*            catch (SQLException ex)
                        {
                            System.out.println("findProf: error connecting to DB!");
                            System.out.println(ex.getMessage());
                            System.exit(1);
                        }*/
            return u;
        }

        public Usuario FindStudentLato(String id)
        {
            Usuario u = null;
            OracleConnection c = ConnectOracle();
            String sql = "select \"tx_matricula\", \"nm_aluno\", c.nocurso " +
                     "from POS_NOVO.VW_PG_ALUNO_MOODLE al " +
                     "inner join GRANDEPORTE.CURSO c " +
                     "on al.\"cd_curso\" = c.cdcurso " +
                     "where \"tx_matricula\" = '";
            string mat = id.Substring(0, id.Length - 2);
            sql += mat+"'";
            c.Open();
            OracleCommand cmd = c.CreateCommand();

            cmd.CommandText = sql;

            OracleDataReader or = cmd.ExecuteReader();
            if (or.HasRows)
            {
                or.Read();
                string nounid = or.GetString(2);
                string matr = or.GetString(0);
                string nome = or.GetString(1);
                u = new Usuario(matr, nome, nounid, Usuario.TipoUsuario.ALUNO_LATO);
            }
            c.Close();
            /*            catch (SQLException ex)
                        {
                            System.out.println("findProf: error connecting to DB!");
                            System.out.println(ex.getMessage());
                            System.exit(1);
                        }*/
            return u;
        }

        public Usuario FindStudentStricto(String id)
        {
            Usuario u = null;
            OracleConnection c = ConnectOracle();
            String sql = "select cdmatricula, noaluno, cdcurso " +
                     "from SIPOS.VW_PO_ALUNO_MOODLE " +
                     "where cdmatricula = '";
            string mat = id.Substring(0, id.Length - 2);
            sql += mat + "'";
            c.Open();
            OracleCommand cmd = c.CreateCommand();

            cmd.CommandText = sql;

            OracleDataReader or = cmd.ExecuteReader();
            if (or.HasRows)
            {
                or.Read();
                string nounid = or.GetString(2);
                string matr = or.GetString(0);
                string nome = or.GetString(1);
                u = new Usuario(matr, nome, nounid, Usuario.TipoUsuario.ALUNO_STRICTO);
            }
            c.Close();
            /*            catch (SQLException ex)
                        {
                            System.out.println("findProf: error connecting to DB!");
                            System.out.println(ex.getMessage());
                            System.exit(1);
                        }*/
            return u;
        }
        public LogData FindLatestActivity(String resource)
        {
            LogData log = null;
            SqlConnection c = ConnectSqlServer();
            c.Open();
            String sql = "select top 1 horario, acao, usuario, unidadecurso, tipousuario from logdata " +
                         "where recurso = '" + resource +
                         //"' and CAST(horario as DATE) = CAST(GETDATE() as DATE) " +
                         "' order by horario desc";

            SqlCommand cmd = c.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                dr.Close();
                c.Close();
                return null;
            }
            dr.Read();
            DateTime horario = (DateTime)dr["horario"];
            string acao = dr["acao"] as string;
            string usuario = dr["usuario"] as string;
            string unidadeCurso = dr["unidadecurso"] as string;
            string tipoUsuario = dr["tipousuario"] as string;
            dr.Close();
            c.Close();
            if (acao == "R")
                acao = "RETIRADA";
            else
                acao = "ENTREGA";
            log = new LogData(horario, acao, usuario, resource, unidadeCurso, tipoUsuario);

            return log;
        }

        public void AddToLog(LogData log)
        {
            SqlConnection c = ConnectSqlServer();
            String sql = "insert into logdata (logid, horario, acao, usuario, recurso, unidadecurso, tipousuario) " +
                         "values ('{0}',@horario,'{1}','{2}','{3}','{4}','{5}')";
            //               "values (@logid, @horario, @acao, @usuario, @recurso, @unidade, @tipouser)";
            c.Open();
            SqlCommand cmd = c.CreateCommand();            
            string acao = log.Acao;
            if (acao == "RETIRADA")
                acao = "R";
            else
                acao = "E";
            Guid guid = System.Guid.NewGuid();
            //cmd.Parameters.Add("@logid", System.Data.SqlDbType.UniqueIdentifier).Value = guid;
            cmd.Parameters.Add("@horario", System.Data.SqlDbType.DateTime).Value = log.Horario;
            //cmd.Parameters.Add("@acao", System.Data.SqlDbType.VarChar).Value = log.Acao;
            //cmd.Parameters.Add("@usuario", System.Data.SqlDbType.VarChar).Value = log.Usuario;
            //cmd.Parameters.Add("@recurso", System.Data.SqlDbType.VarChar).Value = log.Recurso;
            //cmd.Parameters.Add("@unidade", System.Data.SqlDbType.VarChar).Value = log.UnidadeCurso;
            //cmd.Parameters.Add("@tipouser", System.Data.SqlDbType.VarChar).Value = log.TipoUsuario;


            sql = string.Format(sql, guid, acao, log.Usuario, log.Recurso, log.UnidadeCurso, log.TipoUsuario);
            cmd.CommandText = sql;

            int linhas = cmd.ExecuteNonQuery();
            c.Close();
        }
        public List<Usuario> LoadProfs()
        {
            List<Usuario> lista = new List<Usuario>();
            SqlConnection c = ConnectSqlServer();

            String sql = "select nome, matricula, unidade " +                        
                         "from UsuariosSRRC u " +                         
                         "order by u.nome";
            c.Open();
            SqlCommand cmd = c.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                dr.Close();
                c.Close();
                return lista;
            }
            while (dr.Read())
            {
                string nome = dr["nome"] as string;
                string matricula= dr["matricula"] as string;
                string unidade = dr["unidade"] as string;
                Usuario novo = new Usuario(matricula, nome, unidade, Usuario.TipoUsuario.PROFESSOR);
                lista.Add(novo);
            }
            dr.Close();
            c.Close();
            return lista;
        }

        public List<Usuario> ImportProfs()
        {
            List<Usuario> lista = new List<Usuario>();
            OracleConnection c = ConnectOracle();
            String sql = "select pr.cdunid, un.nounid, pr.cdmatricula, pr.nmprofessor from GRANDEPORTE.VW_PROFESSORES_MOODLE pr " +
                     "inner join grandeporte.unidade un " +
                     "on pr.cdunid = un.cdunid " +
                     //"where pr.cdunid = 98 " + 
                     "order by pr.cdmatricula";
            c.Open();
            OracleCommand cmd = c.CreateCommand();

            cmd.CommandText = sql;

            OracleDataReader or = cmd.ExecuteReader();
            while(or.Read())
            {
                string cdunid = or.GetString(0);
                string nounid = or.GetString(1);
                string matr = or.GetString(2);
                string nome = or.GetString(3);
                Usuario novo = new Usuario(matr, nome, nounid, Usuario.TipoUsuario.PROFESSOR);
                lista.Add(novo);
            }
            c.Close();
            return lista;
        }

        public int ImportProfsToSARC(List<Usuario> lista)
        {
            int total = 0;
            List<Usuario> listaAtual = LoadProfs();
            Dictionary<string, Usuario> dic = new Dictionary<string, Usuario>();
            foreach (Usuario user in listaAtual)
                dic.Add(user.Matricula, user);

            SqlConnection c = ConnectSqlServer();
            try
            {

                c.Open();

                foreach (Usuario user in lista)
                {
                    if (dic.ContainsKey(user.Matricula)) continue;
                    String sql = "insert into UsuariosSRRC (matricula, nome, unidade) " +
                                 "values ('{0}','{1}','{2}')";

                    SqlCommand cmd = c.CreateCommand();

                    sql = string.Format(sql, user.Matricula.Trim(), user.Nome.Trim(), user.Unidade);
                    cmd.CommandText = sql;

                    total +=  cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                c.Close();
            }
            return total;       
        }

        public List<RecursoSRRC> LoadResources()
        {
            List<RecursoSRRC> lista = new List<RecursoSRRC>();
            SqlConnection c = ConnectSqlServer();

            String sql = "select recursoid, " +
                         "cat.descricao as categoria, " +
                         "rec.descricao as descr, rec.tipo as tipo, rec.abrev as abrev " +
                         "from recursos rec " +
                         "inner join CategoriasRecurso cat " +
                         "on cat.CategoriaRecursoId = rec.CategoriaId " +
                         "where estaDisponivel = 1 and ExibirRetirada = 1 " +
                         "order by categoria, descr";
            c.Open();
            SqlCommand cmd = c.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                dr.Close();
                c.Close();
                return null;
            }
            while (dr.Read())
            {
                string id = (string)dr["recursoid"].ToString().Substring(9, 4).ToUpper();
                string categoria = dr["categoria"] as string;
                string descricao = dr["descr"] as string;
                string aux = dr["tipo"] as string;
                char tipo = '?';
                if (aux != null)
                    tipo = aux[0];
                string abrev = dr["abrev"] as string;
                RecursoSRRC novo = new RecursoSRRC(id, descricao, categoria, tipo, abrev);
                lista.Add(novo);
            }
            dr.Close();
            c.Close();
            return lista;
        }

        public List<LogData> LoadActivity(bool daily)
        {
            List<LogData> lista = new List<LogData>();
            String wheres = "";
            if (daily)
                wheres = "where CAST(horario as DATE) = CAST(GETDATE() as DATE) ";
            SqlConnection c = ConnectSqlServer();
            String sql = "select horario, acao, usuario,recurso, unidadecurso, tipousuario from logdata " +
                         wheres +
                         "order by horario desc";
            c.Open();
            SqlCommand cmd = c.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                dr.Close();
                c.Close();
                return null;
            }
            while (dr.Read())
            {
                DateTime horario = (DateTime)dr["horario"];
                string acao = dr["acao"] as string;

                if (acao == "R")
                    acao = "RETIRADA";
                else
                    acao = "ENTREGA";
                string usuario = dr["usuario"] as string;
                string recurso = dr["recurso"] as string;
                string unidadeCurso = dr["unidadecurso"] as string;
                string tipoUsuario = dr["tipousuario"] as string;
                LogData log = new LogData(horario, acao, usuario, recurso, unidadeCurso, tipoUsuario);
                lista.Add(log);
            }
            dr.Close();
            c.Close();
            return lista;
        }
    }
}