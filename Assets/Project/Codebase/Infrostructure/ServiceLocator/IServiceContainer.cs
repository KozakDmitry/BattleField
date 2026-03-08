using System.Collections.Generic;

namespace Assets.Project.Codebase.Infrostructure.ServiceLocator
{
    public interface IServiceContainer
    {
        object GetValue();
        IEnumerable<object> GetValues();
    }
}
