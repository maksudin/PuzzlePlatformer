using System.Collections;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Components.Dialogs.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
    public class ShowDialogComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}