using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities.Decorator
{
    [Serializable]
    public class DecoratorRequisicoes : IRequisicao
    {
        public enum EstadoRequisicao
        {
            Inserida,
            Removida,
            Original,
            Atualizada
        };
        #region Atributos
        private Requisicao componente;
        private EstadoRequisicao estadoOriginal;
        private EstadoRequisicao estadoAtual;
        #endregion

        public DecoratorRequisicoes(Requisicao requisicao, EstadoRequisicao estadoInicial)
        {
            this.componente = requisicao;
            this.estadoOriginal = this.estadoAtual = estadoInicial;
        }

        public DecoratorRequisicoes() { }
        #region IRequisicao Members

        public Aula Aula
        {
            get
            {
                return componente.Aula;
            }
            set
            {
                componente.Aula = value;
            }
        }

        public CategoriaRecurso CategoriaRecurso
        {
            get
            {
                return componente.CategoriaRecurso;
            }
            set
            {
                componente.CategoriaRecurso = value;
            }
        }

        public Guid IdRequisicao
        {
            get
            {
                return componente.IdRequisicao;
            }
        }

        public int Prioridade
        {
            get
            {
                return componente.Prioridade;
            }
            set
            {
                componente.Prioridade = value;
            }
        }

        #endregion


        public EstadoRequisicao EstadoOriginal
        {
            get
            {
                return estadoOriginal;
            }
        }

        public EstadoRequisicao EstadoAtual
        {
            get
            {
                return estadoAtual;
            }
            set
            {
                estadoAtual = value;
            }
        }
    }
}
