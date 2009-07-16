using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Entities;

namespace BusinessData.Distribuicao.Catalogos
{
    public class ColecaoDias : ICollection<Dia>
    {
        private ICollection<Dia> listaDias;

        public ColecaoDias()
        {
            listaDias = new List<Dia>();
        }
        #region ICollection<Dia> Members

        public void Add(Dia item)
        {
            listaDias.Add(item);
        }

        public void Clear()
        {
            listaDias.Clear();
        }

        public bool Contains(Dia item)
        {
            return listaDias.Contains(item);
        }

        public void CopyTo(Dia[] array, int arrayIndex)
        {
            listaDias.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return listaDias.Count; }
        }

        public bool IsReadOnly
        {
            get { return listaDias.IsReadOnly; }
        }

        public bool Remove(Dia item)
        {
            return listaDias.Remove(item);
        }

        #endregion

        #region IEnumerable<Dia> Members

        public IEnumerator<Dia> GetEnumerator()
        {
            return listaDias.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return listaDias.GetEnumerator();
        }

        #endregion

        public Dia Find(DateTime data)
        {
            foreach (Dia d in listaDias)
            {
                if (d.Data.Equals(data))
                    return d;
            }
            return null;
        }
    }
}