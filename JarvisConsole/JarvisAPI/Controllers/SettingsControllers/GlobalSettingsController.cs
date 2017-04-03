using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using JarvisAPI.Models;
using JarvisAPI.Models.Settings;
using JarvisAPI.Models.Providers;

namespace JarvisAPI.Controllers.SettingsControllers
{
    public class GlobalSettingsController : ApiController
    {
        private string _settingsLogPath = "settings_controller";
        SettingsDBProvider settingsDb = new SettingsDBProvider();
        // GET: api/GlobalSettings
        public string Get(string userId)
        {
            Logging.Log(_settingsLogPath, string.Format("Attempting to send Global Settings to user '{0}'.", userId));
            string serialized = "";
            try
            {
                serialized = settingsDb.ReadGlobalSettings();
            }
            catch(Exception ex)
            {
                Logging.Log(_settingsLogPath, "Error retrieving global settings: " + ex.Message);
            }

            return serialized;
        }

        // GET: api/GlobalSettings/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/GlobalSettings
        public void Post([FromUri]string userId, string value)
        {
            Logging.Log(_settingsLogPath, string.Format("Recieved Global Settings from user '{0}'.", userId));
            GlobalSettings settings = new GlobalSettings();
            try
            {
                JObject jobject = JObject.Parse(value);
                settings.BeaconAlphaLocation = jobject["BeaconAlphaLocation"].ToObject<Location>();
                settings.BeaconBetaLocation = jobject["BeaconBetaLocation"].ToObject<Location>();
                settings.BeaconSigmaLocation = jobject["BeaconSigmaLocation"].ToObject<Location>();

                string serialized = value;
                settingsDb.WriteGlobalSettings(serialized);
            }
            catch(Exception ex)
            {
                Logging.Log(_settingsLogPath, "Error parsing global settings: " + ex.Message);
            }
        }

        // PUT: api/GlobalSettings/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GlobalSettings/5
        public void Delete(int id)
        {
        }
    }
}
