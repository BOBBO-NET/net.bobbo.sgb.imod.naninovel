using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using BobboNet.SGB.IMod.Naninovel;
using Naninovel;

namespace BobboNet.Editor.SGB.IMod.Naninovel
{
    public class SGBFontTargetListDrawer
    {
        private static readonly GUIContent nameContent = new GUIContent("SGB Project Name", "The name of the SGB project to target when applying this font. When this project is loaded, it will set SGB's font to the font chosen below.");
        private static readonly GUIContent valueContent = new GUIContent("Font", "The font to use when the target SGB project is loaded.");

        private const float projectNameWidthRatio = 0.6f;
        private const float headerLeftMargin = 15;
        private const float paddingWidth = 5;

        private ReorderableList reorderableList;
        private EditorResources editorResources;

        //
        //  Public Methods
        //

        public void DrawProperty(SerializedProperty property, SerializedObject serializedObject)
        {
            // Write the label for this list
            var label = EditorGUI.BeginProperty(Rect.zero, null, property);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            // If necessary, load editor resources
            if (editorResources is null) editorResources = EditorResources.LoadOrDefault();

            // If necessary, initialize the list
            if (reorderableList is null || reorderableList.serializedProperty.serializedObject != serializedObject)
            {
                InitializeList(serializedObject);
            }

            // Actually draw the list layout
            reorderableList.DoLayoutList();
            EditorGUI.EndProperty();
        }

        //
        //  Private Methods
        //

        private void InitializeList(SerializedObject serializedObject)
        {
            // Create the reorderable list object representing the list
            var serializedProperty = serializedObject.FindProperty(nameof(SGBIModConfiguration.fontTargets));
            reorderableList = new ReorderableList(serializedObject, serializedProperty, true, true, true, true)
            {
                // Specify callbacks to draw the header and elements of the list
                drawHeaderCallback = OnDrawListHeader,
                drawElementCallback = OnDrawListElement,
                elementHeightCallback = OnDrawListHeight
            };
        }

        private void OnDrawListHeader(Rect rect)
        {
            float projectNameWidth = rect.width * projectNameWidthRatio;
            var propertyRect = new Rect(headerLeftMargin + rect.x, rect.y, projectNameWidth - headerLeftMargin, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(propertyRect, nameContent);
            propertyRect.x += propertyRect.width + paddingWidth;
            propertyRect.width = rect.width - projectNameWidth;
            EditorGUI.LabelField(propertyRect, valueContent);
        }

        private void OnDrawListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            float projectNameWidth = rect.width * projectNameWidthRatio;
            var projectNameRect = new Rect(rect.x, rect.y + EditorGUIUtility.standardVerticalSpacing, projectNameWidth, EditorGUIUtility.singleLineHeight);

            var fontRect = new Rect(
                projectNameRect.x + projectNameRect.width + paddingWidth,
                projectNameRect.y,
                rect.width - projectNameWidth - paddingWidth,
                EditorGUIUtility.singleLineHeight
            );

            var elementProperty = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var projectNameProperty = elementProperty.FindPropertyRelative("projectName");
            var fontProperty = elementProperty.FindPropertyRelative("font");

            EditorGUI.PropertyField(projectNameRect, projectNameProperty, GUIContent.none);
            EditorGUI.PropertyField(fontRect, fontProperty, GUIContent.none);
        }

        private float OnDrawListHeight(int index)
        {
            return (EditorGUIUtility.singleLineHeight * 1) + (EditorGUIUtility.standardVerticalSpacing * 2);
        }
    }
}