using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;

namespace BusinessData.Entities
{
    public class Aula : IEquatable<Aula>
    {
        private Guid? id;
        private Turma turmaId;
        private string hora;
        private DateTime data;
        private string descricaoAtividade;
        private CategoriaAtividade categoriaAtividade;

        public Guid? Id { get { return id; } set { id = value; } }

        public Turma TurmaId { get { return turmaId; } set { turmaId = value; } }

        public string Hora { get { return hora; } set { hora = value; } }

        public DateTime Data { get { return data; } set { data = value; } }

        public string DescricaoAtividade { get { return descricaoAtividade; } set { descricaoAtividade = value; } }

        public CategoriaAtividade CategoriaAtividade { get { return categoriaAtividade; } set { categoriaAtividade = value; } }

        private Aula(Guid? id, Turma turmaId, string hora, DateTime data, string descricao, CategoriaAtividade categoriaId) 
        {
            Id = id;
            TurmaId = turmaId;
            Hora = hora;
            Data = data;
            DescricaoAtividade = descricao;
            CategoriaAtividade = categoriaId;
        }

        public static Aula GetAula(Guid? id, Turma turmaId, string hora, DateTime data, string descricao, CategoriaAtividade categoriaId)
        {
            return new Aula(id, turmaId, hora, data, descricao, categoriaId);
        }

        /// <summary>
        /// Cria uma nova aula contendo um Guid
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="turmaId">TurmaId</param>
        /// <param name="hora">Hora</param>
        /// <param name="data">Data</param>
        /// <param name="descricao">DescricaoAtividade</param>
        /// <param name="categoriaId">CategoriaAtividadeId</param>
        /// <returns></returns>

        public static Aula newAula(Turma turmaId, string hora, DateTime data, string descricao, CategoriaAtividade categoriaId)
        {
            return new Aula(Guid.NewGuid(), turmaId, hora, data, descricao, categoriaId);
        }

        #region IEquatable<Aula> Members

        public bool Equals(Aula other)
        {
            if (this.Id == other.Id)
                return true;
            else
                return false;
        }

        #endregion
    }
}
