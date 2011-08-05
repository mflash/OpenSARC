using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using BusinessData.Entities;
using BusinessData.BusinessLogic;

public partial class Teste_CriaDomingo : System.Web.UI.Page
{

    AlocacaoBO alocBO = new AlocacaoBO();
    RecursosBO controleRecursos = new RecursosBO();


    protected void Page_Load(object sender, EventArgs e)
    {
        
    }


    public void PreencheCalendarioDeAlocacoes(Calendario cal, Recurso rec, bool datasOrdenadas)
    {
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
                    //try
                    //{
                        alocBO.InsereAlocacao(alocacao);
                    //}
                    //catch (Exception e)
                    //{
                    //    throw e;
                    //}
                }
                data = data.AddDays(1);
            }
            
          data = data.AddDays(1);
            
        }
    }

    public void criaDomingo()
    {
        lblStatus.Text = "";
        //try
        //{
            Calendario cal = (Calendario)Session["Calendario"];

            List<Recurso> listaRecursos = controleRecursos.GetRecursos();
            
            foreach (Recurso recursoAux in listaRecursos)
            {
                if (recursoAux.Id != new Guid("c0575255-cf03-482f-9022-75c38b7d8ebb"))
                {
                    PreencheCalendarioDeAlocacoes(cal, recursoAux, true);
                }
            }

            lblStatus.Text = "Domingos criados com sucesso!";
        //}
        //catch (Exception e)
        //{
        //  lblStatus.Text = "Deu pau\n" + e.Message;
        //}


    
    }

    protected void criar_OnClick(object sender, EventArgs e)
    {
        criaDomingo();
    }
}
