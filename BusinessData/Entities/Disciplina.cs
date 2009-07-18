using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessData.Entities
{
    public class Disciplina:IEquatable<Disciplina>
    {
        private string cod;
        private int cred;
        private string nome;
        private bool g2;
        private Calendario calendario;
        private CategoriaDisciplina categoria;

        public string Cod
        {
            get { return cod; }
            set { cod = value; }
        }

        public int Cred
        {
            get { return cred; }
            set { cred = value; }
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public bool G2
        {
            get { return g2; }
            set { g2 = value; }
        }

        public Calendario Calendario
        {
            get { return calendario; }
            set { calendario = value; }
        }

        public CategoriaDisciplina Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        private Disciplina() { }

        /// <summary>
        /// Construtor privado
        /// </summary>
        private Disciplina(string cod,
                            int cred,
                            string nome,
                            bool g2,
                            Calendario calendario,
                            CategoriaDisciplina categoria)
        {
            Cod = cod;
            Cred = cred;
            Nome = nome;
            G2 = g2;
            Calendario = calendario;
            Categoria = categoria;
        }

        /// <summary>
        /// Retorna uma instância da classe completa com os valores passados
        /// </summary>
        public static Disciplina GetDisciplina(string cod,
                                    int cred,
                                    string nome,
                                    bool g2,
                                    Calendario calendario,
                                    CategoriaDisciplina categoria)
        {
            return new Disciplina(cod, cred, nome, g2, calendario, categoria);
        }

        

        /// <summary>
        /// override do método para ele escrever direito quando é chamado
        /// </summary>
        public override string ToString()
        {
            return nome;
        }

        public static int GetNumeroDeCreditos(string horarioPucrs)
        {
            horarioPucrs = Regex.Replace(horarioPucrs, "[a-zA-Z]", String.Empty);
            int creditos = horarioPucrs.Length;
            return 2 * creditos;
        }

        public static void ValidaHorario(string dataHora)
        {
            string[] horarios = dataHora.Split(new char[] { '1', '2', '3', '4', '5', '6', '7' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string horario in horarios)
            {

                if (horario.Length == 1) continue;
                if (horario == "NP") continue;
                else
                {
                    if (!(horario[0] < horario[1] && horario[1] - horario[0] <= 1))
                    {
                        throw new FormatException("Horário inválido.");
                    }
                }
            }
        }

        public string G2PorExtenso
        {
            get
            {
                if (g2)
                    return "Sim";
                else
                    return "Não";
            }
        }

        /// <summary>
        /// override para comparar duas disciplinas
        /// </summary>
        public override bool Equals(object obj)
        {
            return this.cod.ToUpper().Equals(((Disciplina)obj).Cod.ToUpper());
        }

        #region IEquatable<Disciplina> Members

        public bool Equals(Disciplina other)
        {
            return this.cod.ToUpper().Equals(other.Cod.ToUpper());
        }




        #endregion
    }
}
