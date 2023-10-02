using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using BobboNet.SGB.IMod.Naninovel;
using Naninovel;

namespace BobboNet.Editor.SGB.IMod.Naninovel
{
    public class NaniscriptReturnTargetListDrawer
    {
        private static readonly GUIContent nameContent = new GUIContent("Scene Name", "The name of the UNITY scene to hook into. When this scene is loaded, it will unload from SGB and into the desired Naniscript.");
        private static readonly GUIContent valueContent = new GUIContent("Target Naniscript", "The Naniscript to load into once the given UNITY scene is loaded.");

        private const float sceneNameWidth = 140;
        private const float headerLeftMargin = 15;
        private const float elementContentLeftMargin = 30;
        private const float paddingWidth = 10;

        private ReorderableList reorderableList;

        //
        //  Public Methods
        //

        public void DrawProperty(SerializedProperty property, SerializedObject serializedObject)
        {
            // Write the label for this list
            var label = EditorGUI.BeginProperty(Rect.zero, null, property);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            // If necessary, initialize the list
            if (reorderableList is null || reorderableList.serializedProperty.serializedObject != serializedObject)
            {
                InitializeReturnTargetsList(serializedObject);
            }

            // Actually draw the list layout
            reorderableList.DoLayoutList();
            EditorGUI.EndProperty();
        }

        //
        //  Private Methods
        //

        private void InitializeReturnTargetsList(SerializedObject serializedObject)
        {
            // Create the reorderable list object representing the return target list
            var serializedProperty = serializedObject.FindProperty(nameof(SGBIModConfiguration.returnTargets));
            reorderableList = new ReorderableList(serializedObject, serializedProperty, true, true, true, true)
            {
                // Specify callbacks to draw the header and elements of the return target list
                drawHeaderCallback = OnDrawReturnTargetListHeader,
                drawElementCallback = OnDrawReturnTargetListElement,
                elementHeightCallback = OnDrawReturnTargetListHeight
            };
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
                rect.width,
                EditorGUIUtility.singleLineHeight
            );

            var returnScriptRect = new Rect(
                rect.x + elementContentLeftMargin,
                sceneNameRect.y + sceneNameRect.height + EditorGUIUtility.standardVerticalSpacing,
                rect.width - elementContentLeftMargin,
                EditorGUIUtility.singleLineHeight
            );

            var returnLabelRect = new Rect(
                rect.x + elementContentLeftMargin,
                returnScriptRect.y + returnScriptRect.height + EditorGUIUtility.standardVerticalSpacing,
                rect.width - elementContentLeftMargin,
                EditorGUIUtility.singleLineHeight
            );

            var elementProperty = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var sceneNameProperty = elementProperty.FindPropertyRelative("sceneName");
            var returnScriptProperty = elementProperty.FindPropertyRelative("returnScript");
            var returnLabelProperty = elementProperty.FindPropertyRelative("returnLabel");

            EditorGUI.PropertyField(sceneNameRect, sceneNameProperty);
            EditorGUI.PropertyField(returnScriptRect, returnScriptProperty);
            EditorGUI.PropertyField(returnLabelRect, returnLabelProperty);
        }

        private float OnDrawReturnTargetListHeight(int index)
        {
            return (EditorGUIUtility.singleLineHeight * 3) + (EditorGUIUtility.standardVerticalSpacing * 4);
        }
    }
}