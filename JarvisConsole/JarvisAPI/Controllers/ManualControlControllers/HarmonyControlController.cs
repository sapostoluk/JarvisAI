using JarvisAPI.DataProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JarvisAPI.Controllers.ManualControlControllers
{
    public class HarmonyControlController : ApiController
    {
        private string _harmonyLogPath = "harmony_controller";
        // GET: api/HarmonyControl
        public ObservableCollection<string> Get(string userId)
        {
            Logging.Log(_harmonyLogPath, string.Format("Attempting to send activity list to user '{0}'.", userId));
            ObservableCollection<string> activities = new ObservableCollection<string>();
            try
            {
                if (!HarmonyDataProvider.IsInitialized)
                {
                    HarmonyDataProvider.Initialize();
                }
                foreach (HarmonyHub.Activity activity in HarmonyDataProvider.ActivityList)
                {
                    activities.Add(activity.Label);                    
                }
            }
            catch(Exception ex)
            {
                Logging.Log(_harmonyLogPath, "Failed to send activities list: " + ex.Message);
            }
            
            return activities;
        }

        public string Get(string userId, string item)
        {
            Logging.Log(_harmonyLogPath, string.Format("Attempting to send current activity to user '{0}'.", userId));
            string serialized = "";
            string currentActivity = "";
            if (!HarmonyDataProvider.IsInitialized)
            {
                HarmonyDataProvider.Initialize();
            }
            try
            {
                switch (item)
                {
                    case "CurrentActivity":
                        {
                            currentActivity = HarmonyDataProvider.CurrentActivity.Label;
                            serialized = JsonConvert.SerializeObject(currentActivity);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(_harmonyLogPath, "Error getting current activity: " + ex.Message);
            }
            return currentActivity;
        }

        // GET: api/HarmonyControl/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/HarmonyControl
        public bool Post([FromUri]string userId, string activity)
        {
            Logging.Log(_harmonyLogPath, string.Format("Recieved activity to actuate from user '{0}'.", userId));
            bool success = false;
            try
            {
                if (!HarmonyDataProvider.IsInitialized)
                {
                    HarmonyDataProvider.Initialize();
                }
                HarmonyHub.Activity hActivity = HarmonyDataProvider.ActivityLookup(activity).FirstOrDefault();
                Task<bool> t = HarmonyDataProvider.StartActivity(hActivity.Id);
                t.Wait();
                success = t.Result;

            }
            catch (Exception ex)
            {
                Logging.Log(_harmonyLogPath, "Error starting activity " + activity + ": " + ex.Message);
            }
            return success;
        }

        // PUT: api/HarmonyControl/5
        public void Put([FromUri]string activity)
        {
        }

        // DELETE: api/HarmonyControl/5
        public void Delete(int id)
        {
        }
    }
}
