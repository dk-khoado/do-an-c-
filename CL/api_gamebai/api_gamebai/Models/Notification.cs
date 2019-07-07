using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_gamebai.Models
{
    public class Notification
    {
        public string response;
        public string message;

        public Notification()
        {
        }

        public Notification(string res,string mess)
        {
            this.response = res;
            this.message = mess;
        }
    }
}