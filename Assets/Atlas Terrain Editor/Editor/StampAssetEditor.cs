using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Atlas.Unity {

    [CustomEditor(typeof(StampAsset))]
    [CanEditMultipleObjects]
    public class StampAssetEditor : Editor {

        private RenderTexture previewRT;
        private Material previewMat;

        private SerializedProperty height;
        private SerializedProperty mask;

        /*
        public Texture2D height;
        public Texture2D color;
        [Tooltip("Main mask of the stamp.")]
        public Texture2D mask;
        public Texture2D splat1;
        public Texture2D splat2;
        [Tooltip("Used exclusively for road blending, this mask is usually a copy of the main mask but narrower.")]
        public Texture2D roadMask;
*/

        public void OnEnable() {

            height = serializedObject.FindProperty("height");
            mask = serializedObject.FindProperty("mask");

        }

        public void OnDisable() {

            if (previewRT != null) {

                previewRT.Release();

                GameObject.DestroyImmediate(previewRT, false);

            }

            if (previewMat != null) {

                GameObject.DestroyImmediate(previewMat, false);

            }

        }

        public override void OnInspectorGUI() {

            DrawDefaultInspector();

            if( height.objectReferenceValue != null && height.objectReferenceValue is Texture2D && !TextureIs16Or32Bit(height.objectReferenceValue as Texture2D)) {

                EditorGUILayout.HelpBox("Height texture is lower than 16bit.\n16 or 32bit is adviced.\n\nTexture format: " + (height.objectReferenceValue as Texture2D).format.ToString(), MessageType.Warning);

            } 

            if (mask.objectReferenceValue != null && mask.objectReferenceValue is Texture2D && !TextureIs16Or32Bit(mask.objectReferenceValue as Texture2D)) {

                EditorGUILayout.HelpBox("Mask texture is lower than 16bit.\n16 or 32bit is adviced.\n\nTexture format: " + (mask.objectReferenceValue as Texture2D).format.ToString(), MessageType.Warning);

            }

        }

        private bool TextureIs16Or32Bit(Texture2D texture) {

            var format = texture.format;

            return format == TextureFormat.R16 ||
                format == TextureFormat.RG32 ||
                format == TextureFormat.RFloat ||
                format == TextureFormat.RGBAFloat ||
                format == TextureFormat.RGFloat ||
                format == TextureFormat.RGBAHalf ||
                format == TextureFormat.RGHalf ||
                format == TextureFormat.RHalf;

        }

        public override bool HasPreviewGUI() {
            return true;
        }

        public override void DrawPreview(Rect previewArea) {

            var o = (StampAsset)target;

            if (o == null || o.color == null || o.mask == null && o.height != null) {

                return;

            }

            var resolution = (int)Mathf.Min(previewArea.width, previewArea.height);
            
            if( previewRT == null ) {

                previewRT = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);

            } 

            if( previewMat == null ) {

                previewMat = new Material(Shader.Find("Hidden/Atlas/AtlasThumbnail"));
                
                previewMat.SetTexture("_MainTex", o.color);
                previewMat.SetTexture("_Mask", o.mask);
                previewMat.SetTexture("_Height", o.height);

                Graphics.Blit(null, previewRT, previewMat);

                RenderTexture.active = null;

            }

            GUI.DrawTexture(new Rect(previewArea.x, previewArea.y, resolution, resolution), previewRT);

        }

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {

            var o = (StampAsset)target;

            if (o == null || o.color == null || o.mask == null && o.height != null) {

                return null;

            }

            var rt = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);

            var shader = Shader.Find("Hidden/Atlas/AtlasThumbnail");

            if( shader != null) {

                var mat = new Material(shader);

                mat.SetTexture("_MainTex", o.color);
                mat.SetTexture("_Mask", o.mask);
                mat.SetTexture("_Height", o.height);

                Graphics.Blit(null, rt, mat);

                RenderTexture.active = rt;

                Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, true, true);

                tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);

                tex.Apply();

                RenderTexture.active = null;

                rt.Release();

                GameObject.DestroyImmediate(rt, false);

                GameObject.DestroyImmediate(mat, false);

                return tex;

            }

            return null;

        }

    }

}