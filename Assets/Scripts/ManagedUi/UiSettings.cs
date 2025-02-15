using System;
using UnityEngine;

namespace UI.ManagedUi
{

    [CreateAssetMenu(fileName = "UiManager", menuName = "UiManager/UiSettings")]
    public class UiSettings : ScriptableObject
    {
        
        public static UiSettings GetSettings()
        {
            UiSettings matchingAssets = Resources.Load<UiSettings>("ManagedUi/DefaultUiSettings");
            return matchingAssets;
        }
        
        [Serializable]
        public enum ColorMode
        {
            Default
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
                this.ColorAccent = new Color(1.0f,0.71f,0.01f );
                ColorAccentLighter = new Color(1.0f,0.91f,0.01f );;
                ColorMain = new Color(0.98f,0.51f,0.0f );
                ColorLight = new Color(0.15f,0.32f,0.48f );
                ColorLighter = new Color(0.2f,0.4f,0.6f );
                ColorDark = new Color(0.01f,0.19f,0.27f );
                ColorDarker = new Color(0.011f,0.1f,0.25f );
                ColorBackground = new Color(0.2f,0.3f,0.3f );
                ColorBackgroundDarker = new Color(0.1f,0.1f,0.1f );
            }
        }

        public ColorTheme ImageColors = new ColorTheme(ColorMode.Default);
        public ColorTheme TextColors = new ColorTheme(ColorMode.Default);

        public Color GetColorByEnum(ColorName colorTheme)
        {
            switch (colorTheme)
            {
                case ColorName.Accent:
                    return ImageColors.ColorAccent;
                case ColorName.AccentLighter:
                    return ImageColors.ColorAccentLighter;
                case ColorName.Main:
                    return ImageColors.ColorMain;
                case ColorName.Light:
                    return ImageColors.ColorLight;
                case ColorName.Lighter:
                    return ImageColors.ColorLighter;
                case ColorName.Dark:
                    return ImageColors.ColorDark;
                case ColorName.Darker:
                    return ImageColors.ColorDarker;
                case ColorName.Background:
                    return ImageColors.ColorBackground;
                case ColorName.BackgroundDarker:
                    return ImageColors.ColorBackgroundDarker;
                default:
                    return Color.black;
            }
        }
    }
}