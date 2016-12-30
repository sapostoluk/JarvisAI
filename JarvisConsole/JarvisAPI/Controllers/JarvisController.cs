using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JarvisConsole.DataProviders.Wit;
using Newtonsoft.Json;
using JarvisConsole.DataProviders;

namespace JarvisAPI.Controllers
{
    public class JarvisController : ApiController
    {
        // GET: api/Jarvis
        public string Get(string conversationId, string message)
        {
            ThreadContent thread = WitDataProvider.SendMessage(conversationId, message); 
            return JsonConvert.SerializeObject(thread); ;
        }

        // GET: api/Jarvis/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Jarvis
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Jarvis/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Jarvis/5
        public void Delete(int id)
        {
        }
    }
}
