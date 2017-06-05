
using JarvisAPI.DataProviders;
using HarmonyHub;
using System;
using System.Collections.Generic;
using System.Linq;
using ApiAiSDK.Model;
using JarvisAPI.Models.Domain;
using JarvisAPI.Models.Globals;
using JarvisAPI.Models.Domain.Matrix;

namespace JarvisAPI.Actions.ApiAiActions
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
            //Check parameters for Stereo or Television entities
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

        //private static List<AIContext> HarmonyVolume(Dictionary<string, object> parameters)
        //{
        //    string directionValue = "";
        //    object returnContext = null;
        //    List<AIContext> contexts = new List<AIContext>();
        //    AIContext context = new AIContext();
        //    context.Name = "HarmonyVolumeReturn";
        //    Dictionary<string, string> contextParameters = new Dictionary<string, string>();

        //    if (parameters.Any(e => e.Key == _contextDirection))
        //    {
        //        directionValue = parameters.Where(x => x.Key == _contextDirection).FirstOrDefault().Value.ToString();
        //    }

        //    if (!HarmonyDataProvider.IsInitialized)
        //    {
        //        HarmonyDataProvider.Initialize();
        //    }
        //    Function function = null;

        //    switch (directionValue)
        //    {
        //        case "up":
        //            {
        //                IEnumerable<ControlGroup> controlGroups = null;
        //                if (HarmonyDataProvider.CurrentActivity != null)
        //                {
        //                    controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == "Volume");
        //                }                       
        //                ControlGroup control = controlGroups.FirstOrDefault();
        //                if (control.Functions.Any(e => e.Name == _contextVolumeUp))
        //                {
        //                    function = control.Functions.Where(x => x.Name == _contextVolumeUp).FirstOrDefault();
        //                }
        //            }
        //            break;

        //        case "down":
        //            {
        //                IEnumerable<ControlGroup> controlGroups = null;
        //                if(HarmonyDataProvider.CurrentActivity != null)
        //                {
        //                    controlGroups = HarmonyDataProvider.CurrentActivity.ControlGroups.Where(e => e.Name == "Volume");
        //                }
        //                ControlGroup control = controlGroups.FirstOrDefault();
        //                if (control.Functions.Any(e => e.Name == _contextVolumeDown))
        //                {
        //                    function = control.Functions.Where(x => x.Name == _contextVolumeDown).FirstOrDefault();
        //                }
        //            }
        //            break;
        //    }
        //    bool success = false;

        //    //Change volume correct number of times
        //    int volInterval;
        //    try
        //    {
        //        int.TryParse(configuration.AppSettings.Settings["volume_interval"].Value, out volInterval);
        //        for (int i = 0; i < volInterval; i++)
        //        {
        //            ActuateHarmonyCommand(function);
        //        }
        //        success = true;
        //    }
        //    catch(Exception e)
        //    {
        //        Logging.Log(_actionLogPath, "Harmony volume action failed: " + e.Message);
        //        success = false;
        //    }
        //    if (!success)
        //    {
        //        contextParameters.Add("command", "Volume");
        //        context.Parameters = contextParameters;

        //        contexts.Add(context);
        //    }

        //    return contexts;
        //}

        private static List<AIContext> HarmonyVolume(Dictionary<string, object> parameters)
        {
            Logging.Log(_actionLogPath, string.Format("Beginning HarmonyVolume()"));
            string directionValue = "";
            string room = "";
            List<AIContext> contexts = new List<AIContext>();
            AIContext context = new AIContext();
            context.Name = "HarmonyVolumeReturn";
            Dictionary<string, string> contextParameters = new Dictionary<string, string>();

            if (parameters.Any(e => e.Key == _contextDirection))
            {
                directionValue = parameters.Where(x => x.Key == _contextDirection).FirstOrDefault().Value.ToString();
            }
            room = ParseLocation(parameters);

            Logging.Log(_actionLogPath, "Rooms is: " + room);

            if (!HarmonyDataProvider.IsInitialized)
            {
                HarmonyDataProvider.Initialize();
            }
            Function function = null;
            HarmonyDevice volumeDevice = null;
            Device harmonyVolDevice = null;

            
            Room rm = Globals.Domain.RoomInDomain(room).FirstOrDefault();
            Logging.Log(_actionLogPath, string.Format("Found room '{0}' in domain", rm.RoomName));
            if (rm != null)
            {
                if (rm.CurrentHarmonyActivity != null && rm.CurrentHarmonyActivity.VolumeControlDevice != null)
                {
                    volumeDevice = rm.CurrentHarmonyActivity.VolumeControlDevice;
                    Logging.Log(_actionLogPath, "Volume device: " + volumeDevice.DeviceName + " CurrentActivity: " + rm.CurrentHarmonyActivity.ActivityName);
                }
                else
                {
                    Logging.Log(_actionLogPath, string.Format("Room '{0}' does not have a volume control device", room));
                }
            }
            
            harmonyVolDevice = HarmonyDataProvider.DeviceLookup(volumeDevice.DeviceName).FirstOrDefault();
            Logging.Log(_actionLogPath, string.Format("harmonyVolDevice: " + harmonyVolDevice.Label));
            
            ControlGroup ctrGrp = null;
            Logging.Log(_actionLogPath, string.Format("Finding control group for direction '{0}'", directionValue));
            switch (directionValue)
            {
                case "Up":
                    {
                        ctrGrp = harmonyVolDevice.ControlGroups.Where(e => e.Name == "Volume").FirstOrDefault();                      
                        if(ctrGrp != null)
                        {
                            Logging.Log(_actionLogPath, "Control group is: " + ctrGrp.Name);
                            if (ctrGrp.Functions.Any(e => e.Name == _contextVolumeUp))
                            {
                                function = ctrGrp.Functions.Where(x => x.Name == _contextVolumeUp).FirstOrDefault();
                                Logging.Log(_actionLogPath, "Function is: " + function.Name);
                            }
                        }
                        
                        
                    }
                    break;

                case "Down":
                    {
                        ctrGrp = harmonyVolDevice.ControlGroups.Where(e => e.Name == "Volume").FirstOrDefault();
                        if (ctrGrp != null)
                        {
                            if (ctrGrp.Functions.Any(e => e.Name == _contextVolumeDown))
                            {
                                function = ctrGrp.Functions.Where(x => x.Name == _contextVolumeDown).FirstOrDefault();
                            }
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
            catch (Exception e)
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
            Logging.Log(_actionLogPath, "HarmonyCheckStatus()");
            string room = "";

            room = ParseLocation(parameters);

            string activity = Globals.Domain.RoomInDomain(room).FirstOrDefault().CurrentHarmonyActivity.ActivityName;

            Logging.Log(_actionLogPath, string.Format("Location '{0}'", room));
            List<AIContext> contexts = new List<AIContext>();
            AIContext context = new AIContext();
            context.Name = "HarmonyCheckStatusReturn";
            Dictionary<string, string> contextParameters = new Dictionary<string, string>();
            if (activity != null && room != null)
            {
                contextParameters.Add("activity", activity);
                contextParameters.Add("location", room);
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

        private static List<AIContext> HarmonyRouteActivity(Dictionary<string, object> parameters)
        {
            Logging.Log(_actionLogPath, "Beginning Action: 'HarmonyRouteActivity'");
            string stereoActivity = "";
            string televisionActivity = "";
            string room = "";
            List<AIContext> contexts = new List<AIContext>();
            AIContext context = new AIContext();
            context.Name = "HarmonyControlReturn";
            Dictionary<string, string> contextParameters = new Dictionary<string, string>();

            object returnContext = null;
            

            
            //Check parameters for Stereo or Television entities
            if (parameters.Any(e => e.Key == _contextStereo))
            {
                stereoActivity = parameters.Where(x => x.Key == _contextStereo).FirstOrDefault().Value.ToString();
            }
            if (parameters.Any(e => e.Key == _contextTelevision))
            {
                televisionActivity = parameters.Where(x => x.Key == _contextTelevision).FirstOrDefault().Value.ToString();
            }
            room = ParseLocation(parameters);
            Logging.Log(_actionLogPath, "Parsed APIAI parameters");

            //Set return
            //ActuateActivity

            bool success = false;

            Logging.Log(_actionLogPath, "Calling start method");
            //Stereo present / Television present
            if (!string.IsNullOrWhiteSpace(stereoActivity) && !string.IsNullOrWhiteSpace(televisionActivity))
            {
                returnContext = new { Stereo = stereoActivity, Television = televisionActivity };
                string activity = "";
                switch (televisionActivity)
                {
                    case "Play Wii": success = RouteHarmonyActivity(_playWii_Music, room); activity = _playWii_Music; break;

                    case "Play Xbox One": success = RouteHarmonyActivity(_xboxOne_Music, room); activity = _xboxOne_Music; break;

                    case "Play PS4": success = RouteHarmonyActivity(_ps4_Music, room); activity = _ps4_Music; break;

                    case "Watch TV": success = RouteHarmonyActivity(_watchTv_Music, room); activity = _watchTv_Music; break;
                }
                if (!success)
                {
                    contextParameters.Add("activity", activity);
                    context.Parameters = contextParameters;

                    contexts.Add(context);
                }
            }

            //Stereo present / Television missing
            else if (!string.IsNullOrWhiteSpace(stereoActivity) && string.IsNullOrWhiteSpace(televisionActivity))
            {

                //Actuate stereo activity
                success = RouteHarmonyActivity(stereoActivity, room);
                if (!success)
                {
                    contextParameters.Add("activity", stereoActivity);
                    context.Parameters = contextParameters;

                    contexts.Add(context);
                }
            }
            //Stereo missing / television present
            else if (string.IsNullOrWhiteSpace(stereoActivity) && !string.IsNullOrWhiteSpace(televisionActivity))
            {
                success = RouteHarmonyActivity(televisionActivity, room);
                if (!success)
                {
                    contextParameters.Add("activity", televisionActivity);
                    context.Parameters = contextParameters;

                    contexts.Add(context);
                }
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
                Logging.Log(_actionLogPath, string.Format("Attempting to actuate command '{0}' for device with id '{1}'", func.Name, func.Action.DeviceId));
                HarmonyDataProvider.SendCommand(func.Name, func.Action.DeviceId);
            }
            else
            {
                Logging.Log(_actionLogPath, "Harmony failed cannot send command");
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

        private static bool RouteHarmonyActivity(string activityName, string roomName)
        {
            Logging.Log(_actionLogPath, string.Format("Beginning : 'RouteHarmonyActivity' method for activity: {0} and room {1}", activityName, roomName));
            if (!HarmonyDataProvider.IsInitialized)
            {
                HarmonyDataProvider.Initialize();
            }

            //TODO should I load this domain each time??
            Room rm = new Room();
            HarmonyActivity activity = new HarmonyActivity();
            bool controlDeviceInUse = false;
            Logging.Log(_actionLogPath, string.Format("Looking up Room: '{0}'", roomName));
            if (Globals.Domain.Rooms.Any(e => e.RoomName == roomName))
            {               
                rm = Globals.Domain.Rooms.FirstOrDefault(e => e.RoomName == roomName);
                Logging.Log(_actionLogPath, string.Format("Found room: '{0}' in domain", roomName));
            }
            
            if (rm != null)
            {
                //PowerOff
                if (activityName == _powerOff && rm.CurrentHarmonyActivity != null)
                {
                    Logging.Log(_actionLogPath, string.Format("Activity is 'PowerOff' activity"));
                    foreach(HarmonyDeviceSetupItem deviceItem in rm.CurrentHarmonyActivity.DeviceSetupList)
                    {
                        if(deviceItem.HarmonyDevice != null)
                        {
                            Logging.Log(_actionLogPath, string.Format("Device '{0}' is not null", deviceItem.HarmonyDevice.DeviceName));
                            HarmonyDataProvider.PowerOffDevice(deviceItem.HarmonyDevice);
                            Logging.Log(_actionLogPath, string.Format("Finished powering off device '{0}'", deviceItem.HarmonyDevice.DeviceName));
                        }
                        else
                        {
                            Logging.Log(_actionLogPath, string.Format("Device '{0}' IS null", deviceItem.HarmonyDevice.DeviceName));
                        }                      
                    }
                }
                //Not a power off
                else
                {
                    if (rm.HarmonyActivities.Any(e => e.ActivityName == activityName))
                    {
                        rm.CurrentHarmonyActivity = rm.HarmonyActivities.FirstOrDefault(e => e.ActivityName == activityName);
                        Logging.Log(_actionLogPath, string.Format("Found activity: {0} in room: {1}", rm.CurrentHarmonyActivity.ActivityName, rm.RoomName));
                    }
                    //Check to see if the control device is currently being used. If so, warn.
                    Logging.Log(_actionLogPath, string.Format("Found '{0}' rooms in the domain", Globals.Domain.Rooms.Count));
                    foreach (Room room in Globals.Domain.Rooms)
                    {
                        if (room.CurrentHarmonyActivity.ControlDevice == activity.ControlDevice)
                        {
                            controlDeviceInUse = true;
                            Logging.Log(_actionLogPath, "Control device '" + room.CurrentHarmonyActivity.ControlDevice + "' is in use");
                        }
                    }

                    //TODO if true warn user

                    //Turn on the devices in the room
                    Logging.Log(_actionLogPath, string.Format("Found '{0}' devices for activity", rm.CurrentHarmonyActivity.DeviceSetupList.Count()));

                    ////Find all unused devices
                    //foreach (HarmonyDevice turnOffDevice in rm.HarmonyDevices)
                    //{
                    //    if (rm.CurrentHarmonyActivity.DeviceSetupList.Any(e => e.HarmonyDevice.DeviceName == turnOffDevice.DeviceName))
                    //    {
                    //        //Turn off unused devices
                    //        HarmonyDataProvider.PowerOffDevice(turnOffDevice);
                    //    }
                    //}

                    foreach (HarmonyDeviceSetupItem deviceItem in rm.CurrentHarmonyActivity.DeviceSetupList)
                    {
                        Logging.Log(_actionLogPath, "Actuating device: " + deviceItem.HarmonyDevice.DeviceName);
                        if (activityName != "PowerOff")
                        {                                                      
                            HarmonyDataProvider.PowerOnDevice(deviceItem.HarmonyDevice);
                            //Change the input for each device
                            if (deviceItem.HarmonyDevice is Matrix)
                            {
                                Logging.Log(_actionLogPath, string.Format("Matrix found in room. Attempting to Route."));
                                /**************************************************************************************************
                                 * 
                                 * 
                                //This is the part where matrix routing will go. This will be done after a matrix switch can be tested
                                 *
                                 * 
                                 *****************************************************************************************************/
                                Matrix matrix = new Matrix();
                                matrix = deviceItem.HarmonyDevice as Matrix;

                                //Check if control device is in inputs
                                if (matrix.Inputs.Any(e => e.InputDevice == activity.ControlDevice))
                                {
                                    Input input = matrix.Inputs.FirstOrDefault(e => e.InputDevice == activity.ControlDevice);
                                    if (matrix.Outputs.Any(x => x.OutputDevice == activity.OutputDevice))
                                    {
                                        Output output = matrix.Outputs.FirstOrDefault(e => e.OutputDevice == activity.OutputDevice);
                                        //Matrix def contains input and output devices.

                                        //Set input
                                        if (input != null && output != null)
                                        {
                                            HarmonyDataProvider.SendCommand("input" + input.InputNumber.ToString(),
                                                HarmonyDataProvider.DeviceLookup(matrix.DeviceName).FirstOrDefault().Id);

                                            //Set output
                                            HarmonyDataProvider.SendCommand("output" + output.OutputNumber.ToString(),
                                                HarmonyDataProvider.DeviceLookup(matrix.DeviceName).FirstOrDefault().Id);
                                        }

                                    }
                                }
                            }
                            else
                            {
                                Logging.Log(_actionLogPath, string.Format("Attempting to change input for device: '{0}' to '{1}'", deviceItem.HarmonyDevice.DeviceName, deviceItem.Input));
                                //Switch input for non Matrix
                                HarmonyDataProvider.SendCommand("Input" + deviceItem.Input,
                                    HarmonyDataProvider.DeviceLookup(deviceItem.HarmonyDevice.DeviceName).FirstOrDefault().Id);
                            }

                        }
                        else
                        {
                            HarmonyDataProvider.PowerOffDevice(deviceItem.HarmonyDevice);
                        }
                    }
                                   
                }

                //Route device through the matrix
            }
            //TODO define success criteria
            return true;          
        }

        private static string ParseLocation(Dictionary<string, object> parameters)
        {
            string room = "";
            //Get location dictated by user
            string homeLocation = string.Empty;
            if (parameters.Any(e => e.Key == _contextHomeLocation))
            {
                homeLocation = parameters.FirstOrDefault(e => e.Key == _contextHomeLocation).Value.ToString();
            }
            //get location dictated by client
            string inputLocation = string.Empty;
            if (parameters.Any(e => e.Key == _contextInputLocation))
            {
                inputLocation = parameters.FirstOrDefault(e => e.Key == _contextInputLocation).Value.ToString();
            }
            Logging.Log(_actionLogPath, "Parsed APIAI parameters");
            //If voice client location exists use that else use other
            if (homeLocation != string.Empty)
            {
                room = homeLocation;
            }
            else
            {
                room = inputLocation;
            }

            return room;
        }

        #endregion
    }
}
