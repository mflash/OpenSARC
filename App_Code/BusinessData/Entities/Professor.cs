using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    [Serializable]
    public class Professor : PessoaBase, ICloneable
    {
        public string Curto { get; set; }

        private Professor(Guid id, string matricula, string nome, string email, string curto=null)
            : base(id, matricula, nome, email )
        {
            Curto = curto;
        }

        public static Professor NewProfessor(string matricula, string nome, string email, string curto=null)
        {
            return new Professor(Guid.NewGuid(), matricula, nome, email, curto);
        }

        public static Professor NewProfessor(string matricula)
        {
            return new Professor(Guid.NewGuid(), matricula, "", "", null);
        }

        public static Professor GetProfessor(Guid id, string matricula, string nome, string email, string curto=null)
        {
            return new Professor(id, matricula, nome, email, curto);
        }

        public override string ToString()
        {
            return Nome;
        }

        public object Clone()
        {
            return Professor.GetProfessor(Id, (string)Matricula.Clone(), (string)Nome.Clone(), (string)Email.Clone(), (string)Curto.Clone());
        }

        public bool Equals(Professor other)
        {
            return this.Matricula.Equals(other.Matricula);
        }

        
    }
}
