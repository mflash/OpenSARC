using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessData.BusinessLogic;
using BusinessData.Entities;

public partial class Secretarios_DetalhesEvento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Evento"] != null)
        {
            try
            {
                Guid idEvento = new Guid(Request.QueryString["Evento"]);
                //Mengue old code
                HorariosEventoBO heBO = new HorariosEventoBO();
                try
                {
                    EventoBO gerenciadorEventos = new EventoBO();
                    Evento evento =gerenciadorEventos.GetEventoById(idEvento);
                    lblTituloEvento.Text = evento.Titulo;

                    List<HorariosEvento> horariosEvento = heBO.GetHorariosEventosByIdDetalhados(idEvento);
                    horariosEvento.Sort();
                    dgHorariosEvento.DataSource = horariosEvento;
                    dgHorariosEvento.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/Default/Erro.aspx?Erro=Erro ao listar horários: "+ex.Message);
                }
            }
             
            catch (ArgumentNullException)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Evento Inválido.");
            }
            catch (FormatException)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Evento Inválido.");
            }
            catch (OverflowException)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=Evento Inválido.");
            }
        }
    }

    protected void dgHorariosEvento_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            AlocacaoBO alocBO = new AlocacaoBO();

            Label lblEventoId = (Label)e.Item.FindControl("lblEventoId");
            Label lblRecursosAlocados = (Label)e.Item.FindControl("lblRecursosAlocados");
            Label lblRecursosAlocadosId = (Label)e.Item.FindControl("lblRecursosAlocadosId");
            Label lblData = (Label)e.Item.FindControl("lblData");
            Label lblHorario = (Label)e.Item.FindControl("lblHorario");

            List<HorariosEvento> horariosEvento = (List<HorariosEvento>)dgHorariosEvento.DataSource;
            if (horariosEvento[e.Item.ItemIndex].HorarioInicio == "EE")
            {
                lblHorario.Text = "E";
            }
            else lblHorario.Text = horariosEvento[e.Item.ItemIndex].HorarioInicio;

            DateTime data = Convert.ToDateTime(lblData.Text);

            List<Recurso> recAlocados = alocBO.GetRecursoAlocadoByEvento(data, horariosEvento[e.Item.ItemIndex].HorarioInicio, new Guid(lblEventoId.Text));

            if (recAlocados.Count != 0)
            {
                for (int i = 0; i < recAlocados.Count - 1; i++)
                {

                    lblRecursosAlocados.Text += recAlocados[i].Descricao + ", ";
                    lblRecursosAlocadosId.Text += recAlocados[i].Id + ",";
                }
                lblRecursosAlocados.Text += recAlocados[recAlocados.Count - 1].Descricao;
                lblRecursosAlocadosId.Text += recAlocados[recAlocados.Count - 1].Id.ToString();
            }
            else lblRecursosAlocados.Text = "";
        }

    }

    protected void dgHorariosEvento_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        Label lblData = (Label)e.Item.FindControl("lblData");
        Label lblHorario = (Label)e.Item.FindControl("lblHorario");
        Label lblEventoId = (Label)e.Item.FindControl("lblEventoId");

        Session["Data"] = lblData.Text;
        Session["Horario"] = lblHorario.Text;

        string id = lblEventoId.Text;

        if (e.CommandName == "Selecionar")
        {
            // abre a popup de selecao de recursos
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
            @"window.open('Recursos.aspx?EventoId=" + id + "', '','width=350, height=220, menubar=no, resizable=no');", true);

        }

        if (e.CommandName == "Trocar")
        {
            Label lblRecursosAlocadosId = (Label)e.Item.FindControl("lblRecursosAlocadosId");
            Session["RecursosIds"] = lblRecursosAlocadosId.Text;

            // abre a popup de troca de recursos
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
            @"window.open('TrocarRecurso.aspx?EventoId=" + id + "', '','width=400, height=220, menubar=no, resizable=no');", true);
        }

        if (e.CommandName == "Transferir")
        {
            Label lblRecursosAlocadosId = (Label)e.Item.FindControl("lblRecursosAlocadosId");
            Session["RecursosIds"] = lblRecursosAlocadosId.Text;

            // abre a popup de transferencia de recursos
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "OnClick",
            @"window.open('TransferirRecurso.aspx?EventoId=" + id + "', '','width=350, height=220, menubar=no, resizable=no');", true);
        }
    }
}
