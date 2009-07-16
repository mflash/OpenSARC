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

public partial class Admin_Results : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["Complete"] != true)
        {
            Response.Write("<META HTTP-EQUIV='refresh' content='2;URL=''>");
        }
        else
        {
            pnlAguarde.Visible = false;
            lblStatus.Text = "Recursos distribuidos com sucesso!";
            lblStatus.Visible = true;
            lbtnVoltar.Visible = true;
        }
    }
}
