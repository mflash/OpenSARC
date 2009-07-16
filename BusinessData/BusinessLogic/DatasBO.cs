using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

namespace BusinessData.BusinessLogic
{
    public class DatasBO
    {
        private DataAccess.DatasDAO dao;
        private Usuario usr;

        public DatasBO()
        {
            try
            {
                dao = new BusinessData.DataAccess.DatasDAO();
            }
            catch (DataAccess.DataAccessException ex)
            {
                throw;
            }
            usr = new Usuario();
        }

        public void DeletaData(Guid id, Entities.Data data)
        {
            if (usr.IsAdmin())
                {
                    try
                    {
                        dao.DeletaData(id, data);
                    }
                    catch (DataAccess.DataAccessException ex)
                    {
                        throw;
                    }
                }
                else
                {
                    throw new SecurityException("Acesso Negado.");
                }
        }
        
        public void InsereData(Entities.Data data, Entities.Calendario cal)
        {
            
            if (usr.IsAdmin())
            {
                try
                {
                    dao.InsereData(data, cal.Id);
                }
                catch (DataAccess.DataAccessException ex)
                {
                    throw;
                }
            }
            else
            {
                throw new SecurityException("Acesso Negado.");
            }
        }
        
             
        public List<Entities.Data> GetDatasByCalendario(Guid id)
        {
            try
            {
                return dao.GetDatasByCalendario(id);
            }
            catch (DataAccess.DataAccessException ex)
            {
                throw;
            }
        }
    }
}
