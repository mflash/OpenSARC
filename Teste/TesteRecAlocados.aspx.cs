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
using BusinessData.Entities;
using System.Collections.Generic;
using BusinessData.DataAccess;

public partial class Teste_TesteRecAlocados : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        AulaBO aulabo = new AulaBO();
        Guid g = new Guid("63529733-08BB-466C-BDE5-1F822BDC358C");
        //Aula aula = aulabo.GetAulaById(new Guid("63529733-08BB-466C-BDE5-1F822BDC358C"));
        Aula aula = null;
        Evento evento = null;
        AlocacaoDAO aloc = new AlocacaoDAO();
        RecursosBO rec = new RecursosBO();
        List<Alocacao> lista = new List<Alocacao>();
        DateTime data = new DateTime(2007, 08, 01);
        string hora = "np";
        Alocacao a = new Alocacao(rec.GetRecursoById(g), data, hora, aula, evento);

        aloc.UpdateAlocacao(a);

        //lista = aloc.GetRecursosAlocados(data, hora);

        

    }
}
