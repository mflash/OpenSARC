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
using System.IO;
using System.Drawing;
using System.Text;

public partial class AcessoProfessores_DownloadHTML : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DownHtml"] == null)
        {
            Server.Transfer("~/AcessoProfessores/SelecionaTurma.aspx");
        }
        else
        {
            
            dgAulas.ItemDataBound += new DataGridItemEventHandler(dgAux_ItemDataBound);
            dgAulas.DataSource = Session["DownHtml"] as DataTable;
            dgAulas.DataBind();

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms,Encoding.UTF8);
            HtmlTextWriter txtSaida = new HtmlTextWriter(sw);
            try
            {
                string path = Server.MapPath("~/AcessoProfessores/BrowserTop.txt");
                StreamReader sr = null;
                try
                {
                    sr = File.OpenText(path);


                    txtSaida.Write(sr.ReadToEnd());
                    dgAulas.RenderControl(txtSaida);
                    txtSaida.Write("\n</body>\n</html>");
                }
                catch (Exception )
                {
                    Response.Redirect("~/Default/Erro.aspx");
                }
                finally
                {
                    sr.Dispose();
                }
             
            }
            catch (System.IO.IOException)
            {
                Response.Redirect("~/Default/Erro.aspx");
            }
            finally
            {
                sw.Dispose();
            }

            Response.AddHeader("Content-disposition",
                  "attachment; filename=Horarios.html");
            Response.ContentType = "application/octet-stream";

            try
            {
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
            finally
            {
                ms.Close();
            }
        }
    }

    void dgAux_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
         if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
         {
             Label lblAux = (Label)e.Item.FindControl("lblCorDaData");
             e.Item.BackColor = Color.FromName(lblAux.Text);
         }
    }
}
