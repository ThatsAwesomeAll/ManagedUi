using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI.ManagedUi
{
    
    [ExecuteInEditMode]
    public class ManagedImage : Image
    {
        [Header("Style")] public bool onHoverEffect = true;
        public bool fixColor = false;
        public UiSettings.ColorName colorTheme;
        public Color imageColor;


        [SerializeField] private UiSettings _manager;

        protected override void Awake()
        {
            SetUp();
        }

        public void SetUp()
        {
            if (!_manager) _manager = UiSettings.GetSettings();
        }
        
        #if UNITY_EDITOR
        private void Update()
        {
            if (!_manager)
            {
                return;
            }
            if (Application.isPlaying)
            {
                return;
            }
            if (fixColor)
            {
                SetColorByTheme(colorTheme);
            }
        }
        #endif

        public void SetColorByTheme(UiSettings.ColorName currentEnumValue)
        {
            if (!_manager) return;
            var colorTemp = _manager.GetColorByEnum(currentEnumValue);
            color = colorTemp;
        }

        public void SetColorByFixed(Color colorTypeColorValue)
        {
            color = colorTypeColorValue;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ManagedImage))]
    public class ManagedImageEditor : Editor
    {
        private ManagedImage image;

        private void OnEnable()
        {
            image = (ManagedImage)target;
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
            var fixColor = serializedObject.FindProperty("fixColor");
            var colorTheme = serializedObject.FindProperty("colorTheme");

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