using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;

namespace BusinessData.Entities
{
    [Serializable]
    public class Requisicao : BusinessData.Entities.IRequisicao
    {
        private Aula aula;
        private Guid idRequisicao;
        private CategoriaRecurso categoriaRecurso;
        private int prioridade;
        private bool estaAtendida;

       private Requisicao(Aula _aula, Guid _idRequisicao,
           CategoriaRecurso _categoriaRecurso, int _prioridade, bool _estaAtendida)
       {
           aula = _aula;
           idRequisicao = _idRequisicao;
           categoriaRecurso = _categoriaRecurso;
           prioridade = _prioridade;
           estaAtendida = _estaAtendida;
       }

        public Requisicao() { }
        public Aula Aula
        {
            get { return aula; }
            set { aula = value; }
        }

        public Guid IdRequisicao
        {
            get { return idRequisicao; }
        }
        
        public CategoriaRecurso CategoriaRecurso
        {
            get { return categoriaRecurso; }
            set { categoriaRecurso = value; }
        }
        
        public int Prioridade
        {
            get { return prioridade; }
            set { prioridade = value; }
        }

        public bool EstaAtendida
        {
            get { return estaAtendida; }
            set { estaAtendida = value; }
        }

       public static Requisicao GetRequisicao(Aula aula, Guid idRequisicao,
           CategoriaRecurso categoriaRecurso, int prioridade, bool estaAtendida)
       {
           return new Requisicao(aula, idRequisicao, categoriaRecurso, prioridade, estaAtendida);
       }

        public static Requisicao NewRequisicao(Aula aula, CategoriaRecurso categoriaRecurso, int prioridade)
        {
            return new Requisicao(aula, Guid.NewGuid(), categoriaRecurso, prioridade, false);
        }

        public static Requisicao NewRequisicao(Aula aula, CategoriaRecurso categoriaRecurso, int prioridade, bool estaAtendida)
       {
           return new Requisicao(aula, Guid.NewGuid(), categoriaRecurso, prioridade, estaAtendida);
       }
    }
}
