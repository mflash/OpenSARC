using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BusinessData.Entities
{
    public class CategoriaData : CategoriaBase
    {
        private Color cor;
        private bool diaLetivo;

        
        private CategoriaData() : base() { }
        private CategoriaData(Guid id, string descricao, Color c, bool dl) : base(id, descricao) 
        { 
            cor = c;
            diaLetivo = dl;
        }

        public Color Cor
        {
            get { return cor; }
            set { cor = value; }
        }

        public bool DiaLetivo
        {
            get { return diaLetivo; }
            set { diaLetivo = value; }
        }

        public static CategoriaData NewCategoriaData(string descricao, Color c, bool dl)
        {
            return new CategoriaData(Guid.NewGuid(), descricao, c, dl);
        }

        public static CategoriaData GetCategoriaData(Guid id, string descricao, Color c, bool dl)
        {
            return new CategoriaData(id, descricao, c, dl);
        }

        public override string ToString()
        {
                return Descricao;
        }
    }
}