using DataProviders.JarvisAPI;
using JarvisAPI.Models.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace JarvisAPI.DataProviders
{
    public static class DomainDataProvider
    {
        private static XmlSerializer _serializer;
        private static string _domainProviderLogLocation = "domainProvider";
        private static FileInfo _outputFile;
        private static string _outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JarvisAI\\" + "\\Domain\\";
        private static string _outputFileName = "Domain.xml";

        public static void Save(Domain domain)
        {
            try
            {
                if (_serializer == null)
                {
                    _serializer = new XmlSerializer(typeof(Domain));
                }

                if (_outputFile == null)
                {
                    Directory.CreateDirectory(_outputDirectory);
                    _outputFile = new FileInfo(Path.Combine(_outputDirectory, _outputFileName));
                }
            }
            catch(Exception e)
            {
                Logging.Log(_domainProviderLogLocation, "Error creating serializer: " + e.Message + ": " + e.InnerException);

            }
            
            if(domain != null)
            {
                FileStream stream = null;
                try
                {
                    stream = _outputFile.Create();
                    _serializer.Serialize(stream, domain);
                }
                catch(Exception e)
                {
                    Logging.Log(_domainProviderLogLocation, "Error serializing domain object: " + e.Message);
                }
                finally
                {
                    if(stream != null)
                    {
                        stream.Close();
                    }
                }
            }
        }
        public static Domain Load()
        {
            FileInfo file = new FileInfo(Path.Combine(_outputDirectory, _outputFileName));
            Domain loadedDomain = new Domain();
            if (_serializer == null)
            {
                _serializer = new XmlSerializer(typeof(Domain));
            }

            if (file.Exists == true)
            {
                FileStream stream = null;

                try
                {
                    stream = file.OpenRead();
                    loadedDomain = _serializer.Deserialize(stream) as Domain;
                }
                catch(Exception e)
                {
                    Logging.Log(_domainProviderLogLocation, "Error loading the domain: " + e.Message);
                }
                finally
                {
                    if(stream != null)
                    {
                        stream.Close();
                    }
                }               
            }
            return loadedDomain;
        }
    }
}