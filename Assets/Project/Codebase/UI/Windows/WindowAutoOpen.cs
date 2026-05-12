using Assets.Project.CodeBase.Infostructure.Services;
using Assets.Project.CodeBase.Infostructure.Services.SceneService;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WindowAutoOpen : MonoBehaviour
{
    [SerializeField] Dictionary<object, int> m_WindowsToOpen = new Dictionary<object, int>();

    private bool m_IsSceneLoaded = false;
    private Coroutine m_InitWaiter;
    private Window m_ActiveWindow;
    private ISceneService _sceneService;


    public void Init()
    {     
        _sceneService = DI.ResolveSync<ISceneService>();
        DI.Register(this, RegisterMode.scene);
        m_InitWaiter = StartCoroutine(WaitSceneInit());
    }

    private IEnumerator WaitSceneInit()
    {
        yield return new WaitForSeconds(0.5f);

        while (_sceneService.IsLoading())
        {
            yield return null;
        }

        
        m_IsSceneLoaded = true;
        OpenWindow();
    }

    private void OpenWindow()
    {
        //if (m_WindowsToOpen.Count() == 0 || (Tutorial.Instance && Tutorial.Instance.IsTutorialActive) || m_ActiveWindow)
        if (m_WindowsToOpen.Count() == 0 || m_ActiveWindow)
        {
            return;
        }

        OrderWindows();

        object objectToOpen = m_WindowsToOpen.First().Key;
        m_WindowsToOpen.Remove(objectToOpen);

        if (objectToOpen.GetType() == typeof(WindowButton))
        {
            WindowButton button = objectToOpen as WindowButton;
            button.click();
            DIContainer.Get().Resolve<WindowsManager>((WM) =>
            {
                WindowsManager _winManager = WM as WindowsManager;

                Window window = _winManager.Windows.FirstOrDefault(a => a.Name == button.WindowName);

                if (window)
                    SubscriptToWindow(window);
            });
        }
        else if (objectToOpen.GetType() == typeof(Window))
        {
            Window window = objectToOpen as Window;
            window.OpenThisWindow();
            SubscriptToWindow(window);
        }
    }

    private void SubscriptToWindow(Window _window)
    {
        m_ActiveWindow = _window;
        _window.onClose.AddListener(WindowClosed);
    }

    private void OrderWindows()
    {
        m_WindowsToOpen = m_WindowsToOpen.OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
    }

    public void AddWindowToOpen(object _objectToOpen, int _priority)
    {
        if (m_WindowsToOpen.ContainsKey(_objectToOpen))
            return;

        m_WindowsToOpen.Add(_objectToOpen, _priority);

        if (m_InitWaiter != null)
            StopCoroutine(m_InitWaiter);

        m_InitWaiter = StartCoroutine(WaitSceneInit());
    }

    private void WindowClosed()
    {
        if (!m_IsSceneLoaded || m_ActiveWindow == null)
            return;
        
        m_ActiveWindow.onClose.RemoveListener(WindowClosed);
        m_ActiveWindow = null;
        StopAllCoroutines();
        OpenWindow();
    }
}
