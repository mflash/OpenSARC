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

public partial class Pagina2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            try
            {
                Calendario cal = (Calendario)Session["Calendario"];
                TurmaBO turma = new TurmaBO();
                List<Turma> listaTurma = turma.GetTurmas(cal);
                listaTurma.Sort();
                if (listaTurma.Count == 0)
                {
                    lblStatus.Text = "Nenhuma turma cadastrada.";
                    lblStatus.Visible = true;
                }
                else
                {
                    grvListaTurmas.DataSource = listaTurma;
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
        }
    }          

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }

}
