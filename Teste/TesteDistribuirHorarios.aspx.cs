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
using BusinessData.Entities;
using BusinessData.BusinessLogic;

public partial class Teste_TesteDistribuirHorarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnDistribuirHorarios_Click(object sender, EventArgs e)
    {
        Calendario calendario = (Calendario)Session["Calendario"];
        AulaBO aulaBO = new AulaBO();
        aulaBO.CriarAulas(calendario);
        
        //AlocacaoBO alocacaoBO = new AlocacaoBO();
        //alocacaoBO.preencheCalendario(calendario);
    }
}
