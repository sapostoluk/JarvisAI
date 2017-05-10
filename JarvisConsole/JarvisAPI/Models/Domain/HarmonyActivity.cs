using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Domain
{
    public class HarmonyActivity
    {
        #region Fields
        private string _activityName;
        //The string represents the input
        private List<HarmonyDeviceSetupItem> _deviceSetupList;
        private HarmonyDevice _volumeControlDevice;
        private HarmonyDevice _controlDevice;
        #endregion Fields

        #region Properties
        public string ActivityName
        {
            get { return _activityName; }
            set
            {
                if(value != _activityName)
                {
                    _activityName = value;
                }
            }
        }
        public List<HarmonyDeviceSetupItem> DeviceSetupList
        {
            get { return _deviceSetupList; }
            set
            {
                if(value != _deviceSetupList)
                {
                    _deviceSetupList = value;
                }
            }
        }
        public HarmonyDevice VolumeControlDevice
        {
            get { return _volumeControlDevice; }
            set
            {
                if(value != _volumeControlDevice)
                {
                    _volumeControlDevice = value;
                }
            }
        }
        public HarmonyDevice ControlDevice
        {
            get { return _controlDevice; }
            set
            {
                if(value != _controlDevice)
                {
                    _controlDevice = value;
                }
            }
        }
        #endregion Properties

        #region Constructor
        public HarmonyActivity()
        {
            DeviceSetupList = new List<HarmonyDeviceSetupItem>();
            _volumeControlDevice = new HarmonyDevice();
            _controlDevice = new HarmonyDevice();
        }
        #endregion Constructor
    }
}