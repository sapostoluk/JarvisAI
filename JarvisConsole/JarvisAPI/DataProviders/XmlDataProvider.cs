
using JarvisAPI.Models.Domain;
using JarvisAPI.Models.Globals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace JarvisAPI.DataProviders
{
    public static class XmlDataProvider
    {
        private static XmlSerializer _serializer;
        private static string _domainProviderLogLocation = "domainProvider";
        private static FileInfo _outputFile;
        private static string _outputDomainDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JarvisAI\\" + "\\Config\\";
        private static string _outputMatrixDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JarvisAI\\" + "\\Config\\";
        private static string _outputDomainFileName = "Domain.xml";
        private static string _outputMatrixConfigFileName = "Matrix.xml";

        public static void SaveDomain()
        {
            try
            {
                if (_serializer == null)
                {
                    _serializer = new XmlSerializer(typeof(Domain));
                }

                if (_outputFile == null)
                {
                    Directory.CreateDirectory(_outputDomainDirectory);
                    _outputFile = new FileInfo(Path.Combine(_outputDomainDirectory, _outputDomainFileName));
                }
            }
            catch(Exception e)
            {
                Logging.Log(_domainProviderLogLocation, "Error creating serializer: " + e.Message + ": " + e.InnerException);

            }
            
            if(Globals.Domain != null)
            {
                FileStream stream = null;
                try
                {
                    stream = _outputFile.Create();
                    _serializer.Serialize(stream, Globals.Domain);
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
        public static void LoadDomain()
        {
            FileInfo file = new FileInfo(Path.Combine(_outputDomainDirectory, _outputDomainFileName));
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
            Globals.Domain = loadedDomain;
        }
        public static void SaveMatrix()
        {
            try
            {
                if (_serializer == null)
                {
                    _serializer = new XmlSerializer(typeof(Domain));
                }

                if (_outputFile == null)
                {
                    Directory.CreateDirectory(_outputDomainDirectory);
                    _outputFile = new FileInfo(Path.Combine(_outputDomainDirectory, _outputDomainFileName));
                }
            }
            catch (Exception e)
            {
                Logging.Log(_domainProviderLogLocation, "Error creating serializer: " + e.Message + ": " + e.InnerException);

            }

            if (Globals.Domain != null)
            {
                FileStream stream = null;
                try
                {
                    stream = _outputFile.Create();
                    _serializer.Serialize(stream, Globals.Domain);
                }
                catch (Exception e)
                {
                    Logging.Log(_domainProviderLogLocation, "Error serializing domain object: " + e.Message);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
        }
        public static void LoadMatrix()
        {
            FileInfo file = new FileInfo(Path.Combine(_outputDomainDirectory, _outputDomainFileName));
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
                catch (Exception e)
                {
                    Logging.Log(_domainProviderLogLocation, "Error loading the domain: " + e.Message);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            Globals.Domain = loadedDomain;
        }
    }
}