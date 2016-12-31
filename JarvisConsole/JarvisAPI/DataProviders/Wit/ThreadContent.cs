using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisConsole.DataProviders.Wit
{
    public class ThreadContent
    {
        #region Fields
        private string _conversationId = "";

        private string _aiMessage = "";

        private string _userMessage = "";

        private string _action = "";

        private object _context ="";

        private ObservableCollection<KeyValuePair<string, List<Entity>>> _entities;

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

        public ObservableCollection<KeyValuePair<string, List<Entity>>> Entities
        {
            get { return _entities; }
            set
            {
                if(!_entities.Equals(value))
                {
                    _entities = value;
                }
            }
        }

        #endregion

        #region Constructor
        public ThreadContent()
        {
            _entities = new ObservableCollection<KeyValuePair<string, List<Entity>>>();
            //Entities = new ObservableCollection<KeyValuePair<string, List<Entity>>>();
        }


        #endregion

        #region Methods
        public void ClearAll()
        {
            Entities.Clear();           
        }

        #endregion
    }
}
