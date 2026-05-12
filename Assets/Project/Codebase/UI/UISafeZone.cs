using UnityEngine;

public class UISafeZone : MonoBehaviour
{
    [SerializeField] [Range(0f,1f)] private float multYDown;

    public RectTransform Calculate()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 resolution = new (Screen.width, Screen.height);

        rect.anchorMin = Screen.safeArea.position / resolution;
        rect.anchorMax = Screen.safeArea.max / resolution;

        rect.anchorMin = new Vector2 (rect.anchorMin.x, rect.anchorMin.y * multYDown);

        return rect;
    }
}