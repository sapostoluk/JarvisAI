using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using FirebaseSharp.Portable;

namespace JarvisConsole.DataProviders
{
    public static class NestDataProvider
    {
        public enum NestItem
        {
            TemperatureScale,
            TargetTemperature,
            HvacMode,
            AmbientTemperature,
            Humidity,
            HvacState,
            Label,
            Name,
            TimeToTarget             
        }

        #region Fields
        private static bool _isInitialized = false;

        private static bool _authenticated;
        private static string _accessToken;
        private static string _authorizationUrl;

        private static Firebase firebaseClient;

        private static Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

        private static string _device_id = configuration.AppSettings.Settings["nest_thermostat_id"].Value;
        private static string _initPath         = "devices/thermostats/" + _device_id + "/";
        private static string _pathTempScale           = _initPath + "temperature_scale";
        private static string _pathTargetTemperature   = _initPath + "target_temperature_f";
        private static string _pathMode                = _initPath + "hvac_mode";
        private static string _pathAmbientTemperature  = _initPath + "ambient_temperature_f";
        private static string _pathHumidity            = _initPath + "humidity";
        private static string _pathState               = _initPath + "hvac_state";
        private static string _pathLabel               = _initPath + "label";
        private static string _pathWhereName           = _initPath + "where_name";
        private static string _pathTimeToTarget        = _initPath + "time_to_target";

        #endregion

        #region Properties
        public static bool IsInitialized
        {
            get { return _isInitialized; }
        }
        public static bool Authenticated
        {
            get { return _authenticated; }
        }
        public static string AccessToken
        {
            get { return _accessToken; }
        }
        public static string AuthorizationUrl
        {
            get { return _authorizationUrl; }
            set
            {
                if (value != _authorizationUrl)
                {
                    _authorizationUrl = value;
                }
            }
        }
        public static Firebase FirebaseClient
        {
            get { return firebaseClient; }
            set
            {
                if(value != firebaseClient)
                {
                    firebaseClient = value;
                }
            }
        }
        
        #endregion

        #region Initializer
        public static bool Initialize()
        {            
            Console.WriteLine("Nest Initializing");

            _authorizationUrl = string.Format("https://home.nest.com/login/oauth2?client_id={0}&state={1}",
                configuration.AppSettings.Settings["nest_client-id"].Value, "dummy-random-value-for-anti-csfr");
            if (!string.IsNullOrEmpty(configuration.AppSettings.Settings["NestAccessToken"].Value))
            {
                _accessToken = configuration.AppSettings.Settings["NestAccessToken"].Value;
            }
            else
            {
                BeginAuthenticateNest();                
                configuration.Save();
            }            
            firebaseClient = new Firebase("https://developer-api.nest.com", _accessToken);

            if(!string.IsNullOrEmpty(_accessToken))
            {
                _isInitialized = true;
            }

            Console.WriteLine("Nest done initializing");
            return _isInitialized;
        }

        #endregion

        #region Public Methods

        public static bool FinishAuthenticateNest(string pin)
        {
            bool authenticated = false;
            //Enter credentials
            Console.WriteLine("Please enter the pin");

            HttpResponseMessage message = Get("", pin);
            configuration.AppSettings.Settings["NestAccessToken"].Value = _accessToken;
            if (!string.IsNullOrWhiteSpace(_accessToken))
            {
                Console.WriteLine("Nest authentication successful");
                authenticated = true;
            }
            else
            {
                Console.WriteLine("Failed to recieve access token");
            }
            return authenticated;
        }

