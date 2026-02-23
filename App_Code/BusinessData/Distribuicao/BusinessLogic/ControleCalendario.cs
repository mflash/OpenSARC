using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Distribuicao.Entities;
using BusinessData.BusinessLogic;
using BusinessData.Distribuicao.Catalogos;
using BusinessData.Util;

namespace BusinessData.Distribuicao.BusinessLogic
{
    public class ControleCalendario
    {
        public Calendario GetCalendario(int ano, int semestre)
        {
            BusinessData.BusinessLogic.CalendariosBO controleCalendarios = new CalendariosBO();
            BusinessData.Entities.Calendario cal = controleCalendarios.GetCalendarioByAnoSemestre(ano,semestre);
            
            ControleDias dias = new ControleDias();
            ControleCategorias categorias = new ControleCategorias();
            ControleRequisicoes requisicoes = new ControleRequisicoes();
            ControleTurmas turmas = new ControleTurmas();
            BusinessData.BusinessLogic.CategoriaDisciplinaBO categoriasDeDisciplina = new CategoriaDisciplinaBO();

            List<BusinessData.Entities.CategoriaDisciplina> catalogoCategoriasDisciplina = categoriasDeDisciplina.GetCategoriaDisciplinas();
            DebugLog.Write("Obtendo dias...");
            ColecaoDias catalogoDias = dias.GetColecaoDias(cal);
            ColecaoCategoriaDeRecursos catalogoCategorias = categorias.GetCategorias();
            DebugLog.Write("Obtendo turmas...");
            ColecaoTurmas catalogoTurmas = turmas.GetTurmas(cal,catalogoCategoriasDisciplina);

            DebugLog.Write("Obtendo requisições...");
            ColecaoRequisicoes colReq = requisicoes.GetRequisicoes(cal, catalogoDias, catalogoCategorias, catalogoTurmas);
            
            return new Calendario(
                cal,
                catalogoDias,
                colReq,
                catalogoTurmas,
                catalogoCategorias,
                catalogoCategoriasDisciplina
                );
        }
    }
}
