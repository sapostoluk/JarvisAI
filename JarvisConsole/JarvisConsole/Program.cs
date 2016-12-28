using System;
using JarvisConsole.DataProviders.Wit;
using JarvisConsole.DataProviders;

namespace JarvisConsole
{
    class Program
    {
        static void Main(string[] args)
        {            
            NestDataProvider.Initialize();
            HarmonyDataProvider.Initialize();
            bool continueConvo = true;
            WitDataProvider wit = new WitDataProvider("test");
            Console.WriteLine("Talk to jarvis");
            while (continueConvo)
            {
                string sndMsg = Console.ReadLine();                
                Console.WriteLine("Jarvis: " + wit.SendMessage(sndMsg));
                
            }
        }
    }
}
