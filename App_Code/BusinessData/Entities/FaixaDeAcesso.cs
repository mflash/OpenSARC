using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class FaixaDeAcesso
    {
        private int horaInicial;
        private int horaFinal;

        /// <summary>
        /// Inclusive lower boud
        /// </summary>
        public int HorarioInicial
        {
            get { return horaInicial; }
        }
        /// <summary>
        /// Exclusive upper bound
        /// </summary>
        public int HorarioFinal
        {
            get { return horaFinal; }
        }

        
        public FaixaDeAcesso(int horaInicial, int horaFinal)
        {
            this.horaInicial = horaInicial;
            this.horaFinal = horaFinal;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(horaInicial);
            sb.Append(":00 - ");
            if (horaFinal == 24)
            {
                sb.Append("23:59");
            }
            else
            {
                sb.Append(horaFinal);
                sb.Append(":00");
            }

            return sb.ToString();
        }
    }
}
