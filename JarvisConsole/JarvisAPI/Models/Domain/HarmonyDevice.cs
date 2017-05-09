using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace JarvisAPI.Models.Domain
{
    public class HarmonyDevice
    {       
        public enum HarmonyDeviceState
        {
            on,
            off,
        }

        #region Fields
        private string _manufacturer;
        private string _modelName;
        private string _displayName;
        private string _deviceName;

        [XmlIgnore]
        private HarmonyDeviceState _onState;

        #endregion Fields

        #region Properties
        public string Manufacturer
        {
            get
            {
                return _manufacturer;
            }
            set
            {
                if(value != _manufacturer)
                {
                    _manufacturer = value;
                }
            }
        }

        public string ModelName
        {
            get { return _modelName; }
            set
            {
                if(value != _modelName)
                {
                    _modelName = value;
                }
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if(value != _displayName)
                {
                    _displayName = value;
                }
            }
        }

        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                if(value != _deviceName)
                {
                    _deviceName = value;
                }
            }
        }

        [XmlIgnore]
        public HarmonyDeviceState OnState
        {
            get { return _onState; }
            set
            {
                if (value != _onState)
                {
                    _onState = value;
                }
            }
        }
        #endregion Properties
    }
}