using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;
using BusinessData.DataAccess;
using System.Security;


namespace BusinessData.BusinessLogic
{
    public class RequisicoesBO
    {
        private RequisicaoDAO dao;

        public RequisicoesBO()
        {
            try
            {
                dao = new BusinessData.DataAccess.RequisicaoDAO();
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }

        public void DeletaRequisicao(Guid id)
        {

                    dao.DeleteRequisicao(id);

            
        }

        public void InsereRequisicao(IRequisicao requisicao)
        {

                    dao.InsertRequisicao(requisicao);

        }
        
        public List<Requisicao> GetRequisicaoByAulaAndPrioridade(Guid aulaId, Calendario cal, int prioridade)
        {

                return dao.GetRequisicaoByAulaAndPrioridade(aulaId, cal, prioridade);

        }

        public Requisicao GetRequisicaoByAulaPrioridadeAndCategoria(Guid aulaId, Calendario cal, int prioridade, Guid catrecursoId)
        {

                return dao.GetRequisicaoByAulaPrioridadeAndCategoria(aulaId, cal, prioridade, catrecursoId);

        }

        public List<Requisicao> GetRequisicoesPorAula(Guid? aulaId, Calendario cal)
        {

                return dao.GetRequisicoesPorAula(aulaId, cal);

        }

        public IList<Requisicao> GetRequisicoesPorCalendario(Calendario cal)
        {

                return dao.GetRequisicoesByCalendario(cal);

        }
        
        public IList<Requisicao> GetRequisicoesPorCalendario(Calendario cal,CategoriaRecurso categoriaRecurso)
        {

                return dao.GetRequisicoesByCalendario(cal,categoriaRecurso);

        }
    
        public Requisicao GetRequisicaoByAula(Turma turma, Entities.Calendario cal)
        {

                return dao.GetRequisicaoByTurma(turma, cal);


        }

        public void SetAtendida(Guid reqId)
        {

                dao.SetAtendida(reqId);

        }

        public void UpdateRequisicoes(IRequisicao req)
        {

                dao.UpdateRequisicoes(req);

        }
    }
}
