using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Distribuicao.Entities
{
    public class Dia
    {
        private DateTime data;
        private ICollection<Horarios.HorariosPUCRS> horarios;

        public Dia(DateTime data)
        {
            this.Data = data;

            horarios = new List<Horarios.HorariosPUCRS>();
            for (int i = 0; i < 8; i++)
            {
                horarios.Add((Horarios.HorariosPUCRS)i);
            }
        }

        public ICollection<Horarios.HorariosPUCRS> Horarios
        {
            get { return horarios; }
        }

        public DateTime Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}