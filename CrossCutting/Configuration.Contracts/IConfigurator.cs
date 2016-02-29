using Aegis.CrossCutting.Configuration.Contracts.Classes;

namespace Aegis.CrossCutting.Configuration.Contracts
{
    public interface IConfigurator
    {
        T Get<T>(ConfigArea area, ConfigKey key);
        void Load();
        string AppPath(params string[] filename);
    }
}