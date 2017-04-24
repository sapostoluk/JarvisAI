using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using JarvisAPI.DataProviders.Orvibo;


namespace JarvisAPI.Actions.WitActions
{
    public static partial class Actions
    {
        private static object OrviboControl(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string productValue = "";
            string numberValue = "";
            string onOffValue = "";
            object returnContext = null;
            if (entities.Any(e => e.Key == _contextProduct))
            {
                productValue = entities.FirstOrDefault(e => e.Key == _contextProduct).Value.FirstOrDefault().value.ToString();
            }
            if (entities.Any(e => e.Key == _contextNumber))
            {
                numberValue = entities.FirstOrDefault(e => e.Key == _contextNumber).Value.FirstOrDefault().value.ToString();
            }
            if (entities.Any(e => e.Key == _contextOnOff))
            {
                onOffValue = entities.FirstOrDefault(e => e.Key == _contextOnOff).Value.FirstOrDefault().value.ToString();
            }

            if (!OrviboDataProvider.isInitialized)
            {
                OrviboDataProvider.Initialize();
            }

            OrviboDevice device = OrviboDataProvider.GetDevice("OrviboDevice" + numberValue);
            if(device != null)
            {
                switch (onOffValue)
                {
                    case "on":
                        {
                            OrviboDataProvider.OnCommand(device.Name);
                        }
                        break;

                    case "off":
                        {
                            OrviboDataProvider.OffCommand(device.Name);
                        }
                        break;
                }

            }
            
            returnContext = new { number = numberValue, Product = productValue, on_off = onOffValue };
            return returnContext;
        }
    }
}