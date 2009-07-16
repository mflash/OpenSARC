using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;

namespace BusinessData.Entities
{
    public class CategoriaDisciplina : BusinessData.Entities.CategoriaBase
    {
        private CategoriaDisciplina() { }
        private CategoriaDisciplina(Guid id, string descricao, Dictionary<CategoriaRecurso, double> prioridades)
        {
            base.Id = id;
            base.Descricao = descricao;
            Prioridades = prioridades;
        }

        private Dictionary<CategoriaRecurso, double> prioridades;
        public Dictionary<CategoriaRecurso, double> Prioridades
        {
            get { return prioridades; }
            set { prioridades = value; }
        }

        public void NormalizaPrioridades()
        {
            List<double> listaPrioridades = new List<double>();
            double soma = 0.0;
            
            foreach (double prioridade in prioridades.Values)
            {
                    soma += prioridade;                
            }

            Dictionary<CategoriaRecurso, double> newValues = new Dictionary<CategoriaRecurso, double>();

            foreach (CategoriaRecurso cat in prioridades.Keys)
            {
                newValues.Add(cat, prioridades[cat] / soma);
            }

            this.prioridades = newValues;
        }
        /// <summary>
        /// Returna uma Categoria Disciplina com os valores especificados
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        public static CategoriaDisciplina GetCategoriaDisciplina(Guid id, string descricao, Dictionary<CategoriaRecurso, double> prioridades)
        {
            return new CategoriaDisciplina(id, descricao, prioridades);
        }

        /// <summary>
        /// Cria uma nova Categoria Disciplina contendo um Guid
        /// </summary>
        /// <param name="descricao">Descrição</param>
        /// <returns></returns>
        public static CategoriaDisciplina NewCategoriaDisciplina(string descricao, Dictionary<CategoriaRecurso, double> prioridades)
        {
            return new CategoriaDisciplina(Guid.NewGuid(), descricao, prioridades);
        }
    }
}
