using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class CategoriaRecurso : CategoriaBase, System.IEquatable<CategoriaRecurso>, IComparable<CategoriaRecurso>
    {
        public bool Disponivel { get; set; }

        public CategoriaRecurso() { }
        public CategoriaRecurso(Guid id, string descricao, bool disponivel=true)
        {
            base.Id = id;
            base.Descricao = descricao;
            Disponivel = disponivel;

        }

        /// <summary>
        /// Returna uma Categoria Recurso com os valores especificados
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        public static CategoriaRecurso GetCategoriaRecurso(Guid id, string descricao)
        {
            return new CategoriaRecurso(id, descricao, true);
        }

        public static CategoriaRecurso GetCategoriaRecurso(Guid id, string descricao, bool disponivel)
        {
            return new CategoriaRecurso(id, descricao, disponivel);
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
