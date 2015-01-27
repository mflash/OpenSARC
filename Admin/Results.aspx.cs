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
using BusinessData.Distribuicao.Entities;
using BusinessData.Distribuicao.Catalogos;
using BusinessData.Distribuicao.BusinessLogic;
using System.Diagnostics;
//using BusinessData.Entities;

public class TurmaRelat : IComparable<TurmaRelat>
{
    public Guid Id { get; set; }
    public int Numero { get; set; }
    public BusinessData.Entities.Disciplina Disciplina { get; set; }
    public string DataHora { get; set; }
    public BusinessData.Entities.Professor Professor { get; set; }
    public BusinessData.Entities.Curso Curso { get; set; }
    public int Pedidos { get; set; }
    public int Atendidos { get; set; }
    public string Satisfacao { get; set; }
    public double Satisf { get; set; }

    public int CompareTo(TurmaRelat other)
    {
        if (Satisf > other.Satisf)
            return 1;
        else if (Satisf < other.Satisf)
            return -1;
        string meuNome = Disciplina.Nome + " - " + Numero;
        string outroNome = other.Disciplina.Nome + " - " + other.Numero;
        return meuNome.CompareTo(outroNome);
    }
}

public partial class Admin_Results : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((bool)Session["Complete"] != true)
        {
            Response.Write("<META HTTP-EQUIV='refresh' content='2;URL=''>");
        }
        else
        {
            //            pnlAguarde.Visible = false;
            lblStatus.Text = "Recursos distribuidos com sucesso (N/S = recursos não solicitados)";
            lblStatus.Visible = true;
            lbtnVoltar.Visible = true;

            BusinessData.Entities.Calendario cal = (BusinessData.Entities.Calendario)Session["Calendario"];
            ControleCalendario calendarios = new ControleCalendario();
            Calendario calAtual = calendarios.GetCalendario(cal.Ano, cal.Semestre);

            Dictionary<Guid, SatisfacaoTurma> satTurmas = new Dictionary<Guid, SatisfacaoTurma>();
            //Preenche a colecao para todas as turmas do semestre
            foreach (TurmaDistribuicao t in calAtual.Turmas)
            {
                satTurmas.Add(t.EntidadeTurma.Id, new SatisfacaoTurma(t));
            }

            ColecaoRequisicoes listaReq = (ColecaoRequisicoes)Session["ReqResult"];
            string ant = "";
            foreach (Requisicao req in listaReq)
            {
                Guid id = req.Turma.EntidadeTurma.Id;
                string atual = req.Turma.EntidadeTurma.Id + req.Dia.Data.ToShortDateString() + req.Horario;
                // Se a turma estiver no dicionário (todas devem estar) e a turma+dia+horário
                // do último recurso atendido não for o mesmo, conta essa requisição
                // como válida
                if (satTurmas.ContainsKey(id))// && !atual.Equals(ant))
                {
                    /*
                    if (req.Turma.EntidadeTurma.Disciplina.Nome.Contains("Nome da Disciplina") &&
                      req.Turma.EntidadeTurma.Professor.Nome.Contains("Nome do Professor"))
                    {
                        Debug.WriteLine(req.Prioridade + ": " + req.Dia.Data.ToShortDateString() + " " +
                            " - " + req.Horario + " - " +
                            req.CategoriaRecurso.Descricao + " - " + req.EstaAtendido);
                        Debug.WriteLine("   atual: " + atual);
                        Debug.WriteLine("   ant:   " + ant);
                    }
                     */

                    if(!atual.Equals(ant))
                       satTurmas[id].Pedidos++;                    
                    if (req.EstaAtendido)
                    {
                        satTurmas[id].Atendimentos++;
//                        if (atual.Equals(ant))
//                            satTurmas[id].Pedidos--;
                    }
                    ant = req.Turma.EntidadeTurma.Id + req.Dia.Data.ToShortDateString() + req.Horario;
                }
            }

            List<TurmaRelat> listaTurmas = new List<TurmaRelat>();
            foreach (TurmaDistribuicao t in calAtual.Turmas)
            {
                Guid id = t.EntidadeTurma.Id;

                int atend = satTurmas[id].Atendimentos;
                int pedidos = satTurmas[id].Pedidos;
                double satisfDouble = 1;
                string satisf = "100%";
                if (pedidos > 0)
                {
                    satisfDouble = atend / (double)pedidos;
                    satisf = String.Format("{0:P}", satisfDouble);
                }
                bool dadosOK = t.EntidadeTurma.Disciplina.Categoria.Descricao.Contains("Teórica") ||
                            t.EntidadeTurma.Disciplina.Categoria.Descricao.Contains("PPG") ||
                            t.EntidadeTurma.Curso.Nome.Contains("PPG") ||
                            t.EntidadeTurma.Curso.Nome.StartsWith("Física");

                // Recursos não foram solicitados para essa turma, mas deveriam ter sido
                if(!dadosOK && pedidos == 0) {
                    satisfDouble = 0;
                    satisf = "N/S";
                }

                listaTurmas.Add(new TurmaRelat
                {
                    Id = id,
                    Numero = t.EntidadeTurma.Numero,
                    Disciplina = t.EntidadeTurma.Disciplina,
                    Curso = t.EntidadeTurma.Curso,
                    DataHora = t.EntidadeTurma.DataHora,
                    Professor = t.EntidadeTurma.Professor,
                    Pedidos = pedidos,
                    Atendidos = atend,
                    Satisfacao = satisf,
                    Satisf = satisfDouble
                });
                //                Response.Write(t.EntidadeTurma.Disciplina+ " - " + 
                //                    t.EntidadeTurma.Numero + " ("+ t.EntidadeTurma.Professor+") " +
                //                    atend + " de " + pedidos + "<br/>");
            }
            listaTurmas.Sort();
            grvListaTurmas.DataSource = listaTurmas;
            grvListaTurmas.RowDataBound += grvListaTurmas_RowDataBound;
            grvListaTurmas.DataBind();
        }
    }

    void grvListaTurmas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            double satisf = 0;
            if(e.Row.Cells[8].Text != "N/S")
                satisf = double.Parse(e.Row.Cells[8].Text.Replace("%", "")) / 100;
            //            double satisf = Double.Parse(e.Row.Cells[1].Text);
            if (satisf >= 0.8)
                e.Row.Cells[8].ForeColor = System.Drawing.Color.Green;
            else if (satisf >= 0.5)
                e.Row.Cells[8].ForeColor = System.Drawing.Color.Orange;
            else
                e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;
        }
    }
}
