﻿using UnityEditor;
using UnityEditor.UI;

namespace PixelCrew.Components.UI.Widgets.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomButton), true)]
    public class CustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressed"));
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}