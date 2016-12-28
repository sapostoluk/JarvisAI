using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisConsole.Actions
{
    public static partial class Actions
    {
        public static Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>> ActionDictionary => new Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>>
        {
            //Harmony activites
            {"HarmonyStartActivity", HarmonyStartActivity },
            {"HarmonyVolume", HarmonyVolume },         

            //Nest activities
            {"NestSetTemperature", NestSetTemperature },
        };

        //ContextKeys

        //Harmony context keys
        private static string _contextStereo = "Stereo";
        private static string _contextTelevision = "Television";
        private static string _contextDirection = "Direction";
        private static string _volumeDown = "VolumeDown";
        private static string _volumeUp = "VolumeUp";

        //Nest context keys
        private static string _contextProduct = "Product";
        private static string _contextUnit = "Unit";
        private static string _contextNumber = "number";
    }
}
