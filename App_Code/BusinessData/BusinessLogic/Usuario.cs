using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

namespace BusinessData.BusinessLogic
{
    class Usuario 
    {
        
        public Usuario()
        {
        }

        public bool IsAdmin()
        {
            
            return Roles.IsUserInRole("Admin");
        }
    }
}
