using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;


namespace BusinessData.Entities
{
    public class Turma : IEquatable<Turma>, IComparable<Turma>
    {
        private Guid id;
        private int numero;
        private Calendario calendario;
        private Disciplina disciplina;
        private string dataHora;
        private Professor professor;
        private Curso curso;
        private bool notebook;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        public Calendario Calendario
        {
            get { return calendario; }
            set { calendario = value; }
        }

        public Disciplina Disciplina
        {
            get { return disciplina; }
            set { disciplina = value; }
        }

        public string DataHora
        {
            get { return dataHora; }
            set { dataHora = value; }
        }


        public Professor Professor
        {
            get { return professor; }
            set { professor = value; }
        }


        public Curso Curso
        {
            get { return curso; }
            set { curso = value; }
        }

        public bool Notebook { get; set; }

        public string Sala { get; set; }

        public Turma() { }
        public Turma(Guid id,int num, Calendario calend, Disciplina disc, string dh, Professor prof, Curso curso, string sala = "N/A", bool notebook=false)
        {
            Id = id;
            Numero = num;
            Calendario = calend;
            Disciplina = disc;
            DataHora = dh;
            Professor = prof;
            Curso = curso;
            Sala = sala;
            Notebook = notebook;
        }

        public static Turma GetTurma(Guid id, int num, Calendario calend, Disciplina disc, string dh, Professor prof, Curso curso, string sala="N/A", bool notebook= false)
        {
            return new Turma(id,num, calend, disc, dh, prof, curso, sala, notebook);
        }

        /// <summary>
        /// Cria uma nova turma contendo um Guid
        /// </summary>
        /// <param name="numero">Numero</param>
        /// <param name="calendario">Calendario</param>
        /// <param name="disciplina">Disciplina</param>
        /// <param name="dh">DataHora</param>
        /// <param name="prof">Professor</param>
        /// <returns></returns>
        public static Turma NewTurma(int numero, Calendario calendario, Disciplina disciplina, string dh, Professor prof, Curso curso, string sala = "N/A", bool notebook = false)
        {
            return new Turma(Guid.NewGuid(),numero, calendario, disciplina, dh, prof, curso, sala, notebook);
        }

        #region IEquatable<Turma> Members

        public bool Equals(Turma other)
        {
            if (this.Id == other.Id)
                return true;
            else
                return false;
        }

        #endregion

        #region IComparable<Turma> Members

        public int CompareTo(Turma other)
        {
            string meuNome   = Disciplina.Nome + " - " + Numero;
            string outroNome = other.Disciplina.Nome + " - " + other.Numero;
            return meuNome.CompareTo(outroNome);
        }

        #endregion

        public override string ToString()
        {
            return disciplina.Cod + numero;
        }
    }
}
