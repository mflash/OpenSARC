using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Teste_Loggin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string host = Request["host"];
        string user = Request["user"];
        string status = Request["status"];
        string mac = Request["mac"];
        DateTime datetime = DateTime.Now;

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SARCFACINcs"].ConnectionString))
        {
            con.Open();

            string inse = "insert into Loggin (host, usuario, datahora, status, mac) values(@h, @u, @d, @s, @m)";
            using (SqlCommand insertlog = new SqlCommand(inse, con))
            {
                insertlog.Parameters.AddWithValue("@h", host);
                insertlog.Parameters.AddWithValue("@u", user);
                insertlog.Parameters.AddWithValue("@d", datetime);
                insertlog.Parameters.AddWithValue("@s", status);
                insertlog.Parameters.AddWithValue("@m", mac);

                try
                {
                    insertlog.ExecuteNonQuery();
                    //Response.Redirect("login.aspx");
                }
                catch (Exception ex)
                {
                    Response.Write("<b>something really bad happened.....Please try again</b> ");
                }
                finally
                {
                    con.Close();
                }
            }
        }
        //Response.Write(host + " - " + user + " - " + datetime.ToString() + "<br>");
    }
}