using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Entities
{
    public class Acesso
    {
        private Guid id;
        private DateTime horario;

        public Guid Id
        {
            get { return id; }
        }

        public DateTime Horario
        {
            get { return horario; }
        }

        public Acesso(Guid id, DateTime horario)
        {
            this.id = id;
            this.horario = horario;
        }
    }
}
