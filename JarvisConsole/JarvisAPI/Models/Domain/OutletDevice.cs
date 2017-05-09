using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Domain
{
    public class OutletDevice
    {
        #region Fields
        private string _deviceName;
        private string _commonName;
        private string _ipAddress;
        private string _macAddress;
        #endregion Fields

        #region Properties
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
        public string CommonName
        {
            get { return _commonName; }
            set
            {
                if(value != _commonName)
                {
                    _commonName = value;
                }
            }
        }
        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                if(value != _ipAddress)
                {
                    _ipAddress = value;
                }
            }
        }
        public string MacAddress
        {
            get { return _macAddress; }
            set
            {
                if(value != _macAddress)
                {
                    _macAddress = value;
                }
            }
        }
        #endregion Properties

    }
}