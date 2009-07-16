using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class Troca
    {
        private Guid id;
        private Alocacao alocacaoAtual;
        private Alocacao alocacaoDesejada;
        private bool? foiAceita;
        private bool estaPendente;
        private bool foiVisualizada;
        private string horario;
        private DateTime data;
       

        public Troca(Guid id, Alocacao alocAtual, Alocacao alocDesejada, bool? aceitou, bool pendente, bool visualizada, string horario, DateTime data)
        {
            this.id = id;
            alocacaoAtual = alocAtual;
            alocacaoDesejada = alocDesejada;
            foiAceita = aceitou;
            estaPendente = pendente;
            foiVisualizada = visualizada;
            this.horario = horario;
            this.data = data;
        }

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public Alocacao AlocacaoAtual
        {
            get { return alocacaoAtual; }
            set { alocacaoAtual = value; }
        }

        public Alocacao AlocacaoDesejada
        {
            get { return alocacaoDesejada; }
            set { alocacaoDesejada = value; }
        }

        public bool? FoiAceita
        {
            get { return foiAceita; }
            set { foiAceita = value; }
        }

        public bool EstaPendente
        {
            get { return estaPendente; }
            set { estaPendente = value; }
        }

        public bool FoiVisualizada
        {
            get { return foiVisualizada; }
            set { foiVisualizada = value; }
        }

        public DateTime Data
        {
            get { return data; }
            set { data = value; }
        }

        public string Horario
        {
            get { return horario; }
            set { horario = value; }
        }
    }
}
