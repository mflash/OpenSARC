using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class HorarioBloqueado : IEquatable<HorarioBloqueado>
    {
        private string horaInicio;
        private string horaFim;

        public HorarioBloqueado(string hi, string hf)
        {
            horaInicio = hi;
            horaFim = hf;
        }

        public string HoraInicio
        {
            get {return horaInicio;}
            set { horaInicio = value; }
        }

        public string HoraFim
        {
            get { return horaFim; }
            set { horaFim = value; }
        }

        #region IEquatable<HorarioBloqueado> Members

        public bool Equals(HorarioBloqueado other)
        {
            if (this.HoraInicio == other.HoraInicio && this.HoraFim == other.HoraFim)
                return true;
            else
                return false;
        }

        #endregion
    }
}
