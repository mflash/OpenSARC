using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Distribuicao.Entities
{
    public class CategoriaRecurso : BusinessData.Entities.ICategoria, ICollection<Recurso>
    {
        private IList<Recurso> listaRecursos;

        public IList<Recurso> Recursos
        {
            get { return listaRecursos; }
        }
        private BusinessData.Entities.CategoriaRecurso catRec;

        public BusinessData.Entities.CategoriaRecurso EntidadeCategoria
        {
            get { return catRec; }
        }

        public CategoriaRecurso(BusinessData.Entities.CategoriaRecurso catRec, IList<Recurso> recursosCadastrados)
        {
            this.catRec = catRec;
            listaRecursos = recursosCadastrados;
        }

        #region ICollection<Recurso> Members

        public void Add(Recurso item)
        {
            listaRecursos.Add(item);
        }

        public void Clear()
        {
            listaRecursos.Clear();
        }

        public bool Contains(Recurso item)
        {
            return listaRecursos.Contains(item);
        }

        public void CopyTo(Recurso[] array, int arrayIndex)
        {
            listaRecursos.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return listaRecursos.Count; }
        }

        public bool IsReadOnly
        {
            get { return listaRecursos.IsReadOnly; }
        }

        public bool Remove(Recurso item)
        {
            return listaRecursos.Remove(item);
        }


        /// <summary>
        /// Retorna todos os recursos  disponiveis desta categoria na data e horario
        /// especificados. O(n2)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="horario"></param>
        /// <returns></returns>
        public List<Recurso> GetRecursosDisponiveis(DateTime data, string horario)
        {
            List<Recurso> listaAux = new List<Recurso>();
            foreach (Recurso r in this.listaRecursos)
            {
                if (!r.EstaAlocado(data, horario) && r.EstaDisponivel(horario))
                {
                    listaAux.Add(r);
                }
            }
            return listaAux;
        }

        #endregion

        #region IEnumerable<Recurso> Members

        public IEnumerator<Recurso> GetEnumerator()
        {
            return listaRecursos.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return listaRecursos.GetEnumerator();
        }

        #endregion

        #region ICategoria Members

        public string Descricao
        {
            get
            {
                return catRec.Descricao;
            }
            set
            {
                catRec.Descricao = value;
            }
        }

        public Guid Id
        {
            get
            {
                return catRec.Id;
            }
            set
            {
                catRec.Id = value;
            }
        }

        #endregion
    }
}
