using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProviders.Wit
{
    public class WitContext
    {
        #region Fields
        private string _item;

        #endregion

        #region Properties
        public string Item
        {
            get { return _item; }
            set
            {
                if(value != _item)
                {
                    _item = value;
                }
            }
        }

        #endregion

    }
}
