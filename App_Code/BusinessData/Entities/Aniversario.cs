using System;

namespace BusinessData.Entities
    {
    public class Aniversario
    {
        private string nome;
        private DateTime aniver;
        public string Nome { get { return nome; } set { nome = value; } }
        public DateTime Aniver { get { return aniver; } set { aniver = value; } }
        private Aniversario(string nome, DateTime aniver)
        {
            Nome = nome;
            Aniver = aniver;
        }
        public static Aniversario GetAniversario(string nome, DateTime aniver)
        {
            return new Aniversario(nome, aniver);
        }
    }
}