using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using BusinessData.Util;
using BusinessData.Entities;
using System.Text.RegularExpressions;
using BusinessData.DataAccess;
//Log
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;
using System.Web.Security;

namespace BusinessData.BusinessLogic
{
    public class AulaBO
    {
        private AulasDAO dao;
        private Usuario usr;

        public AulaBO()
        {

                dao = new BusinessData.DataAccess.AulasDAO();

            usr = new Usuario();
        }

        public void UpdateAula(Entities.Aula aula)
        {
                try
                {
                    dao.AtualizaAula(aula);
                    //instancia o usuario logado
                    MembershipUser user = Membership.GetUser();
                    //instancia o log
                    LogEntry log = new LogEntry();
                    //monta log
                    log.Message = "Id: " + aula.Id + "; Usuário: " + user.UserName;
                    log.TimeStamp = DateTime.Now;
                    log.Severity = TraceEventType.Information;
                    log.Title = "Update Aula";
                    log.MachineName = Dns.GetHostName();
                    //guarda log no banco
                    Logger.Write(log);
                }
                catch (DataAccess.DataAccessException ex)
                {
                    throw ex;
                }
        }

        public void DeletaAula(Guid id)
        {
            if (usr.IsAdmin())
            {

                    dao.RemoveAula(id);

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

                    dao.InsereAula(aula);

            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }

        public Aula GetAulaById(Guid id)
        {

                return dao.GetAula(id);


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
                foreach (CategoriaAtividade categoria in listaAtividades)
                {
                    if (categoria.Descricao.Equals("Aula"))
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
                foreach (CategoriaAtividade categoria in listaAtividades)
                {
                    if (categoria.Descricao.Equals("Aula"))
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
   
        public List<Aula> GetAulas()
        {
            return dao.GetAulas();
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