        public static string GetNestItem(NestItem item)
        {
            string returnString = string.Empty;
            switch(item)
            {
                case NestItem.AmbientTemperature: {returnString = firebaseClient.Get(_pathAmbientTemperature).ToString(); } break;
                case NestItem.Humidity: { returnString = firebaseClient.Get(_pathHumidity).ToString(); } break;
                case NestItem.HvacMode: { returnString = firebaseClient.Get(_pathMode).ToString(); } break;
                case NestItem.HvacState: { returnString = firebaseClient.Get(_pathState).ToString(); } break;
                case NestItem.Label: { returnString = firebaseClient.Get(_pathLabel).ToString(); } break;
                case NestItem.Name: { returnString = firebaseClient.Get(_pathWhereName).ToString(); } break;
                case NestItem.TargetTemperature: { returnString = firebaseClient.Get(_pathTargetTemperature).ToString(); } break;
                case NestItem.TemperatureScale: { returnString = firebaseClient.Get(_pathTempScale).ToString(); } break;
                case NestItem.TimeToTarget: { returnString = firebaseClient.Get(_pathTimeToTarget).ToString(); } break;
            }

            return returnString;
        }

        public static async Task<string> GetNestItemAsync(NestItem item)
        {
            //Firebase firebaseClient = new Firebase("https://developer-api.nest.com", _accessToken);
            string returnString = string.Empty;
            switch (item)
            {
                case NestItem.AmbientTemperature: { returnString = await firebaseClient.GetAsync(_pathAmbientTemperature); } break;
                case NestItem.Humidity: { returnString = await firebaseClient.GetAsync(_pathHumidity); } break;
                case NestItem.HvacMode: { returnString = await firebaseClient.GetAsync(_pathMode); } break;
                case NestItem.HvacState: { returnString = await firebaseClient.GetAsync(_pathState); } break;
                case NestItem.Label: { returnString = await firebaseClient.GetAsync(_pathLabel); } break;
                case NestItem.Name: { returnString = await firebaseClient.GetAsync(_pathWhereName); } break;
                case NestItem.TargetTemperature: { returnString = await firebaseClient.GetAsync(_pathTargetTemperature); } break;
                case NestItem.TemperatureScale: { returnString = await firebaseClient.GetAsync(_pathTempScale); } break;
                case NestItem.TimeToTarget: { returnString = await firebaseClient.GetAsync(_pathTimeToTarget); } break;
            }

            return returnString;
        }

        public static void SetNestItem(NestItem item, string payload)
        {            
            switch (item)
            {
                case NestItem.HvacMode: {FirebaseClient.Put(_pathMode, payload); } break;
                case NestItem.TargetTemperature: {FirebaseClient.Put(_pathTargetTemperature, payload); } break;
            }
        }

        public static async void SetNestItemAsync(NestItem item, string payload)
        {
            //Firebase firebaseClient = new Firebase("https://developer-api.nest.com", _accessToken);
            switch (item)
            {
                case NestItem.HvacMode: {await firebaseClient.PutAsync(_pathMode, payload); } break;
                case NestItem.TargetTemperature: { await firebaseClient.PutAsync(_pathTargetTemperature, payload); } break;
            }
        }

        #endregion

        #region Private Methods
        private static void BeginAuthenticateNest()
        {            
            var authorizationUrl = string.Format("https://home.nest.com/login/oauth2?client_id={0}&state={1}", configuration.AppSettings.Settings["nest_client-id"].Value, "dummy-random-value-for-anti-csfr");
            using (var process = Process.Start(authorizationUrl))
            {
                Console.WriteLine("Awaiting response, please accept the confirmation to continue");
            }          
        }                    

        private static HttpResponseMessage Get(string state, string code)
        {
            if (!string.Equals("dummy-random-value-for-anti-csfr", state))
            {
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var token = GetAccessToken(code);
            _accessToken = token.Result;
            //Program.SubscribeToNestDeviceDataUpdates(accessToken);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Well done, you now have access")
            };

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");

            return response;
        }

        private static async Task<string> GetAccessToken(string authCode)
        {
            var url = string.Format("https://api.home.nest.com/oauth2/access_token?code={0}&client_id={1}&client_secret={2}&grant_type=authorization_code",
                authCode, configuration.AppSettings.Settings["nest_client-id"].Value,
                configuration.AppSettings.Settings["nest_client-secret"].Value);

            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.PostAsync(url, content: null).Result)
                {
                    var accessToken = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                    return (accessToken as dynamic).access_token;
                }
            }
        }

        #endregion
    }
}
