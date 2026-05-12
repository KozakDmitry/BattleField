using Assets.Project.CodeBase.Extentions;
using Assets.Project.CodeBase.Infostructure.Services;
using Assets.Project.CodeBase.Infostructure.Services.SceneService;
using UnityEngine;
using UnityEngine.UI;


public class SceneButton : MonoBehaviour
{
    public SceneNames.Name Scene;
    Button button;
    private ISceneService _sceneService;
    private void Awake()
    {
        _sceneService = DI.ResolveSync<ISceneService>();
        button.onClick.AddListener(GoScene);
    }

    public void GoScene()
    {
        TimeManager.SetPause(true, GetType());
        _sceneService.LoadScene(SceneNames.GetSceneName(Scene));
    }


}
