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
            CarregaUsuariosRecursos();
            //CarregaUltimaAtividade();
        }
        else
        {
            users = (SortedDictionary<string, Usuario>)Session["USERS"];
            recs = (SortedDictionary<string, RecursoSRRC>)Session["RECS"];
            currentUser = (Usuario)Session["CURRENT"];
        }
    }

    private void CarregaUsuariosRecursos()
    {
        CarregaRecursos();
        Session["USERS"] = users;
        Session["RECS"] = recs;
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
            ld = new LogData(now, "ENTREGA", user, resource, unitCourse, currentUser.TipoUsuarioChar, currentUser.Matricula);
            r.Status = RecursoSRRC.StatusRecurso.DISPONIVEL;

            string u = Dashboard1.getNomeSobrenomeProfessor(user);
        }
        else
        {
            ld = new LogData(now, "RETIRADA", user, resource, unitCourse, currentUser.TipoUsuarioChar, currentUser.Matricula);
            r.Status = RecursoSRRC.StatusRecurso.RETIRADO;

            string u = Dashboard1.getNomeSobrenomeProfessor(user);
        }
        r.LastUser = user;
        r.LastTime = now;
        r.TipoUser = currentUser.Tipo;
        srrcDAO.AddToLog(ld);
        Dashboard1.Refresh();
    }

    private void CarregaRecursos()
    {
        recs = new SortedDictionary<string, RecursoSRRC>();
        List<RecursoSRRC> lista = srrcDAO.LoadResources();
        foreach (RecursoSRRC rec in lista)
        {
            recs[rec.Abrev] = rec;//.Add(rec.Id, rec);
        }
        //CarregaUltimaAtividade();
    }
}
