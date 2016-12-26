using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProviders.HarmonyDataProvider;
using HarmonyHub;

namespace DataProviders.Wit
{
    public static class Actions
    {

        public static Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>> ActionDictionary => new Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>>
        {
            //Harmony activites
            {"HarmonyStartActivity", HarmonyStartActivity },
            {"SelectTelevisionActivity", SelectTelevisionActivity },
            {"StartTelevisionActivity", StartTelevisionActivity },
            {"SelectAudioActivity", SelectAudioActivity },
            {"StartAudioActivity", StartAudioActivity },
            {"StartMixedActivity", StartMixedActivity },
            {"VolumeDown", VolumeDown },
            {"VolumeUp", VolumeUp },

            //Nest activities
            {"NestTempDown", NestTempDown },
            {"NestTempUp", NestTempUp },
            {"NestSetTemp", NestSetTemp },
            {"NestRunFan", NestRunFan }
        };

        #region Harmony Activites
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

        #region General Activities
        public static object EndConv()
        {
            return null;
        }

        #endregion

        #region Harmony Activities
        public static object HarmonyStartActivity(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string StereoActivity = "";
            string TelevisionActivity = "";
            bool stereoPresent = false;
            bool televisionPresent = false;
            int stereoCount = 0;
            int televisionCount = 0;

            object returnContext = null;
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
            HarmonyDataProvider.HarmonyDataProvider harmonyClient = new HarmonyDataProvider.HarmonyDataProvider();
            harmonyClient.InitializeHubConnection("192.168.1.4");
            List<Activity> activityList = null;
            if (stereoPresent && televisionPresent)
            {
                returnContext = new { Stereo = StereoActivity, Television = TelevisionActivity };
                switch (TelevisionActivity)
                {
                    case "Play Wii":
                        {
                            activityList = harmonyClient.ActivityLookup(PlayWii_Music);
                            harmonyClient.StartActivity(activityList.First().Id); break;
                        }

                    case "Play Xbox One": activityList = harmonyClient.ActivityLookup(XboxOne_Music);
                         harmonyClient.StartActivity(activityList.First().Id); break;

                    case "Play PS4": activityList = harmonyClient.ActivityLookup(PS4_Music);
                        harmonyClient.StartActivity(activityList.First().Id); break;

                    case "Watch TV": activityList = harmonyClient.ActivityLookup(WatchTv_Music);
                        harmonyClient.StartActivity(activityList.First().Id); break;
                }
            }
            else if(stereoPresent && !televisionPresent)
            {
                returnContext = new { Stereo = StereoActivity, missingTelevision = "" };
                List<Activity> activity = harmonyClient.ActivityLookup(StereoActivity);
                harmonyClient.StartActivity(activity.First().Id);
            }
            else if(!stereoPresent && televisionPresent)
            {
                returnContext = new { missingStereo = "", Television = TelevisionActivity };
                List<Activity> activity = harmonyClient.ActivityLookup(TelevisionActivity);
                harmonyClient.StartActivity(activity.First().Id);
            }
            else if(!stereoPresent && !televisionPresent)
            {
                returnContext = new { missingStereo = "", missingTelevision = "" };
            }
               
            return returnContext;
        }

        public static object SelectTelevisionActivity(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            object returnObj = null;
            int Contains = 0;
            //Begin looking for the "Television" entity
            foreach(KeyValuePair<string, List<Entity>> pair in entities)
            {
                if(pair.Key.Contains("Television"))
                {
                    Contains++;
                } 
                                            
            }

            //Television entitiy was found. DOn't return context
            if(Contains > 0)
            {
                returnObj = null;
            }
            //Return missingTelevision context
            else
            {
                returnObj = new { missingTelevision = "" };
            }
            return returnObj;
        }

        public static object StartTelevisionActivity(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string HarmonyActivity = "";
            object returnObj = null;
            foreach (KeyValuePair<string, List<Entity>> entity in entities)
            {
                if(entity.Key == "Television")
                {
                    HarmonyActivity = entity.Value.First().value.ToString();
                    //Update context
                    returnObj = new { missingTelevision = HarmonyActivity };
                }
            }

            Console.WriteLine("----The Start Activity started the {0} activity.", HarmonyActivity);
            return returnObj;
        }

        public static object SelectAudioActivity(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            return null;
        }

        public static object StartAudioActivity(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            return null;
        }

        public static object StartMixedActivity(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            return null;
        }

        public static object VolumeDown(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            return null;
        }

        public static object VolumeUp(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            return null;
        }

        #endregion

        #region Nest Activities
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
    }
}
