using Assets.Project.Scripts.UI.LoadingScreen;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Project.CodeBase.Infostructure.Services.SceneService
{
    public class SceneLoader
    {
        public async UniTask<Scene> Load(string nextScene, Action onLoaded = null)
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
            {
                await UniTask.Yield();
            }

            onLoaded?.Invoke();
            return SceneManager.GetSceneByName(nextScene);
        }
      
        public async UniTask LoadBase(string nextScene, SceneTransition transition, Action<Scene> onLoaded = null)
        {
            TimeManager.ResetBlocks();
            float timer = ResetValues(transition);
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);
            waitNextScene.allowSceneActivation = false;
            float f = 0;
            while (waitNextScene.progress < 0.9f || f < 0.5f)
            {
                await UniTask.Delay(10);
                transition.SetProgress(f);
                f += 0.01f;
            }

            for (float k = 0.5f; k < 0.8f; k += 0.01f)
            {
                await UniTask.Delay(10);
                transition.SetProgress(f);
            }

            waitNextScene.allowSceneActivation = true;

            while (!waitNextScene.isDone)
            {
                await UniTask.Yield();
            }


            onLoaded?.Invoke(SceneManager.GetSceneByName(nextScene));


        }
        private static float ResetValues(SceneTransition slider)
        {
            float timer = 0f;
            slider.SetProgress(0);
            return timer;
        }


    }
}
