using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Domain
{
    public class HarmonyDeviceSetupItem
    {
        private HarmonyDevice _harmonyDevice;
        private string _input;

        public HarmonyDevice HarmonyDevice
        {
            get { return _harmonyDevice; }
            set
            {
                if(value != _harmonyDevice)
                {
                    _harmonyDevice = value;
                }
            }
        }
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
            }
        }

        public HarmonyDeviceSetupItem()
        {
            _harmonyDevice = new HarmonyDevice();
        }
    }
}