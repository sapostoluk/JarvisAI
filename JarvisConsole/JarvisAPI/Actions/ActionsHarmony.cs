﻿using com.valgut.libs.bots.Wit.Models;
using JarvisConsole.DataProviders;
using HarmonyHub;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisConsole.Actions
{
    public static partial class Actions
    {
        
        //Harmony Strings
        private static string _playXboxOne = "Play Xbox One";
        private static string _xboxOne_Music = "Xbox One/Music";
        private static string _playPS4 = "Play PS4";
        private static string _ps4_Music = "PS4/Music";
        private static string _playWii = "Play Wii";
        private static string _playWii_Music = "Wii/Music";
        private static string _listenToMusic = "Listen to Music";
        private static string _watchTv = "Watch TV";
        private static string _watchTv_Music = "TV/Music";
        private static string _chromecast = "Chromecast";
        private static string _airplayMusic = "Airplay Music";
        private static string _watchAppleTv = "Watch Apple Tv";
        private static string _powerOff = "PowerOff";
        

        #region Harmony Activities Methods
        private static object HarmonyStartActivity(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string StereoActivity = "";
            string TelevisionActivity = "";

            object returnContext = null;
            //Check entities for Stereo or Television entities
            if (entities.Any(e => e.Key == _contextStereo))
            {
                StereoActivity = entities.Where(x => x.Key == _contextStereo).FirstOrDefault().Value.FirstOrDefault().value.ToString();
            }
            if (entities.Any(e => e.Key == _contextTelevision))
            {
                TelevisionActivity = entities.Where(x => x.Key == _contextTelevision).FirstOrDefault().Value.FirstOrDefault().value.ToString();
            }

            //Set return
            //ActuateActivity

            //Stereo present / Television present
            if (!string.IsNullOrWhiteSpace(StereoActivity) && !string.IsNullOrWhiteSpace(TelevisionActivity))
            {
                returnContext = new { Stereo = StereoActivity, Television = TelevisionActivity };
                switch (TelevisionActivity)
                {
                    case "Play Wii": ActuateHarmonyActivity(_playWii_Music); break;

                    case "Play Xbox One": ActuateHarmonyActivity(_xboxOne_Music); break;

                    case "Play PS4": ActuateHarmonyActivity(_ps4_Music); break;

                    case "Watch TV": ActuateHarmonyActivity(_watchTv_Music); break;
                }
            }

            //Stereo present / Television missing
            else if (!string.IsNullOrWhiteSpace(StereoActivity) && string.IsNullOrWhiteSpace(TelevisionActivity))
            {
                returnContext = new { Stereo = StereoActivity, missingTelevision = "" };
                //Actuate stereo activity
                ActuateHarmonyActivity(StereoActivity);
            }
            //Stereo missing / television present
            else if (string.IsNullOrWhiteSpace(StereoActivity) && !string.IsNullOrWhiteSpace(TelevisionActivity))
            {
                //Actuate television activity                
                if (TelevisionActivity == _powerOff)
                {
                    ActuateHarmonyPowerOff(entities);
                }
                returnContext = new { missingStereo = "", Television = TelevisionActivity };
                ActuateHarmonyActivity(TelevisionActivity);
            }
            //None present
            else if (string.IsNullOrWhiteSpace(StereoActivity) && string.IsNullOrWhiteSpace(TelevisionActivity))
            {
                //There is no activity to actuate
                returnContext = new { missingStereo = "", missingTelevision = "" };
            }

            return returnContext;
        }

        private static object HarmonyVolume(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string directionValue = "";
            object returnContext = null;

            if (entities.Any(e => e.Key == _contextDirection))
            {
                directionValue = entities.Where(x => x.Key == _contextDirection).FirstOrDefault().Value.FirstOrDefault().value.ToString();
            }

            if (!HarmonyDataProvider.IsInitialized)
            {
                HarmonyDataProvider.Initialize();
            }
            Function function = null;

            switch (directionValue)
            {
                case "up":
                    {
                        IEnumerable<ControlGroup> controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == "Volume");
                        ControlGroup control = controlGroups.FirstOrDefault();
                        if (control.Functions.Any(e => e.Name == _volumeUp))
                        {
                            function = control.Functions.Where(x => x.Name == _volumeUp).FirstOrDefault();
                        }
                    }
                    break;

                case "down":
                    {
                        IEnumerable<ControlGroup> controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == "Volume");
                        ControlGroup control = controlGroups.FirstOrDefault();
                        if (control.Functions.Any(e => e.Name == _volumeDown))
                        {
                            function = control.Functions.Where(x => x.Name == _volumeDown).FirstOrDefault();
                        }
                    }
                    break;
            }
            //Change volume correct number of times
            int volInterval = Convert.ToInt32(configuration.AppSettings.Settings["volume_interval"]);
            for (int i = 0; i < volInterval; i++)
            {
                ActuateHarmonyCommand(function);
            }

            return returnContext;
        }

        #endregion

        #region Private Methods
        private static bool ActuateHarmonyActivity(string harmonyActivity)
        {
            if(!HarmonyDataProvider.IsInitialized)
            {
                HarmonyDataProvider.Initialize();
            }

            List<Activity> activity = HarmonyDataProvider.ActivityLookup(harmonyActivity);
            Console.WriteLine("-- System is attempting to actuate the '{0}' activity --", activity.First().Label);
            Task t = HarmonyDataProvider.StartActivity(activity.First().Id);
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

            HarmonyDataProvider.CloseConnection();

            return false;
        }

        private static bool ActuateHarmonyCommand(Function func)
        {
            HarmonyDataProvider.Initialize();

            Console.WriteLine("-- System is attempting to actuate the '{0}' command --", func.Name);
            Task t = HarmonyDataProvider.SendCommand(func.Name, func.Action.DeviceId);
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
            //bool ret = true;

            HarmonyDataProvider.CloseConnection();

            return ret;
        }

        private static bool ActuateHarmonyPowerOff(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string command = "";
            if (entities.Any(e => e.Key == _powerOff))
            {
                command = entities.Where(x => x.Key == _powerOff).FirstOrDefault().Value.FirstOrDefault().value.ToString();
            }

            if(!HarmonyDataProvider.IsInitialized)
            {
                HarmonyDataProvider.Initialize();
            }
            Activity powerOffActivity = HarmonyDataProvider.PowerOffActivity;

            return ActuateHarmonyActivity(powerOffActivity.Label);
        }

        #endregion
    }
}
