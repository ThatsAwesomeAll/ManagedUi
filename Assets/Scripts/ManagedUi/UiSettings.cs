﻿using System;
using TMPro;
using UnityEngine;

namespace ManagedUi
{

[CreateAssetMenu(fileName = "UiManager", menuName = "UiManager/UiSettings")]
public class UiSettings : ScriptableObject
{

    [ContextMenu("Set Default Colors")]
    public void SetDefaultColor()
    {
        ImageColors = new ColorTheme(ColorMode.Default);
        TextColors = new ColorTheme(ColorMode.DefaultText);
    }

    public static UiSettings GetSettings()
    {
        UiSettings matchingAssets = Resources.Load<UiSettings>("ManagedUi/DefaultUiSettings");
        return matchingAssets;
    }

    [Serializable]
    public enum ColorMode
    {
        Default,
        DefaultText,
    }

    public enum ColorName
    {
        Accent,
        AccentLighter,
        Main,
        Light,
        Lighter,
        Dark,
        Darker,
        Background,
        BackgroundDarker
    }

    [Serializable]
    public struct ColorTheme
    {
        public Color ColorAccent;
        public Color ColorAccentLighter;
        public Color ColorMain;
        public Color ColorLight;
        public Color ColorLighter;
        public Color ColorDark;
        public Color ColorDarker;
        public Color ColorBackground;
        public Color ColorBackgroundDarker;

        public ColorTheme(ColorMode mode)
        {
            if (mode == ColorMode.DefaultText)
            {
                ColorAccent = new Color(0.1f, 0.1f, 0.1f);
                ColorAccentLighter = new Color(0.1f, 0.1f, 0.1f);
                ColorMain = new Color(0.1f, 0.1f, 0.1f);
                ColorLight = new Color(0.8f, 0.8f, 0.8f);
                ColorLighter = new Color(0.8f, 0.8f, 0.8f);
                ColorDark = new Color(0.8f, 0.8f, 0.8f);
                ColorDarker = new Color(0.8f, 0.8f, 0.8f);
                ColorBackground = new Color(0.8f, 0.8f, 0.8f);
                ColorBackgroundDarker = new Color(0.8f, 0.8f, 0.8f);
                return;
            }
            ColorAccent = new Color(1.0f, 0.71f, 0.01f);
            ColorAccentLighter = new Color(1.0f, 0.91f, 0.01f);
            ColorMain = new Color(0.98f, 0.51f, 0.0f);
            ColorLight = new Color(0.15f, 0.32f, 0.48f);
            ColorLighter = new Color(0.2f, 0.4f, 0.6f);
            ColorDark = new Color(0.01f, 0.19f, 0.27f);
            ColorDarker = new Color(0.011f, 0.1f, 0.25f);
            ColorBackground = new Color(0.2f, 0.3f, 0.3f);
            ColorBackgroundDarker = new Color(0.1f, 0.1f, 0.1f);
        }
    }

    public Color SelectedColor => ImageColors.ColorAccent;
    public Color ConfirmedColor => ImageColors.ColorAccentLighter;

    public ColorTheme ImageColors = new ColorTheme(ColorMode.Default);
    public ColorTheme TextColors = new ColorTheme(ColorMode.DefaultText);

    public Color GetImageColorByEnum(ColorName colorTheme, ColorTheme colorPalette)
    {
        return colorTheme switch
        {
            ColorName.Accent => colorPalette.ColorAccent,
            ColorName.AccentLighter => colorPalette.ColorAccentLighter,
            ColorName.Main => colorPalette.ColorMain,
            ColorName.Light => colorPalette.ColorLight,
            ColorName.Lighter => colorPalette.ColorLighter,
            ColorName.Dark => colorPalette.ColorDark,
            ColorName.Darker => colorPalette.ColorDarker,
            ColorName.Background => colorPalette.ColorBackground,
            ColorName.BackgroundDarker => colorPalette.ColorBackgroundDarker,
            _ => Color.black
        };
    }
    
    public Color GetImageColorByEnum(ColorName colorTheme)
    {
        return GetImageColorByEnum(colorTheme, ImageColors);
    }
    
    public enum TextStyle
    {
        Header,
        Highlight,
        Text,
        SubText
    }

    private void ScaleRectTrans(RectTransform transform)
    {
        transform.anchorMin = new Vector2(0, 0);
        transform.anchorMax = new Vector2(1, 1);
        transform.anchoredPosition = new Vector2(0, 0);
        transform.pivot = new Vector2(0.5f, 0.5f);
    }

    public void SetTextAutoFormat(TextMeshProUGUI text, TextStyle style, ColorName background)
    {
        text.color = GetImageColorByEnum(background, TextColors);
        ScaleRectTrans(text.rectTransform);
        text.enableAutoSizing = true;
        var textSize = style switch
        {

            TextStyle.Header => new Vector2(30, 100),
            TextStyle.Highlight => new Vector2(30, 80),
            TextStyle.Text => new Vector2(20, 40),
            TextStyle.SubText => new Vector2(10, 30),
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        };
        text.fontSizeMin = textSize.x;
        text.fontSizeMax = textSize.y;
        text.alignment = TextAlignmentOptions.Center;
    }
}
}