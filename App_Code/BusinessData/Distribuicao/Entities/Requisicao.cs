using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Distribuicao.Entities
{

    public class Requisicao
    {
        private Horarios.HorariosPUCRS horario;
        private Dia dia;
        private TurmaDistribuicao turma;
        private CategoriaRecurso categoriaRecurso;
        private Guid id;
        private int prioridade;
        private bool estaAtendido;
        private BusinessData.Entities.Recurso recurso;

        // Retorna o recurso que atendeu esta requisição, se houver
        public BusinessData.Entities.Recurso Recurso
        {
            get { return recurso;  }
            set { recurso = value; }
        }

        public bool EstaAtendido
        {
            get { return estaAtendido; }
            set { estaAtendido = value; }
        }

        public int Prioridade
        {
            get { return prioridade; }
            set { prioridade = value; }
        }

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public Horarios.HorariosPUCRS Horario
        {
            get { return horario; }
            set { horario = value; }
        }
        
        public Dia Dia
        {
            get { return dia; }
            set { dia = value; }
        }
        
        public CategoriaRecurso CategoriaRecurso
        {
            get { return categoriaRecurso; }
            set { categoriaRecurso = value; }
        }
        
        public TurmaDistribuicao Turma
        {
            get { return turma; }
            set { turma = value; }
        }

        public Requisicao(Dia dia, Horarios.HorariosPUCRS horario, TurmaDistribuicao turma,
            CategoriaRecurso cat, int prioridade, bool estaAtendido)
        {
            this.Dia = dia;
            this.Horario = horario;
            this.Turma = turma;
            this.CategoriaRecurso = cat;
            this.Prioridade = prioridade;
            this.EstaAtendido = estaAtendido;
            this.Recurso = null;
        }
    }
}