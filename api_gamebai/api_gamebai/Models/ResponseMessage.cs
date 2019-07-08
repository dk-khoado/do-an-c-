using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_gamebai.Models
{
    public class ResponseMessage
    {
        public string response;
        public string message;
        public object data;
        public int result;
        public ResponseMessage()
        {

        }
        public ResponseMessage(string message)
        {
            this.message = message;
        }
        public ResponseMessage(string message, object data, int result)
        {
            this.message = message;
            this.data = data;
            this.result = result;
        }
        public ResponseMessage(string response,string message, object data, int result)
        {
            this.response = response;
            this.message = message;
            this.data = data;
            this.result = result;
        }
        public ResponseMessage(string message, object data)
        {
            this.message = message;
            this.data = data;            
        }
        public ResponseMessage(string response,string message)
        {
            this.response = response;
            this.message = message;           
        }
        public ResponseMessage(string response, string message, object data)
        {
            this.response = response;
            this.message = message;
            this.data = data;
        }
        public ResponseMessage(string response, string message,int result)
        {
            this.response = response;
            this.message = message;
            this.result = result;
        }
    }
}