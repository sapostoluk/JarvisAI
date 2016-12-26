using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyHub;
using System.IO;
using System.Diagnostics;


namespace HarmonyControl
{
    public class HarmonyDataProvider
    {
        #region Fields
        private Client _hub;
        private string _ipAddress;
        private Config _harmonyConfig;
        private List<Activity> _activityList;
        private List<Device> _deviceList;
        
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

        #endregion

        #region Public Methods
        public async void InitializeHubConnection(string ipAddress)
        {
            _ipAddress = ipAddress;
            _hub = new Client(ipAddress);
            try
            {
                await HarmonyOpenAsync(ipAddress);
                while (!_hub.IsReady)
                {
                    //Wait for hub to be ready
                }

                await HarmonyGetConfigAsync();
                while (_harmonyConfig == null)
                {
                    //wait
                }
            }
            catch(Exception e)
            {
                throw new Exception("Connection error");
            }
            

            GetActivities();
            GetDevices();           
        }

        public async Task HarmonyOpenAsync(string ipAddress)
        {
            if (_hub == null || !_hub.Host.Equals(ipAddress))
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

        #endregion

        #region Private Methods
        private async Task HarmonyConnectAsync()
        {
            await HarmonyOpenAsync(_ipAddress);

            if (_hub.IsReady)
            {
                await HarmonyGetConfigAsync();
            }
        }

        public async Task HarmonyGetConfigAsync()
        {
            //Fetch our config
            var harmonyConfig = await _hub.GetConfigAsync();
            if (harmonyConfig == null)
            {
                return;
            }
            _harmonyConfig = harmonyConfig;
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

    }
}
