using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using Assets.Project.CodeBase.Logic.Shared;

public class WindowsManager : InitializableWindow
{
    public List<Window> Windows = new();
    List<Window> _WindowsOpened = new();

    public override async UniTask Initialize()
    {
        await base.Initialize();
        foreach (var item in Windows)
        {
            item.Initialize(this);
            item.onInit?.Invoke();
        }
        DI.Register(this, RegisterMode.scene);
    }
    public void AddAllWindow()
    {
        Window[] winds = FindObjectsByType<Window>(FindObjectsSortMode.None);
        foreach (var w in winds)
        {
            if (!Windows.Contains(w)) Windows.Add(w);
        }

    }

    public async UniTask ShowWindow(string name, int parametr = -1)
    {
        Window window = Windows.FirstOrDefault((w) => w.Name == name);

        if (window == null)
        {
            Debug.Log("Windows: action open. Window( " + name + " ) is not found");
            return;
        }

        await OpenWindow(window, parametr);
    }

    void OpenRequaireWindows(Window window)
    {
        /*if (Tutorial.Instance != null && Tutorial.Instance.IsTutorialActive)
            return;*/
        if (window.RequiredWindowsOnTop.Length > 0)
        {
            foreach (var item in window.RequiredWindowsOnTop)
            {
                item.Open(-1);
                item.transform.SetAsLastSibling();
            }
        }
    }

    void SetBackButton(Window window)
    {
        if (window.isIgnoreBackButton) return;

        DG.Tweening.DOVirtual.DelayedCall(Time.deltaTime, () =>
        {
            DIContainer.Get().Resolve<BackButton>((b) =>
            {
                BackButton bb = b as BackButton;
                if (window.isNeedBackButton)
                    bb.SetWindowToClose(window);
                else
                    bb.SetNormal();
            });
        });
    }
    async UniTask OpenWindow(Window window, int parametr = -1)
    {
        if (_WindowsOpened.Count > 0 && _WindowsOpened[^1].isCloseIfOpenOther)
        {
            await CloseWindow();
        }

        window.Open(parametr);
        if (window is IWindowAnimation wa) await wa.OpenAnimation();
        SetCurrentWindow(window);

        _WindowsOpened.Remove(window);
        _WindowsOpened.Add(window);
    }

    void SetCurrentWindow(Window win)
    {
        win.transform.SetAsLastSibling();
        OpenRequaireWindows(win);
        SetBackButton(win);
    }

    async UniTask CloseWindow(Window window)
    {
        if (window is IWindowAnimation wa) await wa.CloseAnimation();
        window.Close();
        _WindowsOpened.Remove(window);
        if (_WindowsOpened.Count > 0)
        {
            Window win = _WindowsOpened.Last();
            SetCurrentWindow(win);
        }
    }

    public async UniTask CloseWindow(string name = "")
    {
        Window window = name == "" ? _WindowsOpened.LastOrDefault() : Windows.FirstOrDefault((w) => w.Name == name);
        if (window == null)
        {
            Debug.Log("Windows: action hide. Window( " + name + " ) is not found");
            return;
        }

        await CloseWindow(window);
    }

    public async UniTask ToggleWindow(string name)
    {
        Window window = name == "" ? _WindowsOpened.Last() : Windows.FirstOrDefault((w) => w.Name == name);
        if (window == null)
        {
            Debug.Log("Windows: action hide. Window( " + name + " ) is not found");
            return;
        }

        if (!window.IsOpen)
        {
            OpenWindow(window).Forget();
        }

        else await CloseWindow(window);
    }

    public void ReopenAllWindow()
    {
        foreach (var item in _WindowsOpened)
        {
            item.onOpen?.Invoke(-1);
        }
    }
}
