using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class Faculdade:IEquatable<Faculdade>
    {
        private Guid id;
        private string nome;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private Faculdade() { }
        private Faculdade(Guid id, string nome)
        {
            Id = id;
            Nome = nome;
        }
        
        /// <summary>
        /// Retorna um Vinculo com os valores especificados
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="nome">Nome</param>
        /// <returns></returns>
        public static Faculdade GetFaculdade(Guid id, string nome)
        {
            return new Faculdade(id, nome);   
        }
        
        /// <summary>
        /// Cria um novo Vinculo contendo um Guid
        /// </summary>
        /// <param name="nome">Nome do Vinculo</param>
        /// <returns></returns>
        public static Faculdade NewFaculdade(string nome)
        {
            return new Faculdade(Guid.NewGuid(),nome);
        }

        public override string ToString()
        {
            return Nome;
        }


        public override bool Equals(object obj)
        {
            return this.Nome.ToUpper().Equals(((Faculdade)obj).Nome.ToUpper());
        }




        #region IEquatable<Faculdade> Members

        public bool Equals(Faculdade other)
        {
            return this.Nome.ToUpper().Equals(other.Nome.ToUpper());
        }

        #endregion
    }
}
