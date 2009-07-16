using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Entities;

namespace BusinessData.Distribuicao.Catalogos
{
    public class ColecaoRequisicoes : ICollection<Requisicao>
    {

        private List<Requisicao> listaRequisicoes;

        public ColecaoRequisicoes GetRequisicoes(int prioridade, CategoriaRecurso cat, ColecaoDias dias)
        {
            ColecaoRequisicoes requisicoes = new ColecaoRequisicoes();
            Requisicao req;
            bool anteriorAtendido = false;
            DateTime data;

            
                      
                    for(int i=0; i< listaRequisicoes.Count; i++)
                    {
                        req = listaRequisicoes[i];
                        if (req.Prioridade != prioridade) continue;

                        //Verifica se alguma requisicao de maior prioridade feita pela turma
                        //já foi atendida
                        for (int j = 1; j < prioridade; j++)
                        {
                            int aux = i - j;
                            if (aux>=0 && listaRequisicoes[aux].Turma.Equals(listaRequisicoes[i].Turma) && listaRequisicoes[aux].EstaAtendido)
                            {
                                anteriorAtendido = true;
                                break;
                            }
                            anteriorAtendido = false;
                        }                
                        
                        if (req.CategoriaRecurso.Equals(cat) && req.Prioridade == prioridade && !anteriorAtendido)
                        {
                            requisicoes.Add(req);
                        }
                    }
              
            return requisicoes;
        }

        public IList<Requisicao> GetRequisicoes(DateTime data, Horarios.HorariosPUCRS horario)
        {
            IList<Requisicao> requisicoes = new List<Requisicao>();

            foreach (Requisicao req in listaRequisicoes)
            {
                if (req.Dia.Data.Equals(data) && req.Horario.Equals(horario))
                {
                    requisicoes.Add(req);
                }
            }
            return requisicoes;
        }

        public ColecaoRequisicoes()
        {
            listaRequisicoes = new List<Requisicao>();
        }
        #region ICollection<Requisicao> Members
        public void Add(Requisicao item)
        {
            listaRequisicoes.Add(item);
        }

        public void Clear()
        {
            listaRequisicoes.Clear();
        }

        public bool Contains(Requisicao item)
        {
            return listaRequisicoes.Contains(item);
        }

        public void CopyTo(Requisicao[] array, int arrayIndex)
        {
            listaRequisicoes.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return listaRequisicoes.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Requisicao item)
        {
            return listaRequisicoes.Remove(item);
        }

        #endregion

        #region IEnumerable<Requisicao> Members

        public IEnumerator<Requisicao> GetEnumerator()
        {
            return listaRequisicoes.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return listaRequisicoes.GetEnumerator();
        }

        #endregion
    }
}