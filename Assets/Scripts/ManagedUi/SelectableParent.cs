using PrimeTween;
using UI.ManagedUi;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ManagedUi
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

    Tween _currentScaleTween;
    Tween _currentColorTween;

    public SelectionImage _animationImage;
    public float animationDuration = 0.1f;
    public float animationStrengthPercent = 2f;
    public Ease animationEase = Ease.Default;
    private bool _confirmed;


    public Vector2 anchoredPosition => _rectTransform.anchoredPosition;
    public Vector2 size => new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);

    private void Awake()
    {
        _rectTransform ??= GetComponent<RectTransform>();
    }

    public Vector2Int gridPosition
    {
        get => _internalGridPosition;
        set
        {
            if (!_gridFixed) _internalGridPosition = value;
        }
    }

    public bool Selected => _selected;
    public void SetSelected(bool selected)
    {
        if (selected == _selected)
        {
            return;
        }
        _selected = selected;
        AnimateSelect(_selected);
    }

    public bool Confirmed => _confirmed;
    public void SetConfirmed()
    {
        _confirmed = true;
        if (_confirmed)
        {
            AnimateConfirm();
        }
    }
    
    private void OnEnable()
    {
        _selectableManager ??= GetComponentInParent<ISelectableManager>();
        _animationImage ??= GetComponentInChildren<SelectionImage>();
        if (_animationImage)
        {
            _animationImageDefaultColor = _animationImage.color;
        }
    }

    private void AnimateVisual(float endValuePercent, Color endColor, float inDuration)
    {
        _currentScaleTween.Stop();
        Color startColor = _animationImageDefaultColor;
        float scalingValue = 1 + endValuePercent*0.01f;
        Vector3 startSize = transform.localScale;
        Vector3 endSize = Vector3.one*scalingValue;
        if (startSize == endSize)
        {
            return;
        }
        _currentScaleTween = PrimeTween.Tween.Custom(0.0f, 1.0f, inDuration, (float currentValue) =>
        {
            if (_animationImage != null)
            {
                _animationImage.color = Color.Lerp(startColor, endColor, currentValue*0.2f);
            }
            transform.localScale = Vector3.Lerp(startSize, endSize, currentValue);
        });
    }

    private void AnimateSelect(bool selected)
    {
        Color selectColor;
        float animationSize = 0;
        if (selected)
        {
            selectColor = Color.green;
            animationSize = animationStrengthPercent;
        }
        else
        {
            selectColor = Color.blue;
        }
        AnimateVisual(animationSize, selectColor, animationDuration);
    }
    
    private void AnimateConfirm()
    {
        AnimateVisual(animationStrengthPercent * 1.5f, Color.red, animationDuration * 0.5f);
    }
    
    
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        _selectableManager.TriggerExternalConfirm(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect(eventData);
    }
    public void OnSelect(BaseEventData eventData)
    {
        _selectableManager.TriggerExternalSelect(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _selectableManager.TriggerExternalDeSelect(this);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        // OnDeselect(eventData);
    }
}

}