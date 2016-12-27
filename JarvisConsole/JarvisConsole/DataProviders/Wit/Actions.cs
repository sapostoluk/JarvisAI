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
            {"HarmonyVolume", HarmonyVolume },
            {"HarmonyPowerOff", HarmonyPowerOff },           

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

        public static object HarmonyVolume(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            int directionCount = 0;
            string directionValue = "";
            object returnContext = null;
            foreach (KeyValuePair<string, List<Entity>> pair in entities)
            {
                switch (pair.Key)
                {
                    case "Direction": directionCount++; directionValue = pair.Value.First().value.ToString(); break;
                }                
            }

            if(directionCount > 0)
            {
                returnContext = new { Direction = directionValue };
            }

            HarmonyDataProvider harmonyClient = new HarmonyDataProvider(ConfigurationManager.AppSettings["harmony_ip"]);
            Function function = null;
            
            switch(directionValue)
            {
                case "up":
                    {
                        IEnumerable<ControlGroup> controlGroups = harmonyClient.CurrentActivity.ControlGroups.Where(e => e.Name == "Volume");
                        ControlGroup control = controlGroups.FirstOrDefault();
                        foreach(Function func in control.Functions)
                        {
                            if (func.Name == "VolumeUp")
                            {
                                function = func;
                                break;
                            }
                        }                       
                    }
                    break;

                case "down":
                    {
                        IEnumerable<ControlGroup> controlGroups = harmonyClient.CurrentActivity.ControlGroups.Where(e => e.Name == "Volume");
                        ControlGroup control = controlGroups.FirstOrDefault();
                        foreach (Function func in control.Functions)
                        {
                            if (func.Name == "VolumeDown")
                            {
                                function = func;
                                break;
                            }
                        }
                    }
                    break;
            }
            //Change volume correct number of times
            for (int i = 0; i < Convert.ToInt32(ConfigurationManager.AppSettings["volume_interval"]); i++)
            {
                ActuateHarmonyCommand(function);
            }
            

            return returnContext;
        }

        public static object HarmonyPowerOff(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            object returnContext = null;
            string on_offValue = "";
            on_offValue = entities.Where(e => e.Key == "on_off").FirstOrDefault().Value.ToString();
            returnContext = new { wit_on_offValue = on_offValue };

            HarmonyDataProvider harmonyClient = new HarmonyDataProvider(ConfigurationManager.AppSettings["harmony_ip"]);
            Activity powerOffActivity = harmonyClient.PowerOffActivity;
            ActuateHarmonyActivity(powerOffActivity.Label);

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
            Console.WriteLine("-- System is attempting to actuate the '{0}' activity --", activity.First().Label);
            Task t = harmonyClient.StartActivity(activity.First().Id);
            t.Wait();
            if (t.IsCompleted)
            {
                Console.WriteLine("-- System actuated the '{0}' activity", activity.First().Label);
                return true;
            }
            else
            {
                Console.WriteLine("-- Failed to actuate activity --");
                return false;
            }

            harmonyClient.CloseConnection();

            return false;
        }

        private static bool ActuateHarmonyCommand(Function func)
        {
            HarmonyDataProvider harmonyClient = new HarmonyDataProvider(ConfigurationManager.AppSettings["harmony_ip"]);
            Console.WriteLine("-- System is attempting to actuate the '{0}' command --", func.Name);
            Task t = harmonyClient.SendCommand(func.Name, func.Action.DeviceId );
            t.Wait();
            bool ret = false;
            if (t.IsCompleted)
            {
                Console.WriteLine("-- System actuated the '{0}' command", func.Name);
                ret = true;
            }
            else
            {
                Console.WriteLine("-- Failed to actuate activity --");
                ret = false;
            }

            harmonyClient.CloseConnection();

            return ret;
        }

        #endregion
    }
}
