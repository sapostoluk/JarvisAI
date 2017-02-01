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
        
        private static Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

        #endregion

        #region Properties
        public static bool isInitialized
        {
            get { return (_controller != null); }
        }

        #endregion

        #region Initializer
        public static void Initialize()
        {
            _controller = Controller.CreateController(false);
            _deviceList = new ObservableCollection<OrviboDevice>();
            //Get list of orvibo devices from web config

            int numOfDevices = 0;
            Int32.TryParse(configuration.AppSettings.Settings["OrviboNumberOfDevices"].Value,out numOfDevices);

            for(int i = 0; i < numOfDevices; i++)
            {
                string mac = configuration.AppSettings.Settings[configuration.AppSettings.Settings.AllKeys.FirstOrDefault(e => e.Contains("Orvibo") && e.Contains("MacAddress") && e.Contains(i.ToString()))].Value;
                string ip = configuration.AppSettings.Settings[configuration.AppSettings.Settings.AllKeys.FirstOrDefault(e => e.Contains("Orvibo") && e.Contains("IpAddress") && e.Contains(i.ToString()))].Value;

                OrviboDevice device = new OrviboDevice("OrviboDevice" + i.ToString(), Device.CreateDevice(ip, mac));
                    
                _deviceList.Add(device);
            }            


            //_controller.OnFoundNewDevice += _controller_OnFoundNewDevice;

            //_controller.StartListening();
            //_controller.SendDiscoveryCommand();

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

        public static void OnCommand(OrviboDevice oDevice)
        {
            var success = false;
            Device device = null;

            device = Device.CreateDevice(oDevice.ODevice.IpAddr, oDevice.ODevice.MacAddr);
            success = DoControlPower(device, true);
        }

        public static void OffCommand(OrviboDevice oDevice)
        {
            var success = false;
            Device device = null;

            device = Device.CreateDevice(oDevice.ODevice.IpAddr, oDevice.ODevice.MacAddr);
            success = DoControlPower(device, false);
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
