using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using api_gamebai.Models;

namespace api_gamebai.ApikeyManager
{
    public class ApikeyManagerHandle : DelegatingHandler
    {
        private const string apiKey = "123456789";
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validkey = false;
            IEnumerable<string> resquestHeader;
            var checkAPIkey = httpRequestMessage.Headers.TryGetValues("apiKey", out resquestHeader);
            if (checkAPIkey)
            {
                if (resquestHeader.FirstOrDefault().Equals(apiKey))
                {
                    validkey = true;
                }
            }
            if (!validkey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden,"Hello ít my :))");
            }
            var response = await base.SendAsync(httpRequestMessage,cancellationToken);
            return response;
        }
    }
}