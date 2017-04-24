using JarvisAPI.DataProviders.Orvibo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JarvisAPI.Controllers
{
    public class OrviboController : ApiController
    {
        private string _orivoboLogPath = "orvibo_controller";
        // GET: api/Orvibo
        public ObservableCollection<string> Get(string userId)
        {
            ObservableCollection<string> devices = new ObservableCollection<string>();
            Logging.Log(_orivoboLogPath, string.Format("User {0} attempted to get the list of orvibo devices.", userId));
            try
            {
                if (!OrviboDataProvider.isInitialized)
                {
                    OrviboDataProvider.Initialize();
                }

                devices = OrviboDataProvider.GetDevices();
                
               
            }
            catch(Exception ex)
            {
                Logging.Log("orvibo", "Error getting devices: " + ex.Message);
            }

            string serialized = "";
            serialized = JsonConvert.SerializeObject(devices);
            Logging.Log(_orivoboLogPath,"DEVICES: " + serialized);
            

            return devices;
        }

        // GET: api/Orvibo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Orvibo
        public bool Post([FromUri]string userId, string name, string control)
        {
            Logging.Log(_orivoboLogPath, string.Format("User '{0}' attempted to turn orvibo device '{1}' {2}." , userId, name, control));
            bool success = false;
            try
            {
                if (!OrviboDataProvider.isInitialized)
                {
                    OrviboDataProvider.Initialize();
                }

                if (control == "on" || control == "On")
                {
                    success = OrviboDataProvider.OnCommand(name);
                }
                else if (control == "Off" || control == "off")
                {
                    success = OrviboDataProvider.OffCommand(name);
                }
            }
            catch(Exception ex)
            {
                Logging.Log(_orivoboLogPath, "Error controling orvibo device: " + ex.Message);
            }
            return success;
        }

        // PUT: api/Orvibo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Orvibo/5
        public void Delete(int id)
        {
        }
    }
}
