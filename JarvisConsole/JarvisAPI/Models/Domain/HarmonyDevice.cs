using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Domain
{
    public class HarmonyDevice
    {
        #region Fields
        private string _manufacturer;
        private string _modelName;
        private string _displayName;
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
        #endregion Properties
    }
}