using ApiAiSDK.Model;
using com.valgut.libs.bots.Wit.Models;
using JarvisAPI.DataProviders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAPI.Actions.ApiAiActions
{
    public static partial class ApiAiActions
    {        
        #region Nest Activities Methods
        private static List<AIContext> NestSetTemperature(Dictionary<string, object> parameters)
        {
            //object returnContext = null;
            string directionValue = "";
            bool directionPresent = false;
            string unitValue = "";
            bool unitPresent = false;
            string temperatureValue = "";
            bool numberPresent = false;

            List<AIContext> contexts = new List<AIContext>();
            AIContext context = new AIContext();
            context.Name = "NestSetTemperatureReturn";
            Dictionary<string, string> contextParameters = new Dictionary<string, string>();

            if (parameters.Any(e => e.Key == _contextDirection))
            {
                directionValue = parameters.Where(e => e.Key == _contextDirection).FirstOrDefault().Value.ToString();
            }
            if(parameters.Any(e => e.Key == _contextUnit))
            {
                unitValue = parameters.Where(e => e.Key == _contextUnit).FirstOrDefault().Value.ToString();
            }
            if(parameters.Any(e => e.Key == _contextTemperature))
            {
                temperatureValue = parameters.Where(e => e.Key == _contextTemperature).FirstOrDefault().Value.ToString();
            }

            //set flags
            if (!string.IsNullOrWhiteSpace(directionValue))
                directionPresent = true;

            if (!string.IsNullOrWhiteSpace(unitValue))
                unitPresent = true;

            if (!string.IsNullOrWhiteSpace(temperatureValue))
                numberPresent = true;

            //decisions
            bool success = false;
            if(directionPresent && numberPresent)
            {
                //returnContext = new { Direction = directionValue, number = numberValue };
                success = NestSetItem(NestDataProvider.NestItem.TargetTemperature, temperatureValue);
            }
            else if(!directionPresent && numberPresent)
            {
                //returnContext = new { number = numberValue, missingDirection = "" };
                success = NestSetItem(NestDataProvider.NestItem.TargetTemperature, temperatureValue);
            }
            else if(directionPresent && !numberPresent)
            {
                //returnContext = new { Direction = directionValue, missingNumber = "" };
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
                success = NestSetItem(NestDataProvider.NestItem.TargetTemperature, newTarget.ToString());
            }
            if (!success)
            {
                contextParameters.Add("direction", directionValue);
                contextParameters.Add("temperature", temperatureValue);
                context.Parameters = contextParameters;

                contexts.Add(context);
            }

            return contexts;
        }

        private static List<AIContext> NestCheckStatus(Dictionary<string, object> parameters)
        {
            string ambientTemp = NestGetItem(NestDataProvider.NestItem.AmbientTemperature);
            string targetTemp = NestGetItem(NestDataProvider.NestItem.TargetTemperature);

            List<AIContext> contexts = new List<AIContext>();
            AIContext context = new AIContext();           
            context.Name = "NestCheckStatusReturn";
            Dictionary<string, string> contextParameters = new Dictionary<string, string>();

            if(ambientTemp != null && targetTemp != null)
            {
                contextParameters.Add("ambientTemp", ambientTemp);
                contextParameters.Add("targetTemp", targetTemp);
                context.Parameters = contextParameters;
                contexts.Add(context);
            }
            //else if(ambientTemp == null || targetTemp ==null)
            //{
            //    contextParameters.Add("success", "false");
            //    context.Parameters = contextParameters;
            //    contexts.Add(context);
            //}

            return contexts;
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
