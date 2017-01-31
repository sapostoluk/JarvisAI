using com.valgut.libs.bots.Wit.Models;
using JarvisConsole.DataProviders;
using HarmonyHub;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarvisAPI;

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

        //Harmony commands
        private static string _transportBasic = "TransportBasic";

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
                
                //Actuate stereo activity
                bool success = ActuateHarmonyActivity(StereoActivity);
                returnContext = new { Stereo = StereoActivity, missingTelevision = "", Success = success.ToString() };
            }
            //Stereo missing / television present
            else if (string.IsNullOrWhiteSpace(StereoActivity) && !string.IsNullOrWhiteSpace(TelevisionActivity))
            {
                //Actuate television activity                
                if (TelevisionActivity == _powerOff)
                {
                    ActuateHarmonyPowerOff(entities);
                }
                
                bool success = ActuateHarmonyActivity(TelevisionActivity);
                returnContext = new { missingStereo = "", Television = TelevisionActivity, Success = success.ToString() };
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
                        IEnumerable<ControlGroup> controlGroups = null;
                        if (HarmonyDataProvider.CurrentActivity != null)
                        {
                            controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == "Volume");
                        }                       
                        ControlGroup control = controlGroups.FirstOrDefault();
                        if (control.Functions.Any(e => e.Name == _contextVolumeUp))
                        {
                            function = control.Functions.Where(x => x.Name == _contextVolumeUp).FirstOrDefault();
                        }
                    }
                    break;

                case "down":
                    {
                        IEnumerable<ControlGroup> controlGroups = null;
                        if(HarmonyDataProvider.CurrentActivity != null)
                        {
                            controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == "Volume");
                        }
                        ControlGroup control = controlGroups.FirstOrDefault();
                        if (control.Functions.Any(e => e.Name == _contextVolumeDown))
                        {
                            function = control.Functions.Where(x => x.Name == _contextVolumeDown).FirstOrDefault();
                        }
                    }
                    break;
            }
            //Change volume correct number of times
            int volInterval;
            try
            {
                int.TryParse(configuration.AppSettings.Settings["volume_interval"].Value, out volInterval);
                for (int i = 0; i < volInterval; i++)
                {
                    ActuateHarmonyCommand(function);
                }
                returnContext = new { Success = true.ToString() };
            }
            catch(Exception e)
            {
                Logging.Log(_actionLogPath, "Harmony volume action failed: " + e.Message);
                returnContext = new { Success = false.ToString() };
            }
            

            return returnContext;
        }

        private static object HarmonyCheckStatus(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string activity = HarmonyDataProvider.CurrentActivity.Label;
            string location = entities.FirstOrDefault(e => e.Key == "room").Value.FirstOrDefault().value.ToString();
            object returnContext = null;
            if(activity != null && location != null)
            {
                returnContext = new { expectedActivity = activity, room = location, Success = true.ToString() };
            }
            else
            {
                returnContext = new { Success = false.ToString() };
            }
            return returnContext;
        }

        private static object HarmonySendCommand(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string command = "";
            if(entities.Any(e => e.Key == _contextHarmonyCommand))
            {
                command = entities.FirstOrDefault(e => e.Key == _contextHarmonyCommand).Value.FirstOrDefault().value.ToString();
            }
            Function function = null;
            switch(command)
            {
                case "Play":
                    {
                        IEnumerable<ControlGroup> controlGroups = null;
                        if (HarmonyDataProvider.CurrentActivity != null)
                        {
                            controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == _transportBasic);
                        }
                        ControlGroup control = controlGroups.FirstOrDefault();
                        if (control.Functions.Any(e => e.Name == _contextPlay))
                        {
                            function = control.Functions.Where(x => x.Name == _contextPlay).FirstOrDefault();
                        }
                    }
                    break;

                case "Pause":
                    {
                        IEnumerable<ControlGroup> controlGroups = null;
                        if (HarmonyDataProvider.CurrentActivity != null)
                        {
                            controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == _transportBasic);
                        }
                        ControlGroup control = controlGroups.FirstOrDefault();
                        if (control.Functions.Any(e => e.Name == _contextPause))
                        {
                            function = control.Functions.Where(x => x.Name == _contextPause).FirstOrDefault();
                        }
                    }
                    break;

                case "Rewind":
                    {
                        IEnumerable<ControlGroup> controlGroups = null;
                        if(HarmonyDataProvider.CurrentActivity != null)
                        {
                            controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == _transportBasic);
                        }
                        ControlGroup control = controlGroups.FirstOrDefault();
                        if(control.Functions.Any(e => e.Name == _contextRewind))
                        {
                            function = control.Functions.Where(x => x.Name == _contextRewind).FirstOrDefault();
                        }
                    }
                    break;

                case "FastForward":
                    {
                        IEnumerable<ControlGroup> controlGroups = null;
                        if(HarmonyDataProvider.CurrentActivity != null)
                        {
                            controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == _transportBasic);
                        }
                        ControlGroup control = controlGroups.FirstOrDefault();
                        if(control.Functions.Any(e => e.Name == _contextFastForward))
                        {
                            function = control.Functions.Where(x => x.Name == _contextFastForward).FirstOrDefault();
                        }
                    }
                    break;                  
            }
            object returnContext = null;
            //HACK - need to have consistency in exception handling
            //Is the system on?
            if(HarmonyDataProvider.CurrentActivity != HarmonyDataProvider.PowerOffActivity)
            {
                try
                {
                    ActuateHarmonyCommand(function);
                    returnContext = new { Success = true.ToString() };
                }
                catch (Exception e)
                {
                    Logging.Log(_actionLogPath, string.Format("System failed to actuate command '{0}'", function.Name));
                    returnContext = new { Success = false.ToString() };
                }
            }
            else
            {
                returnContext = new { Success = false.ToString() };
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
            HarmonyDataProvider.StartActivity(activity.First().Id);
            if (HarmonyDataProvider.CurrentActivity == activity.First())
            {
                Console.WriteLine("-- System actuated the '{0}' activity", activity.First().Label);
                return true;
            }
            else
            {
                Console.WriteLine("-- Failed to actuate activity --");
                return false;
            }

            //HarmonyDataProvider.CloseConnection();

            return false;
        }

        private static bool ActuateHarmonyCommand(Function func)
        {

            if (!HarmonyDataProvider.IsInitialized)
            {
                HarmonyDataProvider.Initialize();
            }
            
            //Console.WriteLine("-- System is attempting to actuate the '{0}' command --", func.Name);
            if(func != null)
            {
                HarmonyDataProvider.SendCommand(func.Name, func.Action.DeviceId);
            }
            else
            {
                Logging.Log("general", "Harmony failed cannot send command");
            }
            


            return true;
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
