using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using BusinessData.Entities;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Diagnostics;

public partial class PGTest : System.Web.UI.Page
{
    Dictionary<String, Professor> profs;

    protected void Page_Load(object sender, EventArgs e)
    {
        profs = new Dictionary<string, Professor>();
               
        string cs = ConfigurationManager.ConnectionStrings["OracleCS"].ConnectionString;        
        using (OracleConnection oconn = new OracleConnection(cs))
        {
            oconn.Open();
            OracleCommand c = oconn.CreateCommand();
            c.CommandText = "SELECT DISTINCT CDDISCIPL,CD_DISCIPLINA FROM GRANDEPORTE.EF_DISCIPLINA_TURMA edt ORDER BY CDDISCIPL";

            OracleDataReader or = c.ExecuteReader();
            while (or.Read())
            {
                Response.Write(or.FieldCount + ": "+or.GetString(0) + " " + or.GetString(1) +"<br>");
            }
        }        
    }
}