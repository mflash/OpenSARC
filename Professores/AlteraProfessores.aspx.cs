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

public partial class Professores_AlteraProfessores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["GUID"] != null)
            {
                try
                {
                    ProfessoresBO boProfessores = new ProfessoresBO();

                    try
                    {
                        Professor prof = (Professor)boProfessores.GetPessoaById(new Guid(Request.QueryString["GUID"]));
                        txtEmail.Text = prof.Email;
                        txtMatricula.Text = prof.Matricula;
                        txtNome.Text = prof.Nome;
                        SetBtnLockUnlockText();

                    }
                    catch (FormatException )
                    {
                        Response.Redirect("~/Professores/ListaProfessores.aspx");
                    }

                }
                catch (BusinessData.DataAccess.DataAccessException)
                {
                    Response.Redirect("~/Professores/ListaProfessores.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Professores/ListaProfessores.aspx");
            }
            
        }

    }

    protected void btnResetaSenha_Click(object sender, EventArgs e)
    {
        try
        {
            ProfessoresBO boProfessores = new ProfessoresBO();
            Professor prof = (Professor)boProfessores.GetPessoaById(new Guid(Request.QueryString["GUID"]));

			MembershipUser pessoa = Membership.GetUser(prof.Id);
			string newPassword = pessoa.ResetPassword();
			Membership.UpdateUser(pessoa);
			
			// Nao envia mais email, erro no relay (?)

//            boProfessores.ResetaSenha(prof);
            lblStatus.Text = "Senha resetada com sucesso: nova senha "+newPassword;
            lblStatus.Visible = true;
        }
        catch (ArgumentException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Erro", "alert('Impossível resetar senha. Verifique se o usuário não está bloqueado');", true);
			//ScriptManager.RegisterClientScriptBlock(this, GetType(), "Erro", "alert('"+ex.Message+"')");//Impossível resetar senha. Verifique se o usuário não está bloqueado');", true);
			Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    
        
    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            ProfessoresBO boProfessores = new ProfessoresBO();
            Professor prof = (Professor)boProfessores.GetPessoaById(new Guid(Request.QueryString["GUID"]));
            if (prof != null)
            {
                boProfessores.UpdateEmail(prof, txtEmail.Text);
                lblStatus.Text = "Professor alterado com sucesso";
                lblStatus.Visible = true;
                txtEmail.Text = "";
                Response.Redirect("~/Professores/ListaProfessores.aspx");
            }
            else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Professor não existente.");

        }
        catch (ArgumentException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }


    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Professores/ListaProfessores.aspx");
    }
    protected void btnLockUnlock_Click(object sender, EventArgs e)
    {
        MembershipUser usr = Membership.GetUser(txtMatricula.Text);

        if (usr.IsLockedOut || !usr.IsApproved)
        {
            if (!usr.UnlockUser())
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Impossivel alterar acesso do usuario");
            }
            usr.IsApproved = true;
            Membership.UpdateUser(usr);            
        }
        else
        {
            usr.IsApproved = false;
            Membership.UpdateUser(usr);
        }
        SetBtnLockUnlockText();
    }

    private void SetBtnLockUnlockText()
    {
        MembershipUser usr = Membership.GetUser(txtMatricula.Text);
        if (usr.IsLockedOut || !usr.IsApproved)
        {
            btnLockUnlock.Text = "Desbloquear Conta";
        }
        else
        {
            btnLockUnlock.Text = "Bloquear Conta";
        }
    }
}
