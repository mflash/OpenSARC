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
using System.Security;
using BusinessData.DataAccess;

public partial class Professores_CadastroProfessores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCadastrar_Click(object sender, EventArgs e)
    {
        Professor p = Professor.NewProfessor(txtMatricula.Text, txtNome.Text, txtEmail.Text);
        
        try
        {
            ProfessoresBO professores = new ProfessoresBO();
            professores.InsertPessoa(p,txtPergunta.Text,txtResposta.Text);               
            lblStatus.Text = "Professor cadastrado com sucesso.";
            lblStatus.Visible = true;
            txtMatricula.Text = "";
            txtNome.Text = "";
            txtEmail.Text = "";
            txtPergunta.Text = "";
            txtResposta.Text = "";
            
        }
        catch (DataAccessException ex)
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
