using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Catalogos;
using BusinessData.Distribuicao.Entities;

namespace BusinessData.Distribuicao.BusinessLogic
{
    public class ControleDias
    {
        /// <summary>
        /// Retorna todos os dias letivos de um calendario especificado
        /// </summary>
        /// <param name="cal"></param>
        /// <returns></returns>
        public ColecaoDias GetColecaoDias(BusinessData.Entities.Calendario cal)
        {

            ColecaoDias colecao = new ColecaoDias();
            Dia aux;


            for (DateTime data = cal.InicioG1; data <= cal.FimG2; data = data.AddDays(1.0))
            {
                if (data.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }
                aux = new Dia(data);
                colecao.Add(aux);
            }
            return colecao;
        }
    }
}
