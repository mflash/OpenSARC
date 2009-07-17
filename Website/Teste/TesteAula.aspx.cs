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
using BusinessData.BusinessLogic;
using BusinessData.Entities;

public partial class Teste_TesteAula : System.Web.UI.Page
{
    AulaBO aulaBO = new AulaBO();
    TurmaBO turmaBO = new TurmaBO();
    CategoriaAtividadeBO categoriaAtividadeBO = new CategoriaAtividadeBO();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    //Insere aula na tabela Aulas
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            Guid idTurma = new Guid(txt_id_turma.Text);
            Turma turma = turmaBO.GetTurmaById(idTurma);
            int y, m, d;
            string dataStr = txt_data.Text;
            d = Convert.ToInt16(dataStr.Substring(0, 2));
            m = Convert.ToInt16(dataStr.Substring(3, 2));
            y = Convert.ToInt16(dataStr.Substring(6, 4));
            Guid idCategoria = new Guid(txt_categoria.Text);
            CategoriaAtividade categoria = categoriaAtividadeBO.GetCategoriaAtividadeById(idCategoria);
            
            DateTime data = new DateTime(y, m, d);
            Aula aula = Aula.newAula(turma, txt_hora.Text, data, txt_descricao.Text, categoria);

            aulaBO.InsereAula(aula);

            lbl_resultado.Text = "Aula inserida";
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }
    
    //Deleta aula da tabela Aulas
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            Guid idAula = new Guid(txt_aulaid.Text);
            
            aulaBO.DeletaAula(idAula);

            lbl_resultado2.Text = "Aula deletada";
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }
    
    //Altera aula da tabela Aulas
    protected void  btn_alterar_Click(object sender, EventArgs e)
    {
        try
        {
            Guid idAula = new Guid(txt_aulaid_altera.Text);
            Guid idCategoria = new Guid(txt_categoria_altera.Text);
            CategoriaAtividade categoria = categoriaAtividadeBO.GetCategoriaAtividadeById(idCategoria);
            Aula aula = Aula.GetAula(idAula, null, null, new DateTime(), txt_descricao_altera.Text, categoria);

            aulaBO.UpdateAula(aula);

            lbl_resultado3.Text = "Aula alterada";
        }
        catch (BusinessData.DataAccess.DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
    }

    //Lista aulas da tabela Aulas
    protected void btn_listar_Click(object sender, EventArgs e)
    {
      //  grd_aulas.DataSource = 
    }
}
