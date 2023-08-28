using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Atlas.Unity {

    [CustomPropertyDrawer(typeof(ScatterRuleAsset.ScatterPass))]
    public class ScatterPassEditor : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            EditorGUI.BeginProperty(position, label, property);

            if ( property.FindPropertyRelative("inspectorInitialized").boolValue == false) {
                property.FindPropertyRelative("inspectorInitialized").boolValue = true;
                property.FindPropertyRelative("mode").intValue = 0;
                property.FindPropertyRelative("cullRadius").floatValue = 1;
                property.FindPropertyRelative("detailCullRadius").floatValue = 1;
                property.FindPropertyRelative("spawnRadius").floatValue = 5;
                property.FindPropertyRelative("childCount").intValue = 3;
                property.FindPropertyRelative("scaleMultiplierMin").floatValue = 1;
                property.FindPropertyRelative("scaleMultiplierMax").floatValue = 1;
                property.FindPropertyRelative("widthScaleMin").floatValue = 1;
                property.FindPropertyRelative("widthScaleMax").floatValue = 1;
                property.FindPropertyRelative("heightScaleMin").floatValue = 1;
                property.FindPropertyRelative("heightScaleMax").floatValue = 1;
                property.FindPropertyRelative("distanceScaleMultiplier").floatValue = 1;
            }

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label, EditorStyles.boldLabel);

            if (property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, 18), property.isExpanded, GUIContent.none, EditorStyles.foldout)) {

                EditorGUI.indentLevel++;

                var modeProperty = property.FindPropertyRelative("mode");

                EditorGUI.BeginDisabledGroup(modeProperty.intValue == 0);

                if (GUI.Button(new Rect(position.x, position.y + 20, position.width / 2, 18), "Tree")) {
                    modeProperty.intValue = 0;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(modeProperty.intValue != 0);

                if (GUI.Button(new Rect(position.x + (position.width / 2), position.y + 20, position.width / 2, 18), "Prefab")) {
                    modeProperty.intValue = 2;
                }

                EditorGUI.EndDisabledGroup();

                var prefabsProperty = property.FindPropertyRelative("prefabs");
                EditorGUI.PropertyField(new Rect(position.x, position.y + 40, position.width, 18), prefabsProperty, true);
                var heightOffset = prefabsProperty.isExpanded ? (20 * 2) + (20 * Mathf.Max(1, prefabsProperty.arraySize)) : 0;

                EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 60, position.width, 18), property.FindPropertyRelative("cullRadius"));
                EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 80, position.width, 18), property.FindPropertyRelative("detailCullRadius"));
                EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 100, position.width, 18), property.FindPropertyRelative("spawnRadius"));
                EditorGUI.PropertyField(new Rect(position.x, position.y + heightOffset + 120, position.width, 18), property.FindPropertyRelative("childCount"));

                var foldedProperty = property.FindPropertyRelative("folded");

                foldedProperty.boolValue = EditorGUI.Foldout(new Rect(position.x, position.y + heightOffset + 140, position.width, 18), foldedProperty.boolValue, "Properties", true);

                if (foldedProperty.boolValue) {

                    EditorGUI.indentLevel++;

                    EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 160, position.width, 18), property.FindPropertyRelative("scaleMultiplierMin"));
                    EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 180, position.width, 18), property.FindPropertyRelative("scaleMultiplierMax"));
                    EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 200, position.width, 18), property.FindPropertyRelative("widthScaleMin"));
                    EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 220, position.width, 18), property.FindPropertyRelative("widthScaleMax"));
                    EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 240, position.width, 18), property.FindPropertyRelative("heightScaleMin"));
                    EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 260, position.width, 18), property.FindPropertyRelative("heightScaleMax"));
                    EditorGUI.PropertyField(new Rect(position.x, position.y + heightOffset + 280, position.width, 18), property.FindPropertyRelative("distanceScaleMultiplier"));

                    if (modeProperty.intValue != 0) {

                        EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 300, position.width, 18), property.FindPropertyRelative("alignment"));
                        EditorGUI.PropertyField(new Rect(position.x, position.y +heightOffset+ 320, position.width, 18), property.FindPropertyRelative("heightOffsetMin"));
                        EditorGUI.PropertyField(new Rect(position.x, position.y + heightOffset + 340, position.width, 18), property.FindPropertyRelative("heightOffsetMax"));
                    }

                    EditorGUI.indentLevel--;

                }

                EditorGUI.indentLevel--;

            }

            EditorGUI.EndProperty();

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

            var totalHeight = 20 * 1;

            if (property.isExpanded ) {

                var prefabsProperty = property.FindPropertyRelative("prefabs");

                if( prefabsProperty.isExpanded ) {

                    totalHeight += 20 * 2;

                    totalHeight += 20 * Mathf.Max(1,prefabsProperty.arraySize);

                }

                totalHeight += 20 * 7;

                var foldedProperty = property.FindPropertyRelative("folded");

                if (foldedProperty.boolValue) {

                    var modeProperty = property.FindPropertyRelative("mode");

                    totalHeight += 20 * 7;

                    if (modeProperty.intValue != 0) {

                        totalHeight += 20 * 3;

                    }

                }

            }

            return totalHeight;

        }

    }

}
