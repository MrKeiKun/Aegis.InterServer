namespace Aegis.CrossCutting.Configuration.Contracts.Classes
{
    public class ConfigArea : ConfigIdentifier
    {
        public static implicit operator ConfigArea(string value)
        {
            return new ConfigArea {Name = value};
        }
    }
}