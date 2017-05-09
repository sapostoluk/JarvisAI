using JarvisAPI.DataProviders.Orvibo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace JarvisAPI.Models.Domain
{
    public class Room
    {
        #region Fields
        private string _roomName;
        private string _roomOwner;
        private List<HarmonyDevice> _harmonyDevices;
        private List<OutletDevice> _outletDevices;

        [XmlIgnore]
        private HarmonyActivity _currentHarmonyActivity;
        #endregion Fields

        #region Properties
        public string RoomName
        {
            get { return _roomName; }
            set
            {
                if(value != _roomName)
                {
                    _roomName = value;
                }
            }
        }
        public string RoomOwner
        {
            get { return _roomOwner; }
            set
            {
                if(value != _roomOwner)
                {
                    _roomOwner = value;
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

        [XmlIgnore]
        public HarmonyActivity CurrentHarmonyActivity
        {
            get { return _currentHarmonyActivity; }
            set
            {
                if(value != _currentHarmonyActivity)
                {
                    _currentHarmonyActivity = value;
                }
            }
        }
        #endregion Properties

        #region Constructor
        public Room()
        {
            HarmonyDevices = new List<HarmonyDevice>();
            OutletDevices = new List<OutletDevice>();
            _currentHarmonyActivity = new HarmonyActivity();
        }
        #endregion Constructor
    }
}