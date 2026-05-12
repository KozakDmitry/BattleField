using System.Collections;
using UnityEngine;

namespace Assets.Project.CodeBase.Infostructure
{
    /// <summary>
    /// Для нормального запуска на сцену закидывать именно этот скрипт, он инициализирует Bootstrapper
    /// </summary>
    public class GameRunner : MonoBehaviour
    {
        public Bootstrapper bootstrapperPrefab;
        private void Awake()
        {
            Bootstrapper bootstrapper = FindFirstObjectByType<Bootstrapper>();
            if (bootstrapper == null)
            {
                Instantiate(bootstrapperPrefab); 
            }
        }
    }
}