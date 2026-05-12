using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIBackground : MonoBehaviour
{
    public bool IsRegisterForAll = false;
    [Space(10)]
    [SerializeField] private Image image;
    private Material material;
    [Space(10)]
    [SerializeField] private int index = 0;
    [Space(10)]
    [SerializeField] private Vector2 sizeCurrent;
    [SerializeField] private float ratio;
    [Space(10)]
    private int indexCurrent = -1;
    public List<Preset> presets = new List<Preset>();

    private void Awake()
    {
        if (material == null && image != null && image.material != null)
        {
            material = new Material(image.material);
            image.material = material;
        }

        if (IsRegisterForAll) DI.Register(this, RegisterMode.scene);
    }

    private void OnEnable()
    {
        if (IsValidIndex(index))
        {
            Preset preset = presets[index];
            ApplyPreset(preset);
            indexCurrent = index;
        }
    }

    private void Size()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null) 
        {
            sizeCurrent = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

            float ratioYX = sizeCurrent.y / sizeCurrent.x;
            ratio = ratioYX;
        }
    }

    private bool IsValidIndex(int idx)
    {
        return idx >= 0 && idx < presets.Count && presets[idx] != null;
    }

    void ApplyPreset(Preset preset)
    {
        Size();

        if (material == null || preset == null) return;

        material.SetFloat("_ratio", ratio);

        if (ratio < 1)
        {
            float radius = material.GetFloat("_radius");
            float softness = material.GetFloat("_softness");
            float tex_tile = material.GetFloat("_tex_tile");

            material.SetFloat("_radius", radius / ratio);
            material.SetFloat("_softness", softness / ratio);
            material.SetFloat("_tex_tile", tex_tile / ratio);
        }

        material.SetColor("_color_01", preset._color_01);
        material.SetColor("_color_02", preset._color_02);
        material.SetTexture("_tex", preset._tex);
    }

    public void ApplyPresetButton(int index)
    {
        if (!IsValidIndex(index)) return;

        if (index == indexCurrent)
        {
            //Debug.Log($"Пресет {index} уже активен.");
            return;
        }

        Preset preset = presets[index];
        //Debug.Log($"Применяем пресет #{index} '{preset.index}' (был активен #{indexCurrent})");

        ApplyPreset(preset);

        indexCurrent = index;
    }
}

[System.Serializable]
public class Preset
{
    [Space(10)]
    public string index;
    public Color _color_01;
    public Color _color_02;
    public Texture2D _tex;
}
