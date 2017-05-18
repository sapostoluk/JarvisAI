using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Domain.Matrix
{
    public class Output
    {
        #region Fields
        private int _outputNumber;
        private HarmonyDevice _outputDevice;
        #endregion Fields

        #region Properties
        public int OutputNumber
        {
            get { return _outputNumber; }
            set
            {
                if (value != _outputNumber)
                {
                    _outputNumber = value;
                }
            }
        }

        public HarmonyDevice OutputDevice
        {
            get { return _outputDevice; }
            set
            {
                if (value != _outputDevice)
                {
                    _outputDevice = value;
                }
            }
        }

        #endregion Properties
    }
}