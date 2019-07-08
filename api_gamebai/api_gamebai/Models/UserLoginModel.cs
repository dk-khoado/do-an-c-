using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_gamebai.Models
{
    public class UserLoginModel
    {
        public string username;
        public string password;
        public bool isNull()
        {
            if (username == null || password == null)
            {
                return true;
            }
            return false;
        }
    }
}