using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.valgut.libs.bots.Wit.Models;
using System.Configuration;
using com.valgut.libs.bots.Wit;
using System.Collections.ObjectModel;
using JarvisAPI.DataProviders.Orvibo;
using JarvisAPI.Actions.WitActions;

namespace JarvisAPI.DataProviders.Wit
{
    public static class WitDataProvider
    {
        #region Fields
        private static bool _isInitialized = false;
        private static WitClient _witClient;
        private static string _conversationId;
        private static WitConversation<object> _witConversationClient;
        private static bool didMerge = false;
        private static bool didStop = false;
        private static ObservableCollection<ThreadContent> _ThreadContentCollection;
        private static ThreadContent _currentThreadContent;
        private static Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        private static string _witToken = configuration.AppSettings.Settings["wit_token"].Value;
        private static string _witLogPath = "wit";

        #endregion

        #region Properties
        public static bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public static ObservableCollection<ThreadContent> ThreadContentCollection
        {
            get { return _ThreadContentCollection; }
        }

        public static ThreadContent CurrentThreadContent
        {
            get { return _currentThreadContent; }
        }

        public static string ConversationId
        {
            get { return _conversationId; }            
        }
        #endregion 

        #region Initializer
        public static bool Initialize()
        {
            _currentThreadContent = new ThreadContent();
            return true;
        }

        #endregion

        #region Methods
        public static ThreadContent SendMessage(string conversationId, string message)
        {
            _currentThreadContent = new ThreadContent();
            _conversationId = conversationId;
            _currentThreadContent.ConversationId = conversationId;
            //Inititialization
            //_ThreadContentCollection = new ObservableCollection<ThreadContent>();


            //New wit client
            _witToken = "HJHUAWTSYWSTRCPJBYTJILWEZ7X4EY53";
            _witClient = new WitClient(_witToken);
            _witConversationClient = new WitConversation<object>(_witToken, _conversationId, null,
                doMerge, doSay, doAction, doStop);


            _conversationId = conversationId;
            //Clear threading            
            _currentThreadContent.ClearAll();            

            //Save Message
            _currentThreadContent.UserMessage = message;

            //Get Entities
            Message msg = _witClient.GetMessage(message, _conversationId);
            if(msg.entities.Count > 0 && msg.entities != null)
            {
                foreach (KeyValuePair<string, List<Entity>> entity in msg.entities)
                {
                    _currentThreadContent.Entities.Add(entity);
                }
            }
            

            //Send async message           
            Task<bool> t = _witConversationClient.SendMessageAsync(message);
            t.Wait();

            //_ThreadContentCollection.Add(_currentThreadContent);

            return _currentThreadContent;
        }

        private static bool InitializeDataProviders(string message)
        {
            int Initialized = 0;
            if(NestDataProvider.IsInitialized && HarmonyDataProvider.IsInitialized && OrviboDataProvider.isInitialized)
            {
                return true;
            }

            //Nest is not initialized and we are expecting a pin
            if (!NestDataProvider.IsInitialized && NestDataProvider.ExpectingNestPin)
            {
                NestDataProvider.FinishAuthenticateNest(message);
                if(NestDataProvider.IsInitialized)
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
                    Logging.Log(_witLogPath, "Harmony failed to initialied: " + e.Message);
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
                    Logging.Log(_witLogPath, "Error initialaizing OrviboDataProvider: " + e.Message);
                }
            }

            if(Initialized > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        #region Callbacks
        private static object doMerge(string conversationId, object context, object entities, double confidence)
        {
            didMerge = true;
            return context;
        }

        private static void doSay(string conversationId, object context, string msg, double confidence)
        {
            _currentThreadContent.AiMessage = msg;
        }

        private static object doAction(string conversationId, object context, string action, double confidence)
        {
            InitializeDataProviders(_currentThreadContent.AiMessage);
            _currentThreadContent.Action = action;
            object updateContext = context;
            if(Actions.WitActions.Actions.ActionDictionary.ContainsKey(action))
            {
                updateContext = Actions.WitActions.Actions.ActionDictionary[action].Invoke(_currentThreadContent.Entities);
                _currentThreadContent.Context = updateContext;
            }
            else
            {
                Logging.Log(_witLogPath, string.Format("System does not contain an action '{0}'", action));
            }
            
            return updateContext;
        }

        private static object doStop(string conversationId, object context)
        {
            didStop = true;
            //_ThreadContentCollection.Clear();
            return context;
        }

        #endregion

        #endregion
    }
}
