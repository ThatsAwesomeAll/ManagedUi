using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ManagedUi
{

[RequireComponent(typeof(Image))]
public class SelectionImage : MonoBehaviour
{
    public List<Image> images = new List<Image>();
    public bool enableNeeded = true;

    private Image _selectionMarker;
    private Dictionary<Image,Color> defaultColors = new Dictionary<Image,Color>();
    public void SetColor(Color color)
    {
        foreach (var image in images)
        {
            image.color = color;
        }
    }
    private void OnEnable()
    {
        _selectionMarker ??= GetComponent<Image>();
        if (images.Contains(_selectionMarker))
        {
            Debug.LogError("Selection Marker in Image List. Prohibited");
            images.Remove(_selectionMarker);
        }
        foreach (var image in images)
        {
            defaultColors.Add(image, image.color);
        }
    }
    public void LerpToDefault(float currentValue)
    {
        foreach (var image in images)
        {
            if (defaultColors.TryGetValue(image,out var defaultColor))
            {
                image.color = Color.Lerp(image.color, defaultColor, currentValue);
            }
        }
    }
}
}