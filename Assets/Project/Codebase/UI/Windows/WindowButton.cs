using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowButton : MonoBehaviour, IPointerClickHandler
{
    public Button button;
    [HideInInspector] public string WindowName;
    public bool isCloseAll = false;
    public WindowTypeButton type = WindowTypeButton.Open;
    public int Parametr = -1;
    [HideInInspector]
    public bool IsClick = true;
    bool isInit = false;

    public enum WindowTypeButton
    {
        Open,
        close,
        Toggle
    }


    WindowsManager windowsManager;

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (isInit) return;
        if (button)
        {
                if (IsClick) button.onClick.AddListener(click);
                else button.onClick.AddListener(click);
        }

        //DI.Resolve<DataManager>((d) =>
        //{
        //    DI.Resolve<WindowsManager>((WM) =>
        //    {
        //        windowsManager = WM;
        //        isInit = true;
        //    });

        //});
        
    }

    [ContextMenu("Click!")]
    public void click()
    {
        if (!isInit) Initialize();

        if (type == WindowTypeButton.Open) windowsManager.ShowWindow (WindowName, Parametr).Forget();
        else if (type == WindowTypeButton.close)
        {
            windowsManager.CloseWindow(WindowName).Forget();
        }else if (type == WindowTypeButton.Toggle)
        {
            windowsManager.ToggleWindow(WindowName).Forget();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null) return;
        if (IsClick) click();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button != null) return;
        if (!IsClick) click();
    }
}
