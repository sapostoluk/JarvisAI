using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JarvisConsole.DataProviders.Wit;


namespace WebApplication1.Controllers
{
    public class CommunicationController : ApiController
    {        

        public string Get(string message)
        {
            string conversationId = "test";
            //WitDataProvider wit = new WitDataProvider(conversationId);
            //var response = Request.CreateResponse<string>(System.Net.HttpStatusCode.Created, "hello");

            return "hello";
        }
    }
}
