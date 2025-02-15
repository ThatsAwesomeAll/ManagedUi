#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UI.ManagedUi
{
    public class SelectionImage : ManagedImage
    {
        public bool enableNeeded = false;
    }

    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SelectionImage))]
    public class SelectionImageEditor : Editor
    {
        private SelectionImage image;

        private void OnEnable()
        {
            image = (SelectionImage)target;
            image.SetUp();
        }

        void DrawProperty(SerializedProperty property, string content, string tooltip)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent(content, tooltip), GUILayout.Width(120));
            EditorGUILayout.PropertyField(property, new GUIContent("", tooltip));

            GUILayout.EndHorizontal();
        }

        void DrawCustomHeader()
        {
            GUILayout.Space(2);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        public override void OnInspectorGUI()
        {
            var UIManagerAsset = serializedObject.FindProperty("_manager");
            var colorType = serializedObject.FindProperty("imageColor");
            var enableNeeded = serializedObject.FindProperty("enableNeeded");
            var fixColor = serializedObject.FindProperty("fixColor");
            var colorTheme = serializedObject.FindProperty("colorTheme");

            DrawProperty(enableNeeded, "Enable / Disable needed", "Should be enabled and disable on animation");
            DrawProperty(fixColor, "Color fixed", "Fix your color by Theme");
            if (fixColor.boolValue)
            {
                DrawProperty(colorTheme, "Color", "Select Color");
                int enumIndex = colorTheme.enumValueIndex;
                UiSettings.ColorName currentEnumValue = (UiSettings.ColorName)enumIndex;
                image.SetColorByTheme(currentEnumValue);
            }
            else
            {
                DrawProperty(colorType, "Color", "Select Color");
                image.SetColorByFixed(colorType.colorValue);
            }

            if (UIManagerAsset != null)
            {
                DrawProperty(UIManagerAsset, "Manager Asset", "Dont change this");
            }
            else
            {
                EditorGUILayout.LabelField(new GUIContent("NO MANAGER FOUND"), GUILayout.Width(120));
            }

            serializedObject.ApplyModifiedProperties();
            DrawCustomHeader();
            base.OnInspectorGUI();
        }
    }
    #endif
}
