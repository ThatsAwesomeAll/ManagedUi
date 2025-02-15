using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ManagedUi.Widgets
{

[ExecuteInEditMode]
public class Card : MonoBehaviour
{
    public ManagedImage _image;
    public ManagedImage _background;
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
        }
        if (!_image)
        {
            _image = CreateImage("Image", _background.transform);
            _image.transform.SetAsFirstSibling();
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