using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Entities;

namespace BusinessData.Distribuicao.Catalogos
{
    public class ColecaoDias : ICollection<Dia>
    {
        private SortedDictionary<DateTime, Dia> dictDias;
        //private ICollection<Dia> listaDias;

        public ColecaoDias()
        {
            //listaDias = new List<Dia>();
            dictDias = new SortedDictionary<DateTime, Dia>();
        }

        #region ICollection<Dia> Members

        public void Add(Dia item)
        {
            //listaDias.Add(item);
            dictDias.Add(item.Data, item);
        }

        public void Clear()
        {
            //listaDias.Clear();
            dictDias.Clear();
        }

        public bool Contains(Dia item)
        {
            return dictDias.ContainsKey(item.Data);
//            return listaDias.Contains(item);
        }

        public void CopyTo(Dia[] array, int arrayIndex)
        {
 //           listaDias.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            //get { return listaDias.Count; }
            get { return dictDias.Count; }
        }

        public bool IsReadOnly
        {
            //get { return listaDias.IsReadOnly; }
            get { return false; }
        }

        public bool Remove(Dia item)
        {
            return dictDias.Remove(item.Data);
//            return listaDias.Remove(item);
        }

        #endregion

        #region IEnumerable<Dia> Members

        public IEnumerator<Dia> GetEnumerator()
        {
            //return listaDias.GetEnumerator();            
            return dictDias.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            //return listaDias.GetEnumerator();
            return dictDias.GetEnumerator();
        }

        #endregion

        public Dia Find(DateTime data)
        {
            if (dictDias.ContainsKey(data))
                return dictDias[data];
            return null;
            /*
            foreach (Dia d in listaDias)
            {
                if (d.Data.Equals(data))
                    return d;
            }
            return null;
             */
        }
    }
}