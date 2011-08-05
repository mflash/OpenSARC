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
using BusinessData.BusinessLogic;
using System.Collections.Generic;
using BusinessData.Distribuicao.Entities;
using BusinessData.Distribuicao.BusinessLogic;

public partial class Teste_TesteRecursos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_get_Click(object sender, EventArgs e)
    {
        /*RecursosBO bo = new RecursosBO();
        List<Recurso> lista = new List<Recurso>();
        List<Recurso> lista2 = new List<Recurso>();
        lista = bo.GetRecursosAlocados();
        DateTime dt = Convert.ToDateTime(txtData.Text);
        lista2 = bo.GetRecursosDisponiveis(dt, txtHorario.Text);

        lblstatus.Text = "Deu certo";*/
        ControleCalendario calendarios = new ControleCalendario();
        Calendario cal = calendarios.GetCalendario(2007,2);
    }
}
