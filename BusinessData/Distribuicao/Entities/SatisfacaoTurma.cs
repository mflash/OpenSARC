using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Distribuicao.Entities
{
    public class SatisfacaoTurma
    {
        private int pedidos;

        private double prioridade;
        public double Prioridade
        {
            get { return prioridade; }
            set { prioridade = value; }
        }

        public int Pedidos
        {
            get { return pedidos; }
            set { pedidos = value; }
        }
        private int atendimentos;

        public int Atendimentos
        {
            get { return atendimentos; }
            set { atendimentos = value; }
        }
        private TurmaDistribuicao turma;

        public TurmaDistribuicao Turma
        {
            get { return turma; }
        }

        public double Satisfacao
        {
            get
            {
                if (pedidos == 0)
                {
                    return 1;
                }
                else
                {
                    return atendimentos / (double)pedidos;
                }
            }
        }

        public SatisfacaoTurma(TurmaDistribuicao turma,int pedidos, int atendimentos)
        {
            this.turma = turma;
            Pedidos = pedidos;
            Atendimentos = atendimentos;
            prioridade = 0;
        }

        public SatisfacaoTurma(TurmaDistribuicao turma) : this(turma, 0, 0) { }
    }
}
