using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class Curso : IEquatable<Curso>
    {
        private string codigo;
        private string nome;
        private Faculdade vinculo;
        
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public Faculdade Vinculo
        {
            get { return vinculo; }
            set { vinculo = value; }
        }

        private Curso(string codigo, string nome, Faculdade vinculo)
        {
            Codigo = codigo;
            Nome = nome;
            Vinculo = vinculo;
        }

        public static Curso NewCurso(string codigo,string nome, Faculdade vinculo)
        {
            return new Curso(codigo, nome, vinculo);
        }

        public static Curso NewCurso(string codigo)
        {
            return new Curso(codigo, "", Faculdade.NewFaculdade(""));
        }

        public static Curso GetCurso(string codigo, string nome, Faculdade vinculo)
        {
            return new Curso(codigo, nome, vinculo);
        }

        public override string ToString()
        {
            return nome;
        }


        #region IEquatable<Curso> Members

        public bool Equals(Curso other)
        {
            return (this.Codigo.Equals(other.Codigo));
                
        }

        #endregion
    }
}
