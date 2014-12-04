using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;

namespace BusinessData.Entities
{
    public class HorariosEvento : IEquatable<HorariosEvento>, IComparable<HorariosEvento>,ICloneable
    {
        static string[] horariosPUCRS = Distribuicao.Entities.Horarios.horariosPUCRS;
        private Guid horariosEventoId;
        private Evento eventoId;
        private DateTime data;
        private string horarioInicio;
        private string horarioFim;

        public Guid HorariosEventoId { get { return horariosEventoId; } set { horariosEventoId = value; } }
        public Evento EventoId { get { return eventoId; } set { eventoId = value; } }
        public DateTime Data { get { return data; } set { data = value; } }
        public string HorarioInicio { get { return horarioInicio; } set { horarioInicio = value; } }
        public string HorarioFim { get { return horarioFim; } set { horarioFim = value; } }

        public HorariosEvento()
        { }

        private HorariosEvento(Guid horariosEventoId, Evento eventoId, DateTime data, string horarioInicio, string horarioFim)
        {
            HorariosEventoId = horariosEventoId;
            EventoId = eventoId;
            Data = data;
            HorarioInicio = horarioInicio;
            HorarioFim = horarioFim;
        }

        public static HorariosEvento NewHorariosEvento(DateTime data, Evento eventoId, string horarioInicio, string horarioFim)
        {
            return new HorariosEvento(Guid.NewGuid(), eventoId, data, horarioInicio, horarioFim);
        }

        public static HorariosEvento GetHorariosEvento(Guid horariosEvento, Evento eventoId, DateTime data, string horarioInicio, string horarioFim)
        {
            return new HorariosEvento(horariosEvento, eventoId, data, horarioInicio, horarioFim);
        }

        public bool Equals(HorariosEvento other)
        {
            if (this.Data == other.Data && this.HorarioFim == other.HorarioFim && this.HorarioInicio == other.HorarioInicio)
                return true;
            else
                return false;
        }

        public int CompareTo(HorariosEvento other)
        {
            if (this.Data.CompareTo(other.Data) == 0)
                if (this.HorarioInicio.CompareTo(other.HorarioInicio) == 0)
                    return this.HorarioFim.CompareTo(other.HorarioFim);
                else
                    return this.HorarioInicio.CompareTo(other.HorarioInicio);
            else
                return this.Data.CompareTo(other.Data);
        }

        public static string[] HorariosEntre(string horarioInicial, string horarioFim)
        {
            int indice = 0;
            for (; indice < horariosPUCRS.Length; indice++)
            {
                if (horarioInicial.Equals(horariosPUCRS[indice]))
                    break;
            }

            int final = 0;
            for (; final < horariosPUCRS.Length; final++)
            {
                if (horarioFim.Equals(horariosPUCRS[final]))
                    break;
            }

            int tam = final - indice;
            string[] horarios = new string[tam + 1];


            for (int i = 0; i <= tam; i++)
            {
                horarios[i] = horariosPUCRS[indice++];
            }

            return horarios;
        }

        #region ICloneable Members

        public object Clone()
        {
            return HorariosEvento.GetHorariosEvento(horariosEventoId, (Evento)eventoId.Clone(),data,(string)horarioFim.Clone(), (string)horarioFim.Clone());
        }

        #endregion
    }
}
