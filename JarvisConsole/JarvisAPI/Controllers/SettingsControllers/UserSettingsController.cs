using JarvisAPI.Models;
using JarvisAPI.Models.Providers;
using JarvisAPI.Models.Settings;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JarvisAPI.Controllers
{
    public class UserSettingsController : ApiController
    {
        private string _settingsLogPath = "settings_controller";
        SettingsDBProvider settingsDb = new SettingsDBProvider();
        // GET: api/Settings
        public string Get(string userId)
        {
            Logging.Log(_settingsLogPath, string.Format("User '{0}' is attempting to get User Settings.", userId));
            string serialized = "";
            try
            {
                serialized = settingsDb.ReadUserSettings(userId);
            }
            catch(Exception ex)
            {
                Logging.Log(_settingsLogPath, "Error getting settings for user '" + userId + "': " + ex.Message);
            }
            return serialized;
        }

        // GET: api/Settings/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Settings
        public void Post([FromUri]string userId, string value)
        {
            Logging.Log(_settingsLogPath, string.Format("Recieved user '{0}' User Settings.", userId));
            UserSettings settings = new UserSettings();
            try
            {
                //general
                JObject jObject = JObject.Parse(value);
                settings.UserId = jObject["UserId"].ToString();
                settings.Priority = Int32.Parse(jObject["Priority"].ToString());
                settings.Timeout = Int32.Parse(jObject["Timeout"].ToString());
        
                //demo properties
                settings.OutletOne = bool.Parse(jObject["OutletOne"].ToString());
                settings.OutletTwo = bool.Parse(jObject["OutletTwo"].ToString());
                settings.DefaultActivity = jObject["DefaultActivity"].ToString();
                settings.DefaultVolume = Int32.Parse(jObject["DefaultVolume"].ToString());
                settings.DefaultTemperature = Int32.Parse(jObject["DefaultTemperature"].ToString());

                string serialized = value;
                settingsDb.WriteUserSettings(serialized, jObject["UserId"].ToString());
            }
            catch (Exception ex)
            {
                Logging.Log(_settingsLogPath, "Error Parsing Settings: " + ex.Message);
            }
        }

        // PUT: api/Settings/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Settings/5
        public void Delete(int id)
        {
        }
    }
}
