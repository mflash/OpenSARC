using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessData.Util
{
    public class RecursosEventArgs:EventArgs
    {
        /// <summary>
        /// Recurso
        /// </summary>
        private readonly Entities.Recurso recurso;

        public Entities.Recurso Recurso
        {
            get { return recurso; }
        }

        /// <summary>
        /// Cria um novo objeto RecursosEventArgs
        /// </summary>
        /// <param name="rec">Recurso especificado</param>
        public RecursosEventArgs(Entities.Recurso rec)
            : base()
        {
            this.recurso = rec;
        }
    }
}
