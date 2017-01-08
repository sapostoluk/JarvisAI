using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyHub;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using JarvisAPI;

namespace JarvisConsole.DataProviders
{
    public static class HarmonyDataProvider
    {
        #region Fields
        private static bool _isInitialized;
        private static bool _ready;
        private static Client _hub;
        private static string _ipAddress;
        private static Config _harmonyConfig;
        private static List<Activity> _activityList;
        private static Activity _powerOffActivity;
        private static List<Device> _deviceList;
        private static Activity _currentActivity;
        private static Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

        #endregion

        #region Properties
        public static bool IsInitialized
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_ipAddress) && _harmonyConfig != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public static bool IsReady
        {
            get
            {
                if (_hub.IsReady)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public static string IpAddress
        {
            get { return _ipAddress; }
        }

        public static List<Activity> ActivityList
        {
            get { return _activityList; }
        }

        public static List<Device> DeviceList
        {
            get { return _deviceList; }
        }

        public static Activity CurrentActivity
        {
            get { return _currentActivity; }

        }

        public static Activity PowerOffActivity
        {
            get { return _powerOffActivity; }
        }

        #endregion

        #region Public Methods 
        public static void SendCommand(string command, string deviceId)
        {
            while (!_hub.IsReady)
            {
                //wait
            }
            try
            {
                _hub.SendKeyPressAsync(deviceId, command);
            }
            catch(Exception e)
            {
                Logging.Log("general", string.Format("Harmony failed to send command {0}: " + e.Message, command));
            }
            
        }

        public static async Task StartActivity(string activityId)
        {
            if (_hub.IsReady)
            {
                try
                {
                    await _hub.StartActivityAsync(activityId);
                }
                catch(Exception e)
                {
                    Logging.Log("general", string.Format("Harmony failed to start activity '{0}': " + e.Message, activityId));
                }
            }

        }

        public static List<Activity> ActivityLookup(string name)
        {
            List<Activity> LookupList = new List<Activity>();
            foreach (Activity activity in _activityList)
            {
                if (activity.Label.Contains(name))
                {
                    LookupList.Add(activity);
                }
            }
            return LookupList;
        }

        public static async Task CloseConnection()
        {
            await _hub.CloseAsync();
        }

        #endregion

        #region Private Methods
        private static async Task HarmonyOpenAsync()
        {
            //First create our client and login
            if (File.Exists("SessionToken"))
            {
                var sessionToken = File.ReadAllText("SessionToken");
                Trace.WriteLine("Reusing token: {0}", sessionToken);
                await _hub.TryOpenAsync(sessionToken);
            }
            else
            {
                await _hub.TryOpenAsync();
                File.WriteAllText("SessionToken", _hub.Token);
            }
        }

        private static async Task HarmonyGetConfigAsync()
        {
            //Fetch our config
            var harmonyConfig = await _hub.GetConfigAsync();
            if (harmonyConfig == null)
            {
                return;
            }
            _harmonyConfig = harmonyConfig;
        }

        private static async Task HarmonyCloseAsync()
        {
            await _hub.CloseAsync();
        }

        private static void GetDevices()
        {
            if (_harmonyConfig != null)
            {
                _deviceList = _harmonyConfig.Devices.ToList();
            }
            else
            {
                throw new Exception("Harmony config is empty");
            }
        }

        private static void GetActivities()
        {
            if (_harmonyConfig != null)
            {
                _activityList = _harmonyConfig.Activities.ToList();
            }
            else
            {
                throw new Exception("Harmony config is empty");
            }

            _powerOffActivity = _activityList.Where(e => e.Label == "PowerOff").FirstOrDefault();
        }

        #endregion

        #region Initializer
        public async static void Initialize()
        {
            _ipAddress = configuration.AppSettings.Settings["harmony_ip"].Value;
            if (_hub == null || !_hub.Host.Equals(_ipAddress))
            {
                _hub = new Client(_ipAddress, true);
                _hub.OnTaskChanged += _hub_OnTaskChanged1;
                _hub.OnConnectionClosed += _hub_OnConnectionClosed1;
                _hub.OnActivityChanged += _hub_OnActivityChanged;
            }
            Console.WriteLine("Harmony Initializing");
            Logging.Log("general", "Harmony attempting to initialize"); 
            _activityList = new List<Activity>();
            _deviceList = new List<Device>();

            try
            {
                await HarmonyOpenAsync();


                while (!_hub.IsReady)
                {
                    //wait
                }
                await HarmonyGetConfigAsync();
            }
            catch(Exception e)
            {
                Logging.Log("general","Harmony failed to open connection: " +  e.Message);
            }
            

            //Get current activity
            GetActivities();
            GetDevices();
            string activityId = "";
            try
            {
                activityId = await _hub.GetCurrentActivityAsync();
            }
            catch(Exception e)
            {
                Logging.Log("general", "Harmony failedto get current activity: " + e.Message);
            }
            
            string activityName = _harmonyConfig.ActivityNameFromId(activityId);
            _currentActivity = ActivityLookup(activityName).FirstOrDefault();
           
            Console.WriteLine("Harmony finished initialization attempt");

        }


        #endregion

        #region Event
        private static void _hub_OnActivityChanged(object sender, string e)
        {
            _currentActivity = ActivityLookup(_harmonyConfig.ActivityNameFromId(e)).FirstOrDefault();
        }


        private async static void _hub_OnConnectionClosed1(object sender, bool e)
        {
            Debug.Assert(_hub.IsClosed);
            if(e)
            {
                Initialize();
            }
        }

        private static void _hub_OnTaskChanged1(object sender, bool e)
        {
            //throw new NotImplementedException();
        }

        #endregion

    }
}
