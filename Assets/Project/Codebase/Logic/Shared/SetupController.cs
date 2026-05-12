using Assets.Project.CodeBase.Infostructure.Services;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.CodeBase.Logic.Shared
{
    /// <summary>
    /// Местный инициализатор для окон InitializableWindow, настраивается на сцене.
    /// </summary>
    public class SetupController : MonoBehaviour
    {
        [SerializeField]
        private List<InitializableWindow> InitializableWindowList;


        private void Awake()
        {
            DI.Register<SetupController>(this, mode: RegisterMode.scene);
        }

        public async UniTask Initialize()
        {
            foreach (var item in InitializableWindowList)
            {
                await item.Initialize();
            }

            foreach (var item in InitializableWindowList)
            {
                await item.AfterInitialize();
            }
        }
    }
}
