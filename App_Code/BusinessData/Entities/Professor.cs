using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    [Serializable]
    public class Professor : PessoaBase, ICloneable
    {
        private Professor(Guid id, string matricula, string nome, string email)
            : base(id, matricula, nome, email )
        {
            
        }

        public static Professor NewProfessor(string matricula, string nome, string email)
        {
            return new Professor(Guid.NewGuid(), matricula, nome, email);
        }

        public static Professor NewProfessor(string matricula)
        {
            return new Professor(Guid.NewGuid(), matricula, "", "");
        }

        public static Professor GetProfessor(Guid id, string matricula, string nome, string email)
        {
            return new Professor(id, matricula, nome, email);
        }

        public override string ToString()
        {
            return Nome;
        }

        public object Clone()
        {
            return Professor.GetProfessor(Id, (string)Matricula.Clone(), (string)Nome.Clone(), (string)Email.Clone());
        }

        public bool Equals(Professor other)
        {
            return this.Matricula.Equals(other.Matricula);
        }

        
    }
}
