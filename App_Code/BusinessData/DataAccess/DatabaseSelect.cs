using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DatabaseSelect
/// </summary>
public sealed class DatabaseSelect
{
    private String _db = null;

    public String DB { get { return getDB(); } set { _db = value; } }
    
    private static readonly Lazy<DatabaseSelect> lazy = new Lazy<DatabaseSelect>(() => new DatabaseSelect());

    public static DatabaseSelect Instance { get { return lazy.Value; } }

	private DatabaseSelect()
	{       
	}

    private string getDB()
    {
        if (_db == null)
        {
            string host = HttpContext.Current.Request.Url.Host;
            if (host == "ecplan.pucrs.br")
                _db = "SARCFACINcs";
            else if (host == "gsplan.pucrs.br")
                _db = "SARCFACINcs";
            else
                _db = "SARCFACINcs";
            System.Diagnostics.Debug.WriteLine("\n\nDB: " + _db + "\n\n");
        }
        return _db;
    }

}