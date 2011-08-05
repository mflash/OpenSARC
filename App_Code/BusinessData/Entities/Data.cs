using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class Data :IComparable, IComparable<Data>
    {
        private DateTime date;
        private CategoriaData categoria;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public CategoriaData Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        /// <summary>
        /// Propriedade chamada em datagrids e afins
        /// </summary>
        public String PorExtenso
        {
            get { return this.ToString(); }
        }

        private Data() { }
        private Data(DateTime data, CategoriaData categoria)
        {
            Date = data;
            Categoria = categoria;
        }

        public static Data GetData(DateTime data, CategoriaData categoria)
        {
            return new Data(data, categoria);
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append(date.Day);
            b.Append("/");
            b.Append(date.Month);
            b.Append("/");
            b.Append(date.Year);
            b.Append(" - ");
            b.Append(categoria.Descricao);

            return b.ToString();
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            Data data = (Data)obj;
            return this.Date.CompareTo(data.Date);
        }
        #endregion

        #region IComparable<Data> Members

        public int CompareTo(Data other)
        {
            return this.Date.CompareTo(other.Date);
        }

        #endregion
    }
}
