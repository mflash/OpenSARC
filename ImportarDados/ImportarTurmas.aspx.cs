using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessData.Entities;
using BusinessData.BusinessLogic;
using System.Collections.Specialized;
using System.Security;

public partial class ImportarDados_Default : System.Web.UI.Page
{
    CalendariosBO calBo = new CalendariosBO();
    IList<Turma> listaTurmas;
    IList<TurmaSemProfessor> listaTurmasNone;
    IList<Professor> listaProfessores;

    protected void Page_Load(object sender, EventArgs e)
    {
        listaTurmas = Session["listaTurmas"] as IList<Turma>;
        listaTurmasNone = Session["listaTurmasNone"] as IList<TurmaSemProfessor>;
        if (!Page.IsPostBack)
        {
            
            ProfessoresBO controleProfessores = new ProfessoresBO();
            listaProfessores = controleProfessores.GetProfessores();
            Session.Add("listaProfessores", listaProfessores);
        }
        listaProfessores = Session["listaProfessores"] as IList<Professor>;
    }

    
    
    protected void lbtnImportarTurmas_Click(object sender, EventArgs e)
    {
        SPABO sistAcademico = new SPABO();
        //Calendario calId = (Calendario)(Session["Calendario"]);

        // Ugly fix        
        CalendariosBO cadastroCalendarios = new CalendariosBO();
        List<Calendario> listaCalendarios = cadastroCalendarios.GetCalendarios();
        listaCalendarios.Sort();
        Session["Calendario"] = listaCalendarios[0];
        Calendario calId = (Calendario)(Session["Calendario"]);

        listaTurmas = sistAcademico.getTurmas(calId.Id);
        grvListaTurmaOk.DataSource = listaTurmas;
        grvListaTurmaOk.DataBind();
        //Session["listaTurmas"] = listaTurmas;
        for (int i = 0; i < grvListaTurmaOk.Rows.Count; i++)
        {
            populaDropOk(i, listaTurmas[i].Professor.Nome.ToString());
        }
        listaTurmasNone = sistAcademico.getTurmasSemProfessor(calId.Id);
        grvTurmasNone.DataSource = listaTurmasNone;
        grvTurmasNone.DataBind();
        Session["listaTurmasNone"] = listaTurmasNone;
    }

