using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;

namespace JarvisConsole.Actions
{
    public static partial class Actions
    {
        public static Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>> ActionDictionary => new Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>>
        {
            //Harmony activites
            {"HarmonyStartActivity", HarmonyStartActivity },
            {"HarmonyVolume", HarmonyVolume },
            {"HarmonyCheckStatus", HarmonyCheckStatus },
            {"HarmonySendCommand", HarmonySendCommand },
                    

            //Nest activities
            {"NestSetTemperature", NestSetTemperature },
            {"NestCheckStatus", NestCheckStatus },

            //Orvibo activities
            {"OrviboControl", OrviboControl },
        };

        //ContextKeys

        //Harmony context keys
        private static string _contextStereo = "Stereo";
        private static string _contextTelevision = "Television";
        private static string _contextDirection = "Direction";
        private static string _contextVolumeDown = "VolumeDown";
        private static string _contextVolumeUp = "VolumeUp";
        private static string _contextHarmonyCommand = "HarmonyCommand";
        private static string _contextPlay = "Play";
        private static string _contextPause = "Pause";
        private static string _contextRewind = "Rewind";
        private static string _contextFastForward = "FastForward";

        //Nest context keys
        private static string _contextProduct = "Product";
        private static string _contextUnit = "Unit";
        private static string _contextNumber = "number";

        //Orvibo context keys
        private static string _contextOnOff = "on_off";

        private static Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
    }
}
