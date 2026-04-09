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
using System.Text.RegularExpressions;
using OracleInternal.SqlAndPlsqlParser;

public partial class Docentes_DownloadCSV2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DownCSV"] == null)
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

        DataTable tabela = Session["DownCSV"] as DataTable;
        DisciplinasBO disciplinasBO = new DisciplinasBO();
        //Dictionary<String, int> mapeamentoDisciplinas = Session["codcreds"] as Dictionary<String, int>;

        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
//        HtmlTextWriter txtSaida = new HtmlTextWriter(sw);

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
            string codigo = turma.Disciplina.Cod;
            int codDisciplinaAtas = disciplinasBO.GetMapeamentoDisciplinasAtas(codigo);

            titulo = String.Format("{0}-{1:d2}-{2}", disciplina.Cod, disciplina.Cred, turma.Numero); //+ " " + disciplina.Nome + " (" + turma.Numero + ") - " + Regex.Replace(turma.Sala, "32/A", "32");

//            int codDisciplinaAtas = mapeamentoDisciplinas[disciplina.Cod];
            string cred = string.Format("{0:D2}", disciplina.Cred);

            bool longa = false;
            if (turma.DataHora.Length == 6)
                if (turma.DataHora[0] == turma.DataHora[3])
                    longa = true;
            
            int aula = 1;
            bool pula = true;
            string ultPlanoAula = string.Empty;
            sw.Write("cdDisciplinaOrigem;nr_aula;cd_tipo_aula;tx_plano_aula;");
            foreach (DataRow dr in tabela.Rows)
            {
                string nrAula = dr["#"] as String;
                if (nrAula == String.Empty) continue;

                string tipoAula = dr["Atividade"] as String;
                if (tipoAula == "Suspensão de Aulas") continue;
                switch (tipoAula)
                {
                    case "Aula":
                    case "Ensino Remoto":
                        tipoAula = "6"; break;
                    case "Prova": tipoAula = "2"; break;
                    case "Prova de Substituição": tipoAula = "4"; break;
                    case "Evento Acadêmico": tipoAula = "6"; break;
                    case "Trabalho": tipoAula = "7"; break;
                }

                string planoAula = (dr["Descrição"] as String).Trim().Replace(";",",");
                // string dataAula = (dr["Data"] as String).Replace('/','-');

                if (longa)
                {
                    if (pula)
                    {
                        ultPlanoAula = planoAula;
                        pula = !pula;
                        continue;
                    }
                    else planoAula = ultPlanoAula + " / " + planoAula;
                }
                
                sw.Write(string.Format("\n{0};{1};{2};{3};",
                    codDisciplinaAtas, aula, tipoAula, planoAula));
                aula++;
                ultPlanoAula = string.Empty;
                pula = true;
            }
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
              "attachment; filename=" + titulo + ".csv");
        Response.ContentType = "text/csv";
        Response.ContentEncoding = Encoding.UTF8;
//        Response.Write(sw.ToString());
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
