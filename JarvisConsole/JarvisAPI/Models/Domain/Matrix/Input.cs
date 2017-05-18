using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Domain.Matrix
{
    public class Input
    {
        #region Fields
        private int _inputNumber;
        private HarmonyDevice _inputDevice;
        #endregion Fields

        #region Properties
        public int InputNumber
        {
            get { return _inputNumber; }
            set
            {
                if(value != _inputNumber)
                {
                    _inputNumber = value;
                }
            }
        }

        public HarmonyDevice InputDevice
        {
            get { return _inputDevice; }
            set
            {
                if(value != _inputDevice)
                {
                    _inputDevice = value;
                }
            }
        }

        #endregion Properties
    }
}