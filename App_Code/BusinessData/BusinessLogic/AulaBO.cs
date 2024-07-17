using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using BusinessData.Util;
using BusinessData.Entities;
using System.Text.RegularExpressions;
using BusinessData.DataAccess;
//Log
//using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;
using System.Configuration;
using System.Xml.Schema;
using System.Data.SqlClient;

namespace BusinessData.BusinessLogic
{
    public class AulaBO
    {
        private AulasDAO dao;
        private Usuario usr;

        public AulaBO()
        {
            try
            {
                dao = new BusinessData.DataAccess.AulasDAO();
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
            usr = new Usuario();
        }

        public void UpdateAula(Entities.Aula aula)
        {
                try
                {
                    dao.AtualizaAula(aula);
                    //instancia o usuario logado
                    //MembershipUser user = Membership.GetUser();
                    //instancia o log
                    //LogEntry log = new LogEntry();
                    //monta log
                    //log.Message = "Id: " + aula.Id + "; Usuário: " + user.UserName;
                    //log.TimeStamp = DateTime.Now;
                    //log.Severity = TraceEventType.Information;
                    //log.Title = "Update Aula";
                    //log.MachineName = Dns.GetHostName();
                    //guarda log no banco
                    //Logger.Write(log);
                }
                catch (DataAccess.DataAccessException )
                {
                    throw;
                }
        }

        public void DeletaAula(Guid id)
        {
            if (usr.IsAdmin())
            {
                //try
                //{
                    dao.RemoveAula(id);
                //}
                //catch (DataAccessException )
                //{
                //    throw;
                //}
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public void InsereAula(Aula aula)
        {

            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereAula(aula);
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

        public Aula GetAulaById(Guid id)
        {
            try
            {
                return dao.GetAula(id);
            }
            catch (DataAccessException )
            {
                throw;
            }

        }
        public void CriarAulas(Calendario cal, Turma t)
        {
            try
            {
                //ordena lista da datas
                cal.Datas.Sort();
                //Recebe a lista das turmas
                TurmaBO contadorroleTurmas = new TurmaBO();
                IList<Entities.Turma> listaTurmas = contadorroleTurmas.GetTurmas(cal);
                Util.DataHelper dheper = new BusinessData.Util.DataHelper();
                //Recebe a lista das atividades
                CategoriaAtividadeBO contadorroleAtividades = new CategoriaAtividadeBO();
                IList<CategoriaAtividade> listaAtividades = contadorroleAtividades.GetCategoriaAtividade();
                if (listaAtividades.Count == 0)
                {
                    throw new IndexOutOfRangeException();
                }

                CategoriaAtividade cat = listaAtividades[0];
                string aulaDefault = ConfigurationManager.AppSettings["AulaDefault"];
                foreach (CategoriaAtividade categoria in listaAtividades)
                {
                    if (categoria.Descricao.Equals(aulaDefault))
                        cat = categoria;
                }

                AulaBO contadorroleAulas = new AulaBO();
                Aula aulaAux;
              
              
                    string horario = t.DataHora;

                    //dado um horario pucrs(2ab,4cd), exclui os horarios e guarda os dias em array de inteiros
                    string diasPucrs = Regex.Replace(horario, "[a-zA-Z]", String.Empty);

                    int tamanho = diasPucrs.Length;
                    int[] dias = new int[tamanho];
                    for (int i = 0; i < tamanho; i++)
                    {
                        dias[i] = Convert.ToInt32(diasPucrs.Substring(i, 1));
                    }

                    string[] horariosPucrs = horario.Split(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, StringSplitOptions.RemoveEmptyEntries);

                    DateTime aux = cal.InicioG1;

                    while (aux <= cal.FimG2)
                    {
                        for (int i = 0; i < dias.Length; i++)
                        {
                            if ((int)(aux.DayOfWeek) == (dias[i] - 1))
                            {
                                aulaAux = Aula.newAula(t, horariosPucrs[i], aux, string.Empty, cat);
                                this.InsereAula(aulaAux);
                            }
                        }
                        aux = aux.AddDays(1);
                    }
                
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }
        public void ResolveCagada(Calendario cal)
        {
            TurmaBO controleTurmas = new TurmaBO();
            IList<Entities.Turma> turmas = controleTurmas.GetTurmas(cal);

            AulaBO controleAulas = new AulaBO();

            int totalTurmas = turmas.Count;
            int tot = 1;
            foreach(Turma turma in turmas)
            {
                SortedDictionary<String, List<Aula>> dicRepet = new SortedDictionary<string, List<Aula>>();

                Debug.WriteLine("\nTurma " + tot + " de " + totalTurmas);
                Debug.WriteLine(turma.Disciplina.Nome + " - " + turma.Numero + " " + turma.Professor.Nome);
                //Turma turma2 = controleTurmas.GetTurmaById(new Guid("C6B70B0A-1DE1-4DB7-88B6-0891B6567564"));
                List <Aula> aulas = controleAulas.GetAulas(turma.Id);
                foreach(Aula aula in aulas) {
                    string descAula = aula.Data.ToString() + aula.Hora;
                    //Debug.WriteLine(">>> " + descAula + " " + aula.DescricaoAtividade);
                    if(!dicRepet.ContainsKey(descAula))
                    {
                        dicRepet[descAula] = new List<Aula>();
                    }
                    dicRepet[descAula].Add(aula);
                }
                foreach(var opa in dicRepet)
                {
                    //if(opa.Value.total == 1)
                    Debug.WriteLine("DATA: " + opa.Key);
                    foreach(var a in opa.Value)
                    {
                        Debug.WriteLine("   " + a.Id + "   " + a.DescricaoAtividade);
                        if(opa.Value.Count > 1 && a.DescricaoAtividade == String.Empty)
                        {
                            bool keepTrying = false;
                            Debug.WriteLine(" REMOVENDO: " + a.Id);
                            try
                            {
                                controleAulas.DeletaAula((Guid)a.Id);
                            }
                            catch(DataAccessException e)
                            {
                                Debug.WriteLine("*** ERRO: " + e.Message);
                                keepTrying = true;
                            }
                            if(!keepTrying) break;
                        }
                    }
                }
                tot++;
                //break;
            }
            Debug.WriteLine("\nFINAL DO PROCESSAMENTO");
        }

        public void CriarAulasCompletar(Calendario cal)
        {
            try
            {
                //ordena lista da datas
                cal.Datas.Sort();
                //Recebe a lista das turmas
                TurmaBO contadorroleTurmas = new TurmaBO();
                IList<Entities.Turma> listaTurmas = contadorroleTurmas.GetTurmas(cal);
                Util.DataHelper dheper = new BusinessData.Util.DataHelper();
                //Recebe a lista das atividades
                CategoriaAtividadeBO contadorroleAtividades = new CategoriaAtividadeBO();
                IList<CategoriaAtividade> listaAtividades = contadorroleAtividades.GetCategoriaAtividade();
                if (listaAtividades.Count == 0)
                {
                    throw new IndexOutOfRangeException();
                }

                CategoriaAtividade cat = listaAtividades[0];
                string aulaDefault = ConfigurationManager.AppSettings["AulaDefault"];
                foreach (CategoriaAtividade categoria in listaAtividades)
                {
                    if (categoria.Descricao.Equals(aulaDefault))
                        cat = categoria;
                }

                AulaBO contadorroleAulas = new AulaBO();
                Aula aulaAux;

                int totalTurmas = listaTurmas.Count;
                int tot = 1;

                //Percorre todas as turmas na lista
                foreach (Turma turmaAux in listaTurmas)
                {
                    Debug.WriteLine("\nTurma " + tot + " de " + totalTurmas);
                    Debug.WriteLine(turmaAux.Disciplina.Nome + " - " + turmaAux.Numero + " " + turmaAux.Professor.Nome);

                    List<Aula> todasAulas = this.GetAulas(turmaAux.Id);
                    Dictionary<String, bool> dicAulas = new Dictionary<String, bool>();
                    foreach (Aula aula in todasAulas)
                    {
                        string descAula = aula.Data.ToString() + aula.Hora;
                        if (!dicAulas.ContainsKey(descAula)) {
                            dicAulas.Add(descAula, true);
                        }
                    }

                    string horario = turmaAux.DataHora;

                    //dado um horario pucrs(2ab,4cd), exclui os horarios e guarda os dias em array de inteiros
                    string diasPucrs = Regex.Replace(horario, "[a-zA-Z]", String.Empty);

                    int tamanho = diasPucrs.Length;
                    int[] dias = new int[tamanho];
                    for (int i = 0; i < tamanho; i++)
                    {
                        dias[i] = Convert.ToInt32(diasPucrs.Substring(i, 1));
                    }

                    string[] horariosPucrs = horario.Split(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, StringSplitOptions.RemoveEmptyEntries);

                    DateTime aux = cal.InicioG1;

                    while (aux <= cal.FimG2)
                    {
                        for (int i = 0; i < dias.Length; i++)
                        {
                            if ((int)(aux.DayOfWeek) == (dias[i] - 1))
                            {
                                string descAula = aux.ToString() + horariosPucrs[i];
                                if (dicAulas.ContainsKey(descAula)) // aula ja existe?
                                    continue;
                                aulaAux = Aula.newAula(turmaAux, horariosPucrs[i], aux, string.Empty, cat);
                                Debug.WriteLine("  >>> Criando aula: " + descAula);
                                this.InsereAula(aulaAux);
                            }
                        }
                        aux = aux.AddDays(1);
                    }
                    tot++;
                }
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
            Debug.WriteLine("FIM DA CRIACAO DE AULAS");
        }

        public void CriarAulas(Calendario cal)
        {
            try
            {
                //ordena lista da datas
                cal.Datas.Sort();
                //Recebe a lista das turmas
                TurmaBO contadorroleTurmas = new TurmaBO();
                IList<Entities.Turma> listaTurmas = contadorroleTurmas.GetTurmas(cal);
                Util.DataHelper dheper = new BusinessData.Util.DataHelper();
                //Recebe a lista das atividades
                CategoriaAtividadeBO contadorroleAtividades = new CategoriaAtividadeBO();
                IList<CategoriaAtividade> listaAtividades = contadorroleAtividades.GetCategoriaAtividade();
                if (listaAtividades.Count == 0)
                {
                    throw new IndexOutOfRangeException();
                }
                
                CategoriaAtividade cat = listaAtividades[0];
                string aulaDefault = ConfigurationManager.AppSettings["AulaDefault"];
                foreach (CategoriaAtividade categoria in listaAtividades)
                {
                    if (categoria.Descricao.Equals(aulaDefault))
                        cat = categoria;
                }

                AulaBO contadorroleAulas = new AulaBO();
                Aula aulaAux;
                //Percorre todas as turmas na lista
                foreach (Turma turmaAux in listaTurmas)
                {
                    string horario = turmaAux.DataHora;
                    
                    //dado um horario pucrs(2ab,4cd), exclui os horarios e guarda os dias em array de inteiros
                    string diasPucrs = Regex.Replace(horario, "[a-zA-Z]", String.Empty);

                    int tamanho = diasPucrs.Length;
                    int[] dias = new int[tamanho];
                    for (int i = 0; i < tamanho; i++)
                    {
                        dias[i] = Convert.ToInt32(diasPucrs.Substring(i, 1));
                    }

                    string[] horariosPucrs = horario.Split(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    DateTime aux = cal.InicioG1;
                    
                    while (aux <= cal.FimG2)
                    {
                        for(int i =0; i<dias.Length; i++)
                        {
                            if ((int)(aux.DayOfWeek) == (dias[i] - 1))
                            {
                                aulaAux = Aula.newAula(turmaAux, horariosPucrs[i], aux, string.Empty, cat);
                                this.InsereAula(aulaAux);
                            }
                        }
                        aux = aux.AddDays(1);
                    }
                }

                
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }
   
        public List<Aula> GetAulasByCalendario(Guid CalendarioId)
        {
            return dao.GetAulasByCalendario(CalendarioId);
        }

        public List<Aula> GetAulas(Guid TurmaId)
        {
            try
            {
                return dao.GetAulas(TurmaId);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        }

        public Aula GetAula(Guid turmaId, DateTime data, string hora)
        {
            try
            {
                return dao.GetAula(turmaId,data,hora);
            }
            catch (DataAccessException ex)
            {
                throw ex;
            }
        
        }
    }
}
