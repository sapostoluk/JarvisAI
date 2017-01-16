using OrviboController.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriviboControl
{
    public static class OriviboDataProvider
    {
        private static Controller _controller;
        private static bool _isCycling;
        private static bool _nextCycleIsOn;
        private static ObservableCollection<Device> _deviceList;
        
        public static void Initialize()
        {
            _controller = Controller.CreateController(true);
            
            
            _deviceList = new ObservableCollection<Device>();
            _controller.OnFoundNewDevice += _controller_OnFoundNewDevice;

            _controller.StartListening();
            _controller.SendDiscoveryCommand();

        }

        private static void _controller_OnFoundNewDevice(object sender, DeviceEventArgs e)
        {
            if(!_deviceList.Contains(e.Device))
            {
                _deviceList.Add(e.Device);
            }
        }

        public static void OnCommand(string ipAddress, string macAddress)
        {
            var success = false;
            Device device = null;

            device = Device.CreateDevice(ipAddress, macAddress);
            success = DoControlPower(device, true);
        }

        public static void OffCommand(string ipAddress, string macAddress)
        {
            var success = false;
            Device device = null;

            device = Device.CreateDevice(ipAddress, macAddress);
            success = DoControlPower(device, false);
        }

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

        

        
    }
}
