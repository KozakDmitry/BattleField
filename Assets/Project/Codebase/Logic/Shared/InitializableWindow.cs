using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.CodeBase.Logic.Shared
{

    /// <summary>
    /// Всё, что улетает в SetupController, должно иметь этот скрипт
    /// </summary>
    public abstract class InitializableWindow : MonoBehaviour
    {
#pragma warning disable CS1998
        public virtual async UniTask Initialize()
        {

        }

        public virtual async UniTask AfterInitialize()
        {

        }
#pragma warning restore CS1998

    }
}
