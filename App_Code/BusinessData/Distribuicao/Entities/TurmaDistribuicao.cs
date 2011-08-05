using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;

namespace BusinessData.Distribuicao.Entities
{
    public class TurmaDistribuicao: ICollection<BusinessData.Entities.Aula>
    {
        private Turma turma;
        private ICollection<BusinessData.Entities.Aula> aulas;

        public TurmaDistribuicao(Turma turma, ICollection<Aula> aulas)
        {
            this.turma = turma;
            this.aulas = aulas;
        }
        
        public Turma EntidadeTurma
        {
            get { return turma; }
        }

        public BusinessData.Entities.Aula GetAula(DateTime data, string horario)
        {
            foreach (BusinessData.Entities.Aula aula in aulas)
            {
                if ((aula.Data == data) && aula.Hora.ToUpper().Equals(horario.ToUpper()))
                {
                    return aula;
                }
            }
            return null;
        }

        #region ICollection<Aula> Members

        public void Add(BusinessData.Entities.Aula item)
        {
            aulas.Add(item);
        }

        public void Clear()
        {
            aulas.Clear();
        }

        public bool Contains(BusinessData.Entities.Aula item)
        {
            return aulas.Contains(item);
        }

        public void CopyTo(BusinessData.Entities.Aula[] array, int arrayIndex)
        {
            aulas.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return aulas.Count; }
        }

        public bool IsReadOnly
        {
            get { return aulas.IsReadOnly; }
        }

        public bool Remove(BusinessData.Entities.Aula item)
        {
            return aulas.Remove(item);
        }

        #endregion

        #region IEnumerable<Aula> Members

        public IEnumerator<BusinessData.Entities.Aula> GetEnumerator()
        {
            return aulas.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return aulas.GetEnumerator();
        }

        #endregion
    }
}
