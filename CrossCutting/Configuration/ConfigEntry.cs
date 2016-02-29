namespace Aegis.CrossCutting.Configuration
{
    internal class ConfigEntry
    {
        public ConfigEntry(string area, string key, object value)
        {
            Value = value;
            Key = key;
            Area = area;
        }

        public object Value { get; internal set; }
        public string Key { get; private set; }
        public string Area { get; private set; }
    }
}