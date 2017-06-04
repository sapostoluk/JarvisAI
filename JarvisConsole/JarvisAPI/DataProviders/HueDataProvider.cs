using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Q42.HueApi.Interfaces;
using Q42.HueApi;
using Q42.HueApi.Models.Bridge;

namespace JarvisAPI.DataProviders
{
    public static class HueDataProvider
    {
        #region Public Methods
        public static async void Initialize()
        {
            //Locate bridge
            IBridgeLocator locator = new HttpBridgeLocator();
            IEnumerable<LocatedBridge> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

            //Registe bridge
            ILocalHueClient client = new LocalHueClient(bridgeIPs.FirstOrDefault().IpAddress);
            var appKey = await client.RegisterAsync("JarvisAI", "HueHub");

            client.Initialize(appKey);
        }

        #endregion Public Methods
    }
}