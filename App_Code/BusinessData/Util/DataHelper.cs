using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessData.Util
{
    public class DataHelper
    {
        /// <summary>
        /// Calcula o intervalo entre dois dias da semana
        /// Considerar: 1-Domingo, 2-Segunda ...
        /// </summary>
        /// <param name="inicio">Primeiro dia</param>
        /// <param name="fim">Ultimo dia</param>
        /// <returns>intervalo entre os dias especificados </returns>
        public int CalculaIntervalo(int inicio, int fim)
        {
            if (fim > inicio)
            {
                return inicio - fim + 7;
            }
            else
            {
                return fim - inicio;
            }
        }

        /// <summary>
        /// Calcula o intervalo entre datas especificadas
        /// Considerar: 1-Domingo, 2-Segunda,...
        /// </summary>
        /// <param name="dias">Lista contendo as datas para calcular o intervalo</param>
        /// <returns>Array de inteiros com os intervalos entre os dias especificados</returns>
        public int[] CalculaIntervalo(int[] dias)
        {
            List<int> intervalo = new List<int>(dias.Length);
            //calcula o intervalo entre os dias da mesma semana
            for (int i = 0; i < dias.Length - 1; i++)
            {
                intervalo.Add(dias[i + 1] - dias[i]);
            }
            //calcula o intervalo entre o ultimo dia e o primeiro de outra semana                
            intervalo.Add(dias[0] - dias[dias.Length - 1] + 7);
            return intervalo.ToArray();
        }

        /// <summary>
        /// Calcula o calendario com o intervalo entre as datas passadas por parametro
        /// </summary>
        /// <param name="dias">Lista contendo as datas (2,4,6)</param>
        /// <param name="inicio">Data de inicio</param>
        /// <param name="fim">Data Final</param>
        /// <returns>Lista com as datas</returns>
        public List<DateTime> CalculaCalendario(string horario, DateTime inicio, DateTime fim)
        {
            //dado um horario pucrs(2ab,4cd), exclui os horarios e guarda os dias em array de inteiros
            string diasPucrs = Regex.Replace(horario, "[a-zA-Z]", String.Empty);
            

            int tamanho = diasPucrs.Length;
            List<int> dias = new List<int>(tamanho);
            for(int i=0; i<tamanho;i++)
            {
                dias.Add(Convert.ToInt32(diasPucrs.Substring(i,1)));
            }
            
            dias.Sort();//certifica-se que estao em ordem

        
            
            List<DateTime> calendario = new List<DateTime>();
            DateTime aux = inicio;


            while (aux <= fim)
            {
                foreach (int dia in dias)
                {
                    if ((int)(aux.DayOfWeek) == (dia - 1))
                    {
                        calendario.Add(aux);
                    }
                }
                aux = aux.AddDays(1);
            }
            
            return calendario;
        }
        public static string GetDia(DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Sunday: return "DOM";
                case DayOfWeek.Monday: return "SEG";
                case DayOfWeek.Tuesday: return "TER";
                case DayOfWeek.Wednesday: return "QUA";
                case DayOfWeek.Thursday: return "QUI";
                case DayOfWeek.Friday: return "SEX";
                case DayOfWeek.Saturday: return "SAB";
            }
            throw new ArgumentException("Especifique um dia valido");
        }
        public static string GetDiaPUCRS(DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Sunday: return "D";
                case DayOfWeek.Monday: return "2";
                case DayOfWeek.Tuesday: return "3";
                case DayOfWeek.Wednesday: return "4";
                case DayOfWeek.Thursday: return "5";
                case DayOfWeek.Friday: return "6";
                case DayOfWeek.Saturday: return "7";
            }
            throw new ArgumentException("Especifique um dia valido");
        }
    
    }

    
}
