using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ManagedUi
{
[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class SelectableParent : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    ISelectableManager _selectableManager;
    RectTransform _rectTransform;
    Color _animationImageDefaultColor = Color.clear;

    private bool _selected;
    Vector2Int _internalGridPosition = new Vector2Int(0, 0);
    bool _gridFixed = false;

    // Tween _currentScaleTween;
    // Tween _currentColorTween;

    public SelectionImage _animationImage;
    public float animationDuration = 0.1f;
    public float animationStrengthPercent = 2f;
    // public Ease animationEase = Ease.Default;


    public Vector2 anchoredPosition => _rectTransform.anchoredPosition;
    public Vector2 size => new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);

    public Vector2Int gridPosition
    {
        get => _internalGridPosition;
        set
        {
            if (!_gridFixed) _internalGridPosition = value;
        }
    }

    public bool selected
    {
        get => _selected;
        set
        {
            _selected = value;
            if (selected)
            {
                OnSelect(null);
            }
            else
            {
                OnDeselect(null);
            }
        }
    }

    private void Start()
    {
        _selectableManager ??= GetComponentInParent<ISelectableManager>();
        _rectTransform ??= GetComponent<RectTransform>();
        _animationImage ??= GetComponentInChildren<SelectionImage>();
        if (_animationImage)
        {
            _animationImageDefaultColor = _animationImage.color;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
    }

    public void OnDeselect(BaseEventData eventData)
    {
    }

    private void AnimateVisual(float endValuePercent, Color endColor, float inDuration)
    {
        // _currentScaleTween.Stop();
        Color startColor = _animationImageDefaultColor;
        float scalingValue = 1 + endValuePercent*0.01f;
        Vector3 startSize = transform.localScale;
        Vector3 endSize = Vector3.one*scalingValue;
        if (startSize == endSize)
        {
            return;
        }

        // _currentScaleTween = PrimeTween.Tween.Custom(0.0f, 1.0f, inDuration, (float currentValue) =>
        // {
        //     if (_animationImage != null)
        //     {
        //         _animationImage.color = Color.Lerp(startColor, endColor, currentValue*0.2f);
        //     }
        //     transform.localScale = Vector3.Lerp(startSize, endSize, currentValue);
        // });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // AnimateVisual(animationStrength,animationDuration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color selectColor = Color.white;
        selectColor.a = 0.2f;
        AnimateVisual(animationStrengthPercent, selectColor, animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color selectColor = Color.black;
        selectColor.a = 0.2f;
        AnimateVisual(0f, selectColor, animationDuration);
    }
}

}