using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;
using System.Collections;
using System.Data;
using BusinessData.DataAccess;

namespace Domingo
{
    public class Program
    {
        static void Main(string[] args)
        {
            //criaDomingo();
            CompletaCalendario();
        }


        public static void PreencheCalendarioDeAlocacoes(Calendario cal, Recurso rec, bool datasOrdenadas)
        {
            AlocacaoDAO alocBO = new AlocacaoDAO();

            if (!datasOrdenadas)
            {
                cal.Datas.Sort();
            }
            DateTime data = cal.InicioG1;
            Alocacao alocacao;

            string[] listaHorarios = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P" };

            while (data != cal.FimG2)
            {
                if (data.DayOfWeek == DayOfWeek.Sunday)
                {
                    foreach (string aux in listaHorarios)
                    {
                        alocacao = Alocacao.newAlocacao(rec, data, aux, null, null);
                        try
                        {
                            alocBO.InsereAlocacao(alocacao);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
                data = data.AddDays(1);
            }
        }

        public static void criaDomingo()
        {
            try
            {
                RecursosDAO controleRecursos = new RecursosDAO();
                CalendariosDAO calBO = new CalendariosDAO();

                Calendario cal = calBO.GetCalendario(new Guid("a6003a97-b753-4ea7-9778-de7be7cb9382"));

                List<Recurso> listaRecursos = controleRecursos.GetRecursos();

                foreach (Recurso recursoAux in listaRecursos)
                {
                    PreencheCalendarioDeAlocacoes(cal, recursoAux, false);
                }

                Console.WriteLine("Domingos criados com sucesso!");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro = " + e.Message);
            }

        }

        public static void CompletaCalendario()
        {
            try
            {
                RecursosDAO redDAO = new RecursosDAO();
                AlocacaoDAO alocDAO = new AlocacaoDAO();
                List<Recurso> listaRec = redDAO.GetRecursos();
                
                DateTime data; 
                Alocacao alocacao;

                string[] listaHorarios = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P" };

                DateTime limite = new DateTime(2008, 8, 4);
                foreach (Recurso rec in listaRec)
                {
                    data = new DateTime(2008, 7, 11);
                    while (data < limite)
                    {
                        foreach (string aux in listaHorarios)
                        {
                            alocacao = Alocacao.newAlocacao(rec, data, aux, null, null);
                            alocDAO.InsereAlocacao(alocacao);
                            Console.WriteLine(alocacao.Data.ToShortDateString() + "\n" + alocacao.Horario + "\n" + rec.Descricao + " " + rec.Id + "\n");
                        }
                        data = data.AddDays(1);
                    }

                }

                Console.WriteLine("Data adicionadas com sucesso!");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro = " + e.Message);
                Console.ReadKey();
            }

        }
    }
}

