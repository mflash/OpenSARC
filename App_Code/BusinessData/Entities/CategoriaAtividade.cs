using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BusinessData.Entities
{
    public class CategoriaAtividade : CategoriaBase
    {

        private Color cor;

        public Color Cor
        {
            get { return cor; }
            set { cor = value; }
        }

        private CategoriaAtividade():base()
        {
            
        }


        


        private CategoriaAtividade(Guid id, string descricao, Color c)
            : base(id, descricao)
        {
            cor = c;
        }

        /// <summary>
        /// Retorna uma CategoriaAtividade com os valores especificados
        /// </summary>
         /// <param name="descricao"></param>
        /// <returns></returns>
        public static CategoriaAtividade NewCategoriaAtividade(string descricao,Color c)
        {
            return new CategoriaAtividade(Guid.NewGuid(), descricao, c);
        }

        /// <summary>
        /// Cria uma nova CategoriaAtividade contendo um Guid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        public static CategoriaAtividade GetCategoriaAtividade(Guid id, string descricao, Color c)
        {
            return new CategoriaAtividade(id, descricao,c);
        }

    }
}
