
using ApiAiSDK.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisConsole.DataProviders.APIAI
{
    public class ThreadContent
    {
        #region Fields
        private string _conversationId = "";

        private string _aiMessage = "";

        private string _returnAiMessage = "";

        private string _userMessage = "";

        private string _action = "";

        private object _context ="";        

        #endregion

        #region Properties
        public string AiMessage
        {
            get { return _aiMessage; }
            set
            {
                if(value != _aiMessage)
                {
                    _aiMessage = value;
                }
            }
        }

        public string ReturnAiMessage
        {
            get { return _returnAiMessage; }
            set
            {
                if(value != _returnAiMessage)
                {
                    _returnAiMessage = value;
                }
            }
        }

        public string UserMessage
        {
            get { return _userMessage; }
            set
            {
                if(value != _userMessage)
                {
                    _userMessage = value;
                }
            }
        }

        public string ConversationId
        {
            get { return _conversationId; }
            set
            {
                if(_conversationId != value)
                {
                    _conversationId = value;
                }
            }
        }

        public string Action
        {
            get { return _action; }
            set
            {
                if(value != _action)
                {
                    _action = value;
                }
            }
        }

        public object Context
        {
            get { return _context; }
            set
            {
                if(value != _context)
                {
                    _context = value;
                }
            }
        }

        #endregion

        #region Constructor
        public ThreadContent()
        {
            //_entities = new ObservableCollection<KeyValuePair<string, List<Entity>>>();
            //Entities = new ObservableCollection<KeyValuePair<string, List<Entity>>>();
        }


        #endregion

        #region Methods
        public void ClearAll()
        {
            //Entities.Clear();           
        }

        #endregion
    }
}
