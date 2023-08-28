using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Atlas.Unity {

    [CustomEditor(typeof(AtlasUnityTerrainRenderer))]
    [CanEditMultipleObjects]
    public class AtlasUnityTerrainRendererEditor : Editor {

        private SerializedProperty exportColorMap;
        private SerializedProperty exportHeightMap;
        private SerializedProperty exportNormalMap;
        private SerializedProperty exportSplatMaps;
        private SerializedProperty useScattering;
        private SerializedProperty scatterRuleAssets;
        private SerializedProperty useEventTrigger;
        private SerializedProperty onRender;

        private void OnEnable() {

            exportColorMap = serializedObject.FindProperty("exportColorMap");
            exportHeightMap = serializedObject.FindProperty("exportHeightMap");
            exportNormalMap = serializedObject.FindProperty("exportNormalMap");
            exportSplatMaps = serializedObject.FindProperty("exportSplatMaps");
            useScattering = serializedObject.FindProperty("useScattering");
            scatterRuleAssets = serializedObject.FindProperty("scatterRuleAssets");
            useEventTrigger = serializedObject.FindProperty("useEventTrigger");
            onRender = serializedObject.FindProperty("onRender");

        }

        public override void OnInspectorGUI() {

            Border.Start();

            EditorGUILayout.PropertyField(exportColorMap);
            EditorGUILayout.PropertyField(exportHeightMap);
            EditorGUILayout.PropertyField(exportNormalMap);
            EditorGUILayout.PropertyField(exportSplatMaps);

            Border.End(0);

            GUILayout.Space(5);

            Border.Start();

            EditorGUILayout.PropertyField(useEventTrigger);

            if (useEventTrigger.boolValue) {

                EditorGUILayout.PropertyField(onRender);

            }

            Border.End(1);

            GUILayout.Space(5);

            Border.Start();

            EditorGUILayout.PropertyField(useScattering);

            if (useScattering.boolValue) {

                EditorGUILayout.PropertyField(scatterRuleAssets);

            }

            Border.End(2);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Render")) {

                foreach (var i in targets) {

                    (i as AtlasUnityTerrainRenderer).Render("Assets/Atlas Terrain Editor/Export");// AtlasSettingsAsset.GetOrCreateSettings().textureExportPath);

                }

            }

            GUILayout.EndHorizontal();

            if (serializedObject.hasModifiedProperties) {

                serializedObject.ApplyModifiedProperties();

                foreach( var i in targets) {

                    (i as AtlasUnityTerrainRenderer).SetEventToRuntimeAndEditor();

                }

            }

        }

    }

}
