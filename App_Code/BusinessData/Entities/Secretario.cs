using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    [Serializable]
    public class Secretario : PessoaBase, ICloneable
    {

        private Secretario(Guid id, string matricula, string nome, string email)
            : base(id, matricula, nome, email )
        {
            
        }

        public static Secretario NewSecretario(string matricula, string nome, string email)
        {
            return new Secretario(Guid.NewGuid(), matricula, nome, email);
        }

        public static Secretario GetSecretario(Guid id, string matricula, string nome, string email)
        {
            return new Secretario(id, matricula, nome, email);
        }

        public override string ToString()
        {
            return Nome;
        }

        public object Clone()
        {
            return Secretario.GetSecretario(Id, (string)Matricula.Clone(), (string)Nome.Clone(), (string)Email.Clone());
        }
    }
}
