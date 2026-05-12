using UnityEngine;
using UnityEngine.UI;

public class LevelSelectContent : MonoBehaviour
{
    [SerializeField] private RectTransform Content;
    [SerializeField] private RectTransform[] ContentChild;

    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();

        if (rect != null)
        {
            float rectX = rect.rect.width;

            if (ContentChild == null) return;

            Content.sizeDelta = new Vector2(rectX * ContentChild.Length, Content.sizeDelta.y);

            for (int i = 0; i < ContentChild.Length; i++)
            {
                if (ContentChild[i] != null)
                {
                    ContentChild[i].sizeDelta = new Vector2(rectX, ContentChild[i].sizeDelta.y);
                }
            }
        }
    }
}
