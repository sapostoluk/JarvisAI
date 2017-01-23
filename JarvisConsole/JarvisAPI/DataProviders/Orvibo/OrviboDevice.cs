using OrviboController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarvisAPI.DataProviders.Orvibo
{
    public class OrviboDevice
    {
        #region Fields
        private string _name;
        private string _commonName;
        private Device _device;

        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set
            {
                if(value != _name)
                {
                    _name = value;
                }
            }
        }

        public string CommonName
        {
            get { return _commonName; }
        }

        public Device ODevice
        {
            get { return _device; }
            set
            {
                if(value != _device)
                {
                    _device = value;
                }
            }
            
        }

        #endregion

        #region Constructor
        public OrviboDevice(string name, Device device, string commonName = null)
        {
            _name = name;
            _device = device;
            _commonName = commonName;
        }

        //public OrviboDevice()
        //{

        //}

        #endregion
    }
}