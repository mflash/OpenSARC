using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;

namespace BusinessData.Entities
{
    public class Evento : IEquatable<Evento>,ICloneable
    {
        private Guid eventoId;
        private PessoaBase autorId;
        private string descricao;
        private Calendario calendarioId;
        private string responsavel;
        private string titulo;
        private string unidade;

        public Guid EventoId { get { return eventoId; } set { eventoId = value; } }

        public PessoaBase AutorId { get { return autorId; } set { autorId = value; } }

        public string Descricao { get { return descricao; } set { descricao = value; } }

        public Calendario CalendarioId { get { return calendarioId; } set { calendarioId = value; } }

        public string Responsavel { get { return responsavel; } set { responsavel = value; } }

        public string Titulo { get { return titulo; } set { titulo = value; } }

        public string Unidade { get { return unidade; } set { unidade = value; } }

        private Evento(Guid eventoId, PessoaBase autorId, string descricao, Calendario calendarioId, string responsavel, string titulo, string unidade) 
        {
            EventoId = eventoId;
            AutorId = autorId;
            Descricao = descricao;
            CalendarioId = calendarioId;
            Responsavel = responsavel;
            Titulo = titulo;
            Unidade = unidade;
        }

        public static Evento GetEvento(Guid eventoId, PessoaBase autorId, string descricao, Calendario calendarioId, string responsavel, string titulo, string unidade)
        {
            return new Evento(eventoId, autorId, descricao, calendarioId, responsavel, titulo, unidade);
        }

        public static Evento newEvento(PessoaBase autorId, string descricao, Calendario calendarioId, string responsavel, string titulo, string unidade)
        {
            return new Evento(Guid.NewGuid(), autorId, descricao, calendarioId, responsavel, titulo, unidade);
        }
        
        #region IEquatable<Evento> Members

        public bool Equals(Evento other)
        {
            if (this.EventoId == other.EventoId)
                return true;
            else
                return false;
        }

        #endregion




        #region ICloneable Members

        public object Clone()
        {
            return Evento.GetEvento(eventoId, autorId, (string)descricao.Clone(), (Calendario)calendarioId.Clone(), (string)responsavel.Clone(), (string)titulo.Clone(), (string)unidade.Clone());
        }

        #endregion
    }
}
