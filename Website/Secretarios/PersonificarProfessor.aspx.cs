using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using BusinessData.BusinessLogic;

public partial class Secretarios_PersonificarProfessor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string matricula = DropDownList1.SelectedValue;
        ProfessoresBO controleProfessores = new ProfessoresBO();
        controleProfessores.PersonificarProfessor(matricula);
        Response.Redirect("~/default/default.aspx");
    }
}
