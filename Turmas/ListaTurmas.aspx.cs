using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System.Security;
using Remotion.Collections;
using System.Text.RegularExpressions;

public partial class Pagina2 : System.Web.UI.Page
{
    class TurmaInfra : Turma
    {
        public TurmaInfra(Turma t)
        {
            this.Disciplina = t.Disciplina;
            this.Sala = t.Sala;
            this.Curso = t.Curso;
            this.Professor = t.Professor;
            this.DataHora = t.DataHora;
            this.Numero = t.Numero;
        }

        public string Infra { get; set; }
    };

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (!IsPostBack)
        //{
        try
        {
            Calendario cal = (Calendario)Session["Calendario"];
            TurmaBO turma = new TurmaBO();
            List<Turma> listaTurma = turma.GetTurmas(cal);
            listaTurma.Sort();
            var setSalasClimatizadas = new HashSet<string>
                    { "32/A/310", "32/A/301", "32/A/410", "32/A/508", "32/A/211.04" };
            var regexSalas = new List<string>
                {
                    "15/*", "30/C/*", "30/D/*", "32/A/107*", "32/A/51[3456]"
                };
            List<TurmaInfra> listTurmaInfra = new List<TurmaInfra>();
            foreach (var t in listaTurma)
            {
                if (chkMostrarNotes.Checked && !t.Notebook)
                    continue;
                string laptop = "";
                if (t.Notebook)
                    laptop = "\U0001f4bb";
                if (setSalasClimatizadas.Contains(t.Sala))
                    laptop += "\u2744";
                else
                    foreach (var r in regexSalas)
                        if (Regex.IsMatch(t.Sala, r))
                        {
                            laptop += "\u2744";
                            break;
                        }
                TurmaInfra turmaInfra = new TurmaInfra(t);
                turmaInfra.Infra = laptop;
                listTurmaInfra.Add(turmaInfra);
//                t.Disciplina.Nome = laptop + t.Disciplina.Nome;
            }
            if (listTurmaInfra.Count == 0)
            {
                lblStatus.Text = "Nenhuma turma cadastrada.";
                lblStatus.Visible = true;
            }
            else
            {
                grvListaTurmas.DataSource = listTurmaInfra;
                grvListaTurmas.DataBind();
            }
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        // }
    }

    protected void grvListaTurmas_RowEditing(object sender, GridViewEditEventArgs e)
    {

        try
        {
            TurmaBO turma = new TurmaBO();
            Guid id = (Guid)grvListaTurmas.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/Turmas/EditarTurma.aspx?GUID=" + id.ToString());
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
    protected void grvListaTurmas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            TurmaBO turma = new TurmaBO();
            Calendario cal = (Calendario)Session["Calendario"];
            Guid id = (Guid)grvListaTurmas.DataKeys[e.RowIndex].Value;

            turma.DeletaTurma(id);
            lblStatus.Text = "Turma excluída com sucesso.";
            lblStatus.Visible = true;

            grvListaTurmas.DataSource = turma.GetTurmas(cal);
            grvListaTurmas.DataBind();
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


    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
