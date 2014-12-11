<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        BusinessData.BusinessLogic.RecursosBO recursosBO = new BusinessData.BusinessLogic.RecursosBO();
        // Monta dicionário com bloqueio de recursos devido a uso de outros
        Dictionary<Guid, BusinessData.Entities.Recurso> todos = new Dictionary<Guid, BusinessData.Entities.Recurso>();
        Dictionary<Guid, Tuple<Guid, Guid>> blocks = new Dictionary<Guid, Tuple<Guid, Guid>>();
        List<BusinessData.Entities.Recurso> listRec = recursosBO.GetRecursos();
        foreach (BusinessData.Entities.Recurso r in listRec)
            todos.Add(r.Id, r);
        foreach (BusinessData.Entities.Recurso r in listRec)
        {
            if (r.Bloqueia1 != Guid.Empty || r.Bloqueia2 != Guid.Empty)
            {
                //System.Diagnostics.Debug.WriteLine("block: " + r.Id + " -> " + r.Bloqueia1 + ", " + r.Bloqueia2);
                blocks.Add(r.Id, new Tuple<Guid, Guid>(r.Bloqueia1, r.Bloqueia2));
            }
        }
        Application["blocks"] = blocks;
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        //Ultima excessão ocorrida no servidor
        Exception excessao = Server.GetLastError();
        Exception exInner = excessao.InnerException;
        
        //TODO LOG EXCESSAO
        //Response.Redirect("~/Default/Erro.aspx");
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
