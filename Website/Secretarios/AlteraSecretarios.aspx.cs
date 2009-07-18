using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Net;
using System.Diagnostics;

public partial class Secretarios_AlteraSecretarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["GUID"] != null)
            {
                try
                {
                    SecretariosBO boSecretarios = new SecretariosBO();

                    try
                    {
                        Secretario sec = (Secretario)boSecretarios.GetPessoaById(new Guid(Request.QueryString["GUID"]));
                        txtEmail.Text = sec.Email;
                        txtMatricula.Text = sec.Matricula;
                        txtNome.Text = sec.Nome;
                        SetBtnLockUnlockText();

                    }
                    catch (FormatException )
                    {
                        Response.Redirect("~/Secretarios/ListaSecretarios.aspx");
                    }

                }
                catch (BusinessData.DataAccess.DataAccessException)
                {
                    Response.Redirect("~/Secretarios/ListaSecretarios.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Secretarios/ListaSecretarios.aspx");
            }

        }
    }

    protected void btnResetaSenha_Click(object sender, EventArgs e)
    {
        try
        {
            SecretariosBO boSecretarios = new SecretariosBO();
            Secretario sec = (Secretario)boSecretarios.GetPessoaById(new Guid(Request.QueryString["GUID"]));

            boSecretarios.ResetaSenha(sec);
            lblStatus.Text = "Senha resetada com sucesso!";
            lblStatus.Visible = true;
        }
        catch (ArgumentException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (Exception )
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Erro", "alert('Impossível resetar senha. Verifique se o usuário não está bloqueado');", true);
        }

    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            SecretariosBO boSecretarios = new SecretariosBO();
            Secretario sec = (Secretario)boSecretarios.GetPessoaById(new Guid(Request.QueryString["GUID"]));
            if (sec != null)
            {
                boSecretarios.UpdateEmail(sec, txtEmail.Text);
                lblStatus.Text = "Secretário alterado com sucesso";
                lblStatus.Visible = true;
                txtEmail.Text = "";
                Response.Redirect("~/Secretarios/ListaSecretarios.aspx");
            }
            else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Secretário não existente.");

        }
        catch (ArgumentException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }

    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Secretarios/ListaSecretarios.aspx");
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
        

        LogEntry log = new LogEntry();
        log.Message = usr.UserName + "teve seu acesso alterado por "  + Membership.GetUser().UserName;
        log.TimeStamp = DateTime.Now;
        log.Severity = TraceEventType.Information;
        log.Title = "Acesso";
        log.MachineName = Dns.GetHostName();
        Logger.Write(log);
        
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
