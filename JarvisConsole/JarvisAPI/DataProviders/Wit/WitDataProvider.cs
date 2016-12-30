using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.valgut.libs.bots.Wit.Models;
using System.Configuration;
using com.valgut.libs.bots.Wit;
using System.Collections.ObjectModel;
using JarvisConsole.Actions;


namespace JarvisConsole.DataProviders.Wit
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
            _currentThreadContent.Action = action;
            object updateContext = context;
            if(Actions.Actions.ActionDictionary.ContainsKey(action))
            {
                updateContext = Actions.Actions.ActionDictionary[action].Invoke(_currentThreadContent.Entities);
                _currentThreadContent.Context = updateContext;
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
