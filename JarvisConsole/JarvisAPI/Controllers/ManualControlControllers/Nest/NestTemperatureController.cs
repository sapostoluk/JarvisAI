using JarvisAPI.DataProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JarvisAPI.Controllers.ManualControlControllers.Nest
{
    public class NestTemperatureController : ApiController
    {
        private string _nestLogPath = "nest_controller";
        // GET: api/NestControl
        public string Get(string userId, string type)
        {
            Logging.Log(_nestLogPath, string.Format("Attempting to send temperature to user '{0}'.", userId));
            string temperature = "";
            try
            {
                if (!NestDataProvider.IsInitialized)
                {
                    NestDataProvider.Initialize();
                }
                if (type == "Ambient")
                {
                    temperature = NestDataProvider.GetNestItem(NestDataProvider.NestItem.AmbientTemperature);
                    //t.Wait();
                    //temperature = t.Result;
                }
                else if(type == "Target")
                {
                    temperature = NestDataProvider.GetNestItem(NestDataProvider.NestItem.TargetTemperature);
                    //t.Wait();
                    //temperature = t.Result;
                }
            }
            catch(Exception ex)
            {
                Logging.Log(_nestLogPath, "Error getting nest item: " + ex.Message);
            }

            string serialized = "";
            serialized = JsonConvert.SerializeObject(temperature);
            return serialized;
        }

        // GET: api/NestControl/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/NestControl
        public void Post([FromUri]string userId, string value)
        {
            Logging.Log(_nestLogPath, string.Format("Recieved temperature to set from user '{0}'.", userId));
            try
            {
                if (!NestDataProvider.IsInitialized)
                {
                    NestDataProvider.Initialize();
                }
                Task t = NestDataProvider.SetNestItemAsync(NestDataProvider.NestItem.TargetTemperature, value);
                t.Wait();

                Task<string> x = NestDataProvider.GetNestItemAsync(NestDataProvider.NestItem.TargetTemperature);
                x.Wait();
                string temperature = x.Result;
                if(temperature != value)
                {
                    Logging.Log(_nestLogPath, "Failed to set nest item: temperature");
                }
            }
            catch(Exception ex)
            {
                Logging.Log(_nestLogPath, "Error setting nest item: " + ex.Message);
            }
        }

        // PUT: api/NestControl/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/NestControl/5
        public void Delete(int id)
        {
        }
    }
}