    private void ImportarTurmas()
    {
        SPABO sistAcademico = new SPABO();
        try
        {
            Calendario calId = (Calendario)(Session["Calendario"]);
            sistAcademico.ImportarTurmas(calId.Id);
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }

    protected void grvListaTurmaOk_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grvListaTurmaOk_RowEditing(object sender, GridViewEditEventArgs e)
    {
        listaTurmas = Session["listaTurmas"] as IList<Turma>;
        grvListaTurmaOk.DataSource = listaTurmas;
        grvListaTurmaOk.DataBind();
        grvListaTurmaOk.EditIndex = e.NewEditIndex;
        for (int i = 0; i < grvListaTurmaOk.Rows.Count; i++)
        {
            populaDropOk(i, listaTurmas[i].Professor.Nome.ToString());
        }
    }

    protected void grvListaTurmaOk_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        listaProfessores = Session["listaProfessores"] as IList<Professor>;
        try
        {
            DropDownList ddlProfessores = (DropDownList)grvListaTurmaOk.Rows[e.RowIndex].FindControl("ddlProfessoresOk");
            Guid idSelecionado = new Guid(ddlProfessores.SelectedValue);
            Professor professor = null;
            listaProfessores = Session["listaProfessores"] as IList<Professor>;
            foreach (Professor p in listaProfessores)
            {
                if (p.Id.Equals(idSelecionado))
                {
                    professor = p;
                    break;
                }
            }
            if (professor != null)
            {
                listaTurmas = (IList<Turma>)Session["listaTurmas"];
                listaTurmas[e.RowIndex].Professor = professor;
                TextBox txtHorario = (TextBox)grvListaTurmaOk.Rows[e.RowIndex].FindControl("txtHorario");
                listaTurmas[e.RowIndex].DataHora = txtHorario.Text;
                Session["listaTurmas"] = listaTurmas;
                grvListaTurmaOk.EditIndex = -1;
                grvListaTurmaOk.DataSource = Session["listaTurmas"];
                grvListaTurmaOk.DataBind();
                for (int i = 0; i < grvListaTurmaOk.Rows.Count; i++)
                {
                    populaDropOk(i, listaTurmas[i].Professor.Nome.ToString());
                }
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (SecurityException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    protected void grvListaTurmaOk_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvListaTurmaOk.EditIndex = -1;
        grvListaTurmaOk.DataSource = Session["listaTurmas"];
        grvListaTurmaOk.DataBind();
        for (int i = 0; i < grvListaTurmaOk.Rows.Count; i++)
        {
            populaDropOk(i, listaTurmas[i].Professor.Nome.ToString());
        }
    }

  

    private void populaDropOk(int index, string nome)
    {
        if (grvListaTurmaOk.Rows.Count != 0)
        {
            DropDownList ddlProfessores = (DropDownList)grvListaTurmaOk.Rows[index].FindControl("ddlProfessoresOk");
            if (ddlProfessores != null)
            {
                int indice = -1;
                for (int i = 0; i < listaProfessores.Count; i++)
                {
                    if (listaProfessores[i].Nome == nome)
                    {
                        indice = i;
                        break;
                    }
                }

                ddlProfessores.DataSource = listaProfessores;
                ddlProfessores.DataTextField = "Nome";
                ddlProfessores.DataValueField = "Id";
                ddlProfessores.SelectedIndex = indice;
                ddlProfessores.DataBind();
            }
        }
    }

 

    protected void grvTurmasNone_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvTurmasNone.EditIndex = e.NewEditIndex;
        populaDrop(e.NewEditIndex);
    }

    protected void grvTurmasNone_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvTurmasNone.EditIndex = -1;
        grvTurmasNone.DataSource = Session["listaTurmasNone"];
        grvTurmasNone.DataBind();
        for (int i = 0; i < grvListaTurmaOk.Rows.Count; i++)
        {
            populaDropOk(i, listaTurmas[i].Professor.Nome.ToString());
        }
    }

   

    protected void grvTurmasNone_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            DropDownList ddlProfessores = (DropDownList)grvTurmasNone.Rows[e.RowIndex].FindControl("ddlProfessores");
            Guid idSelecionado = new Guid(ddlProfessores.SelectedValue);
            Professor professor = null;

            listaProfessores = Session["listaProfessores"] as IList<Professor>;
            foreach (Professor p in listaProfessores)
            {
                if (p.Id.Equals(idSelecionado))
                {
                    professor = p;
                    break;
                }
            }
            if (professor != null)
            {
                TurmaSemProfessor turmaSemProfessor = listaTurmasNone[e.RowIndex];
                Turma turma = Turma.NewTurma(turmaSemProfessor.Numero, turmaSemProfessor.Calendario, turmaSemProfessor.Disciplina, turmaSemProfessor.DataHora, professor, turmaSemProfessor.Curso);
                listaTurmas = (IList<Turma>)Session["listaTurmas"];
                listaTurmas.Add(turma);
                Session["listaTurmas"] = listaTurmas;


                listaTurmasNone = (IList<TurmaSemProfessor>)Session["listaTurmasNone"];
                listaTurmasNone.Remove(turmaSemProfessor);
                Session["listaTurmasNone"] = listaTurmasNone;
                grvTurmasNone.DataSource = listaTurmasNone;
                grvTurmasNone.DataBind();
                grvListaTurmaOk.DataSource = listaTurmas;
                grvListaTurmaOk.DataBind();
            }
            populaDrop(e.RowIndex);
            for (int i = 0; i < grvListaTurmaOk.Rows.Count; i++)
            {
                populaDropOk(i, listaTurmas[i].Professor.Nome.ToString());
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (SecurityException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    private void populaDrop(int index)
    {
        if (grvTurmasNone.Rows.Count != 0)
        {
            DropDownList ddlProfessores = (DropDownList)grvTurmasNone.Rows[index].FindControl("ddlProfessores");
            if (ddlProfessores != null)
            {
                ddlProfessores.DataSource = listaProfessores;
                ddlProfessores.DataTextField = "Nome";
                ddlProfessores.DataValueField = "Id";
                ddlProfessores.DataBind();
            }
        }
    }

  

    protected void wzImportarTurmas_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            SPABO sistAcademico = new SPABO();
            //listaTurmas = (IList<Turma>)Session["listaTurmas"];
            //Calendario calId = (Calendario)(Session["Calendario"]);

            // Ugly fix        
            CalendariosBO cadastroCalendarios = new CalendariosBO();
            List<Calendario> listaCalendarios = cadastroCalendarios.GetCalendarios();
            listaCalendarios.Sort();
            Session["Calendario"] = listaCalendarios[0];
            Calendario calId = (Calendario)(Session["Calendario"]);

            listaTurmas = sistAcademico.getTurmas(calId.Id);
            sistAcademico.ImportaTurmas(listaTurmas);
            lblSucesso.Text = "Turmas importadas com sucesso!";
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }
}
