using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class Transferencia
    {
        private Guid id;
        private Recurso recurso;        
        private DateTime data;
        private string horario;
        private Turma turmaRecebeu;       
        private Turma turmaTransferiu;
        private bool foiVisualizada;
        private Evento eventoRecebeu;
        private Evento eventoTransferiu;

        public Transferencia(Guid id, Recurso rec, DateTime data, string h, Turma turmaRec, Turma turmatrans, bool visualizada, Evento eveRecebeu, Evento eveTransferiu)
        {
            this.id = id;
            recurso = rec;
            this.data = data;
            horario = h;
            turmaRecebeu = turmaRec;
            turmaTransferiu = turmatrans;
            foiVisualizada = visualizada;
            eventoRecebeu = eveRecebeu;
            eventoTransferiu = eveTransferiu;
        }

        public Evento EventoTransferiu
        {
            get { return eventoTransferiu; }
            set { eventoTransferiu = value; }
        }

        public Evento EventoRecebeu
        {
            get { return eventoRecebeu; }
            set { eventoRecebeu = value; }
        }

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime Data
        {
            get { return data; }
            set { data = value; }
        }

        public Recurso Recurso
        {
            get { return recurso; }
            set { recurso = value; }
        }

        public Turma TurmaTransferiu
        {
            get { return turmaTransferiu; }
            set { turmaTransferiu = value; }
        }

        public Turma TurmaRecebeu
        {
            get { return turmaRecebeu; }
            set { turmaRecebeu = value; }
        }

        public string Horario
        {
            get { return horario; }
            set { horario = value; }
        }

        public bool FoiVisualizada
        {
            get { return foiVisualizada; }
            set { foiVisualizada = value; }
        }
    }
}
