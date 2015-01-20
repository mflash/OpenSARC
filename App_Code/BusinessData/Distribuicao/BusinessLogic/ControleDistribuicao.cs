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
            Debug.WriteLine("Calendario: " + calAtual.EntidadeCalendario.PorExtenso);
            Random rand = new Random();
            double soma = 0.0;

            Dictionary<TurmaDistribuicao, SatisfacaoTurma> satTurmas = new Dictionary<TurmaDistribuicao, SatisfacaoTurma>();
            //Preenche a colecao para todas as turmas do semestre
//            Debug.WriteLine("Criando coleção de satisfação de turmas...");
            foreach (TurmaDistribuicao t in calAtual.Turmas)
            {
                satTurmas.Add(t, new SatisfacaoTurma(t));
            }
            
            //Para cada prioridade de requisicao
            for (int prioridadePedidos = 1; prioridadePedidos <= calAtual.Categorias.Count; prioridadePedidos++)
            {
                Debug.WriteLine("Prioridade: " + prioridadePedidos + " de "+calAtual.Categorias.Count);
                foreach (CategoriaRecurso cat in calAtual.Categorias)
                {
                    //Inicializa os dados
                    Debug.WriteLine("  Categoria: " + cat.Descricao);
                    foreach (TurmaDistribuicao turma in calAtual.Turmas)
                    {
                        satTurmas[turma].Atendimentos = 0;
                        satTurmas[turma].Pedidos = 0;
                        satTurmas[turma].Prioridade = 0;
                    }

//                    Debug.WriteLine("Obtendo requisições para categoria e prioridade...");
                    requisicoes = calAtual.Requisicoes.GetRequisicoes(prioridadePedidos, cat, calAtual.Dias);

                    soma = 0.0;
                    double prioridadeAux = 0.0;
                    List<double> listaPrioridadesAux = new List<double>();

  //                  Debug.WriteLine("Calculando total de requisições para as turmas...");
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
    //                Debug.WriteLine("Normalizando prioridades...");
                    foreach (Requisicao req in requisicoes)
                    {
                        prioridadeAux = req.Turma.EntidadeTurma.Disciplina.Categoria.Prioridades[req.CategoriaRecurso.EntidadeCategoria];
                        satTurmas[req.Turma].Prioridade = prioridadeAux / soma;
                    }

                    Debug.WriteLine("*** PROCESSAMENTO DOS DIAS ***");
                    int totalDias = calAtual.Dias.Count;
                    int curDia = 1;
                    foreach (Dia dia in calAtual.Dias)
                    {
                        if(curDia++ % 30 == 0)
                            Debug.WriteLine(">>> Dia: " + dia.Data);
                        foreach (Horarios.HorariosPUCRS horario in dia.Horarios)
                        {
                            //Debug.WriteLine("Horario: " + horario.ToString());
                            // Todos os recursos disponíveis nesta categoria, dia e horário
                            //Debug.WriteLine("   Obtendo recursos disponíveis (sem filtrar)");
                            List<Recurso> auxRecursosDisponiveis = cat.GetRecursosDisponiveis(dia.Data, horario.ToString());
                            // Todas as requisições neste dia e horário
                            //Debug.WriteLine("   Obtendo requisições neste dia e horário...");
                            IList<Requisicao> reqDiaHoraGeral = calAtual.Requisicoes.GetRequisicoes(dia.Data, horario);

                            // Lista para armazenar os REALMENTE disponíveis
                            // (depois de remover as dependências de salas duplas, etc)
                            List<Recurso> recursosDisponiveis = new List<Recurso>();

                            //Debug.WriteLine("   Removendo recursos bloqueados por dependências...");
                            foreach (Recurso rec in auxRecursosDisponiveis)
                            {
                                bool bloqueado = false;
                                foreach (Requisicao req in reqDiaHoraGeral)
                                {
                                    if (req.Recurso == null)
                                        continue; // Não alocado, não interfere
                                    Guid bloqueia1 = req.Recurso.Bloqueia1;
                                    Guid bloqueia2 = req.Recurso.Bloqueia2;
                                    // Se o recurso alocado é um dos que bloqueiam
                                    // este...
                                    if(rec.EntidadeRecurso.Id == bloqueia1 ||
                                        rec.EntidadeRecurso.Id == bloqueia2)
                                    {
                                        // Não podemos utilizá-lo!
                                        Debug.WriteLine("!!! Recurso " + rec.EntidadeRecurso.Descricao + " bloqueado por dependência");
                                        bloqueado = true;
                                        break;
                                    }
                                }
                                if (!bloqueado)
                                    recursosDisponiveis.Add(rec);
                            }

                            IList<Requisicao> requisicoesDiaHora = requisicoes.GetRequisicoes(dia.Data, horario);
                            // Se houver recursos suficientes para atender os pedidos
                            // entrega um para cada

                            if (requisicoesDiaHora.Count <= recursosDisponiveis.Count)
                            {
                                //Debug.WriteLine("   Atendendo TODOS os pedidos deste dia e horário!");
                                for (int i = 0; i < requisicoesDiaHora.Count; i++)
                                {
                                    recursosDisponiveis[i].Aloca(dia.Data, horario.ToString(), requisicoesDiaHora[i].Turma);
                                    satTurmas[requisicoesDiaHora[i].Turma].Atendimentos++;
                                    requisicoesDiaHora[i].EstaAtendido = true;
                                    requisicoesDiaHora[i].Recurso = recursosDisponiveis[i].EntidadeRecurso;
                                    /*
                                     * Não precisa: são os mesmos objetos nas duas listas
                                    foreach(Requisicao reqGeral in reqDiaHoraGeral)
                                        if(reqGeral.Id == requisicoesDiaHora[i].Id)
                                        {
                                            reqGeral.Recurso = recursosDisponiveis[i].EntidadeRecurso;
                                        }
                                     */
                                }
                            }
                            else
                            {
                                //Debug.WriteLine("   Tentando atender...");
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
                                    //Debug.WriteLine("   >>> Atendendo:" + rec.EntidadeRecurso.Descricao);
                                    rec.Aloca(dia.Data, horario.ToString(), turmaEscolhida);
                                    satTurmas[turmaEscolhida].Atendimentos++;
                                    reqEscolhida.EstaAtendido = true;
                                    reqEscolhida.Recurso = rec.EntidadeRecurso;
                                    /* Não precisa: são os mesmos objetos nas
                                     * duas listas
                                    // Atualiza a lista geral deste dia e horário
                                    foreach (Requisicao reqGeral in reqDiaHoraGeral)
                                        if (reqGeral.Id == reqEscolhida.Id)
                                        {
                                            reqGeral.Recurso = rec.EntidadeRecurso;
                                        }
                                     */

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
            Debug.WriteLine("");
            Debug.WriteLine("*** FIM DO PROCESSAMENTO ***");
            Debug.WriteLine("");
            //Persiste as alocacoes na base de dados
            BusinessData.BusinessLogic.AlocacaoBO controleAlocacoes = new BusinessData.BusinessLogic.AlocacaoBO();
            foreach (CategoriaRecurso catRec in calAtual.Categorias)
            {
                foreach (Recurso rec in catRec)
                {
                    Debug.WriteLine("Recurso: "+rec.EntidadeRecurso.Descricao);
                    foreach(BusinessData.Entities.Alocacao aloc in rec.Alocacoes)
                    {
                        string aux = "";
                        if(aloc.Aula != null)
                            aux = aloc.Aula.DescricaoAtividade;
                        else if(aloc.Evento != null)
                            aux = aloc.Evento.Descricao + "("+aloc.Evento.Responsavel+")";
                        
                        Debug.WriteLine(" Alocando: "+aloc.Data.ToShortDateString()+
                            " "+aloc.Horario.ToString()+" - "+aux);
                        //controleAlocacoes.UpdateAlocacao(aloc);
                    }
                }
            }
            return;
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
