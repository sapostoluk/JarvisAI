using ApiAiSDK;
using ApiAiSDK.Model;
using JarvisAPI.DataProviders.Orvibo;
using JarvisAPI.Actions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using JarvisAPI.Actions.ApiAiActions;

namespace JarvisAPI.DataProviders.APIAI
{
    public static class APIAIDataProvider
    {
        private static bool _isInitialized = false;
        private static string _accessToken = "63a28bd84bea4d3493af8850752ce6ba";
        private static AIConfiguration _config;
        private static ApiAi _apiAi;
        private static ThreadContent _currentThreadContent;
        private static string _apiaiLogLocation = "apiai";

        public static bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public static void Initialize()
        {
            _config = new AIConfiguration(_accessToken, SupportedLanguage.English);
            _apiAi = new ApiAi(_config);
            _isInitialized = true;
        }

        public static ThreadContent SendMessage(string conversationId,string userId, string requestLocation, string message)
        {
            //TODO lookup user name from userId to send to Jarvis as a context. (So Jarvis can make personal responses).
            _currentThreadContent = new ThreadContent();
            AIResponse response = null;
            if (_isInitialized)
            {
                //Include location in initial AIrequest
                List<AIContext> initContexts = new List<AIContext>();
                AIContext locContext = new AIContext();
                locContext.Name = "Location";
                Dictionary<string, string> initContextParameters = new Dictionary<string, string>();
                initContextParameters.Add("inputLocation", requestLocation);
                locContext.Parameters = initContextParameters;
                initContexts.Add(locContext);
                List<Entity> initEntities = new List<Entity>();
                RequestExtras initExtras = new RequestExtras(initContexts, initEntities);                

                //Send request to apiai
                AIRequest request = new AIRequest(message, initExtras);

                Logging.Log(_apiaiLogLocation, "Sent request");

                response = _apiAi.TextRequest(request);
                _currentThreadContent.Action = response.Result.Action;
                _currentThreadContent.AiMessage = response.Result.Fulfillment.Speech;
                _currentThreadContent.UserMessage = message;
                _currentThreadContent.ConversationId = conversationId;
                //_currentThreadContent.Context = response.Result.Contexts;
                //thread.Entities = response.Result.             

            }
            //invoke actions
            if (response.Result.Action != null)
            {
                InitializeDataProviders(_currentThreadContent.AiMessage);
                if (ApiAiActions.ActionDictionary.ContainsKey(response.Result.Action))
                {
                    //Invoke action
                    List<AIContext> contexts = ApiAiActions.ActionDictionary[response.Result.Action].Invoke(response.Result.Parameters);
                    if (contexts.Count > 0)
                    {
                        List<Entity> entities = new List<Entity>();
                        RequestExtras extras = new RequestExtras(contexts, entities);

                        AIResponse returnResponse = _apiAi.TextRequest(new AIRequest("return", extras));
                        _currentThreadContent.ReturnAiMessage = returnResponse.Result.Fulfillment.Speech;
                    }

                }
            }
            return _currentThreadContent;
        }

        private static bool InitializeDataProviders(string message)
        {
            int Initialized = 0;
            if (NestDataProvider.IsInitialized && HarmonyDataProvider.IsInitialized && OrviboDataProvider.isInitialized)
            {
                return true;
            }

            //Nest is not initialized and we are expecting a pin
            if (!NestDataProvider.IsInitialized && NestDataProvider.ExpectingNestPin)
            {
                NestDataProvider.FinishAuthenticateNest(message);
                if (NestDataProvider.IsInitialized)
                {
                    NestDataProvider.ExpectingNestPin = false;
                }
                else
                {
                    NestDataProvider.ExpectingNestPin = false;
                    Initialized++;
                }
            }

            //Nest is not initialized. Not expecting a pin yet
            else if (!NestDataProvider.IsInitialized && !NestDataProvider.ExpectingNestPin)
            {
                Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                string authorizationUrl = string.Format("https://home.nest.com/login/oauth2?client_id={0}&state={1}",
                    configuration.AppSettings.Settings["nest_client-id"].Value, "dummy-random-value-for-anti-csfr");

                NestDataProvider.ExpectingNestPin = true;
                Initialized++;

            }

            else if (!NestDataProvider.IsInitialized)
            {
                try
                {
                    HarmonyDataProvider.Initialize();
                    if (!HarmonyDataProvider.IsInitialized)
                    {
                        Initialized++;
                    }
                }
                catch (Exception e)
                {
                    Logging.Log("APIAI", "Harmony failed to initialied: " + e.Message);
                }
            }

            else if (!OrviboDataProvider.isInitialized)
            {
                try
                {
                    OrviboDataProvider.Initialize();
                    if (!HarmonyDataProvider.IsInitialized)
                    {
                        //thread = WitDataProvider.SendMessage(conversationId, message);
                        Initialized++;
                    }
                }
                catch (Exception e)
                {
                    //thread.AiMessage = e.Message;
                    Logging.Log("apiai", "Error initialaizing OrviboDataProvider: " + e.Message);
                }
            }

            if (Initialized > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}