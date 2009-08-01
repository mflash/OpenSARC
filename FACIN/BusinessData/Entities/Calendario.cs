using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class Calendario : ICalendario, ICloneable, IComparable
    {
        private Guid id;
        private DateTime inicioG1;
        private DateTime inicioG2;
        private DateTime fimG2;
        private int semestre;
        private int ano;
        List<Data> datas;

        public List<Data> Datas
        {
            get { return datas; }
            set { datas = value; }
        }

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Semestre
        {
            get { return semestre; }
            set { semestre = value; }
        }

        public int Ano
        {
            get { return ano; }
            set { ano = value; }
        }

        public DateTime InicioG1
        {
            get { return inicioG1; }
            set { inicioG1 = value; }
        }

        public DateTime InicioG2
        {
            get { return inicioG2; }
            set { inicioG2 = value; }
        }

        public DateTime FimG2
        {
            get { return fimG2; }
            set { fimG2 = value; }
        }

        private Calendario() { }

        private Calendario(Guid id, int semestre, int ano, List<Data> datas, DateTime iG1, DateTime iG2, DateTime fG2)
        {
            Id = id;
            Semestre = semestre;
            Ano = ano;
            Datas = datas;
            inicioG1 = iG1;
            inicioG2 = iG2;
            fimG2 = fG2;
        }

        /// <summary>
        /// Retorna um Calendario com os valores especificados
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="nome">Nome</param>
        /// <returns></returns>
        
        public static Calendario GetCalendario(Guid id, int semestre, int ano, List<Data> datas, DateTime iG1, DateTime iG2, DateTime fG2)
        {
            return new Calendario(id, semestre, ano, datas, iG1, iG2, fG2);
        }

        public static Calendario NewCalendario(int semestre, int ano, DateTime iG1, DateTime iG2, DateTime fG2)
        {
            return new Calendario(Guid.NewGuid(), semestre, ano, null, iG1, iG2, fG2);
        }

        /// <summary>
        /// Propriedade para ser chamada nos Datagrids/etc...
        /// O ToString() seria chamado por padrão, porém, quando seta-se o 
        /// DataValueField para ID, ele automaticamente seta o DataTextField
        /// para ID também. Neste caso, precisa-se setar o DataTextField para o ToString()
        /// SÓ q ToString() é um método, e não uma propriedade. Cocholão FOH TEH WIN!
        /// </summary>
        public string PorExtenso
        {
            get { return this.ToString(); }
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append(Ano);
            b.Append("/");
            b.Append(Semestre);

            return b.ToString();
        }



        #region ICloneable Members

        public object Clone()
        {
            return Calendario.GetCalendario(id, semestre, ano, datas, inicioG1, inicioG2, fimG2);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            Calendario other = (Calendario) obj;
            DateTime dataX = new DateTime(this.Ano, this.Semestre * 6, 1);
            DateTime dataY = new DateTime(other.Ano, other.Semestre * 6, 1);
            return DateTime.Compare(dataX, dataY);            
        }

        #endregion
    }

    public class ComparadorCalendario : Comparer<Calendario>
    {

        public override int Compare(Calendario x, Calendario y)
        {
            DateTime dataX = new DateTime(x.Ano, x.Semestre * 6, 1);
            DateTime dataY = new DateTime(y.Ano, y.Semestre * 6, 1);
            return DateTime.Compare(dataX, dataY);
        }
    }



}
