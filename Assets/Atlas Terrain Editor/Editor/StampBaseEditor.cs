using UnityEngine;
using UnityEditor;

namespace Atlas.Unity {

    public static class StampBaseEditor {

        public static void DrawBaseStamp(SerializedObject serializedObject, bool allowMaskEditing = false) {

            var stampProperty = serializedObject.FindProperty("stamp");

            EditorGUILayout.PropertyField(stampProperty);

            EditorGUILayout.Space(20);

            if (serializedObject.targetObjects.Length <= 1) {

                if (allowMaskEditing && stampProperty.objectReferenceValue != null) {

                    DrawMaskEditor(serializedObject);

                }

            }

            EditorGUILayout.Space(5);

            var maskMapProperty = serializedObject.FindProperty("maskMap");
            maskMapProperty.FindPropertyRelative("mapType").enumValueIndex = (int)StampBase.StampMap.StampMapType.Mask;

            Border.Start();
            EditorGUILayout.PropertyField(maskMapProperty, new GUIContent("Mask"));
            Border.End(GetMapTypeColor(StampBase.StampMap.StampMapType.Mask));

            EditorGUILayout.Space(5);

            var heightMapProperty = serializedObject.FindProperty("heightMap");
            heightMapProperty.FindPropertyRelative("mapType").enumValueIndex = (int)StampBase.StampMap.StampMapType.Height;

            Border.Start();
            EditorGUILayout.PropertyField(heightMapProperty, new GUIContent("Height"));
            Border.End(GetMapTypeColor(StampBase.StampMap.StampMapType.Height));

            EditorGUILayout.Space(5);

            var colorMapProperty = serializedObject.FindProperty("colorMap");
            colorMapProperty.FindPropertyRelative("mapType").enumValueIndex = (int)StampBase.StampMap.StampMapType.Color;

            Border.Start();
            EditorGUILayout.PropertyField(colorMapProperty, new GUIContent("Color"));
            Border.End(GetMapTypeColor(StampBase.StampMap.StampMapType.Color));

            EditorGUILayout.Space(5);

            var stampMapsProperty = serializedObject.FindProperty("stampMaps");

            for (var i = 0; i < stampMapsProperty.arraySize; i++) {

                var propertyAtIndex = stampMapsProperty.GetArrayElementAtIndex(i);

                if (propertyAtIndex != null) {

                    propertyAtIndex.FindPropertyRelative("selfIndex").intValue = i;
                    propertyAtIndex.FindPropertyRelative("mapType").enumValueIndex = (int)StampBase.StampMap.StampMapType.Other;

                }

            }

            Border.Start();
            EditorGUILayout.PropertyField(stampMapsProperty, new GUIContent("Splat Layers", "splatmap layers:\n\nAdd layers to write to the splatmap of your terrain."));
            Border.End(GetMapTypeColor(StampBase.StampMap.StampMapType.Other));

        }

        private static void DrawMaskEditor(SerializedObject serializedObject) {

            Border.Start();

            if (AtlasPainter.editing) {

                if (AtlasPainter.currentStamp == serializedObject.targetObject) {

                    EditorGUILayout.HelpBox("click and drag to paint\nhold [shift] to remove\nhold [shift] and scroll to change size\nhold [shift] + [ctrl] and scroll to change opacity", MessageType.Info);

                    AtlasPainter.brush = (Texture2D)EditorGUILayout.ObjectField("brush", AtlasPainter.brush, typeof(Texture2D), false);

                    AtlasPainter.size = EditorGUILayout.Slider("size", AtlasPainter.size, 0.01f, 1f);

                    AtlasPainter.opacity = EditorGUILayout.Slider("opacity", AtlasPainter.opacity, 0.0f, 1f);

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("apply")) {

                        AtlasPainter.ApplyChange("Assets/Atlas Terrain Editor/PaintedMasks");// AtlasSettingsAsset.GetOrCreateSettings().paintedMasksPath);

                        AtlasPainter.StopEditStampMask();

                    }

                    if (GUILayout.Button("clear")) {

                        AtlasPainter.Clear();

                    }

                    if (GUILayout.Button("reset")) {

                        AtlasPainter.Reset();

                        AtlasPainter.StopEditStampMask();

                    }

                    if (GUILayout.Button("cancel")) {

                        AtlasPainter.StopEditStampMask();

                    }

                    GUILayout.EndHorizontal();

                } else {

                    EditorGUILayout.HelpBox("Currently editing another stamp mask.", MessageType.Info);

                }

            } else {

                if (GUILayout.Button("edit mask")) {

                    AtlasPainter.EditStampMask((Stamp)serializedObject.targetObject);

                    AtlasStamper.QueRender();

                }

            }

            Border.End();

        }

        private static Color GetMapTypeColor(StampBase.StampMap.StampMapType mapType) {

            switch (mapType) {

                case StampBase.StampMap.StampMapType.Height: return Color.red;
                case StampBase.StampMap.StampMapType.Color: return Color.cyan;
                case StampBase.StampMap.StampMapType.Mask: return Color.black;
                default: return Color.white;

            }

        }

    }

}

