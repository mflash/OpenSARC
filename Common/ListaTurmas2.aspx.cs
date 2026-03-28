using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessData.BusinessLogic;
using BusinessData.Entities;
using System.Security;
using System.Text.RegularExpressions;
using System.Linq;

public partial class Pagina2 : System.Web.UI.Page
{
    class TurmaInfra : Turma
    {
        public TurmaInfra(Turma t)
        {
            this.Id = t.Id;
            this.Disciplina = t.Disciplina;
            this.Sala = t.Sala;
            this.Curso = t.Curso;
            this.Professor = t.Professor;
            this.DataHora = t.DataHora;
            this.Numero = t.Numero;
            this.Calendario = t.Calendario;
            this.Notebook = t.Notebook;
        }

        public string Infra { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                Calendario cal = (Calendario)Session["Calendario"];
                TurmaBO turma = new TurmaBO();
                List<Turma> listaTurma = turma.GetTurmas(cal);
                listaTurma.Sort();

                // Salas com ar-condicionado
                var setSalasClimatizadas = new HashSet<string>
                    { "32/A/310", "32/A/301", "32/A/410", "32/A/508", "32/A/211.04" };
                
                // Regex para salas climatizadas
                var regexSalas = new List<string>
                {
                    "11/*", "15/*", "30/[ABCDF]/*", "32/A/10[789]*", "32/A/51[34567]"
                };

                List<TurmaInfra> listaTurmaInfra = new List<TurmaInfra>();
                foreach (var t in listaTurma)
                {
                    TurmaInfra turmaInfra = new TurmaInfra(t);
                    
                    string icones = "";
                    
                    // Ícone de laptop
                    if (t.Notebook)
                        icones = "💻";
                    
                    // Ícone de ar-condicionado (floco de neve)
                    if (setSalasClimatizadas.Contains(t.Sala))
                    {
                        icones += "❄️";
                    }
                    else
                    {
                        foreach (var r in regexSalas)
                        {
                            if (Regex.IsMatch(t.Sala, r))
                            {
                                icones += "❄️";
                                break;
                            }
                        }
                    }
                    
                    // Format DataHora with space every 3 characters
                    string dataHoraFormatted = string.Join(" ", Regex.Matches(t.DataHora, ".{1,3}").Cast<Match>().Select(m => m.Value));
                    turmaInfra.DataHora = dataHoraFormatted;

                    turmaInfra.Infra = icones;
                    
                    listaTurmaInfra.Add(turmaInfra);
                }

                if (listaTurmaInfra.Count == 0)
                {
                    lblStatus.Text = "Nenhuma turma cadastrada.";
                    lblStatus.Visible = true;
                }
                else
                {
                    grvListaTurmas.DataSource = listaTurmaInfra;
                    grvListaTurmas.DataBind();
                }
            }
            catch (BusinessData.DataAccess.DataAccessException ex)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
            }
        }
    }          

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default/PaginaInicial.aspx");
    }
}
