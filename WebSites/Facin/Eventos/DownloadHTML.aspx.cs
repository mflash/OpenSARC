using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System.Collections.Generic;

public partial class Eventos_DownloadHTML : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DownHtml"] == null)
        {
            Server.Transfer("~/AcessoProfessores/ListaEventos.aspx");
            return;
        }

        dgEvento.DataSource = Session["DownHtml"] as DataTable;
        dgEvento.DataBind();

        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
        HtmlTextWriter txtSaida = new HtmlTextWriter(sw);
        Guid idturma = (Guid)Session["TurmaId"];
        string turma = "***";
        try
        {
            //TODO: criar CSS e remover BrowserTop



            txtSaida.Write("<html>\n");
            //TODO: css faz diferença?
            //txtSaida.Write("<head>\n\t<link href=\"http://localhost:1364/Facin/_layouts/css/exporthtml.css\" rel=\"stylesheet\" type=\"text/css\">\n</head>\n");
            txtSaida.Write("<body>\n");
            txtSaida.Write("<H1>\nEventos</H1>\n");
            dgEvento.RenderControl(txtSaida);
            txtSaida.Write("\n</body>\n</html>");


        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx");
        }
        finally
        {

            sw.Dispose();//FIXMW: close ou dispose?
        }

        Response.AddHeader("Content-disposition",
              "attachment; filename=eventos.html");
        Response.ContentType = "text/html";

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
    protected void dgEvento_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
