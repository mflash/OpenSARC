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
using System.Collections.Generic;

public partial class Default_SelecionarCalendario : System.Web.UI.Page
{

    protected void PasswordRecovery1_VerifyingUser(object sender, LoginCancelEventArgs e)
    {
        // Get the entered email or username
        MembershipUser user = Membership.GetUser(PasswordRecovery1.UserName);
        if (user != null)
        {
            // Generate a new random password
            //string newPassword = GenerateRandomPassword();

            // Set the new password in the database
            string newPassword = user.ResetPassword();
            Membership.UpdateUser(user);
            //user.ChangePassword(user.ResetPassword(), newPassword);

            // Store the new password in ViewState (so it can be used in the email)
            ViewState["NewPassword"] = newPassword;
        }
        else
        {
            // Stop the process if the user does not exist
            e.Cancel = true;
            PasswordRecovery1.GeneralFailureText = "Usuário não encontrado";
        }
    }

    private string GenerateRandomPassword()
    {
        return Membership.GeneratePassword(10, 2);
    }

    protected void PasswordRecovery1_SendingEmail(object sender, MailMessageEventArgs e)
    {
        if (ViewState["NewPassword"] != null)
        {
            // Insert the new password into the email body
            string newPassword = ViewState["NewPassword"].ToString();
            e.Message.Subject = "OpenSARC - Nova senha";
            e.Message.Body = "Uma nova senha foi gerada para você: " + newPassword + "\n\n" + "ATENÇÃO: esta senha é local, preferencialmente faça login com a sua senha de rede";
        }
    }
}
