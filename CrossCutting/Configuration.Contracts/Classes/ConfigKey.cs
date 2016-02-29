namespace Aegis.CrossCutting.Configuration.Contracts.Classes
{
    public class ConfigKey : ConfigIdentifier
    {
        public static implicit operator ConfigKey(string value)
        {
            return new ConfigKey {Name = value};
        }
    }
}