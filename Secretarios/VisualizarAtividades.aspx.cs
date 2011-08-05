using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BusinessData.Entities;
using BusinessData.Reports;
public partial class Secretarios_VisualizarAtividades : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Calendario atual = Session["Calendario"] as Calendario;
            if (atual != null)
            {
                hdfIdCalendario.Value = atual.Id.ToString();
            }
        }
    }
    protected void btnGerarRelatorio_Click(object sender, EventArgs e)
    {
        hdfIdsSelecionados.Value = GetSelectedIDs();   
        //GridView1.DataSource =  adap.GetData(GetSelectedIDs(), ((Calendario)Session["Calendario"]).Id);
        ReportViewer1.LocalReport.Refresh();       
        
    }

    public string GetSelectedIDs()
    {
        StringBuilder sb = new StringBuilder();

        foreach (ListItem li in cblCategoriasSelecionadas.Items)
        {
            if (li.Selected)
            {
                sb.AppendFormat("{0}, ", li.Value);
            }
        }

        sb.Remove(sb.Length - 2, 1);

        return sb.ToString();
    }
}
