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
        private Guid bloqueia1, bloqueia2; // guids do(s) recurso(s) que são bloqueados por este
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

        public Guid Bloqueia1 { get; set; }
        public Guid Bloqueia2 { get; set; }

        private Recurso(Guid id, string descricao, Faculdade vinculo, CategoriaRecurso categoria, bool estaDisponivel, 
            Guid bloq1, Guid bloq2, List<HorarioBloqueado> listaHB)
        {
            Id = id;
            Descricao = descricao;
            Vinculo = vinculo;
            Categoria = categoria;
            EstaDisponivel = estaDisponivel;
            Bloqueia1 = bloq1;
            Bloqueia2 = bloq2;
            horariosBloqueados = listaHB;
        }

        /// <summary>
        /// Retorna um Recurso com os valores especificados
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="nome">Nome</param>
        /// <returns></returns>
        public static Recurso GetRecurso(Guid id, string descricao, Faculdade vinculo, CategoriaRecurso categoria, bool estaDisponivel,
            Guid bloq1, Guid bloq2, List<HorarioBloqueado> listaHB)
        {
            return new Recurso(id, descricao, vinculo, categoria, estaDisponivel, bloq1, bloq2, listaHB);
        }

        /// <summary>
        /// Cria um novo Recurso contendo um Guid
        /// </summary>
        /// <param name="nome">Nome do Recurso</param>
        /// <returns></returns>
        public static Recurso NewRecurso(string descricao, Faculdade vinculo, CategoriaRecurso categoria, bool estaDisponivel, List<HorarioBloqueado> listaHB)
        {
            return new Recurso(Guid.NewGuid(), descricao, vinculo, categoria, estaDisponivel, Guid.NewGuid(), Guid.NewGuid(), listaHB);
        }

        #region IComparable<Recurso> Members

        public int CompareTo(Recurso other)
        {
            return this.Descricao.CompareTo(other.Descricao);
        }

        #endregion
    }
}
