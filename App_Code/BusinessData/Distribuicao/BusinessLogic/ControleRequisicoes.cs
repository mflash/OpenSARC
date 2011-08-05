using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Entities;
using BusinessData.Distribuicao.Catalogos; 
namespace BusinessData.Distribuicao.BusinessLogic
{
    public class ControleRequisicoes
    {

        public ColecaoRequisicoes GetRequisicoes(BusinessData.Entities.Calendario cal, ColecaoDias dias, ColecaoCategoriaDeRecursos categorias, ColecaoTurmas turmas)
        {
            ColecaoRequisicoes colAux = new ColecaoRequisicoes();
            BusinessData.BusinessLogic.RequisicoesBO controleRequisicoes = new BusinessData.BusinessLogic.RequisicoesBO();


            ICollection<BusinessData.Entities.Requisicao> requisicoes = controleRequisicoes.GetRequisicoesPorCalendario(cal);

            Requisicao aux;
            Dia dia;
            Horarios.HorariosPUCRS horario;
            Guid id;
            TurmaDistribuicao turma;
            CategoriaRecurso categoria;


            foreach (BusinessData.Entities.Requisicao req in requisicoes)
            {
                dia = dias.Find(req.Aula.Data);
                horario = Horarios.Parse(req.Aula.Hora);
                id = req.IdRequisicao;
                categoria = categorias.Find(req.CategoriaRecurso);
                turma = turmas.Find(req.Aula.TurmaId);

                aux = new Requisicao(dia, horario, turma, categoria, req.Prioridade,req.EstaAtendida);
                colAux.Add(aux);
            }

            return colAux;
        }

        public ColecaoRequisicoes GetRequisicoes(Calendario cal, CategoriaRecurso cat)
        {
            ColecaoRequisicoes colAux = new ColecaoRequisicoes();
            BusinessData.BusinessLogic.RequisicoesBO controleRequisicoes = new BusinessData.BusinessLogic.RequisicoesBO();
            
            
            ICollection<BusinessData.Entities.Requisicao> requisicoes = controleRequisicoes.GetRequisicoesPorCalendario(cal.EntidadeCalendario, cat.EntidadeCategoria);

            Requisicao aux;
            Dia dia;
            Horarios.HorariosPUCRS horario;
            Guid id;
            TurmaDistribuicao turma;
            CategoriaRecurso categoria;
            

            foreach (BusinessData.Entities.Requisicao req in requisicoes)
            {
                dia = cal.Dias.Find(req.Aula.Data);
                horario = Horarios.Parse(req.Aula.Hora);
                id = req.IdRequisicao;
                categoria = cal.Categorias.Find(req.CategoriaRecurso);
                turma = cal.Turmas.Find(req.Aula.TurmaId);
                
                aux = new Requisicao(dia,horario,turma,categoria,req.Prioridade,req.EstaAtendida);
                colAux.Add(aux);
            }

            return colAux;
        }       
        
    }
}
