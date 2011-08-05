using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Entities;

namespace BusinessData.Distribuicao.Catalogos
{
    public class ColecaoCategoriaDeRecursos:ICollection<CategoriaRecurso>
    {

        private IList<CategoriaRecurso> listaCategorias;

        public ColecaoCategoriaDeRecursos()
        {
            listaCategorias = new List<CategoriaRecurso>();
        }
        #region ICollection<CategoriaRecurso> Members

        public void Add(CategoriaRecurso item)
        {
            listaCategorias.Add(item);
        }

        public void Clear()
        {
            listaCategorias.Clear();
        }

        public bool Contains(CategoriaRecurso item)
        {
            return listaCategorias.Contains(item);
        }

        public void CopyTo(CategoriaRecurso[] array, int arrayIndex)
        {
            listaCategorias.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return listaCategorias.Count; }
        }

        public bool IsReadOnly
        {
            get { return listaCategorias.IsReadOnly; }
        }

        public bool Remove(CategoriaRecurso item)
        {
            return listaCategorias.Remove(item);
        }

        #endregion

        #region IEnumerable<CategoriaRecurso> Members

        public IEnumerator<CategoriaRecurso> GetEnumerator()
        {
            return listaCategorias.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return listaCategorias.GetEnumerator();
        }

        #endregion

        public CategoriaRecurso Find(BusinessData.Entities.CategoriaRecurso categoria)
        {
            foreach(CategoriaRecurso cat in listaCategorias)
            {
                if (cat.EntidadeCategoria.Equals(categoria))
                {
                    return cat;
                }
            }
            return null;
        }

    }
}
