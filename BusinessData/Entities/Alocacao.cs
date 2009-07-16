using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class Alocacao : IComparable<Alocacao>, IEquatable<Alocacao>
    {
        private DateTime data;
        private string horario;
        private Aula aula;
        private Evento evento;
        private Recurso recurso;

        public Alocacao(Recurso r, DateTime d, string h, Aula a, Evento e)
        {
            recurso = r;
            data = d;
            horario = h;
            aula = a;
            evento = e;
        }

        public Recurso Recurso
        {
            get { return recurso; }
            set { recurso = value; }
        }
        public Evento Evento
        {
            get { return evento; }
            set { evento = value; }
        }
        public Aula Aula
        {
            get { return aula; }
            set { aula = value; }
        }
        public string Horario
        {
            get { return horario; }
            set { horario = value; }
        }
        public DateTime Data
        {
            get { return data; }
            set { data = value; }
        }

        public static Alocacao newAlocacao(Recurso r, DateTime d, string h, Aula a, Evento e)
        {
            return new Alocacao(r, d, h, a, e);
        }

        public bool Equals(Alocacao other)
        {
            if (this.Data == other.Data)
                return true;
            else
                return false;
        }

        public int CompareTo(Alocacao other)
        {
            return this.Data.CompareTo(other.Data);
        }
    }
}
