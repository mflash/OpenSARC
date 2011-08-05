using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Entities;

namespace BusinessData.Distribuicao.Catalogos
{
    public class ColecaoTurmas:ICollection<TurmaDistribuicao>
    {
        private ICollection<TurmaDistribuicao> listaTurmas;

        public ColecaoTurmas()
        {
            listaTurmas = new List<TurmaDistribuicao>();
        }
        #region ICollection<TurmaDistribuicao> Members

        public void Add(TurmaDistribuicao item)
        {
            listaTurmas.Add(item);
        }

        public void Clear()
        {
            listaTurmas.Clear();
        }

        public bool Contains(TurmaDistribuicao item)
        {
            return listaTurmas.Contains(item);
        }

        public void CopyTo(TurmaDistribuicao[] array, int arrayIndex)
        {
            listaTurmas.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return listaTurmas.Count; }
        }

        public bool IsReadOnly
        {
            get { return listaTurmas.IsReadOnly; }
        }

        public bool Remove(TurmaDistribuicao item)
        {
            return listaTurmas.Remove(item);
        }

        #endregion

        #region IEnumerable<Turma> Members

        public IEnumerator<Entities.TurmaDistribuicao> GetEnumerator()
        {
            return listaTurmas.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return listaTurmas.GetEnumerator();
        }

        #endregion

        public TurmaDistribuicao Find(BusinessData.Entities.Turma t)
        {
            foreach (TurmaDistribuicao turma in listaTurmas)
            {
                if (turma.EntidadeTurma.Equals(t))
                {
                    return turma;
                }
            }
            return null;
        }
    }
}
