using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.valgut.libs.bots.Wit.Models;
using System.Configuration;
using com.valgut.libs.bots.Wit;
using System.Collections.ObjectModel;


namespace DataProviders.Wit
{
    public class WitDataProvider
    {
        #region Fields
        private WitClient _witClient;
        private string _conversationId;
        private WitConversation<object> _witConversationClient;
        private string _witToken = ConfigurationManager.AppSettings["wit_token"];
        private bool didMerge = false;
        private bool didStop = false;
        private ObservableCollection<ThreadContent> _ThreadContentCollection;
        private ThreadContent _currentThreadContent;

        

        #endregion

        #region Properties
        public ObservableCollection<ThreadContent> ThreadContentCollection
        {
            get { return _ThreadContentCollection; }
        }

        public ThreadContent CurrentThreadContent
        {
            get { return _currentThreadContent; }
        }

        public string ConversationId
        {
            get { return _conversationId; }            
        }
        #endregion 

        #region Constructors
        public WitDataProvider(string conversationId)
        {
            //Inititialization
            _ThreadContentCollection = new ObservableCollection<ThreadContent>();
            _currentThreadContent = new ThreadContent();
            _conversationId = conversationId;

            //New wit client
            _witClient = new WitClient(_witToken);
            _witConversationClient = new WitConversation<object>(_witToken, _conversationId, null,
                doMerge, doSay, doAction, doStop);
            
        }

        #endregion

        #region Methods
        public string SendMessage(string message)
        {
            //Clear threading            
            _currentThreadContent.ClearAll();            

            //Save Message
            _currentThreadContent.UserMessage = message;

            //Get Entities
            Message msg = _witClient.GetMessage(message, _conversationId);
            foreach (KeyValuePair<string, List<Entity>> entity in msg.entities)
            {
                _currentThreadContent.Entities.Add(entity);
            }

            //Send async message           
            Task<bool> t = _witConversationClient.SendMessageAsync(message);
            t.Wait();

            _ThreadContentCollection.Add(_currentThreadContent);

            return _currentThreadContent.AiMessage;
        }

        #region Callbacks
        private object doMerge(string conversationId, object context, object entities, double confidence)
        {
            didMerge = true;
            return context;
        }

        private void doSay(string conversationId, object context, string msg, double confidence)
        {
            _currentThreadContent.AiMessage = msg;
        }

        private object doAction(string conversationId, object context, string action, double confidence)
        {
            _currentThreadContent.Action = action;
            object updateContext = context;
            if(Actions.ActionDictionary.ContainsKey(action))
            {
                updateContext = Actions.ActionDictionary[action].Invoke(_currentThreadContent.Entities);
            }
            
            return updateContext;
        }

        private object doStop(string conversationId, object context)
        {
            didStop = true;
            _ThreadContentCollection.Clear();
            return context;
        }

        #endregion

        #endregion
    }
}
