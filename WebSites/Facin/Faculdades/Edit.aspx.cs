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
using BusinessData.Entities;
using BusinessData.BusinessLogic;
using System.Security;
using BusinessData.DataAccess;

public partial class Vinculos_Edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["GUID"] != null)
            {
              
                try
                {
                    FaculdadesBO boVinculos = new FaculdadesBO();

                    try
                    {
                        Faculdade vinc = boVinculos.GetFaculdadeById(new Guid(Request.QueryString["GUID"]));
                        txtDescricao.Text = vinc.Nome;
                    }
                    catch (FormatException ex)
                    {
                        Response.Redirect("~/Faculdades/List.aspx");  
                    }                
                    
                    
                }
                catch (DataAccessException)
                {
                    Response.Redirect("~/Faculdades/List.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Faculdades/List.aspx");
            }
        }
    }
    
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        

        try
        {
            FaculdadesBO boVinculos = new FaculdadesBO();
            Faculdade vinc = boVinculos.GetFaculdadeById(new Guid(Request.QueryString["GUID"]));
            if (vinc != null)
            {
                vinc.Nome = txtDescricao.Text;


                boVinculos.UpdateFaculdade(vinc);
                lblStatus.Text = "Faculdade atualizada com sucesso.";
                lblStatus.Visible = true;
                Response.Redirect("~/Faculdades/List.aspx");
            }
            else Response.Redirect("~/Default/Erro.aspx?Erro=" + "Faculdade não existente.");
        }
        catch (DataAccessException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        catch (SecurityException ex)
        {
            Response.Redirect("~/Default/Erro.aspx?Erro=" + ex.Message);
        }
        
    }

    protected void lbtnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Faculdades/List.aspx");
    }

}
