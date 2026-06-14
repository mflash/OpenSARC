using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Caching;
using System.Web.UI;
using BusinessData.BusinessLogic;
using BusinessData.DataAccess;
using BusinessData.Entities;

public partial class _Painel : System.Web.UI.Page
{
    private SRRCDAO srrcDAO = new SRRCDAO();
    private SortedDictionary<string, RecursoSRRC> recs;
    private SortedDictionary<string, Usuario> users;

    private Usuario currentUser = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            recs = new SortedDictionary<string, RecursoSRRC>();
            CarregaUsuarios();
            CarregaRecursos();
            //CarregaUltimaAtividade();

            Session["USERS"] = users;
            Session["RECS"] = recs;
        }
        else
        {
            users = (SortedDictionary<string, Usuario>)Session["USERS"];
            recs = (SortedDictionary<string, RecursoSRRC>)Session["RECS"];
            currentUser = (Usuario)Session["CURRENT"];
        }
    }

    protected void btnConsultaMatricula_Click(object sender, EventArgs e)
    {
        if(ConsultarMatricula(txtMatricula.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "focusRecurso",
                string.Format("document.getElementById('{0}').focus();", txtRecurso.ClientID), true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "focusMatricula",
                string.Format("document.getElementById('{0}').focus();", txtMatricula.ClientID), true);
        }
    }

    protected void btnConsultaRecurso_Click(object sender, EventArgs e)
    {
        RecursoSRRC rec = ConsultarRecurso(txtRecurso.Text.Trim());
        if (rec != null)
        {
            Debug.WriteLine("RECURSO: [" + rec.ToString() + "]");
            Confirm(rec);
        }
    }

    private bool ConsultarMatricula(string matricula)
    {
        try
        {
            Debug.WriteLine("FIND: [" + matricula + "]");
            Usuario u = FindUser(matricula);
            bool ok = false;

            if (u != null)
            {
                string nome = UserControls_DashboardRecursos.getNomeSobrenomeProfessor(u.Nome);
                lblAviso.CssClass = "small text-white";
                lblAviso.Text = string.Format("<i class='bi bi-person-check-fill me-1'></i>{0}", nome);
                ok = true;
                currentUser = u;
            }
            else
            {
                lblAviso.CssClass = "small text-danger";
                lblAviso.Text = "<i class='bi bi-person-x-fill me-1'></i>Matrícula não encontrada.";
                currentUser = null;
            }
            Session["CURRENT"] = currentUser;
            return ok;
        }
        catch (Exception)
        {
            lblAviso.CssClass = "text-warning small";
            lblAviso.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i>Erro ao consultar matrícula.";
        }
        return false;
    }

    private RecursoSRRC ConsultarRecurso(string id)
    {
        try
        {
            id = id.ToUpper();
            if (id.Length < 4) return null;
            foreach(RecursoSRRC r in recs.Values)
            {
                if(r.Id == id)
                {
                    //lblStatus.Text = r.Descricao;
                    lblAviso.CssClass = "small text-white";
                    lblAviso.Text = string.Format("<i class='bi bi-building-check me-1'></i>{0}", r.Descricao);
                    return r;
                }
            }
            lblAviso.CssClass = "text-danger small";
            lblAviso.Text = "<i class='bi bi-building-x me-1'></i>Recurso não encontrado.";
            return null;
        }
        catch (Exception)
        {
            lblAviso.CssClass = "text-warning small";
            lblAviso.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i>Erro ao consultar recurso.";
        }
        return null;
    }

    protected void Confirm(RecursoSRRC r)
    {
        DateTime now = DateTime.Now;
        LogData ld = srrcDAO.FindLatestActivity(r.Descricao);
        string user = currentUser.Nome;
        string resource = r.Descricao;
        string unitCourse = currentUser.Unidade;

        if(ld != null && ld.Acao == "RETIRADA")
        {
            string lastUser = ld.Usuario;
            ld = new LogData(now, "ENTREGA", user, resource, unitCourse, currentUser.TipoUsuarioChar);
            r.Status = RecursoSRRC.StatusRecurso.DISPONIVEL;

            string u = UserControls_DashboardRecursos.getNomeSobrenomeProfessor(user);
            lblAviso.CssClass = "small text-white";
            lblAviso.Text = string.Format(
                "<i class='bi bi-box-arrow-in-left me-1 text-warning'></i>{0} — {1} &nbsp; <span class='text-white-50'>{2}</span>",
                u, resource, now.ToString("HH:mm"));
        }
        else
        {
            ld = new LogData(now, "RETIRADA", user, resource, unitCourse, currentUser.TipoUsuarioChar);
            r.Status = RecursoSRRC.StatusRecurso.RETIRADO;

            string u = UserControls_DashboardRecursos.getNomeSobrenomeProfessor(user);
            lblAviso.CssClass = "small text-white";
            lblAviso.Text = string.Format(
                "<i class='bi bi-box-arrow-right me-1 text-warning'></i>{0} — {1} &nbsp; <span class='text-white-50'>{2}</span>",
                u, resource, now.ToString("HH:mm"));
        }
        r.LastUser = user;
        r.LastTime = now;
        r.TipoUser = currentUser.Tipo;
        srrcDAO.AddToLog(ld);
        Dashboard1.Refresh();
        ScriptManager.RegisterStartupScript(this, GetType(), "confirmDone",
            string.Format("document.getElementById('{0}').value=''; document.getElementById('{1}').value=''; document.getElementById('{0}').focus();",
                txtMatricula.ClientID, txtRecurso.ClientID), true);
    }

    private void CarregaRecursos()
    {
        List<RecursoSRRC> lista = srrcDAO.LoadResources();
        foreach (RecursoSRRC rec in lista)
        {
            recs[rec.Abrev] = rec;//.Add(rec.Id, rec);
        }
        //CarregaUltimaAtividade();
    }

    private void CarregaUsuarios()
    {
        List<Usuario> lista = null;
        if (Cache["PROFS"] == null)
        {
            lista = srrcDAO.LoadProfs();
            Cache.Add("PROFS", lista, null, DateTime.Now.AddYears(1), TimeSpan.Zero, CacheItemPriority.High, null);
        }
        else
        {
            lista = (List<Usuario>)Cache["PROFS"];
        }
        users = new SortedDictionary<string, Usuario>();
        foreach (Usuario user in lista)
        {
            users.Add("01" + user.Matricula.Substring(2) + "01", user);
        }
    }

    public Usuario FindUser(string userId)
    {
        if (users.ContainsKey(userId))
            return users[userId];
        Usuario u = null;// srrcDAO.FindProf(userId);

        //u = srrcDAO.FindProf("10"+userId.Substring(2,6)); 
        //if (u == null)
        u = srrcDAO.FindFunc(userId);
        if (u == null)
            u = srrcDAO.FindStudentUndergrad(userId);
        if (u == null)
            u = srrcDAO.FindStudentLato(userId);
        if (u == null)
            u = srrcDAO.FindStudentStricto(userId);
        //Usuario u = users.get(userId);
        if(u != null)
            users.Add(userId, u);
        return u;
    }
}
