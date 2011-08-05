using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;

namespace BusinessData.Distribuicao.Entities
{
    public class Recurso
    {
        private ICollection<BusinessData.Entities.Alocacao> alocacoes;
        private BusinessData.Entities.Recurso rec;

        public Recurso(BusinessData.Entities.Recurso rec)
        {
            this.rec = rec;
            alocacoes = new List<BusinessData.Entities.Alocacao>();
        }
        public ICollection<BusinessData.Entities.Alocacao> Alocacoes
        {
            get { return alocacoes; }
        }

        public bool EstaDisponivel(string horario)
        {
            if (!rec.EstaDisponivel)
                return false;
            
            foreach (HorarioBloqueado h in rec.HorariosBloqueados)
            {
                if(h.HoraInicio[0] <= horario[0] && h.HoraFim[h.HoraFim.Length-1] >= horario[horario.Length-1])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Verifica se um Recurso está alocado na data e horário Especificados
        /// através de uma pesquisa sequencial O(n)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="horario"></param>
        public bool EstaAlocado(DateTime data, string horario)
        {
            foreach (Alocacao aloc in alocacoes)
            {
                if (aloc.Data == data && aloc.Horario.Equals(horario))
                {
                    return true;
                }
            }
            return false;
        }

        public void Aloca(DateTime data, string horario, TurmaDistribuicao t)
        {
            Aula aula = t.GetAula(data, horario);
            Alocacao aloc = Alocacao.newAlocacao(rec, data, horario, aula, null);
            alocacoes.Add(aloc);
        }
    }
}
