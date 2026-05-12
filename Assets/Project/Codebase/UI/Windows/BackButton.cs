using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public GameObject Normal;
    public GameObject Back;
    WindowsManager _winManager;
    Window _window;
    void Start()
    {
        DI.Register(this, RegisterMode.scene);
        DI.Resolve<WindowsManager>((WM) =>_winManager = WM);

    }

    public void SetWindowToClose(Window window)
    {
        _window = window;
        Normal.SetActive(false);
        Back.SetActive(true);
    }

    public void SetNormal()
    {
        Normal.SetActive(true);
        Back.SetActive(false);
    }

    public void Click()
    {
        if (_window && _winManager)
        {
            _winManager.CloseWindow(_window.Name).Forget();
            _window = null;
            Normal.SetActive(true);
            Back.SetActive(false);
        }
    }
}
