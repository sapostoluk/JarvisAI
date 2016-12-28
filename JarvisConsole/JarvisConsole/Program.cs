using System;
using JarvisConsole.DataProviders.Wit;
using JarvisConsole.DataProviders;
using FirebaseSharp.Portable;

namespace JarvisConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            NestDataProvider.Initialize();
            HarmonyDataProvider.Initialize();
            bool continueConvo = true;
            //WitDataProvider.Initialize();
            Console.WriteLine("Talk to jarvis");
            while (continueConvo)
            {
                string sndMsg = Console.ReadLine();
                Console.WriteLine("Jarvis: " + WitDataProvider.SendMessage("test", sndMsg).AiMessage);

            }
            //bool go = true;
            //while (go)
            //{

            //}
        }
    }
}
