using com.valgut.libs.bots.Wit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisConsole.Actions
{
    public static partial class Actions
    {
        public static Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>> ActionDictionary => new Dictionary<string, Func<ObservableCollection<KeyValuePair<string, List<Entity>>>, object>>
        {
            //Harmony activites
            {"HarmonyStartActivity", HarmonyStartActivity },
            {"HarmonyVolume", HarmonyVolume },         

            //Nest activities
            {"NestTempDown", NestTempDown },
            {"NestTempUp", NestTempUp },
            {"NestSetTemp", NestSetTemp },
            {"NestRunFan", NestRunFan }
        };
    }
}
