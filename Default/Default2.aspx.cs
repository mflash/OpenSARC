using System;
public partial class Default_Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Redireciona para Default.aspx
        Response.Redirect("Default.aspx", false);
    }
}