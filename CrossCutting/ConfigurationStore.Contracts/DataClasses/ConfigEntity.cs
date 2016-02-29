namespace Aegis.CrossCutting.ConfigurationStore.Contracts.DataClasses
{
    public class ConfigEntity
    {
        public ConfigEntity(string area, string key, string value, string type)
        {
            Value = value;
            Type = type;
            Key = key;
            Area = area;
        }

        public string Type { get; private set; }
        public string Value { get; private set; }
        public string Key { get; private set; }
        public string Area { get; private set; }
    }
}