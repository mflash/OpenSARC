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

public partial class Teste_TesteEvento : System.Web.UI.Page
{
    EventoBO eventoBO = new EventoBO();
    ProfessoresBO professorBO = new ProfessoresBO();
    CalendariosBO calendariosBO = new CalendariosBO();
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void btnInserir_Click(object sender, EventArgs e)
    {
        /*try
        {
            Guid idProfessor = new Guid(txtProfessorId.Text);
            Professor professor = professorBO.GetProfessorById(idProfessor);
            string descricao = txtDescricao.Text;
            string dataS = txtDataHora.Text;
            int d, m, y, h, mi, s;
            d = Convert.ToInt16(dataS.Substring(0, 2));
            m = Convert.ToInt16(dataS.Substring(3, 2));
            y = Convert.ToInt16(dataS.Substring(6, 4));
            h = Convert.ToInt16(dataS.Substring(11, 2));
            mi = Convert.ToInt16(dataS.Substring(14, 2));
            s = Convert.ToInt16(dataS.Substring(17, 2));
            DateTime dataDT = new DateTime(y, m, d, h, mi, s);
            Guid idCalendario = new Guid(txtCalendarioId.Text);
            Calendario calendario = calendariosBO.GetCalendario(idCalendario);
            Evento evento = Evento.newEvento(professor, descricao, calendario);
            eventoBO.InsereEvento(evento);
            lblResultadoInserir.Text = "Evento inserido com sucesso!";

        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }*/
    }


    protected void btnDeletar_Click(object sender, EventArgs e)
    {
        /*try
        {
            Guid idEvento = new Guid(txtEventoId.Text);
            eventoBO.DeletaEvento(idEvento);
            lblResultadoDeletar.Text = "Evento deletado!";
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }*/
    }
    protected void btnAlterar_Click(object sender, EventArgs e)
    {
        /*try
        {
            Guid idEvento = new Guid(txtEventoId2.Text);
            Guid idProfessor = new Guid(txtProfessorId2.Text);
            Professor professor = professorBO.GetProfessorById(idProfessor);
            string descricao = txtDescricao2.Text;
            Evento evento = Evento.GetEvento(idEvento, professor, descricao, null);

            eventoBO.UpdateEvento(evento);

            lblResultadoAlterar.Text = "Evento alterado";
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }*/
    }
}
