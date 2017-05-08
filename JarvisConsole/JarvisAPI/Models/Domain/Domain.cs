using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Domain
{
    public class Domain
    {
        #region Fields
        private string _name;
        private List<Room> _rooms;
        #endregion Fields

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
        public List<Room> Rooms
        {
            get { return _rooms; }
            set
            {
                if(value != _rooms)
                {
                    _rooms = value;
                }
            }
        }
        #endregion Properties 

        #region Constructor
        public Domain()
        {
            _rooms = new List<Room>();
        }
        #endregion Constructor
    }
}