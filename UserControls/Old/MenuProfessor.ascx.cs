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

public partial class Default_MenuProfessor : System.Web.UI.UserControl
{
    
    //protected void Page_Load(object sender, EventArgs e)
    protected internal override void OnInit(EventArgs e)
    {

        Label lbl = new Label();
        lbl.Text = "Algoritmos...";
        lbl.CssClass = "ms-toolbar";
        phClassListing.Controls.Add(lbl);

    }
}
