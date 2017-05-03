using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JarvisAPI.Models.Providers
{
    public class SettingsDBProvider
    {
        string globalSettingsFileName = "GlobalSettings.json";
        string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JarvisAI\\";

        /// <summary>
        /// Writes serialized json settings to a flat settings file
        /// </summary>
        /// <param name="serializedSettings"></param>
        /// <param name="userId"></param>
        public void WriteUserSettings(string serializedSettings, string userId)
        {
            string fileName = userId + "_Settings" + ".json";
            string timeStamp = DateTime.Now.ToString();
            try
            {
                //string newLog = timeStamp + ": " + lines;          

                Directory.CreateDirectory(path);
                File.WriteAllText(path + fileName, serializedSettings);
            }
            catch(Exception ex)
            {
                Logging.Log("Settings", "Error writing to settings file for user " + userId + ": " + ex.Message);
            }
            
        }

        public void WriteGlobalSettings(string serializedSettings)
        {
            string fileName = globalSettingsFileName;
            string timeStamp = DateTime.Now.ToString();
            try
            {
                Directory.CreateDirectory(path);
                File.WriteAllText(path + fileName, serializedSettings);
            }
            catch(Exception ex)
            {
                Logging.Log("Settings", "Error writing to global settings file: " + ex.Message);
            }
        }

        /// <summary>
        /// Reads flat settings file associated with userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string ReadUserSettings(string userId)
        {
            string serialized = "";
            try
            {
                string fileName = userId + "_Settings" + ".json";
                serialized = File.ReadAllText(path + fileName);
            }
            catch(Exception ex)
            {
                Logging.Log("Settings", "Error reading settings file for " +  userId + ": " + ex.Message);
            
            }
            
            return serialized;
        }

        public string ReadGlobalSettings()
        {
            string serialized = "";
            try
            {
                string fileName = globalSettingsFileName;
                serialized = File.ReadAllText(path + fileName);
            }
            catch(Exception ex)
            {
                Logging.Log("Settings", "Error reading global settings file: " + ex.Message);
            }

            return serialized;
        }
    }
}