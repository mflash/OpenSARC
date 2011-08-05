using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public abstract class CategoriaBase: IEquatable<CategoriaBase>, BusinessData.Entities.ICategoria
    {
        private Guid id;
        private string descricao;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }

        public CategoriaBase() { }
        public CategoriaBase(Guid id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }
        public override string ToString()
        {
            return Descricao;
        }


        #region IEquatable<CategoriaBase> Members

        public bool Equals(CategoriaBase other)
        {
            if (this.Id == other.Id)
                return true;
            else
                return false;
        }

        #endregion
    }
}
