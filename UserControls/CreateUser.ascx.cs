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
using System.Security;

public partial class Default_CreateUser : System.Web.UI.UserControl
{
    public string RedirectUrl
    {
        get
        {
            try
            {
                return (string)ViewState["RedirectURL"];
            }
            catch (IndexOutOfRangeException)
            {
                return string.Empty;
            }
        }

        set
        {   
            ViewState["RedirectURL"] = value; 
        }
    }
    
    public string UserName
    {
        get { return txtLogin.Text; }
    }

    public string Email
    {
        get { return txtEmail.Text; }
    }

    public Guid UserId
    {
        get { return (Guid)ViewState["UserId"]; }
    }
 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rblRoles.DataSource = Roles.GetAllRoles();
            rblRoles.DataBind();
            rblRoles.Items.Remove("Professor");
            lblsenha.Text = "A senha deve conter " + Membership.MinRequiredPasswordLength + " dígitos.";
        }

    }

    protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (rblRoles.SelectedIndex != -1)
        {
            if (rblRoles.SelectedValue == "Secretario")
            {
                cuwCriarUsuario(this,EventArgs.Empty);         
            }
        }  
    }

    protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (rblRoles.SelectedIndex == 0)
            EscondeNome();
        if (rblRoles.SelectedIndex == 1)
            MostraNome();
    }

    public void MostraNome()
    {
        txtNome.Enabled = true;
        txtNome.Visible = true;
        lblNome.Visible = true;
        RequiredFieldValidator5.Enabled = true;
    }

    public void EscondeNome()
    {
        txtNome.Enabled = false;
        txtNome.Visible = false;
        lblNome.Visible = false;
        RequiredFieldValidator5.Enabled = false;
    }

    protected void cuwCriarUsuario(object sender, EventArgs e)
    {
        if (rblRoles.SelectedValue == "Admin")
        {
            MembershipCreateStatus resultado;
            string pass = txtConfirmarSenha.Text;
            Membership.CreateUser(this.UserName, pass, this.Email, "question", "answer", true, out resultado);
            Roles.AddUserToRole(this.UserName, "Admin");

           if (resultado != MembershipCreateStatus.Success)
           { 
               throw new MembershipCreateUserException(resultado.ToString());
           }
        }
        else if (rblRoles.SelectedValue == "Secretario")
        {
            try
            {
                Secretario sec = Secretario.NewSecretario(txtLogin.Text, txtNome.Text, txtEmail.Text);
                try
                {
                    MembershipFactory fabricaMembership = MembershipFactory.GetInstance();
                    PessoaBaseBO pessoaBaseBO = fabricaMembership.CreatePessoaBase(sec);
                    if (ckbSenha.Checked)
                        pessoaBaseBO.InsertPessoa(sec, "Uma Pergunta?", "Uma Resposta.");
                    else
                        pessoaBaseBO.InsertPessoa(sec, txtSenha.Text, "Uma Pergunta?", "Uma Resposta.");
                    rblRoles.SelectedIndex = -1;
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
            catch (System.Configuration.Provider.ProviderException)
            {
                throw new Exception("Impossivel adicionar usuario ao papel selecionado");
            }
        }
    }

    protected void btnRedirecionar_Click(object sender, EventArgs e)
    {
        Response.Redirect(this.RedirectUrl);
    }
    protected void ckbSenha_CheckedChanged(object sender, EventArgs e)
    {
        if (ckbSenha.Checked)
        {
            txtSenha.Enabled = false;
            txtConfirmarSenha.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
            RequiredFieldValidator3.Enabled = false;
        }
        else
        {
            txtSenha.Enabled = true;
            txtConfirmarSenha.Enabled = true;
            RequiredFieldValidator2.Enabled = true;
            RequiredFieldValidator3.Enabled = true;
        }
    }
    protected void btnRedirecionar_Click(object sender, WizardNavigationEventArgs e)
    {
        Response.Redirect("~/Default/CadastrarAdmin.aspx");
    }
}
