using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyHub;
using System.IO;
using System.Diagnostics;


namespace DataProviders.Harmony
{
    public class HarmonyDataProvider
    {
        #region Fields
        private Client _hub;
        private string _ipAddress;
        private Config _harmonyConfig;
        private List<Activity> _activityList;
        private Activity _powerOffActivity;
        private List<Device> _deviceList;
        private Activity _currentActivity;
        
        #endregion

        #region Properties
        public string IpAddress
        {
            get { return _ipAddress; }
        }

        public List<Activity> ActivityList
        {
            get { return _activityList; }
        }

        public List<Device> DeviceList
        {
            get { return _deviceList; }
        }

        public Activity CurrentActivity
        {
            get { return _currentActivity; }
            
        }

        public Activity PowerOffActivity
        {
            get { return _powerOffActivity; }
        }

        #endregion

        #region Public Methods 
        public async Task SendCommand(string command, string deviceId)
        {
            await _hub.SendKeyPressAsync(deviceId, command);
        }
           
        public async Task StartActivity(string activityId)
        {
            await _hub.StartActivityAsync(activityId);
        }

        public List<Activity> ActivityLookup(string name)
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

        public async Task CloseConnection()
        {
            await _hub.CloseAsync();
        }

        #endregion

        #region Private Methods
        private async Task HarmonyOpenAsync()
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

        private async Task HarmonyConnectAsync()
        {
            await HarmonyOpenAsync();

            if (_hub.IsReady)
            {
                await HarmonyGetConfigAsync();
            }
        }

        private async Task HarmonyGetConfigAsync()
        {
            //Fetch our config
            var harmonyConfig = await _hub.GetConfigAsync();
            if (harmonyConfig == null)
            {
                return;
            }
            _harmonyConfig = harmonyConfig;
        }

        private async Task HarmonyCloseAsync()
        {
            Task t = _hub.CloseAsync();
            t.Wait();
        }

        private void GetDevices()
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

        private void GetActivities()
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

        private void _hub_OnConnectionClosed(object sender, bool e)
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

        private void _hub_OnTaskChanged(object sender, bool e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Constructor
        public HarmonyDataProvider(string ipAddress)
        {
            _activityList = new List<Activity>();
            _deviceList = new List<Device>();
            _ipAddress = ipAddress;

            _hub = new Client(_ipAddress);
            try
            {
                Task t = HarmonyOpenAsync();
                t.Wait();

                Task x = HarmonyGetConfigAsync();
                x.Wait();
            }
            catch (Exception e)
            {
                throw new Exception("Connection error");
            }
            
            GetActivities();
            GetDevices();

            //Get current activity
            Task<string> y = _hub.GetCurrentActivityAsync();
            y.Wait();

            string activityId = y.Result;
            string activityName = _harmonyConfig.ActivityNameFromId(activityId);
            _currentActivity = ActivityLookup(activityName).First();

        }

        #endregion

    }
}
