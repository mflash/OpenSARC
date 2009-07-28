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
            try
            {
                dao = new BusinessData.DataAccess.HorariosEventoDAO();
            }
            catch (DataAccess.DataAccessException ex)
            {
                throw;
            }
            usr = new Usuario();
        }

        public void DeletaHorariosEvento(Guid id)
        {
            try
            {
                dao.RemoveHorariosEvento(id);
            }
            catch (DataAccess.DataAccessException ex)
            {
                throw;
            }
        }

        public void InsereHorariosEvento(Entities.HorariosEvento horariosEvento)
        {
            try
            {
                dao.InsereHorariosEvento(horariosEvento);
            }
            catch (DataAccess.DataAccessException ex)
            {
                throw;
            }
        }

        public Entities.HorariosEvento GetHorariosEventoById(Guid id)
        {
            try
            {
                return dao.GetHorariosEvento(id);
            }
            catch (DataAccessException ex)
            {
                throw;
            }

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
            try
            {
                return dao.GetHorariosEventosByIdDetalhados(eventoId);
            }
            catch (DataAccessException ex)
            {
                throw;
            }
        }
    }
}
