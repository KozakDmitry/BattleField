using Assets.Project.CodeBase.Infostructure.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Project.CodeBase.Infostructure
{
    /// <summary>
    /// Стартовый скрипт, с него начинает загрузка(точнее с GameRunner, который вызывает уже этот)
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        private Game _game;
        private async void Awake()
        {
            _game = new Game();
            DontDestroyOnLoad(this);
            await _game._stateMachine.Enter<BootstrapState, string>(SceneManager.GetActiveScene().name);
        }
    }
}