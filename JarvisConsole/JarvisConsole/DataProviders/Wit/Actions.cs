using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProviders.Harmony;
using HarmonyHub;
using System.Configuration;

namespace DataProviders.Wit
{
    public static class Actions
    {

        public static Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>> ActionDictionary => new Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>>
        {
            //Harmony activites
            {"HarmonyStartActivity", HarmonyStartActivity },            

            //Nest activities
            {"NestTempDown", NestTempDown },
            {"NestTempUp", NestTempUp },
            {"NestSetTemp", NestSetTemp },
            {"NestRunFan", NestRunFan }
        };

        #region Harmony Strings
        private static string PlayXboxOne = "Play Xbox One";
        private static string XboxOne_Music = "Xbox One/Music";
        private static string PlayPS4 = "Play PS4";
        private static string PS4_Music = "PS4/Music";
        private static string PlayWii = "Play Wii";
        private static string PlayWii_Music = "Wii/Music";
        private static string ListenToMusic = "Listen to Music";
        private static string WatchTv = "Watch TV";
        private static string WatchTv_Music = "TV/Music";
        private static string Chromecast = "Chromecast";
        private static string AirplayMusic = "Airplay Music";
        private static string WatchAppleTv = "Watch Apple Tv";

        #endregion

        #region Harmony Activities Methods
        public static object HarmonyStartActivity(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string StereoActivity = "";
            string TelevisionActivity = "";
            bool stereoPresent = false;
            bool televisionPresent = false;
            int stereoCount = 0;
            int televisionCount = 0;

            object returnContext = null;
            //Check entities for Stereo or Television entities
            foreach(KeyValuePair<string, List<Entity>> pair in entities)
            {
                if(pair.Key == "Stereo")
                {
                    StereoActivity = pair.Value.First().value.ToString();
                    stereoCount++;
                }
                
                if(pair.Key == "Television")
                {
                    TelevisionActivity = pair.Value.First().value.ToString();
                    televisionCount++;
                }
                
            }
            if(stereoCount > 0)
            {
                stereoPresent = true;
            }

            if(televisionCount > 0)
            {
                televisionPresent = true;
            }

            //Set return
            //ActuateActivity
            if (stereoPresent && televisionPresent)
            {
                returnContext = new { Stereo = StereoActivity, Television = TelevisionActivity };
                switch (TelevisionActivity)
                {
                    case "Play Wii": ActuateHarmonyActivity(PlayWii_Music); break;

                    case "Play Xbox One": ActuateHarmonyActivity(XboxOne_Music); break;

                    case "Play PS4": ActuateHarmonyActivity(PS4_Music); break;

                    case "Watch TV": ActuateHarmonyActivity(WatchTv_Music); break;
                }
            }
            else if(stereoPresent && !televisionPresent)
            {
                returnContext = new { Stereo = StereoActivity, missingTelevision = "" };
                //Actuate stereo activity
                ActuateHarmonyActivity(StereoActivity);
            }
            else if(!stereoPresent && televisionPresent)
            {
                //Actuate television activity
                returnContext = new { missingStereo = "", Television = TelevisionActivity };
                ActuateHarmonyActivity(TelevisionActivity);
            }
            else if(!stereoPresent && !televisionPresent)
            {
                //There is no activity to actuate
                returnContext = new { missingStereo = "", missingTelevision = "" };
            }
               
            return returnContext;
        }
        
        #endregion

        #region Nest Activities Methods
        public static object NestTempUp(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            return null;
        }

        public static object NestTempDown(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            return null;
        }

        public static object NestSetTemp(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            return null;
        }

        public static object NestRunFan(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            return null;
        }

        #endregion

        #region Private Methods
        private static bool ActuateHarmonyActivity(string harmonyActivity)
        {
            HarmonyDataProvider harmonyClient = new HarmonyDataProvider(ConfigurationManager.AppSettings["harmony_ip"]);
            List<Activity> activity = harmonyClient.ActivityLookup(harmonyActivity);
            //Task t = harmonyClient.StartActivity(activity.First().Id);
            //t.Wait();
            //if (t.IsCompleted)
            //{
            //    Console.WriteLine("-- System actuated '{0}' activity", activity.First().Label);
            //    return true;
            //}
            //else
            //{
            //    Console.WriteLine("-- Failed to actuate activity --");
            //    return false;
            //}
            return false;
        }

        #endregion
    }
}
