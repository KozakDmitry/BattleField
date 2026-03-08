using System.ComponentModel.Design;
using UnityEngine;


namespace Codebase.Infrostructure.Initialization
{
    public abstract class MonoInstaller : MonoBehaviour
    {
        protected ServiceContainer Container { get; set; }

        public void InstallBindings(ServiceContainer serviceContainer)
        {
            Container = serviceContainer;
            OnInstallBindings();
        }
        protected abstract void OnInstallBindings();
    }
}