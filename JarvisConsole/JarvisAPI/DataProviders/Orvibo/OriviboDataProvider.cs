using JarvisAPI.DataProviders.Orvibo;
using OrviboController.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAPI.DataProviders.Orvibo
{
    public static class OrviboDataProvider
    {
        #region Fields
        private static Controller _controller;
        private static bool _isCycling;
        private static bool _nextCycleIsOn;
        private static ObservableCollection<OrviboDevice> _deviceList;
        private static string _orvibo_log_location = "orvibo";
        
        private static Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

        #endregion

        #region Properties
        public static bool isInitialized
        {
            get { return (_controller != null && _deviceList.Count > 0); }
        }

        #endregion

        #region Initializer
        public static void Initialize()
        {
            Logging.Log(_orvibo_log_location, "Orvibo begin initialization");

            if(_controller == null || _deviceList.Count <= 0)
            {
                Logging.Log("orvibo", "Controller is null");
                //Logging.Log("orvibo", "Count" + _deviceList.Count.ToString());           
                _controller = Controller.CreateController(false);
                _deviceList = new ObservableCollection<OrviboDevice>();
                //Get list of orvibo devices from web config

                Logging.Log(_orvibo_log_location, "Got to Try");
                int numOfDevices = 0;
                try
                {
                    Int32.TryParse(configuration.AppSettings.Settings["OrviboNumberOfDevices"].Value, out numOfDevices);

                    for (int i = 0; i < numOfDevices; i++)
                    {
                        string mac = configuration.AppSettings.Settings[configuration.AppSettings.Settings.AllKeys.FirstOrDefault(e => e.Contains("Orvibo") && e.Contains("MacAddress") && e.Contains(i.ToString()))].Value;
                        string ip = configuration.AppSettings.Settings[configuration.AppSettings.Settings.AllKeys.FirstOrDefault(e => e.Contains("Orvibo") && e.Contains("IpAddress") && e.Contains(i.ToString()))].Value;

                        OrviboDevice device = new OrviboDevice("OrviboDevice" + i.ToString(), Device.CreateDevice(ip, mac));

                        _deviceList.Add(device);
                    }

                    //_controller.OnFoundNewDevice += _controller_OnFoundNewDevice;

                    _controller.StartListening();
                    _controller.SendDiscoveryCommand();
                }
                catch (Exception ex)
                {
                    Logging.Log(_orvibo_log_location, "Exception while initializing orvibo: " + ex.Message);
                }
            }
            

            Logging.Log(_orvibo_log_location, "Orvibo finished initialization");
        }

        #endregion

        #region Events
        private static void _controller_OnFoundNewDevice(object sender, DeviceEventArgs e)
        {
            //if(!_deviceList.Contains(e.Device))
            //{
            //    _deviceList.Add(e.Device);
            //}
        }

        #endregion

        #region Public Methods
        public static OrviboDevice GetDevice(string name)
        {
            OrviboDevice returnDevice = null;
            if(_deviceList.Any(e => e.Name == name))
            {
                returnDevice = _deviceList.FirstOrDefault(x => x.Name == name);
            }
            return returnDevice;
        }

        public static bool OnCommand(string name)
        {
            bool success = false;
            try
            {
                OrviboDevice oDevice = GetDevice(name);
                Device device = null;

                device = Device.CreateDevice(oDevice.ODevice.IpAddr, oDevice.ODevice.MacAddr);
                success = DoControlPower(device, true);

            }
            catch(Exception ex)
            {
                Logging.Log(_orvibo_log_location, string.Format("Exception while changing device {0} to 'on': " + ex.Message, name));
            }

            return success;
        }

        public static bool OffCommand(string name)
        {
            bool success = false;
            try
            {
                OrviboDevice oDevice = GetDevice(name);
                Device device = null;
                device = Device.CreateDevice(oDevice.ODevice.IpAddr, oDevice.ODevice.MacAddr);
                
                success = DoControlPower(device, false);
            }
            catch(Exception ex)
            {
                Logging.Log(_orvibo_log_location, string.Format("Exception while changing device {0} to 'off': " + ex.Message, name));
            }

            return success;
        }

        public static ObservableCollection<string> GetDevices()
        {
            ObservableCollection<string> stringList = new ObservableCollection<string>();
            if (_deviceList != null)
            {               
                foreach (OrviboDevice device in _deviceList)
                {
                    stringList.Add(device.Name);
                }
            }
            return stringList;
        }
        #endregion

        #region Private Methods


        private static bool DoControlPower(Device device, bool on)
        {
            var subscription = Command.CreateSubscribeCommand(device);

            var success = _controller.SendCommandWaitResponse(device, subscription);
            if(success)
            {
                var control = Command.CreatePowerControlCommand(device, on);
                success = _controller.SendCommandWaitResponse(device, control);           
            }
            return success;
        }

        #endregion

    }

}
