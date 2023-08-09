using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using BobboNet.SGB.IMod.Naninovel;
using Naninovel;

namespace BobboNet.Editor.SGB.IMod.Naninovel
{
    public class SGBIModSettings : ConfigurationSettings<SGBIModConfiguration>
    {
        private const float sceneNameWidth = 140;
        private const float headerLeftMargin = 15;
        private const float paddingWidth = 10;

        private static readonly GUIContent nameContent = new GUIContent("Scene Name", "The name of the UNITY scene to hook into. When this scene is loaded, it will unload from SGB and into the desired Naniscript.");
        private static readonly GUIContent valueContent = new GUIContent("Target Naniscript", "The Naniscript to load into once the given UNITY scene is loaded.");

        private ReorderableList returnTargetsReorderableList;

        //
        //  Override Methods
        //

        protected override Dictionary<string, Action<SerializedProperty>> OverrideConfigurationDrawers()
        {
            var drawers = base.OverrideConfigurationDrawers();
            drawers[nameof(SGBIModConfiguration.returnTargets)] = DrawReturnTargetsEditor;
            return drawers;
        }

        //
        //  Return Target Methods
        //

        private void DrawReturnTargetsEditor(SerializedProperty property)
        {
            // Write the label for this list
            var label = EditorGUI.BeginProperty(Rect.zero, null, property);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            // If necessary, initialize the list
            if (returnTargetsReorderableList is null || returnTargetsReorderableList.serializedProperty.serializedObject != SerializedObject)
            {
                InitializeReturnTargetsList();
            }

            returnTargetsReorderableList.DoLayoutList();

            EditorGUI.EndProperty();
        }

        private void InitializeReturnTargetsList()
        {
            // Create the reorderable list object representing the return target list
            var serializedProperty = SerializedObject.FindProperty(nameof(SGBIModConfiguration.returnTargets));
            returnTargetsReorderableList = new ReorderableList(SerializedObject, serializedProperty, true, true, true, true);

            // Specify callbacks to draw the header and elements of the return target list
            returnTargetsReorderableList.drawHeaderCallback = OnDrawReturnTargetListHeader;
            returnTargetsReorderableList.drawElementCallback = OnDrawReturnTargetListElement;
        }

        private void OnDrawReturnTargetListHeader(Rect rect)
        {
            var propertyRect = new Rect(headerLeftMargin + rect.x, rect.y, sceneNameWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(propertyRect, nameContent);
            propertyRect.x += propertyRect.width + paddingWidth;
            propertyRect.width = rect.width - sceneNameWidth;
            EditorGUI.LabelField(propertyRect, valueContent);
        }

        private void OnDrawReturnTargetListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var sceneNameRect = new Rect(
                rect.x,
                rect.y + EditorGUIUtility.standardVerticalSpacing,
                sceneNameWidth,
                EditorGUIUtility.singleLineHeight
            );

            var returnScriptRect = new Rect(
                rect.x + sceneNameRect.width + paddingWidth,
                rect.y + EditorGUIUtility.standardVerticalSpacing,
                rect.width - sceneNameWidth - paddingWidth,
                EditorGUIUtility.singleLineHeight
            );

            var elementProperty = returnTargetsReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var sceneNameProperty = elementProperty.FindPropertyRelative("sceneName");
            var returnScriptProperty = elementProperty.FindPropertyRelative("returnScript");

            EditorGUI.PropertyField(sceneNameRect, sceneNameProperty, GUIContent.none);
            EditorGUI.PropertyField(returnScriptRect, returnScriptProperty, GUIContent.none);
        }
    }
}
