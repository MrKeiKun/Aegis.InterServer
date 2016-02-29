using System.Collections.Generic;
using Aegis.CrossCutting.ConfigurationStore.Contracts.DataClasses;

namespace Aegis.CrossCutting.ConfigurationStore.Contracts
{
    public interface IConfigurationStore
    {
        IEnumerable<ConfigEntity> Load();
    }
}