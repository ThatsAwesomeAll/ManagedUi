using ManagedUi.GridSystem;
using ManagedUi.Selectables;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ManagedUi.Widgets
{

[ExecuteInEditMode]
public class Card : MonoBehaviour
{
    [Header("Content")]
    public string Title;
    public string Text;
    public Sprite Image;
    public UiSettings.ColorTheme BackgroundColor;
    public UiSettings.ColorTheme TitleColor;
    public UiSettings.ColorTheme TextColor;
    
    [Header("UI Elements")]
    public ManagedImage _image;
    public ManagedImage _background;
    public SelectionAnimation _selectionAnimation;
    public TextMeshProUGUI _title;
    public TextMeshProUGUI _text;

    protected void Awake()
    {
        SetUp();
    }

    private void OnEnable()
    {
        SetUp();
        SetUpImages();
        SetUpAllText();
        SetupSelectionAnimation();
    }

    private void SetContent()
    {
        if (Image)
        {
            _image.sprite = Image;
        }
        else
        {
            _image.sprite = _manager.DefaultImage();
        }
    }
    
    private void SetupSelectionAnimation()
    {
        var allAnimations = GetComponentsInChildren<SelectionAnimation>();
        foreach (var animation in allAnimations)
        {
            if (animation.name == "SelectionAnimation")
            {
                _selectionAnimation ??= animation;
            }
        }
        if (_selectionAnimation != null)
            return;
        var animationChild = new GameObject( "SelectionAnimation");
        animationChild.transform.SetParent(_background.transform, false); 
        var animationTemp = animationChild.AddComponent<SelectionAnimation>();
        animationTemp.images.Add(_background);
        _selectionAnimation = animationTemp;
        _selectionAnimation.selectionSprite = _manager.DefaultSelectionImage();
    }

    private TextMeshProUGUI CreateText(string name, string defaultText, UiSettings.TextStyle style)
    {
        var textChild = new GameObject(name);
        textChild.transform.SetParent(_background.transform, false);
        var text = textChild.AddComponent<TextMeshProUGUI>();
        text.text = defaultText;
        _manager.SetTextAutoFormat(text, style, UiSettings.ColorName.Dark);
        return text;
    }
    private void SetUpAllText()
    {
        var allText = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in allText)
        {
            switch (text.name)
            {
                case "Title":
                    _title ??= text;
                    break;
                case "Text":
                    _text ??= text;
                    break;
            }
        }
        if (!_title)
        {
            _title = CreateText("Title", "No Title", UiSettings.TextStyle.Highlight);
            _title.transform.SetAsLastSibling();
        }
        if (!_text)
        {
            _text = CreateText("Text", "No Content", UiSettings.TextStyle.Text);
            _title.transform.SetAsLastSibling();
        }
    }
    private void SetUpImages()
    {
        var allImages = GetComponentsInChildren<ManagedImage>();
        foreach (var image in allImages)
        {
            switch (image.name)
            {
                case "Background":
                    _background ??= image;
                    break;
                case "Image":
                    _image ??= image;
                    break;
            }
        }
        if (!_background)
        {
            _background = CreateImage("Background", transform);
            _background.fixColor = true;
            _background.colorTheme = UiSettings.ColorName.Background;
            _background.AddComponent<GrowGridLayout>();
        }
        if (!_image)
        {
            _image = CreateImage("Image", _background.transform);
            _image.transform.SetAsFirstSibling();
            _image.growth = Vector2Int.one * 5;
        }
    }
    private ManagedImage CreateImage(string name, Transform parent)
    {
        var textChild = new GameObject(name);
        textChild.transform.SetParent(parent, false);
        return textChild.AddComponent<ManagedImage>();
    }
    
    
    [SerializeField] private UiSettings _manager;
    public void SetUp()
    {
        if (!_manager) _manager = UiSettings.GetSettings();
    }
}
}