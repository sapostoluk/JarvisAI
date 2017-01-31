using ApiAiSDK.Model;
using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;

namespace JarvisConsole.Actions
{
    public static partial class ApiAiActions
    {
        public static Dictionary<string, Func<Dictionary<string, object>, List<AIContext>>> ActionDictionary => new Dictionary<string, Func<Dictionary<string, object>, List<AIContext>>>
        {
            //General activites
            {"CheckSuccess", CheckSuccess },

            //Harmony activites
            {"HarmonyStartActivity", HarmonyStartActivity },
            {"HarmonyVolume", HarmonyVolume },
            {"HarmonyCheckStatus", HarmonyCheckStatus },
            {"HarmonySendCommand", HarmonySendCommand },
                    

            //Nest activities
            //{"NestSetTemperature", NestSetTemperature },
            //{"NestCheckStatus", NestCheckStatus },

            //Orvibo activities
            //{"OrviboControl", OrviboControl },
        };

        private static string _actionLogPath = "actions";

        #region ContextKeys

        //General Context Keys
        private static string _contextSuccess = "Success";
        private static string _contextSuccessful = "true";
        private static string _contextUnsuccessful = "false";

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

        #endregion
        private static Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

        #region Methods
        private static List<AIContext> CheckSuccess(Dictionary<string, object> parameters)
        {
            //object returnContext = null;
            //string status = "";

            //if(parameters.Any(e => e.Key == _contextSuccess))
            //{
            //    status = parameters.FirstOrDefault(e => e.Key == _contextSuccess).Value.ToString();
            //}
            //if(!string.IsNullOrWhiteSpace(status))
            //{
            //    if (status == _contextSuccessful)
            //    {
            //        returnContext = new { Successful = "true" };
            //    }
            //    else if (status == _contextUnsuccessful)
            //    {
            //        returnContext = new { Unsuccessful = "true" };
            //    }
            //}
            ////return returnContext;
            List<AIContext> contexts = null;
            return contexts;
        }

        #endregion
    }
}
