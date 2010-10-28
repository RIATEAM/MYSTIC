using System;
using System.Collections.Generic;
using System.Web;

namespace SSO
{
    public class UserDTO
    {

        public string Role;
                
        public bool isAdmin()
        {
            return Role == "admin" ? true : false;
        }

    }
}
