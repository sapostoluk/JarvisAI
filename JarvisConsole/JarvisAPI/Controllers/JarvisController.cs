using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using JarvisConsole.DataProviders;
using System.Configuration;
using JarvisAPI.DataProviders.Orvibo;
using JarvisAPI.DataProviders.APIAI;
using JarvisConsole.DataProviders.APIAI;

namespace JarvisAPI.Controllers
{
    public class JarvisController : ApiController
    {                      
        // GET: api/Jarvis
        public string Get(string conversationId, string message)
        {            
             ThreadContent thread = new ThreadContent();
             APIAIDataProvider.Initialize();
             thread = APIAIDataProvider.SendMessage(conversationId, message);


            return JsonConvert.SerializeObject(thread);

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
