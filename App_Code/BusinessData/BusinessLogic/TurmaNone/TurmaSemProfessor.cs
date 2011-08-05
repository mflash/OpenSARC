using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;


namespace BusinessData.Entities
{
    public class TurmaSemProfessor : IEquatable<Turma>
    {
        private Guid id;
        private int numero;
        private Calendario calendario;
        private Disciplina disciplina;
        private string dataHora;
        private Curso curso;

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


        public Curso Curso
        {
            get { return curso; }
            set { curso = value; }
        }


        private TurmaSemProfessor(Guid id, int num, Calendario calend, Disciplina disc, string dh, Curso curso)
        {
            Id = id;
            Numero = num;
            Calendario = calend;
            Disciplina = disc;
            DataHora = dh;
            Curso = curso;
        }

        public static TurmaSemProfessor GetTurma(Guid id, int num, Calendario calend, Disciplina disc, string dh, Curso curso)
        {
            return new TurmaSemProfessor(id, num, calend, disc, dh, curso);
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
        public static TurmaSemProfessor NewTurma(int numero, Calendario calendario, Disciplina disciplina, string dh, Curso curso)
        {
            return new TurmaSemProfessor(Guid.NewGuid(), numero, calendario, disciplina, dh, curso);
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
    }
}
