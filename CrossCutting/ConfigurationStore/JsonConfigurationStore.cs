using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Aegis.CrossCutting.ConfigurationStore.Contracts;
using Aegis.CrossCutting.ConfigurationStore.Contracts.DataClasses;
using Aegis.CrossCutting.ConfigurationStore.Contracts.Exceptions;
using Newtonsoft.Json;

namespace Aegis.CrossCutting.ConfigurationStore
{
    public class JsonConfigurationStore : IConfigurationStore
    {
        private readonly string _pathToJsonFile;

        public JsonConfigurationStore(string pathToJsonFile)
        {
            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            if (directory != null) _pathToJsonFile = Path.Combine(directory, pathToJsonFile);
        }

        public IEnumerable<ConfigEntity> Load()
        {
            try
            {
                var fileExist = File.Exists(_pathToJsonFile);
                if (!fileExist)
                {
                    throw new ConfigurationFileNotException("Can't find the configuration file at" + _pathToJsonFile);
                }

                var jsonSerializer = new JsonSerializer();
                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(_pathToJsonFile);
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        reader = null;
                        var entities = jsonSerializer.Deserialize<IEnumerable<ConfigEntity>>(jsonReader);
                        return entities;
                    }
                }
                finally
                {
                    reader?.Dispose();
                }
            }
            catch (ConfigurationFileNotException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConfigurationStorageException("Error on loading configuration", e);
            }
        }
    }
}