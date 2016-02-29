using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Aegis.CrossCutting.Configuration.Contracts;
using Aegis.CrossCutting.Configuration.Contracts.Classes;
using Aegis.CrossCutting.Configuration.Contracts.Exceptions;
using Aegis.CrossCutting.ConfigurationStore.Contracts;
using log4net;
using ConfigurationException = System.Configuration.ConfigurationException;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace Aegis.CrossCutting.Configuration
{
    public class Configurator : IConfigurator
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private List<ConfigEntry> _entries;
        private readonly IConfigurationStore _configurationStore;

        public Configurator(IConfigurationStore configurationStore)
        {
            if (configurationStore == null)
            {
                throw new ArgumentNullException("configurationStore");
            }

            _configurationStore = configurationStore;
            _entries = new List<ConfigEntry>();
        }

        public T Get<T>(ConfigArea area, ConfigKey key)
        {
            if (area == null)
            {
                throw new ArgumentNullException("area");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var areaExist = AreaExist(area);
            if (!areaExist)
            {
                throw new AreaNotFoundException("Can't find area " + area.Name);
            }

            var keyExist = EntryExist(area, key);
            if (!keyExist)
            {
                var message = $"Can't find key {key.Name} in area {area.Name}";
                throw new KeyNotFoundException(message);
            }

            try
            {
                var entry = GetEntry(area, key);

                var typeToReturn = typeof (T);
                var typeOfValue = entry.Value.GetType();
                var typesAreEqual = typeToReturn == typeOfValue;
                if (!typesAreEqual)
                {
                    var message = $"Can't convert source type {typeOfValue} to requested type {typeToReturn}";
                    throw new InvalidTypeException(message);
                }

                return (T) (object) entry.Value;
            }
            catch (InvalidTypeException)
            {
                throw;
            }
            catch (Exception e)
            {
                var message = $"Can't get a value for {area.Name} in area {key.Name}";
                throw new ConfigurationErrorsException(message, e);
            }
        }

        public void Load()
        {
            try
            {
                Logger.Debug("Configuration Load");
                var entities = _configurationStore.Load();
                var entries = new List<ConfigEntry>();

                foreach (var entity in entities)
                {
                    var entry = new ConfigEntry(entity.Area, entity.Key, null);

                    try
                    {
                        entry.Value = Convert.ChangeType(entity.Value, Type.GetType(entity.Type));
                    }
                    catch (InvalidCastException e)
                    {
                        var message = $"Error converting key {entity.Key} in area {entity.Area} on Load";
                        throw new ConfigurationErrorsException(message, e);
                    }

                    entries.Add(entry);
                }
                _entries = entries;
            }
            catch (ConfigurationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException("Error on loading configuration values from store", e);
            }
        }

        public string AppPath(params string[] args)
        {
            var ea = new FileInfo(Assembly.GetEntryAssembly().Location);
            var l = new List<string> { ea.Directory.FullName };
            l.AddRange(args);

            var dir = Path.Combine(l.ToArray());
            return dir;
        }

        private bool AreaExist(ConfigArea area)
        {
            var areaExist = _entries.Any(e => e.Area == area.Name);
            return areaExist;
        }

        private ConfigEntry GetEntry(ConfigArea area, ConfigKey key)
        {
            var entry = _entries.Single(e => e.Area == area.Name && e.Key == key.Name);
            return entry;
        }

        private bool EntryExist(ConfigArea area, ConfigKey key)
        {
            var entryExist = _entries.Any(e => e.Area == area.Name && e.Key == key.Name);
            return entryExist;
        }
    }
}