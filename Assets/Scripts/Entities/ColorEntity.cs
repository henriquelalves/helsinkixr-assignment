using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class ColorEntity : MonoBehaviour
{
    public ColorsData.ColorType CurrentColorType => _currentColorType;
    
    [SerializeField] ColorsData.ColorType _initialColorType;
    private ColorsData.ColorType _currentColorType;
    private SpriteRenderer _spriteRenderer;
    public void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetColorType(_initialColorType);
    }

    public void SetColorType(ColorsData.ColorType colorType)
    {
        _currentColorType = colorType;
        _spriteRenderer.color = Globals.ColorsData.GetColor(colorType);
    }
    
    #if UNITY_EDITOR
    private void Update()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }
        if (_initialColorType != _currentColorType)
        {
            SetColorType(_initialColorType);   
        }
    }
#endif
}
