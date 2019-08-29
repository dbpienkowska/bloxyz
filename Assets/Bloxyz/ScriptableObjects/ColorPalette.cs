using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
    public Color[] colors;
}
