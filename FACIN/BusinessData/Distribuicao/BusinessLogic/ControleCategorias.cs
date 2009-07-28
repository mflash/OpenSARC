using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Catalogos;
using BusinessData.BusinessLogic;
using BusinessData.Distribuicao.Entities;

namespace BusinessData.Distribuicao.BusinessLogic
{
    class ControleCategorias
    {
        public ColecaoCategoriaDeRecursos GetCategorias()
        {
            CategoriaRecursoBO controleCategoriaRecursos = new CategoriaRecursoBO();
            RecursosBO controleRecursos = new RecursosBO();
            AlocacaoBO controleAlocacoes = new AlocacaoBO();
            ColecaoCategoriaDeRecursos catalogoRecursos = new ColecaoCategoriaDeRecursos();

            IList<Recurso> listaRecursos;
            CategoriaRecurso categ = null;
            Recurso recAux;
            foreach (BusinessData.Entities.CategoriaRecurso cat in controleCategoriaRecursos.GetCategoriaRecurso())
            {
                listaRecursos = new List<Recurso>();
                foreach (BusinessData.Entities.Recurso rec in controleRecursos.GetRecursosPorCategoria(cat))
                {
                    recAux = new Recurso(rec);
                    listaRecursos.Add(recAux);
                }
                categ = new CategoriaRecurso(cat, listaRecursos);
                catalogoRecursos.Add(categ);
            }
            return catalogoRecursos;
        }
    }
}
