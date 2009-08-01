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
                try
                {
                    dao.DeleteRequisicao(id);
                }
                catch (DataAccess.DataAccessException )
                {
                    throw;
                }
            
        }

        public void InsereRequisicao(IRequisicao requisicao)
        {
                try
                {
                    dao.InsertRequisicao(requisicao);
                }
                catch (DataAccess.DataAccessException )
                {
                    throw;
                }
        }
        
        public List<Requisicao> GetRequisicaoByAulaAndPrioridade(Guid aulaId, Calendario cal, int prioridade)
        {
            try
            {
                return dao.GetRequisicaoByAulaAndPrioridade(aulaId, cal, prioridade);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }

        public Requisicao GetRequisicaoByAulaPrioridadeAndCategoria(Guid aulaId, Calendario cal, int prioridade, Guid catrecursoId)
        {
            try
            {
                return dao.GetRequisicaoByAulaPrioridadeAndCategoria(aulaId, cal, prioridade, catrecursoId);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }

        public List<Requisicao> GetRequisicoesPorAula(Guid? aulaId, Calendario cal)
        {
            try
            {
                return dao.GetRequisicoesPorAula(aulaId, cal);
            }
            catch (DataAccess.DataAccessException )
            {
                throw;
            }
        }

        public IList<Requisicao> GetRequisicoesPorCalendario(Calendario cal)
        {
            try
            {
                return dao.GetRequisicoesByCalendario(cal);
            }
            catch (DataAccess.DataAccessException)
            {
                throw;
            }
        }
        
        public IList<Requisicao> GetRequisicoesPorCalendario(Calendario cal,CategoriaRecurso categoriaRecurso)
        {
            try
            {
                return dao.GetRequisicoesByCalendario(cal,categoriaRecurso);
            }
            catch(DataAccess.DataAccessException)
            {
                throw;
            }
        }
    
        public Requisicao GetRequisicaoByAula(Turma turma, Entities.Calendario cal)
        {
            try
            {
                return dao.GetRequisicaoByTurma(turma, cal);
            }
            catch (DataAccessException )
            {
                throw;
            }

        }

        public void SetAtendida(Guid reqId)
        {
            try
            {
                dao.SetAtendida(reqId);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }

        public void UpdateRequisicoes(IRequisicao req)
        {
            try
            {
                dao.UpdateRequisicoes(req);
            }
            catch (DataAccessException )
            {
                throw;
            }
        }
    }
}
