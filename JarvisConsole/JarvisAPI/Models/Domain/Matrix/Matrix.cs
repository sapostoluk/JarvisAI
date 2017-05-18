using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Domain.Matrix
{
    public class Matrix : HarmonyDevice
    {
        private List<Input> _inputs;
        private List<Output> _outputs;

        public List<Input> Inputs
        {
            get { return _inputs; }
            set
            {
                if(value != _inputs)
                {
                    _inputs = value;
                }                
            }
        }
        public List<Output> Outputs
        {
            get { return _outputs; }
            set
            {
                if(value != _outputs)
                {
                    _outputs = value;
                }
            }
        }

        public Matrix()
        {
            _inputs = new List<Input>();
            _outputs = new List<Output>();
        }

    }
}