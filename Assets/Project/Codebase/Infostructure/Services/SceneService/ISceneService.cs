using Cysharp.Threading.Tasks;
using System;
using static Assets.Project.CodeBase.Infostructure.Services.SceneService.SceneService;

namespace Assets.Project.CodeBase.Infostructure.Services.SceneService
{
    public interface ISceneService 
    {
        event OnLoad OnSceneLoaded;

        bool IsLoading();
        UniTask LoadFirstScene(string scene);
        UniTask LoadScene(string nextScene, Action<string> callback = null);
    }
}