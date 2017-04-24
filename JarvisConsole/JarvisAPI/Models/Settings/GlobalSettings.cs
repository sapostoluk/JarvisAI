using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Settings
{
    public class GlobalSettings
    {
        public GlobalSettings()
        {
            BeaconAlphaLocation = new Location();
            BeaconBetaLocation = new Location();
            BeaconSigmaLocation = new Location();
        }
        
        public Location BeaconAlphaLocation { get; set; }
        public Location BeaconBetaLocation { get; set; }
        public Location BeaconSigmaLocation { get; set; }
    }


}