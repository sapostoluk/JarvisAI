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
using JarvisAPI;
using JarvisAPI.DataProviders.APIAI;
using ApiAiSDK.Model;

namespace JarvisConsole.Actions
{
    public static partial class ApiAiActions
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
        private static List<AIContext> HarmonyStartActivity(Dictionary<string, object> parameters)
        {
            string StereoActivity = "";
            string TelevisionActivity = "";
            List<AIContext> contexts = new List<AIContext>();
            AIContext context = new AIContext();
            context.Name = "HarmonyControlReturn";
            Dictionary<string, string> contextParameters = new Dictionary<string, string>();

            object returnContext = null;
            //Check entities for Stereo or Television entities
            if (parameters.Any(e => e.Key == _contextStereo))
            {
                StereoActivity = parameters.Where(x => x.Key == _contextStereo).FirstOrDefault().Value.ToString();
            }
            if (parameters.Any(e => e.Key == _contextTelevision))
            {
                TelevisionActivity = parameters.Where(x => x.Key == _contextTelevision).FirstOrDefault().Value.ToString();
            }

            //Set return
            //ActuateActivity

            bool success = false;

            //Stereo present / Television present
            if (!string.IsNullOrWhiteSpace(StereoActivity) && !string.IsNullOrWhiteSpace(TelevisionActivity))
            {
                returnContext = new { Stereo = StereoActivity, Television = TelevisionActivity };
                string activity = "";
                switch (TelevisionActivity)
                {
                    case "Play Wii": success = ActuateHarmonyActivity(_playWii_Music); activity = _playWii_Music; break;

                    case "Play Xbox One": success = ActuateHarmonyActivity(_xboxOne_Music); activity =_xboxOne_Music; break;

                    case "Play PS4": success = ActuateHarmonyActivity(_ps4_Music); activity = _ps4_Music; break;

                    case "Watch TV": success = ActuateHarmonyActivity(_watchTv_Music); activity = _watchTv_Music; break;
                }
                if(!success)
                {                    
                    contextParameters.Add("activity", activity);
                    context.Parameters = contextParameters;

                    contexts.Add(context);
                }
            }

            //Stereo present / Television missing
            else if (!string.IsNullOrWhiteSpace(StereoActivity) && string.IsNullOrWhiteSpace(TelevisionActivity))
            {
                
                //Actuate stereo activity
                success = ActuateHarmonyActivity(StereoActivity);
                if (!success)
                {
                    contextParameters.Add("activity", StereoActivity);
                    context.Parameters = contextParameters;

                    contexts.Add(context);
                }
            }
            //Stereo missing / television present
            else if (string.IsNullOrWhiteSpace(StereoActivity) && !string.IsNullOrWhiteSpace(TelevisionActivity))
            {
                //Actuate television activity                
                if (TelevisionActivity == _powerOff)
                {
                    ActuateHarmonyPowerOff(parameters);
                }
                
                success = ActuateHarmonyActivity(TelevisionActivity);
                if (!success)
                {
                    contextParameters.Add("activity", TelevisionActivity);
                    context.Parameters = contextParameters;

                    contexts.Add(context);
                }
            }
            //None present
            else if (string.IsNullOrWhiteSpace(StereoActivity) && string.IsNullOrWhiteSpace(TelevisionActivity))
            {
                //There is no activity to actuate
                //returnContext = new { missingStereo = "", missingTelevision = "" };
            }
            return contexts;
        }

        private static List<AIContext> HarmonyVolume(Dictionary<string, object> parameters)
        {
            string directionValue = "";
            object returnContext = null;
            List<AIContext> contexts = new List<AIContext>();
            AIContext context = new AIContext();
            context.Name = "HarmonyVolumeReturn";
            Dictionary<string, string> contextParameters = new Dictionary<string, string>();

            if (parameters.Any(e => e.Key == _contextDirection))
            {
                directionValue = parameters.Where(x => x.Key == _contextDirection).FirstOrDefault().Value.ToString();
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
            bool success = false;

            //Change volume correct number of times
            int volInterval;
            try
            {
                int.TryParse(configuration.AppSettings.Settings["volume_interval"].Value, out volInterval);
                for (int i = 0; i < volInterval; i++)
                {
                    ActuateHarmonyCommand(function);
                }
                success = true;
            }
            catch(Exception e)
            {
                Logging.Log(_actionLogPath, "Harmony volume action failed: " + e.Message);
                success = false;
            }
            if (!success)
            {
                contextParameters.Add("command", "Volume");
                context.Parameters = contextParameters;

                contexts.Add(context);
            }

            return contexts;
        }

        private static List<AIContext> HarmonyCheckStatus(Dictionary<string, object> parameters)
        {
            string activity = HarmonyDataProvider.CurrentActivity.Label;
            string location = parameters.FirstOrDefault(e => e.Key == "room").Value.ToString();
            List<AIContext> contexts = new List<AIContext>();
            AIContext context = new AIContext();
            context.Name = "HarmonyCheckStatusReturn";
            Dictionary<string, string> contextParameters = new Dictionary<string, string>();
            if (activity != null && location != null)
            {
                contextParameters.Add("activity", activity);
                contextParameters.Add("location", location);
                context.Parameters = contextParameters;

                contexts.Add(context);
            }

            return contexts;
        }

        private static List<AIContext> HarmonySendCommand(Dictionary<string, object> parameters)
        {
            string command = "";
            List<AIContext> contexts = new List<AIContext>();
            AIContext context = new AIContext();
            context.Name = "HarmonySendCommandReturn";
            Dictionary<string, string> contextParameters = new Dictionary<string, string>();

            if (parameters.Any(e => e.Key == _contextHarmonyCommand))
            {
                command = parameters.FirstOrDefault(e => e.Key == _contextHarmonyCommand).Value.ToString();
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
            bool success = false;
            if(HarmonyDataProvider.CurrentActivity != HarmonyDataProvider.PowerOffActivity)
            {
                try
                {
                    ActuateHarmonyCommand(function);
                    success = true;
                }
                catch (Exception e)
                {
                    Logging.Log(_actionLogPath, string.Format("System failed to actuate command '{0}'", function.Name));
                    contextParameters.Add("command", command);
                    context.Parameters = contextParameters;

                    contexts.Add(context);
                }
            }
            else
            {
                contextParameters.Add("command", command);
                context.Parameters = contextParameters;

                contexts.Add(context);
            }

            return contexts;
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

        private static bool ActuateHarmonyPowerOff(Dictionary<string, object> parameters)
        {
            string command = "";
            if (parameters.Any(e => e.Key == _powerOff))
            {
                command = parameters.Where(x => x.Key == _powerOff).FirstOrDefault().Value.ToString();
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
