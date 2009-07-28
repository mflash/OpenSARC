using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.DataAccess;
using BusinessData.Entities;
using System.Collections.ObjectModel;
using System.Data;
 

namespace BusinessData.BusinessLogic
{
    public class AcessosBO
    {
        private AcessosDAO dao;

        public AcessosBO()
        {
            dao = new AcessosDAO();
        }

        public void InserirAcesso(Acesso umAcesso)
        {
            try
            {
                dao.Add(umAcesso);
            }
            catch (DataAccessException ex)
            {
                throw;
            }
        }

        public DataTable CriarRelatorioDeAcessos()
        {
            ICollection<FaixaDeAcesso> faixasDeAcesso = GetFaixasDeAcesso();           
            DataTable tabela = new DataTable("Relatorio de Acessos");
            tabela.Columns.Add("Período");

            foreach (FaixaDeAcesso faixa in faixasDeAcesso)
            {
                tabela.Columns.Add(faixa.ToString());
            }
            //Adiciona PorDia
            DataRow dr = tabela.NewRow();
            dr[0] = "Dia: " + DateTime.Now.ToShortDateString();
            foreach (FaixaDeAcesso faixa in faixasDeAcesso)
            {
                dr[faixa.ToString()] = dao.GetNumeroDeAcessosPorData(faixa, DateTime.Now);
            }
            tabela.Rows.Add(dr);

            // Adicionar na ultima semana
            DateTime[] diasSemanaPassada = SemanaPassada();
            
            dr = tabela.NewRow();
            dr[0] = "Ultima semana: " + diasSemanaPassada[0].Day + " - " + diasSemanaPassada[1].ToShortDateString();

            foreach (FaixaDeAcesso faixa in faixasDeAcesso)
            {
                dr[faixa.ToString()] = dao.GetNumeroDeAcessosPorPeriodo(faixa, diasSemanaPassada[0],diasSemanaPassada[1]);
            }
            tabela.Rows.Add(dr);

            //Adiciona acessos do mes
            dr = tabela.NewRow();
            dr[0] = "Mês: " + DateTime.Now.Month + "/" + DateTime.Now.Year;
            foreach (FaixaDeAcesso faixa in faixasDeAcesso)
            {
                dr[faixa.ToString()] = dao.GetNumeroDeAcessosPorMes(faixa,DateTime.Now.Month, DateTime.Now.Year);
            }
            tabela.Rows.Add(dr);
            

            //Adicionar acessos do ano
            dr = tabela.NewRow();
            dr[0] = "Ano: " + DateTime.Now.Year;
            foreach (FaixaDeAcesso faixa in faixasDeAcesso)
            {
                dr[faixa.ToString()] = dao.GetNumeroDeAcessosPorAno(faixa, DateTime.Now.Year);
            }
            tabela.Rows.Add(dr);
            return tabela;
        }


        private DateTime[] SemanaPassada()
        {

            DateTime[] ret = new DateTime[2];
            DateTime dt = DateTime.Now;

            int t = ((int)dt.DayOfWeek) * -1;
            DateTime domingo = dt.AddDays(t);



            ret[0] = domingo.AddDays(-7);
            ret[1] = domingo.AddDays(-1);
            return ret; 
        }
        

        public ReadOnlyCollection<FaixaDeAcesso> GetFaixasDeAcesso()
        {          
            List<FaixaDeAcesso> lst = new List<FaixaDeAcesso>();
            lst.Add(new FaixaDeAcesso(0, 6));
            lst.Add(new FaixaDeAcesso(6, 12));
            lst.Add(new FaixaDeAcesso(12, 18));
            lst.Add(new FaixaDeAcesso(18, 24));

            return lst.AsReadOnly();   

        }
    }
}
