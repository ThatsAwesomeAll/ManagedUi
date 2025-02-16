using ManagedUi.GridSystem;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

namespace ManagedUi.Selectables
{

[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
public class SelectionAnimation : MonoBehaviour, IGridElement
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
        SetUpSelectionMarker();
        if (images.Contains(_selectionMarker))
        {
            Debug.LogError("Selection Marker in Image List. Prohibited");
            images.Remove(_selectionMarker);
        }
        defaultColors.Clear();
        foreach (var image in images)
        {
            defaultColors.Add(image, image.color);
        }
    }
    private void SetUpSelectionMarker()
    {
        _selectionMarker.rectTransform.anchorMax = Vector2.one;
        _selectionMarker.rectTransform.anchorMin = Vector2.zero;
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
    public int VerticalLayoutGrowth() => 1;
    public int HorizontalLayoutGrowth() => 1;
    public bool IgnoreLayout() => true;
}
}