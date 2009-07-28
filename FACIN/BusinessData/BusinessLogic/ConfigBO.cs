using System;
using System.Collections.Generic;
using System.Text;
using BusinessData.Entities;
using BusinessData.DataAccess;

namespace BusinessData.BusinessLogic
{
    public class ConfigBO
    {
        private ConfigDAO acessoDados;

        public ConfigBO()
        {
            acessoDados = new ConfigDAO();
        }

        public AppState GetAppState(Calendario calId)
        {
            return acessoDados.GetAppState(calId);
        }

        public void SetAppState(AppState novoEstado, Calendario calId)
        {
            acessoDados.SetAppState(novoEstado, calId);
        }

        public bool IsAulasDistribuidas(Calendario calId)
        {
            return acessoDados.IsAulasDistribuidas(calId);
        }

        public bool IsRecursosDistribuidos(Calendario calId)
        {
            return acessoDados.IsRecursosDistribuidos(calId);
        }

        public void setAulasDistribuidas(Calendario CalId, bool dist)
        {
            acessoDados.setAulasDistribuidas(CalId, dist);
        }

        public void setRecursosDistribuidos(Calendario CalId, bool dist)
        {
            acessoDados.setRecursosDistribuidos(CalId, dist);
        }

        public void createConfig(Calendario calId)
        {
            acessoDados.createConfig(calId);
        }
    }
}
