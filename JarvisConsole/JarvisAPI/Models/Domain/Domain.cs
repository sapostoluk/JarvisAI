using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace JarvisAPI.Models.Domain
{
    public class Domain
    {
        #region Fields
        private string _name;
        private List<Room> _rooms;
        private List<HarmonyDevice> _harmonyDevices;
        private List<OutletDevice> _outletDevices;
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
        public List<HarmonyDevice> HarmonyDevices
        {
            get { return _harmonyDevices; }
            set
            {
                if(value != _harmonyDevices)
                {
                    _harmonyDevices = value;
                }
            }
        }
        public List<OutletDevice> OutletDevices
        {
            get { return _outletDevices; }
            set
            {
                if(value != _outletDevices)
                {
                    _outletDevices = value;
                }
            }
        }
        #endregion Properties 

        #region Constructor
        public Domain()
        {
            _rooms = new List<Room>();
            _harmonyDevices = new List<HarmonyDevice>();
            _outletDevices = new List<OutletDevice>();
        }
        #endregion Constructor      

        #region Methods
        public IEnumerable<Room> RoomInDomain(string room)
        {
            if(this.Rooms.Any(e => e.RoomName == room))
            {
                return Rooms.Where(e => e.RoomName == room);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<HarmonyActivity> ActivityInRoom(Room room, string activityName)
        {
            if(room.HarmonyActivities.Any(e => e.ActivityName == activityName))
            {
                return room.HarmonyActivities.Where(e => e.ActivityName == activityName);
            }
            else
            {
                return null;
            }
        }

        #endregion Methods
    }
}