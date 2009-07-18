using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using BusinessData.Util;
using BusinessData.Entities;
using BusinessData.DataAccess;

namespace BusinessData.BusinessLogic
{
    public class HorariosEventoBO
    {
        private DataAccess.HorariosEventoDAO dao;
        private Usuario usr;

        public HorariosEventoBO()
        {

                dao = new BusinessData.DataAccess.HorariosEventoDAO();

            usr = new Usuario();
        }

        public void DeletaHorariosEvento(Guid id)
        {

                dao.RemoveHorariosEvento(id);

        }

        public void InsereHorariosEvento(Entities.HorariosEvento horariosEvento)
        {

                dao.InsereHorariosEvento(horariosEvento);

        }

        public Entities.HorariosEvento GetHorariosEventoById(Guid id)
        {

                return dao.GetHorariosEvento(id);


        }

        public List<HorariosEvento> GetHorariosEventos()
        {
            return dao.GetHorariosEventos();
        }

        public List<HorariosEvento> GetHorariosEventosById(Guid id)
        {
            return dao.GetHorariosEventosById(id);
        }

        public List<HorariosEvento> GetHorariosEventosByIdDetalhados(Guid eventoId)
        {

                return dao.GetHorariosEventosByIdDetalhados(eventoId);

        }
    }
}
