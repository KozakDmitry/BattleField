using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Window : MonoBehaviour
{
    public string Name;
    public bool isCloseIfOpenOther = false;
    public bool isNeedBackButton = false;
    public bool isIgnoreBackButton = false;
    public Window[] RequiredWindowsOnTop;
    public UnityEvent onInit;
    public UnityEvent<int> onOpen;
    public UnityEvent onClose;
    public bool IsOpen => gameObject.activeSelf;
    WindowsManager _wm;
    public virtual void Initialize(WindowsManager wm)
    {
        _wm = wm;
    }

    public virtual void OpenThisWindow()
    {
        _wm.ShowWindow(Name).Forget();
    }

    public virtual void CloseThisWindow()
    {
        _wm.CloseWindow(Name).Forget();
    }

    public virtual void Open(int Parametr = -1)
    {
        if (!IsOpen) onOpen?.Invoke(Parametr);
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        if (IsOpen) onClose?.Invoke();
        gameObject.SetActive(false);
    }
}
