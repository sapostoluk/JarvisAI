using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using JarvisConsole.DataProviders;
using System.Configuration;
using JarvisAPI.DataProviders.Orvibo;
using JarvisAPI.DataProviders.APIAI;
using JarvisConsole.DataProviders.APIAI;

namespace JarvisAPI.Controllers
{
    public class JarvisController : ApiController
    {                      
        // GET: api/Jarvis
        public string Get(string conversationId, string message)
        {            
             ThreadContent thread = new ThreadContent();

            ////Nest is not initialized and we are expecting a pin
            //if (!NestDataProvider.IsInitialized && NestDataProvider.ExpectingNestPin)
            //{
            //    NestDataProvider.FinishAuthenticateNest(message);
            //    if (NestDataProvider.IsInitialized)
            //    {
            //        NestDataProvider.ExpectingNestPin = false;
            //        thread.AiMessage = "Nest authentication successful";                   
            //    }
            //    else
            //    {
            //        NestDataProvider.ExpectingNestPin = true;
            //        thread.AiMessage = "Nest authentication uncessful, please try again";
            //    }                   
            //}
            ////Nest is not initialized. Not expecting a pin yet
            //else if(!NestDataProvider.IsInitialized && !NestDataProvider.ExpectingNestPin)
            //{
            //    Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            //    string authorizationUrl = string.Format("https://home.nest.com/login/oauth2?client_id={0}&state={1}",
            //        configuration.AppSettings.Settings["nest_client-id"].Value, "dummy-random-value-for-anti-csfr");

            //    NestDataProvider.ExpectingNestPin = true;
            //    thread.AiMessage = string.Format("Follow this link to connect your nest thermostat: {0} then reply with the pin", authorizationUrl);

                
            //}
            //else if(!HarmonyDataProvider.IsInitialized)
            //{
            //    try
            //    {
            //        HarmonyDataProvider.Initialize();
            //        if(HarmonyDataProvider.IsInitialized)
            //        {
            //            thread = WitDataProvider.SendMessage(conversationId, message);
            //        }                    
            //    }
            //    catch(Exception e)
            //    {
            //        thread.AiMessage = e.Message;
            //    }
                
            //}
            //else if(!OrviboDataProvider.isInitialized)
            //{
            //    try
            //    {
            //        OrviboDataProvider.Initialize();
            //        if(HarmonyDataProvider.IsInitialized)
            //        {
            //            thread = WitDataProvider.SendMessage(conversationId, message);
            //        }
            //    }
            //    catch(Exception e)
            //    {
            //        thread.AiMessage = e.Message;
            //        Logging.Log("general", "Error initialaizing OrviboDataProvider: " + e.Message);
            //    }
            //}

            //Everything is good 
            //else
            //{
                try
                {
                    thread = APIAIDataProvider.SendMessage(conversationId, message);
                }  
                catch(Exception e)
                {
                    Logging.Log("general", "Error executing wit 'SendMessage': " + e.Message);
                }           
                             
            //}

            return JsonConvert.SerializeObject(thread);

        }

        // GET: api/Jarvis/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Jarvis
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Jarvis/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Jarvis/5
        public void Delete(int id)
        {
        }
    }
}
