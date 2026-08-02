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
using System.Collections.Generic;
using BusinessData.Entities;
using BusinessData.BusinessLogic;
using BusinessData.DataAccess;
using System.Security;
using System.Net.Mail;

public partial class Usuarios_Default: System.Web.UI.Page
{
    private IList<HorariosEvento> listaHorarios = new List<HorariosEvento>();
    private HorariosEventoBO horariosEventoBO = new HorariosEventoBO();
    
    private EventoBO eventoBO = new EventoBO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["listaHorarios"] = listaHorarios;
        }
    }

    protected void ckbEhRecorrente_CheckedChanged(object sender, EventArgs e)
    {
        if (RadioButtonList1.Visible == true)
        {
            RadioButtonList1.Visible = false;
            LimpaCampos();
        }
        else RadioButtonList1.Visible = true;
    }
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = RadioButtonList1.SelectedIndex;
        switch (index)
        {
            case 0 :
                txtNome.Enabled = false;
                break;

            case 1 :
                txtNome.Enabled = true;
                break;

            case 2 :
                txtNome.Enabled = false;
                break;
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }
    private void LimpaCampos()
    {
        txtNome.Text = "";
        RadioButtonList1.Visible = false;
        RadioButtonList1.SelectedIndex = -1;
    }

    protected void lbtnVoltar_Click1(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
    private void EnviarEmail(string pessoa, string descricaoEvento, DateTime data)
    {
        string para = "bernardosalada@gmail.com";
        string de = ConfigurationManager.AppSettings["MailMessageFrom"];
        MailMessage email = new MailMessage(de, para);
        email.Subject = "Alocação de Recursos";
        email.Body = "Sistema de Alocação de Recursos Computacionais FACIN \n\n" +
               "Um novo evento foi cadastrado por " + pessoa + "." +
               "\nDescrição: " + descricaoEvento +
               "\nData do evento: " + data.ToShortDateString() +
               "\nPara mais detalhes acesse a lista de eventos.";

        SmtpClient client = new SmtpClient();
        client.Send(email);
    }
    private void EnviarEmail(string pessoa, string descricaoEvento, DateTime dataInicial, DateTime dataFinal)
    {
        string para = "bernardosalada@gmail.com";
        string de = ConfigurationManager.AppSettings["MailMessageFrom"];
        MailMessage email = new MailMessage(de, para);
        email.Subject = "Alocação de Recursos";
        email.Body = "Sistema de Alocação de Recursos Computacionais FACIN \n\n" +
               "Um novo evento foi cadastrado por " + pessoa + "." +
               "\nDescrição: " + descricaoEvento +
               "\nData inicial do evento: " + dataInicial.ToShortDateString() +
               "\nData final do evento: " + dataFinal.ToShortDateString() +
               "\nPara mais detalhes acesse a lista de eventos.";

        SmtpClient client = new SmtpClient();
        client.Send(email);
    }
}
