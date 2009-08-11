using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class CategoriaRecurso : CategoriaBase, System.IEquatable<CategoriaRecurso>, IComparable<CategoriaRecurso>
    {
        public CategoriaRecurso() { }
        public CategoriaRecurso(Guid id, string descricao)
        {
            base.Id = id;
            base.Descricao = descricao;
        }

        /// <summary>
        /// Returna uma Categoria Recurso com os valores especificados
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        public static CategoriaRecurso GetCategoriaRecurso(Guid id, string descricao)
        {
            return new CategoriaRecurso(id, descricao);
        }

        /// <summary>
        /// Cria uma nova Categoria Recurso contendo um Guid
        /// </summary>
        /// <param name="descricao">Descrição</param>
        /// <returns></returns>
        public static CategoriaRecurso NewCategoriaRecurso(string descricao)
        {
            return new CategoriaRecurso(Guid.NewGuid(), descricao);
        }

        public override string ToString()
        {
            return Descricao;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region IEquatable<CategoriaRecurso> Members

        public bool Equals(CategoriaRecurso other)
        {
            return this.Id.Equals(other.Id);
        }

        #endregion



        #region IComparable<CategoriaRecurso> Members

        public int CompareTo(CategoriaRecurso other)
        {
            return Descricao.CompareTo(other.Descricao);
        }

        #endregion
    }
}
