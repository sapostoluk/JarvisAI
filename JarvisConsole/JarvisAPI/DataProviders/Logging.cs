using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace JarvisAPI
{
    public static class Logging
    {
        public static void Log(string location, string lines)
        {
            string fileName = location + ".txt";
            string timeStamp = DateTime.Now.ToString();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JarvisAI\\" + "\\logs\\";
            string newLog = timeStamp + ": " + lines;

            Directory.CreateDirectory(path);
            StreamWriter file = new StreamWriter(path + fileName, true);
            file.WriteLine(newLog);

            file.Close();
        }
    }
}