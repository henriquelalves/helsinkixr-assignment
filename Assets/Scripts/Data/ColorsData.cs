using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorsData", order = 1)]
public class ColorsData: ScriptableObject
{
    public enum ColorType
    {
        Green,
        Blue,
        Orange,
        Magenta
    }
    
    [Serializable]
    public struct ColorData
    {
        public ColorType ColorType;
        public Color Color;
    }
    
    [SerializeField] private List<ColorData> _colorsData;

    public Color GetColor(ColorType type)
    {
        var data = _colorsData.Find(d => d.ColorType == type);
        return data.Color;
    }

    public ColorType GetRandomColorType(ColorType? except = null)
    {
        ColorType[] values = (ColorType[])Enum.GetValues(typeof(ColorType));
        if (except != null)
        {
            values = values.Where(t => t != except).ToArray();
        }
        
        var rndIdx = Random.Range(0, values.Length - 1);
        return values[rndIdx];
    }
}
