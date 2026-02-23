using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    [Serializable]
    public class Usuario: IComparable<Usuario>
    {

        public Usuario(string matr, string nome, string nounid, TipoUsuario tipo)
        {
            this.Matricula = matr;
            this.Nome = nome;
            this.Unidade = nounid;
            this.Tipo = tipo;
        }

        public enum TipoUsuario {
            PROFESSOR, ALUNO_GRAD, ALUNO_LATO, ALUNO_STRICTO, FUNCIONARIO
        };

        public string Matricula { get; set; }
        public string Nome { get; set; }
        public string Unidade { get; set; }
        public TipoUsuario Tipo { get; set; }

        public string TipoUsuarioChar
        {
            get
            {
                switch (Tipo)
                {
                    case TipoUsuario.PROFESSOR:
                        return "P";
                    case TipoUsuario.ALUNO_GRAD:
                        return "G";
                    case TipoUsuario.ALUNO_LATO:
                        return "L";
                    case TipoUsuario.ALUNO_STRICTO:
                        return "S";
                    case TipoUsuario.FUNCIONARIO:
                        return "F";
                    default:
                        return "?";

                }
            }
        }

        public static string TipoUsuarioFull(TipoUsuario tipo)
        {
            switch (tipo)
            {
                case TipoUsuario.PROFESSOR:
                    return "PROFESSOR";
                case TipoUsuario.ALUNO_GRAD:
                    return "GRADUAÇÃO";
                case TipoUsuario.ALUNO_LATO:
                    return "LATO";
                case TipoUsuario.ALUNO_STRICTO:
                    return "STRICTO";
                case TipoUsuario.FUNCIONARIO:
                    return "FUNCIONÁRIO";
                default:
                    return "?";

            }
        }

        public int CompareTo(Usuario other)
        {
            return Matricula.CompareTo(other.Matricula);
        }
    }
}