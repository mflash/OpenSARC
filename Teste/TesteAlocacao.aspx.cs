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
using BusinessData.Distribuicao.BusinessLogic;

public partial class Teste_TesteEvento : System.Web.UI.Page
{
    
    

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnInserir_Click(object sender, EventArgs e)
    {
        ControleDistribuicao distrib = new ControleDistribuicao();
        distrib.DistribuirRecursos(2050, 2);
        //    try
    //    {
            
    //        Guid idEvento = new Guid(txtEventoId.Text);
    //        EventoBO eve = new EventoBO();
    //        Evento evento = eve.GetEventoById(idEvento);

    //        Guid idAula = new Guid(txtAulaId.Text);
    //        AulaBO aulabo = new AulaBO();
    //        Aula aula = aulabo.GetAulaById(idAula);

    //        Guid idRecurso = new Guid(txtRecursoId.Text);
    //        RecursosBO recursobo = new RecursosBO();
    //        Recurso rec = recursobo.GetRecursoById(idRecurso);

    //        DateTime dt = Convert.ToDateTime(txtData.Text);
    //        string horario = txtHorario.Text;
            
    //        Alocacao alo = new Alocacao(rec, dt, horario, aula, evento);
    //        AlocacaoBO alocacaoBO = new AlocacaoBO();

    //        alocacaoBO.InsereAlocacao(alo);

    //        lblResultadoInserir.Text = "Alocacao inserido com sucesso!";

    //    }
    //    catch (BusinessData.DataAccess.DataAccessException ex)
    //    {
    //        Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
    //    }
    }


    protected void btnDeletar_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    Guid idEvento = new Guid(txtHorario2.Text);
        //    eventoBO.DeletaEvento(idEvento);
        //    lblResultadoDeletar.Text = "Evento deletado!";
        //}
        //catch (BusinessData.DataAccess.DataAccessException ex)
        //{
        //    Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        //}
    }
    protected void btnAlterar_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    Guid idEvento = new Guid(txtRecursoId3.Text);
        //    Guid idProfessor = new Guid(txtProfessorId2.Text);
        //    Professor professor = professorBO.GetProfessorById(idProfessor);
        //    string descricao = txtDescricao2.Text;
        //    Evento evento = Evento.GetEvento(idEvento, professor, descricao, new DateTime(), null);

        //    eventoBO.UpdateEvento(evento);

        //    lblResultadoAlterar.Text = "Evento alterado";
        //}
        //catch (BusinessData.DataAccess.DataAccessException ex)
        //{
        //    Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        //}
    }
    
}
