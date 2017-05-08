using System;
using System.Web.Http;
using Newtonsoft.Json;
using JarvisAPI.DataProviders.APIAI;

namespace JarvisAPI.Controllers
{
    public class JarvisController : ApiController
    {                      
        // GET: api/Jarvis
        public string Get(string conversationId, string requestLocation, string userId, string message)
        {
            Logging.Log("JarvisAPIController", string.Format("Recieved a Jarvis request from user: '{0}': REQUEST: '{1}'", userId, message));
            ThreadContent thread = new ThreadContent();
            string serialized = "";
            try
            {
                APIAIDataProvider.Initialize();
                thread = APIAIDataProvider.SendMessage(conversationId, userId, requestLocation, message);
                serialized = JsonConvert.SerializeObject(thread);
            }
            catch(Exception e)
            {
                Logging.Log("JarvisAPIController", "Error with apiai: " + e.Message);
            }

            return serialized;

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
