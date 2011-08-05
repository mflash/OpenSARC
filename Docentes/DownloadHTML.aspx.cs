using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessData.BusinessLogic;
using BusinessData.Entities;

public partial class Docentes_DownloadHTML : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DownHtml"] == null)
        {
            Server.Transfer("~/Docentes/SelecionaTurma.aspx");
            return;
        }

        if (Session["TurmaId"] == null)
        {
            // FIXME: nem sempre o usuário estava na ListaEventos.
            Server.Transfer("~/Docentes/ListaEventos.aspx");
            return;
        }

        dgAulas.ItemDataBound += new DataGridItemEventHandler(dgAux_ItemDataBound);
        dgAulas.DataSource = Session["DownHtml"] as DataTable;
        dgAulas.DataBind();

        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
        HtmlTextWriter txtSaida = new HtmlTextWriter(sw);

        Guid idturma = (Guid)Session["TurmaId"];
        string titulo = "***";
        try
        {
            AulaBO AulaBO = new AulaBO();
            List<Aula> listaAulas = null;
            try
            {
                listaAulas = AulaBO.GetAulas(idturma);
            }
            catch (Exception)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Codigo de turma inválido!");
                return;
            }
            Turma turma = listaAulas[0].TurmaId;
            Disciplina disciplina = turma.Disciplina;
            titulo = disciplina.Cod + "-"+disciplina.Cred + " " +disciplina.Nome + " (" + turma.Numero + ")";

            txtSaida.Write("<html>\n");
            txtSaida.Write("<head>\n</head>\n");
            txtSaida.Write("<body>\n");
            txtSaida.Write("<H1>\n" + titulo + "</H1>\n");
            dgAulas.RenderControl(txtSaida);
            txtSaida.Write("\n</body>\n</html>");
        }
        catch (System.IO.IOException)
        {
            Response.Redirect("~/Default/Erro.aspx");
            return;
        }
        finally
        {
            sw.Dispose();
        }

        Response.AddHeader("Content-disposition",
              "attachment; filename=cronograma.html");
        Response.ContentType = "text/html";
        Response.ContentEncoding = Encoding.UTF8;

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

    void dgAux_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lblAux = (Label)e.Item.FindControl("lblCorDaData");
            e.Item.BackColor = Color.FromName(lblAux.Text);
        }
    }
}
