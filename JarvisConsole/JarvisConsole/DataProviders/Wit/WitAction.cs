using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProviders.Wit
{
    public class WitAction
    {
        #region Field
        private string _action;

        private ObservableCollection<Entity> _entityCollection;

        #endregion

        #region Properties
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

        public ObservableCollection<Entity> EntityCollection
        {
            get { return _entityCollection; }
            set
            {
                if(value != _entityCollection)
                {
                    _entityCollection = value;
                }
            }
        }

        #endregion

        #region Constructor
        public WitAction()
        {
            EntityCollection = new ObservableCollection<Entity>();
        }

        #endregion
    }
}
