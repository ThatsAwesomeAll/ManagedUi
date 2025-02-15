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
    [Header("Animation")]
    public SelectionImage _animationImage;

    public float animationDuration = 0.1f;
    public float animationStrengthPercent = 2f;
    public Ease animationEase = Ease.Default;

    [Header("Grid Settings")]
    public bool _gridFixed = false;

    [SerializeField] private UiSettings _manager;


    ISelectableManager _selectableManager;
    RectTransform _rectTransform;
    Vector2Int _internalGridPosition = new Vector2Int(0, 0);
    private bool _confirmed;
    private bool _selected;
    Tween _currentScaleTween;


    public Vector2 anchoredPosition => _rectTransform.anchoredPosition;
    public Vector2 size => new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);

    void SetUpSettings()
    {
        if (!_manager) _manager = UiSettings.GetSettings();
    }

    private void Awake()
    {
        _rectTransform ??= GetComponent<RectTransform>();
        SetUpSettings();
    }

    public Vector2Int GridPosition
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
        SetUpSettings();
    }

    private void AnimateVisual(float endValuePercent, Color startColor, Color endColor, float inDuration, bool withFadeout = false)
    {
        _currentScaleTween.Stop();
        float scalingValue = 1 + endValuePercent*0.01f;
        Vector3 startSize = transform.localScale;
        Vector3 endSize = Vector3.one*scalingValue;
        if (startSize == endSize)
        {
            return;
        }
        _currentScaleTween = Tween.Custom(0.0f, 1.0f, inDuration, (float currentValue) =>
        {
            if (_animationImage != null)
            {
                _animationImage.SetColor(Color.Lerp(startColor, endColor, currentValue));
            }
            transform.localScale = Vector3.Lerp(startSize, endSize, currentValue);
        }, ease: animationEase);
        if (withFadeout)
        {
            _currentScaleTween.OnComplete(() =>
            {
                if (_animationImage != null)
                {
                    _animationImage.SetColor(startColor);
                }
                transform.localScale = startSize;
            });
        }
    }
    
    private void AnimateVisual(float endValuePercent, float inDuration)
    {
        _currentScaleTween.Stop();
        float scalingValue = 1 + endValuePercent*0.01f;
        Vector3 startSize = transform.localScale;
        Vector3 endSize = Vector3.one*scalingValue;
        if (startSize == endSize)
        {
            return;
        }
        _currentScaleTween = Tween.Custom(0.0f, 1.0f, inDuration, (float currentValue) =>
        {
            transform.localScale = Vector3.Lerp(startSize, endSize, currentValue);
            if (_animationImage != null)
            {
                _animationImage.LerpToDefault(currentValue);
            }
        }, ease: animationEase);
    }

    private void EnableVisualImage(bool enable)
    {
        if (!_animationImage) return;
        if (_animationImage.enableNeeded)
        {
            _animationImage.gameObject.SetActive(enable);
        }
    }

    private void AnimateSelect(bool selected)
    {
        if (selected)
        {
            AnimateVisual(animationStrengthPercent, _manager.selectedColor, _manager.selectedColor, animationDuration);
        }
        else
        {
            AnimateVisual(0, animationDuration);
        }
        EnableVisualImage(selected);
    }

    private void AnimateConfirm()
    {
        AnimateVisual(animationStrengthPercent*1.2f, _manager.selectedColor, _manager.confirmedColor, animationDuration*0.5f, true);
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