using com.valgut.libs.bots.Wit.Models;
using JarvisConsole.DataProviders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisConsole.Actions
{
    public static partial class ApiAiActions
    {        
        #region Nest Activities Methods
        private static object NestSetTemperature(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            object returnContext = null;
            string directionValue = "";
            bool directionPresent = false;
            string unitValue = "";
            bool unitPresent = false;
            string numberValue = "";
            bool numberPresent = false;
            
            if(entities.Any(e => e.Key == _contextDirection))
            {
                directionValue = entities.Where(e => e.Key == _contextDirection).FirstOrDefault().Value.FirstOrDefault().value.ToString();
            }
            if(entities.Any(e => e.Key == _contextUnit))
            {
                unitValue = entities.Where(e => e.Key == _contextUnit).FirstOrDefault().Value.FirstOrDefault().value.ToString();
            }
            if(entities.Any(e => e.Key == _contextNumber))
            {
                numberValue = entities.Where(e => e.Key == _contextNumber).FirstOrDefault().Value.FirstOrDefault().value.ToString();
            }

            //set flags
            if (!string.IsNullOrWhiteSpace(directionValue))
                directionPresent = true;

            if (!string.IsNullOrWhiteSpace(unitValue))
                unitPresent = true;

            if (!string.IsNullOrWhiteSpace(numberValue))
                numberPresent = true;

            //decisions
            if(directionPresent && numberPresent)
            {
                returnContext = new { Direction = directionValue, number = numberValue };
                NestSetItem(NestDataProvider.NestItem.TargetTemperature, numberValue);
            }
            else if(!directionPresent && numberPresent)
            {
                returnContext = new { number = numberValue, missingDirection = "" };
                NestSetItem(NestDataProvider.NestItem.TargetTemperature, numberValue);
            }
            else if(directionPresent && !numberPresent)
            {
                returnContext = new { Direction = directionValue, missingNumber = "" };
                int tempInterval; //= Convert.ToInt32(configuration.AppSettings.Settings["temperature_interval"].Value);
                int.TryParse(configuration.AppSettings.Settings["temperature_interval"].Value, out tempInterval);
                
                //Get current target temp and add to interval. Set new value
                int currentTemp;
                int.TryParse(NestGetItem(NestDataProvider.NestItem.TargetTemperature), out currentTemp);
                int newTarget = 0;
                if(directionValue == "down")
                {
                    newTarget = currentTemp - tempInterval;
                }
                else if(directionValue == "up")
                {
                    newTarget = currentTemp + tempInterval;
                }
                NestSetItem(NestDataProvider.NestItem.TargetTemperature, newTarget.ToString());
            }
            else if(!directionPresent && !numberPresent)
            {
                returnContext = new { missingNumber = "", missingValue = "" };
            }

            return returnContext;
        }

        private static object NestCheckStatus(ObservableCollection<KeyValuePair<string, List<Entity>>> entities)
        {
            string ambientTemp = NestGetItem(NestDataProvider.NestItem.AmbientTemperature);
            string targetTemp = NestGetItem(NestDataProvider.NestItem.TargetTemperature);
            return new { expectedAmbientTemp = ambientTemp, expectedTargetTemp = targetTemp };
        }

        #endregion

        #region private methods
        private static bool NestSetItem(NestDataProvider.NestItem item, string payload)
        {
            bool success = false;
            Console.WriteLine("-- System is attempting to set the nest item '{0}' to '{1}' --", item.ToString(), payload);
            if(!NestDataProvider.IsInitialized)
            {
                NestDataProvider.Initialize();
            }
            try
            {
                NestDataProvider.SetNestItem(item, payload);
                if (NestDataProvider.GetNestItem(item) == payload)
                {
                    success = true;
                    Console.WriteLine("-- System has successfully set nest item '{0}' to '{1}' --", item.ToString(), payload);
                }
                else
                    Console.WriteLine("-- Something went wrong while attempting setting nest item '{0}' to '{1}' --", item.ToString(), payload);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return success;
        }

        private static string NestGetItem(NestDataProvider.NestItem item)
        {
            if (!NestDataProvider.IsInitialized)
            {
                NestDataProvider.Initialize();
            }
            Console.WriteLine("-- System is attempting to get the nest item '{0}' --", item.ToString());
            string returnItem = "";
            try
            {
                Console.WriteLine("-- System is successfully aquired the nest item '{0}' --", item.ToString());
                returnItem = NestDataProvider.GetNestItem(item);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return returnItem;
        }

        #endregion
    }
}
