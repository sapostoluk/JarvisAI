using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyHub;
using System.IO;
using System.Diagnostics;
using System.Configuration;

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
                if (_hub.IsReady && !string.IsNullOrWhiteSpace(_ipAddress) && _harmonyConfig!= null)
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
            //while (!_hub.IsReady)
            //{
            //    //wait
            //}
            _hub.SendKeyPressAsync(deviceId, command);                    
        }
           
        public static async Task StartActivity(string activityId)
        {
            if(_hub.IsReady)
            {
                await _hub.StartActivityAsync(activityId);
            }
            else
            {
                await _hub.StartActivityAsync(activityId);
            }         
        }

        public static List<Activity> ActivityLookup(string name)
        {
            List<Activity> LookupList = new List<Activity>();
            foreach(Activity activity in _activityList)
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
            if (_hub == null || !_hub.Host.Equals(_ipAddress))
            {
                _hub.OnTaskChanged += _hub_OnTaskChanged;
                _hub.OnConnectionClosed += _hub_OnConnectionClosed;
            }
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

        private static async Task HarmonyConnectAsync()
        {
            await HarmonyOpenAsync();

            if (_hub.IsReady)
            {
                await HarmonyGetConfigAsync();
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

        private static void _hub_OnConnectionClosed(object sender, bool e)
        {
            //    // Consistency check
            //    Debug.Assert(_hub.IsClosed);

            //    // We know this notification is not coming from the UI thread.
            //    // Therefore we Invoke to be able to modifiy our tree view control.
            //    // Try opening our connection again to keep it alive.
            //    if (e)
            //    {
            //        //Server closed our connection try reconnect then
            //        BeginInvoke(new MethodInvoker(delegate () { HarmonyConnectAsync(); }));
            //    }
            //    else
            //    {
            //        // Just clear our config
            //        BeginInvoke(new MethodInvoker(delegate () { ClearConfig(); }));
            //    }
        }

        private static void _hub_OnTaskChanged(object sender, bool e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Initializer
        public static bool Initialize()
        {
            Console.WriteLine("Harmony Initializing");
            _activityList = new List<Activity>();
            _deviceList = new List<Device>();
            _ipAddress = configuration.AppSettings.Settings["harmony_ip"].Value;            
 
            if(_hub == null)
            {
                _hub = new Client(_ipAddress);
                _hub.CancelCurrentTask();

                Task t = HarmonyOpenAsync();
                t.Wait();

                _hub.CancelCurrentTask();

                Task x = HarmonyGetConfigAsync();
                x.Wait();

                //Get current activity
                Task<string> y = _hub.GetCurrentActivityAsync();
                y.Wait();

                string activityId = y.Result;
                string activityName = _harmonyConfig.ActivityNameFromId(activityId);
                _currentActivity = ActivityLookup(activityName).FirstOrDefault();

                if (_harmonyConfig != null)
                {
                    _isInitialized = true;
                }
                _hub.OnActivityChanged += _hub_OnActivityChanged;
                _hub.OnTaskChanged += _hub_OnTaskChanged1;
                _hub.OnConnectionClosed += _hub_OnConnectionClosed1;

            }


            GetActivities();
            GetDevices();

            //if(_hub.RequestPending)
            //{
            //    _hub.CancelCurrentTask();
            //}

            
            Console.WriteLine("Harmony Initialized");
            return _isInitialized;

        }

        private static void _hub_OnConnectionClosed1(object sender, bool e)
        {
            Task t = HarmonyOpenAsync();
            t.Wait();
        }

        private static void _hub_OnTaskChanged1(object sender, bool e)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Event
        private static void _hub_OnActivityChanged(object sender, string e)
        {
            while(!_hub.IsReady)
            {
                //Wait until hub is ready
            }

            _currentActivity = ActivityLookup(_harmonyConfig.ActivityNameFromId(e)).FirstOrDefault();
        }

        #endregion

    }
}
