// $Id$
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using BusinessData.DataAccess;
using BusinessData.Entities;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class _Default : System.Web.UI.Page
{
    private SRRCDAO srrcDAO = new SRRCDAO();
    private SortedDictionary<string, RecursoSRRC> recs;
    private SortedDictionary<string, Usuario> users;

    private Usuario currentUser = null;

    private List<RecursoSRRC> recsLabs;
    private List<RecursoSRRC> recsKits;
    private List<RecursoSRRC> recsHDMI;
    private List<RecursoSRRC> recsAuds;
    private List<RecursoSRRC> recsNotes;
    private List<RecursoSRRC> recsSpeakers;

    Color[] pal6 = { Color.FromArgb(0x48,0x90,0x74),
                     Color.FromArgb(0x4a,0x6b,0x8a),
                     Color.FromArgb(0xd4,0xa8,0x6a),
                     Color.FromArgb(0xd4,0x8b,0x6a),
                     Color.FromArgb(0xaa,0x5c,0x39),
                     Color.FromArgb(0x8a,0x3c,0x19)
    };

    private GrupoRecursos grupoLabs;
    private GrupoRecursos grupoNotes;
    private GrupoRecursos grupoKits;
    private GrupoRecursos grupoHDMI;
    private GrupoRecursos grupoAuds;
    private GrupoRecursos grupoSom;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            recs = new SortedDictionary<string, RecursoSRRC>();
            CarregaUsuarios();
            CarregaRecursos();
            CarregaUltimaAtividade();

            Session["USERS"] = users;
            Session["RECS"] = recs;
        }
        else
        {
            users = (SortedDictionary<string, Usuario>)Session["USERS"];
            recs = (SortedDictionary<string, RecursoSRRC>)Session["RECS"];
            currentUser = (Usuario)Session["CURRENT"];
        }

        Redraw();

   }

    private void Redraw()
    {
        recsLabs = GetUltimasAtividades('L');
        recsKits = GetUltimasAtividades('K');
        recsHDMI = GetUltimasAtividades('H');
        recsAuds = GetUltimasAtividades('A');
        recsNotes = GetUltimasAtividades('N');
        recsSpeakers = GetUltimasAtividades('S');

        /*            foreach (RecursoSRRC rec in recs.Values)
                    {
                        Debug.WriteLine(rec.ToString());
                       // lblContent.Text += rec.Descricao + " - " + rec.LastUser + " - " + rec.LastTime + " - " + rec.Status + "<br>";
                    }*/

        grupoLabs = new GrupoRecursos(recsLabs, "Labs",
            pal6[0], "lab.png", 0, 0, 1020, 161, 157, 78, 140, 71);

        grupoNotes = new GrupoRecursos(recsNotes, "Notebooks",
                pal6[1], "notebook.png", 0, grupoLabs.getTop() + grupoLabs.getHeight(),
                1020, 161, 157, 78, 140, 71);

        grupoHDMI = new GrupoRecursos(recsHDMI, "Kits HDMI",
                pal6[2], "cabo-hdmi.png", 0, grupoNotes.getTop() + grupoNotes.getHeight(),
                1020, 84, 157, 80, 140, 71);

        grupoKits = new GrupoRecursos(recsKits, "Kits VGA",
                pal6[3], "cabo-vga.png", 0, grupoHDMI.getTop() + grupoHDMI.getHeight(),
                1020, 161, 157, 80, 140, 71);

        grupoAuds = new GrupoRecursos(recsAuds, "Auditórios",
                pal6[4], "auditorio.png", 0, grupoKits.getTop() + grupoKits.getHeight(),
                380, 84, 150, 80, 140, 71);

        grupoSom = new GrupoRecursos(recsSpeakers, "Caixas som",
                pal6[5], "speaker.png", grupoAuds.getLeft() + grupoAuds.getWidth(),
                grupoAuds.getTop(), 1020 - 380, 84, 140, 80, 135, 71);

        string aux = grupoLabs.DesenhaGrupo();
        aux += grupoNotes.DesenhaGrupo();
        aux += grupoHDMI.DesenhaGrupo();
        aux += grupoKits.DesenhaGrupo();
        aux += grupoAuds.DesenhaGrupo();
        aux += grupoSom.DesenhaGrupo();
        canvas.InnerHtml = aux;
    }

    private void CarregaRecursos()
    {
        List<RecursoSRRC> lista = srrcDAO.LoadResources();
        foreach (RecursoSRRC rec in lista)
        {
            recs[rec.Abrev] = rec;//.Add(rec.Id, rec);
        }
        CarregaUltimaAtividade();
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
    private void CarregaUltimaAtividade()
    {
        foreach (string r in recs.Keys)
        {
            RecursoSRRC rec = recs[r];
            string recName = rec.Descricao;
            LogData ld = srrcDAO.FindLatestActivity(recName);
            if (ld != null)
            {
                rec.LastTime = ld.Horario;
                rec.LastUser = ld.Usuario;
                Usuario.TipoUsuario tipo = Usuario.TipoUsuario.PROFESSOR;
                string tipostr = ld.TipoUsuario;
                if (tipostr != null)
                {
                    switch (tipostr)
                    {
                        case "G":
                            tipo = Usuario.TipoUsuario.ALUNO_GRAD;
                            break;
                        case "L":
                            tipo = Usuario.TipoUsuario.ALUNO_LATO;
                            break;
                        case "S":
                            tipo = Usuario.TipoUsuario.ALUNO_STRICTO;
                            break;
                        case "F":
                            tipo = Usuario.TipoUsuario.FUNCIONARIO;
                            break;
                        case "P":
                            tipo = Usuario.TipoUsuario.PROFESSOR;
                            break;
                    }
                }
                rec.TipoUser = tipo;
                if (ld.Acao == "RETIRADA")
                {
                    rec.Status = RecursoSRRC.StatusRecurso.RETIRADO;
                }
                else
                    rec.Status = RecursoSRRC.StatusRecurso.DISPONIVEL;
            }
        }
    }

    public List<RecursoSRRC> GetUltimasAtividades()
    {
        return recs.Values.ToList<RecursoSRRC>();
    }

    public List<RecursoSRRC> GetUltimasAtividades(char filt)
    {
        return recs.Values.ToList<RecursoSRRC>().Where(r =>  r.Tipo == filt).ToList();
    }

    protected bool ValidateUser(String id)
    {
        if (id.Length < 7) {
            lblStatus.Text = "IDENTIFICAÇÃO INVÁLIDA";
            return false;
        }
        Debug.WriteLine("FIND: [" + id + "]");
        Usuario u = FindUser(id);
        bool ok = false;
        if (u != null)
        {
            lblStatus.Text = u.Nome + " - " + Usuario.TipoUsuarioFull(u.Tipo) + " (" + u.Unidade + " )";
            lblStatus.ForeColor = Color.Black;
            ok = true;
            currentUser = u;
        }
        else
        {
            lblStatus.Text = "IDENTIFICAÇÃO INVÁLIDA";
            currentUser = null;
        }
        Session["CURRENT"] = currentUser;
        return ok;
        //if (u != null)
        //    tf2.requestFocus();
    }

    protected RecursoSRRC ValidateResource(string id)
    {
        id = id.ToUpper();
        if (id.Length < 4) return null;
        foreach(RecursoSRRC r in recs.Values)
        {
            if(r.Id == id)
            {
                lblStatus.Text = r.Descricao;
                return r;
            }
        }
        lblStatus.Text = "CODIGO DE RECURSO INVALIDO";
        return null;
    }
    protected void txtMatricula_TextChanged(object sender, EventArgs e)
    {
        ValidateUser(txtMatricula.Text);
    }

    protected void txtRecurso_TextChanged(object sender, EventArgs e)
    {
        RecursoSRRC r = ValidateResource(txtRecurso.Text);
        if (r != null && currentUser != null)
        {
            Confirm(r);
        }
        txtMatricula.Text = "";
        txtRecurso.Text = "";
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
            lblStatus.Text = user + " entregou " + resource + " em " + now.ToString();
            lblStatus.ForeColor = Color.ForestGreen;
        }
        else
        {
            ld = new LogData(now, "RETIRADA", user, resource, unitCourse, currentUser.TipoUsuarioChar);
            r.Status = RecursoSRRC.StatusRecurso.RETIRADO;
            lblStatus.Text = user + " retirou " + resource + " em " + now.ToString();
            lblStatus.ForeColor = Color.Firebrick;
        }
        txtMatricula.Text = "";
        txtRecurso.Text = "";
        r.LastUser = user;
        r.LastTime = now;
        r.TipoUser = currentUser.Tipo;
        srrcDAO.AddToLog(ld);
        Redraw();
    }

    private class GrupoRecursos
    {
        private List<RecursoSRRC> lista;
        private string description;
        private string icon;
        private double left, top;
        private double width, height;
        private double offsetX, offsetY;
        private double boxWidth, boxHeight;
        private Color background;
        private Color backgroundBox;

        static string fontname = "Sans";
        //static Font largeFont = Font.font(fontname, 23);
        //static Font smallFont = Font.font(fontname, 13);

        public GrupoRecursos(List<RecursoSRRC> recs, string descr, Color back, string icon,
               double left, double top, double width, double height, double ofx, double ofy,
               double boxWidth, double boxHeight)
        {
            //        System.out.println(largeFont.getName());
            this.lista = recs;
            this.description = descr;
            this.background = back;
            this.backgroundBox = ControlPaint.Light(back, 0.8f);
            this.icon = icon;
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.offsetX = ofx;
            this.offsetY = ofy;
            this.boxWidth = boxWidth;
            this.boxHeight = boxHeight;
            calculaPosicoes();
        }

        public double getLeft() { return left; }
        public double getTop() { return top; }
        public double getHeight() { return height; }
        public double getWidth() { return width; }
        public double getOffsetX() { return offsetX; }
        public double getOffsetY() { return offsetY; }
        public Color getBackgroundBox() { return backgroundBox; }

        private void calculaPosicoes()
        {
            double xini = 80; // left + 85;
            double yini = 5; // top + 5;
            double xmax = left + width - 120;
            double x = xini;
            double y = yini;
            // Compute positions for each resource (labs)
            foreach (RecursoSRRC r in lista)
            {
                r.PosX = x;
                r.PosY = y;
                //r.setGrupo(this);
                x += offsetX; //180;
                if (x > xmax)
                {
                    x = xini;
                    y += offsetY; //120;
                }
            }
        }
        public string DesenhaGrupo()
        {
            string aux = "";

            string bgcolor = ColorTranslator.ToHtml(Color.FromArgb(background.ToArgb()));

            aux = "<div style=\"background-color: " + bgcolor + "; position: absolute; top: " + top
                + "px; left: " + left + "px; width: " + width + "px; height: " + height + "px;\">";
            /*
            gc.setFill(background);
            gc.fillRect(left, top, width, height);
            gc.drawImage(icon, left + 10, top + 10, 40, 40);
            gc.setFont(smallFont);
            gc.setFill(Color.BLACK);
            gc.fillText(description, left + 10, top + 70);
            */
            aux += "<div class=\"smallfont\" style=\"font-color: black; position: absolute; top: 50px; left: 10px; width: 30px; height: 32px; font-size: 13px; font-family: arial\">";
            aux += description + "</div>";
            aux += "<img src=\"img/" + icon + "\" style=\"position: absolute; top: 10px; left: 10px; width: 40px; height: 40px\"/>";

            foreach (RecursoSRRC r in lista)
            {
                aux += Desenha(r, this);
            }
            aux += "</div>";
            return aux;
        }

        // Desenha a "caixa" do recurso na tela, com os seus dados
        public string Desenha(RecursoSRRC r, GrupoRecursos grupo)
        {
            string aux = "";
            double yTextOffset = 37;
            double x = r.PosX;
            double y = r.PosY;

            Color back = grupo.backgroundBox;
            Color front = Color.FromArgb(100, 100, 100);

            if (r.Status == RecursoSRRC.StatusRecurso.RETIRADO)
            {
                back = Color.FromArgb(120, 120, 120); //gc.setFill(Color.DARKGRAY.darker());
                front = Color.Black;
            }

            string bgcolor = ColorTranslator.ToHtml(Color.FromArgb(back.ToArgb()));
            string fgcolor = ColorTranslator.ToHtml(Color.FromArgb(front.ToArgb()));

            aux = "<div style=\"background-color: " + bgcolor + "; position: absolute; top: " + y
                + "px; left: " + x + "px; width: " + grupo.boxWidth + "px; height: " + grupo.boxHeight + "px;\">";

            /*
            gc.fillRect(x, y, grupo.boxWidth, grupo.boxHeight);
            gc.setFont(largeFont);
            gc.setFill(Color.BLACK);
            */
            string abrev = r.Abrev.Trim();
            aux += "<div class=\"largefont\" style=\"color: black; position: absolute; left: "
                + (grupo.boxWidth - 15 * abrev.Length) + "px; top: 0px; height: 64px; font-size: 24px; font-family: arial; font-weight: bold\">";
            aux += abrev + "</div>";

            aux += "<div class=\"smallfont\" style=\"color: " + fgcolor + "; position: absolute; left: 5px;"
                + " top: 40px; height: 32px; font-size: 13px; font-family: arial;\">";
            aux += r.AbrevLastUser + "</div>";

            string lastTime = null;
            if (r.LastTime != null)
            {
                lastTime = r.LastTime.ToString(); // LastTime.ToLongTimeString();
                aux += "<div class=\"smallfont\" style=\"color: " + fgcolor + "; position: absolute; left: 5px;"
                + " top: 25px; height: 32px; font-size: 13px; font-family: arial;\">";
                aux += lastTime + "</div>";
            }

            string tipo = Usuario.TipoUsuarioFull(r.TipoUser);
            aux += "<div class=\"smallfont\" style=\"color: " + fgcolor + "; position: absolute; left: 5px;"
                + " top: 55px; height: 32px; font-size: 13px; font-family: arial;\">";
            aux += tipo + "</div>";
            aux += "</div>";
            return aux;
        }
    }
}