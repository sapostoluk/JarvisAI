using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Q42.HueApi.Interfaces;
using Q42.HueApi;
using Q42.HueApi.Models.Bridge;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.ColorConverters.Original;
using Q42.HueApi.ColorConverters.OriginalWithModel;
using Q42.HueApi.ColorConverters;
using System.Configuration;
using Q42.HueApi.Models.Groups;
using System.Threading.Tasks;

namespace JarvisAPI.DataProviders
{
    public static class HueDataProvider
    {
        #region Fields

        #endregion Fields
        private static Dictionary<string, string> _hueColors;
        private static ILocalHueClient _client;
        private static string _hudLogPath = "hue";
        private static string _appKey = "";
        private static Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

        #region Properties
        public static Dictionary<string, string> HueColors
        {
            get { return _hueColors; }
        }

        public static bool IsInitialized
        {
            get { return (_client != null); }
        }

        #endregion Properties

        #region Initializer      
        public static async void Initialize()
        {
            //Add colors
            _hueColors.Add("Black", "#000000");
            _hueColors.Add("White", "#FFFFFF");
            _hueColors.Add("Red", "#FF0000");
            _hueColors.Add("Lime", "#00FF00");
            _hueColors.Add("Blue", "#0000FF");
            _hueColors.Add("Yellow", "#FFFF00");
            _hueColors.Add("Cyan", "#00FFFF");
            _hueColors.Add("Magenta", "#FF00FF");
            _hueColors.Add("Maroon", "#800000");
            _hueColors.Add("Olive", "#808000");
            _hueColors.Add("Green", "#008000");
            _hueColors.Add("Purple", "#800080");
            _hueColors.Add("Teal", "#008080");
            _hueColors.Add("Navy", "#000080");

            //Locate bridge
            IBridgeLocator locator = new HttpBridgeLocator();
            IEnumerable<LocatedBridge> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

            //Registe bridge
            ILocalHueClient client = new LocalHueClient(bridgeIPs.FirstOrDefault().IpAddress);

            //Check if appkey exists
            _appKey = configuration.AppSettings.Settings["hue_app_key"].Value;
            if (_appKey == null)
            {
                _appKey = await client.RegisterAsync("JarvisAI", "HueHub");
                configuration.AppSettings.Settings["hue_app_key"].Value = _appKey;
            }           

            client.Initialize(_appKey);
        }

        #endregion Initializer

        #region Public Methods

        public async static Task<bool> LightsOn(List<string> lightNames)
        {
            bool success = true;
            LightCommand command = new LightCommand();
            command.On = true;
            command.TransitionTime = TimeSpan.FromSeconds(3);
            string hexColor = _hueColors.Where(e => e.Key == "Yellow").FirstOrDefault().Value;
            command.TurnOn().SetColor(new RGBColor(hexColor));
            command.Brightness = 100;

            HueResults result = await _client.SendCommandAsync(command, lightNames);
            if (result.Any(e => e.Success.Id == null))
            {
                success = false;
            }

            return success;
        }

        public async static Task<bool> LightsOn(List<string> lightNames, int brightness)
        {
            bool success = true;
            LightCommand command = new LightCommand();
            command.On = true;
            command.TransitionTime = TimeSpan.FromSeconds(3);
            string hexColor = _hueColors.Where(e => e.Key == "Yellow").FirstOrDefault().Value;
            command.TurnOn().SetColor(new RGBColor(hexColor));
            command.Brightness = (byte)brightness;

            HueResults result = await _client.SendCommandAsync(command, lightNames);
            if (result.Any(e => e.Success.Id == null))
            {
                success = false;
            }

            return success;
        }

        public async static Task<bool> LightsOn(List<string> lightNames, string color)
        {
            bool success = true;
            LightCommand command = new LightCommand();
            command.On = true;
            command.TransitionTime = TimeSpan.FromSeconds(3);
            command.TurnOn().SetColor(new RGBColor(color));
            command.Brightness = 100;

            HueResults result = await _client.SendCommandAsync(command, lightNames);
            if (result.Any(e => e.Success.Id == null))
            {
                success = false;
            }

            return success;
        }

        public async static Task<bool> LightsOn(List<string> lightNames, string color, int brightness)
        {
            bool success = true;
            LightCommand command = new LightCommand();
            command.On = true;
            command.TransitionTime = TimeSpan.FromSeconds(3);
            command.TurnOn().SetColor(new RGBColor(color));
            command.Brightness = (byte)brightness;

            HueResults result = await _client.SendCommandAsync(command, lightNames);
            if (result.Any(e => e.Success.Id == null))
            {
                success = false;
            }

            return success;
        }

        public async static Task<bool> LightsOff(List<string> lightNames)
        {
            bool success = true;
            LightCommand command = new LightCommand();
            command.On = false;
            command.TransitionTime = TimeSpan.FromSeconds(3);
            command.TurnOff();

            HueResults result = await _client.SendCommandAsync(command, lightNames);
            if (result.Any(e => e.Success.Id == null))
            {
                success = false;
            }

            return success;
        }

        public async static Task<bool> ApplyColorFilter(List<string> lightNames, string color)
        {
            bool success = true;
            LightCommand command = new LightCommand();
            command.SetColor(new RGBColor(color));

            HueResults result = await _client.SendCommandAsync(command, lightNames);
            if (result.Any(e => e.Success.Id == null))
            {
                success = false;
            }

            return success;
        }

        public async static Task<bool> ApplyBrightnessFilter(List<string> lightNames, int brightness)
        {
            bool success = true;
            LightCommand command = new LightCommand();
            command.Brightness = (byte)brightness;

            HueResults result = await _client.SendCommandAsync(command, lightNames);
            if (result.Any(e => e.Success.Id == null))
            {
                success = false;
            }

            return success;
        }

        public async static Task<bool> ApplyFilter(List<string> lightNames, int brightness, string color, int seconds)
        {
            bool success = true;
            LightCommand command = new LightCommand();
            command.Brightness = (byte)brightness;
            command.TurnOn().SetColor(new RGBColor(color));
            command.TransitionTime = TimeSpan.FromSeconds(seconds);
            command.On = true;

            HueResults result = await _client.SendCommandAsync(command, lightNames);
            if (result.Any(e => e.Success.Id == null))
            {
                success = false;
            }
            
            return success;
        }

        #endregion Public Methods
    }
}