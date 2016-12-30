using JarvisConsole.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JarvisAPI.Controllers
{
    public class NestController : ApiController
    {
        // GET: api/Nest
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Nest/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Nest
        public HttpResponseMessage Post(string value)
        {
            return Request.CreateResponse<bool>(System.Net.HttpStatusCode.Created, NestDataProvider.FinishAuthenticateNest(value));
        }

        // PUT: api/Nest/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Nest/5
        public void Delete(int id)
        {
        }
    }
}
