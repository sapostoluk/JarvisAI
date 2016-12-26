using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProviders.Wit
{
    public class ThreadContent
    {
        #region Fields
        private string _conversationId;

        private bool _sentByAi;

        private string _aiMessage;

        private string _userMessage;

        private WitAction _witAction;

        private ObservableCollection<WitContext> _contextList;

        private ObservableCollection<Entity> _entities;

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

        public WitAction WitAction
        {
            get { return _witAction; }
            set
            {
                if(value != _witAction)
                {
                    _witAction = value;
                }
            }
        }

        public ObservableCollection<WitContext> ContextList
        {
            get { return _contextList; }
            set
            {
                if(value != _contextList)
                {
                    _contextList = value;
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

        public bool SentByAi
        {
            get { return _sentByAi; }
            set
            {
                if(_sentByAi != value)
                {
                    _sentByAi = value;
                }
            }
        }

        public ObservableCollection<Entity> Entities
        {
            get { return _entities; }
            set
            {
                if(value != _entities)
                {
                    _entities = value;
                }
            }
        }

        #endregion

        #region Constructor
        public ThreadContent()
        {
            Entities = new ObservableCollection<Entity>();
            ContextList = new ObservableCollection<WitContext>();
            WitAction = new Wit.WitAction();
        } 


        #endregion
    }
}
