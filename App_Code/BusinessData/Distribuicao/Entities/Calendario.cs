using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Catalogos;

namespace BusinessData.Distribuicao.Entities
{
    public class Calendario:BusinessData.Entities.ICalendario
    {
        private BusinessData.Entities.Calendario cal;
        private ColecaoDias dias;
        private ColecaoRequisicoes requisicoes;
        private ColecaoTurmas turmas;
        private ColecaoCategoriaDeRecursos categorias;
        private List<BusinessData.Entities.CategoriaDisciplina> categoriasDeDisciplina;

        public List<BusinessData.Entities.CategoriaDisciplina> CategoriasDeDisciplina
        {
            get { return categoriasDeDisciplina; }
            set { categoriasDeDisciplina = value; }
        }


        public Calendario(BusinessData.Entities.Calendario cal, ColecaoDias dias, ColecaoRequisicoes requisicoes,
            ColecaoTurmas turmas, ColecaoCategoriaDeRecursos categorias, List<BusinessData.Entities.CategoriaDisciplina> categoriasDeDisciplina)
        {
            this.cal = cal;
            this.dias = dias;
            this.requisicoes = requisicoes;
            this.turmas = turmas;
            this.categorias = categorias;
            this.categoriasDeDisciplina = categoriasDeDisciplina;
        }

        public BusinessData.Entities.Calendario EntidadeCalendario
        {
            get { return cal; }
        }

        public ColecaoDias Dias
        {
            get { return dias; }
            set { dias = value; }
        }

        public ColecaoRequisicoes Requisicoes
        {
           get { return requisicoes; }
           set { requisicoes = value; }
        }

        public ColecaoTurmas Turmas
        {
            get { return turmas; }
            set { turmas = value; }
        }

        public ColecaoCategoriaDeRecursos Categorias
        {
            get { return categorias; }
            set { categorias = value; }
        }

        #region ICalendario Members
       
        public int Ano
        {
            get
            {
                return cal.Ano;
            }
            set
            {
                cal.Ano = value;
            }
        }

        public List<BusinessData.Entities.Data> Datas
        {
            get
            {
                return cal.Datas;
            }
        }

        public DateTime FimG2
        {
            get
            {
                return cal.FimG2;
            }
            set
            {
                cal.FimG2 = value;
            }
        }

        public Guid Id
        {
            get
            {
                return cal.Id;
            }
            set
            {
                cal.Id = value;
            }
        }

        public DateTime InicioG1
        {
            get
            {
                return cal.InicioG1;
            }
            set
            {
                cal.InicioG1 = value;
            }
        }

        public DateTime InicioG2
        {
            get
            {
                return cal.InicioG2;
            }
            set
            {
                cal.InicioG2 = value;
            }
        }

        public int Semestre
        {
            get
            {
                return cal.Semestre;
            }
            set
            {
                cal.Semestre = value;
            }
        }

        #endregion
    }
}
