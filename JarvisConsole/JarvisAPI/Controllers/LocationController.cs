
using JarvisAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JarvisAPI.Controllers
{
    public class LocationController : ApiController
    {
        private string _locationLogPath = "location_controller";
        // GET: api/Location
        public IEnumerable<string> Get(string userId)
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Location/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Location
        public void Post([FromUri]string userId, string value)
        {
            Logging.Log(_locationLogPath, string.Format("Recieved user '{0}' location.", userId)); 
            Location location = new Location();
            try
            {
                JObject jObject = JObject.Parse(value);
                location.X = double.Parse(jObject["X"].ToString());
                location.Y = double.Parse(jObject["Y"].ToString());
                location.Z = double.Parse(jObject["Z"].ToString());
            }
            catch(Exception ex)
            {
                Logging.Log(_locationLogPath, "Error Parsing Location: " + ex.Message);
            }
           
        }

        // PUT: api/Location/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Location/5
        public void Delete(int id)
        {
        }
    }
}
