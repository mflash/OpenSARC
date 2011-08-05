using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class Recurso : IComparable<Recurso>
    {
        private Guid id;
        private string descricao;
        private Faculdade vinculo;
        private CategoriaRecurso categoria;
        private bool estaDisponivel;
        private List<HorarioBloqueado> horariosBloqueados;

        public Recurso() {}

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }
        
        public CategoriaRecurso Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        public Faculdade Vinculo
        {
            get { return vinculo; }
            set { vinculo = value; }
        }

        public List<HorarioBloqueado> HorariosBloqueados
        {
            get { return horariosBloqueados; }
            set { horariosBloqueados = value; }
        }

        public bool EstaDisponivel
        {
            get { return estaDisponivel; }
            set { estaDisponivel = value; }
        }

        private Recurso(Guid id, string descricao, Faculdade vinculo, CategoriaRecurso categoria, bool estaDisponivel, List<HorarioBloqueado> listaHB)
        {
            Id = id;
            Descricao = descricao;
            Vinculo = vinculo;
            Categoria = categoria;
            EstaDisponivel = estaDisponivel;
            horariosBloqueados = listaHB;
        }

        /// <summary>
        /// Retorna um Recurso com os valores especificados
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="nome">Nome</param>
        /// <returns></returns>
        public static Recurso GetRecurso(Guid id, string descricao, Faculdade vinculo, CategoriaRecurso categoria, bool estaDisponivel, List<HorarioBloqueado> listaHB)
        {
            return new Recurso(id, descricao, vinculo, categoria, estaDisponivel, listaHB);
        }

        /// <summary>
        /// Cria um novo Recurso contendo um Guid
        /// </summary>
        /// <param name="nome">Nome do Recurso</param>
        /// <returns></returns>
        public static Recurso NewRecurso(string descricao, Faculdade vinculo, CategoriaRecurso categoria, bool estaDisponivel, List<HorarioBloqueado> listaHB)
        {
            return new Recurso(Guid.NewGuid(), descricao, vinculo, categoria, estaDisponivel,listaHB);
        }

        #region IComparable<Recurso> Members

        public int CompareTo(Recurso other)
        {
            return this.Descricao.CompareTo(other.Descricao);
        }

        #endregion
    }
}
