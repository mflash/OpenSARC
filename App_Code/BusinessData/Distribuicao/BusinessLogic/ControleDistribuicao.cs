using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Entities;
using BusinessData.Distribuicao.Catalogos;
using System.Diagnostics;
//Log
//using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Web.Security;

namespace BusinessData.Distribuicao.BusinessLogic
{
    public class ControleDistribuicao
    {
        public void DistribuirRecursos(int ano, int semestre)
        {
           ControleCalendario calendarios = new ControleCalendario();
           ControleRequisicoes controleRequisicoes = new ControleRequisicoes();

            ColecaoRequisicoes requisicoes;
            Calendario calAtual = calendarios.GetCalendario(ano, semestre);
            Random rand = new Random();
            double soma = 0.0;

            Dictionary<TurmaDistribuicao, SatisfacaoTurma> satTurmas = new Dictionary<TurmaDistribuicao, SatisfacaoTurma>();
            //Preenche a colecao para todas as turmas do semestre
            foreach (TurmaDistribuicao t in calAtual.Turmas)
            {
                satTurmas.Add(t, new SatisfacaoTurma(t));
            }
            
            //Para cada prioridade de requisicao
            for (int prioridadePedidos = 1; prioridadePedidos <= calAtual.Categorias.Count; prioridadePedidos++)
            {
                foreach (CategoriaRecurso cat in calAtual.Categorias)
                {
                    //Inicializa os dados
                    foreach (TurmaDistribuicao turma in calAtual.Turmas)
                    {
                        satTurmas[turma].Atendimentos = 0;
                        satTurmas[turma].Pedidos = 0;
                        satTurmas[turma].Prioridade = 0;
                    }

                    requisicoes = calAtual.Requisicoes.GetRequisicoes(prioridadePedidos, cat, calAtual.Dias);

                    soma = 0.0;
                    double prioridadeAux = 0.0;
                    List<double> listaPrioridadesAux = new List<double>();

                    //Calcula o numero de requisicoes para as turmas
                    foreach (Requisicao req in requisicoes)
                    {
                        satTurmas[req.Turma].Pedidos++;
                        //Calcula o total para a normalizacao
                        prioridadeAux = req.Turma.EntidadeTurma.Disciplina.Categoria.Prioridades[cat.EntidadeCategoria];
                        if (!listaPrioridadesAux.Contains(prioridadeAux))
                        {
                            soma += prioridadeAux;
                            listaPrioridadesAux.Add(prioridadeAux);
                        }
                    }

                    //Normaliza as prioridades entre categorias de disciplina e categorias de recurso atual para
                    //um total de 100%
                    foreach (Requisicao req in requisicoes)
                    {
                        prioridadeAux = req.Turma.EntidadeTurma.Disciplina.Categoria.Prioridades[req.CategoriaRecurso.EntidadeCategoria];
                        satTurmas[req.Turma].Prioridade = prioridadeAux / soma;
                    }

                    foreach (Dia dia in calAtual.Dias)
                    {
                        foreach (Horarios.HorariosPUCRS horario in dia.Horarios)
                        {
                            List<Recurso> recursosDisponiveis = cat.GetRecursosDisponiveis(dia.Data, horario.ToString());

                            IList<Requisicao> requisicoesDiaHora = requisicoes.GetRequisicoes(dia.Data, horario);
                            //Se houverem recursos suficientes para atnder os pedios
                            //entrega um para cada
                            if (requisicoesDiaHora.Count <= recursosDisponiveis.Count)
                            {
                                for (int i = 0; i < requisicoesDiaHora.Count; i++)
                                {
                                    recursosDisponiveis[i].Aloca(dia.Data, horario.ToString(), requisicoesDiaHora[i].Turma);
                                    satTurmas[requisicoesDiaHora[i].Turma].Atendimentos++;
                                    requisicoesDiaHora[i].EstaAtendido = true;
                                }
                            }
                            else
                            {
                                SortedList<double, List<Requisicao>> listaPrioridades = new SortedList<double, List<Requisicao>>();
                                double prioridadeSelecionada = 0.0;
                                //Cria os Conjuntos prioridade/Requisicao
                                foreach (Requisicao req in requisicoesDiaHora)
                                {
                                    //prioridadeSelecionada = req.Turma.EntidadeTurma.Disciplina.Categoria.Prioridades[cat.EntidadeCategoria];
                                    prioridadeSelecionada = satTurmas[req.Turma].Prioridade;
                                    if (!listaPrioridades.Keys.Contains(prioridadeSelecionada))
                                    {
                                        listaPrioridades.Add(prioridadeSelecionada, new List<Requisicao>());
                                    }
                                    listaPrioridades[prioridadeSelecionada].Add(req);
                                }

                                foreach (Recurso rec in recursosDisponiveis)
                                {
                                    #region Sorteia em qual conjunto irá efetuar a distribuicao
                                    double sorteado = rand.NextDouble();
                                    prioridadeSelecionada = double.PositiveInfinity;

                                    //Se for o primeiro item da lista
                                    if (sorteado <= listaPrioridades.Keys[0])
                                    {
                                        prioridadeSelecionada = listaPrioridades.Keys[0];
                                    }

                                    else
                                    {
                                        for (int i = 1; i < listaPrioridades.Keys.Count - 1; i++)
                                        {
                                            if (sorteado <= listaPrioridades.Keys[i] + listaPrioridades.Keys[i - 1])
                                            {
                                                prioridadeSelecionada = listaPrioridades.Keys[i];
                                            }
                                        }
                                    }
                                    //se for o ultimo da lista
                                    if (prioridadeSelecionada == double.PositiveInfinity)
                                    {
                                        prioridadeSelecionada = listaPrioridades.Keys[listaPrioridades.Keys.Count - 1];
                                    }
                                    //-----------------------------------------------------------
                                    #endregion
                                    double satMin = 1;
                                    double best = double.PositiveInfinity;

                                    foreach (Requisicao req in listaPrioridades[prioridadeSelecionada])
                                    {
                                        if (satTurmas[req.Turma].Satisfacao < satMin)
                                        {
                                            satMin = satTurmas[req.Turma].Satisfacao;
                                        }
                                    }

                                    TurmaDistribuicao turmaEscolhida = null;
                                    Requisicao reqEscolhida = null;
                                    foreach (Requisicao req in listaPrioridades[prioridadeSelecionada])
                                    {
                                        //Se a satisfacao da turma atual foi a menor e ela fez o menor numero de pedidos é selecionada
                                        if ((satTurmas[req.Turma].Satisfacao == satMin) && (satTurmas[req.Turma].Pedidos < best))
                                        {
                                            best = satTurmas[req.Turma].Pedidos;
                                            turmaEscolhida = req.Turma;
                                            reqEscolhida = req;
                                        }
                                    }

                                    
                                    //Atende uma requisicao
                                    rec.Aloca(dia.Data, horario.ToString(), turmaEscolhida);
                                    satTurmas[turmaEscolhida].Atendimentos++;
                                    reqEscolhida.EstaAtendido = true;

                                    //Remove requisicao do grupo de requisicoes
                                    listaPrioridades[prioridadeSelecionada].Remove(reqEscolhida);
                                    
                                    //Se nao houverem mais requisicoes no grupo selecionado,o remove
                                    if (listaPrioridades[prioridadeSelecionada].Count == 0)
                                    {
                                        listaPrioridades.Remove(prioridadeSelecionada);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Persiste as alocacoes na base de dados
            BusinessData.BusinessLogic.AlocacaoBO controleAlocacoes = new BusinessData.BusinessLogic.AlocacaoBO();
            foreach (CategoriaRecurso catRec in calAtual.Categorias)
            {
                foreach (Recurso rec in catRec)
                {
                    foreach(BusinessData.Entities.Alocacao aloc in rec.Alocacoes)
                    {
                        controleAlocacoes.UpdateAlocacao(aloc);
                    }
                }
            }


            BusinessData.BusinessLogic.RequisicoesBO cReq = new BusinessData.BusinessLogic.RequisicoesBO();
            foreach (Requisicao req in calAtual.Requisicoes)
            {
                if (req.EstaAtendido)
                {
                    cReq.SetAtendida(req.Id);
                }
            }

            ///End
            ///
            //LOG
            //instancia o usuario logado
            //MembershipUser user = Membership.GetUser();
            //instancia o log
            //LogEntry log = new LogEntry();
            //monta log
            //log.Message = "Calendário: " + calAtual.Ano + "/" + calAtual.Semestre + "; Administrador: " + user.UserName;
            //log.TimeStamp = DateTime.Now;
            //log.Severity = TraceEventType.Information;
            //log.Title = "Iniciar Semestre";
            //log.MachineName = Dns.GetHostName();
            //guarda log no banco
            //Logger.Write(log);
        }
    }
}
