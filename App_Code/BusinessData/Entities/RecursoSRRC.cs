using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class RecursoSRRC : IComparable<RecursoSRRC>
    {
        public RecursoSRRC(string id, string descricao, string categoria, char tipo, string abrev)
        {
            Id = id;
            Descricao = descricao;
            Categoria = categoria;
            Abrev = abrev;
            Tipo = tipo;
        }

        public enum StatusRecurso { RETIRADO, DISPONIVEL };
        public string Id { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public string Abrev { get; set; }
        public char Tipo { get; set; }
        public string LastUser { get; set; }

        public Usuario.TipoUsuario TipoUser { get; set; }
        public DateTime LastTime { get; set; }
        public StatusRecurso Status { get; set; }

        public double PosX { get; set; }
        public double PosY { get; set; }
        //private GrupoRecursos grupo;

        public string AbrevLastUser
        {
            get
            {
                if (LastUser == null) return "";
                //return LastUser;
                string[] aux = LastUser.Trim().Split(' ');
                return aux[0][0] + ". " + aux[aux.Length-1];
            }
        }

        public int CompareTo(RecursoSRRC other)
        {
            return Abrev.CompareTo(other.Abrev);
        }

        public override string ToString()
        {
            return Id + " - " + Descricao + " (" + Abrev + " - " + Tipo + " - " + LastUser + " - " + Categoria;
        }
    }
}
