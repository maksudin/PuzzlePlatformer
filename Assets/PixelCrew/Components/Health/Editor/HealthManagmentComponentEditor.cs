using PixelCrew.Components.Health;
using PixelCrew.Utils.Editor;
using UnityEditor;

namespace Assets.PixelCrew.Components.Health.Editor
{
    [CustomEditor(typeof(HealthManagmentComponent))]
    public class HealthManagmentComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);
            HealthMode mode;

            if (_modeProperty.GetEnum(out mode))
            {
                switch (mode)
                {
                    case HealthMode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_damage"));
                        break;
                    case HealthMode.ExternalStat:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_statId"));
                        break;
                }
            }

            serializedObject.ApplyModifiedProperties();

        }
    }
}