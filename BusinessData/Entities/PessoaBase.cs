using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    [Serializable]
    public abstract class PessoaBase : IEquatable<PessoaBase>
    {
        private Guid id;
        private string matricula;
        private string nome;
        private string email;
        
        public Guid Id
        {
            get{return id; }
            set{id = value; }
        }
        public string Matricula
        {
            get { return matricula; }
            set { matricula = value; }
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public PessoaBase(Guid id, string matricula, string nome, string email)
        {
            Id = id;
            Matricula = matricula;
            Nome = nome;
            Email = email;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(PessoaBase other)
        {
            return this.Matricula.Equals(other.Matricula);
        }
    }
}
