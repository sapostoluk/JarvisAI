using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NestControl;

using DataProviders.Wit;
using System.Collections.ObjectModel;

namespace JarvisConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continueConvo = true;
            WitDataProvider wit = new WitDataProvider("test");
            wit.ThreadContentCollection.CollectionChanged += ThreadContentCollection_CollectionChanged;
            while (continueConvo)
            {
                string sndMsg = Console.ReadLine();                
                Console.WriteLine(wit.SendMessage(sndMsg));

                
            }
        }

        private static void ThreadContentCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //
        }
    }
}
