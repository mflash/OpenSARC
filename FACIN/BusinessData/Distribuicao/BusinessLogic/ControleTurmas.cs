using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Catalogos;
using BusinessData.Distribuicao.Entities;

namespace BusinessData.Distribuicao.BusinessLogic
{
    class ControleTurmas
    {
        public ColecaoTurmas GetTurmas(BusinessData.Entities.Calendario cal,List<BusinessData.Entities.CategoriaDisciplina> categoriasDeDisciplina)
        {
            BusinessData.BusinessLogic.TurmaBO controleTurmas = new BusinessData.BusinessLogic.TurmaBO();
            BusinessData.BusinessLogic.AulaBO controleAulas = new BusinessData.BusinessLogic.AulaBO();
            ColecaoTurmas colecao = new ColecaoTurmas();
            List<BusinessData.Entities.Turma> listaTurmas = controleTurmas.GetTurmas(cal,categoriasDeDisciplina);

            TurmaDistribuicao turmaAux;
            foreach (BusinessData.Entities.Turma turma in listaTurmas)
            {
                turmaAux = new TurmaDistribuicao(turma, controleAulas.GetAulas(turma.Id));
                colecao.Add(turmaAux);
            }

            return colecao;
        }
    }
}
