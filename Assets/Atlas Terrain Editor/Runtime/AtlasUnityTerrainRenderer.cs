using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Atlas.Unity {

    [RequireComponent(typeof(Terrain))]
    public class AtlasUnityTerrainRenderer : MonoBehaviour {

        [Tooltip("When enabled objects will be scattered based on the assigned scattering rules.")]
        public bool useScattering = false;
        public List<ScatterRuleAsset> scatterRuleAssets;
        [Tooltip("When enabled the color map will be exported.")]
        public bool exportColorMap;
        [Tooltip("When enabled the heightmap will be exported.")]
        public bool exportHeightMap;
        [Tooltip("When enabled the normal map will be exported.")]
        public bool exportNormalMap;
        [Tooltip("When enabled the splat map will be exported.")]
        public bool exportSplatMaps;
        [Tooltip("When enabled events can be triggered whenever this terrain renders again")]
        public bool useEventTrigger;
        [Tooltip("Event triggered whenever we stop editing o when manually pressing the render button")]
        public UnityEvent onRender;

        public List<int> preTreePrototypeIndexes;
        public List<int> preDetailPrototypeIndexes;

        public bool firstRender = true;

        public void Render(string directoryPath = "") {

#if UNITY_EDITOR

            if( firstRender ) {

                if(!UnityEditor.EditorUtility.DisplayDialog("Atlas Terrain Renderer", "Atlas is about to alter your unity terrain do you wish to proceed?\n\nCancel if you want to backup your terrain.", "proceed", "cancel")) {

                    return;

                }

                firstRender = false;

            }

#endif

            var terrain = GetComponent<Terrain>();

            if (terrain != null) {

                using (var stamper = new AtlasStamper(new AtlasTerrainData(transform, new Vector3(terrain.terrainData.size.x, terrain.terrainData.size.y * 2f, terrain.terrainData.size.z), new Vector2Int(1, 1)), terrain.terrainData.heightmapResolution - 1, terrain.terrainData.alphamapResolution, true, false)) {

                    stamper.Render(true);

                    ApplyTerrain(terrain, stamper);

                    Scatter(terrain, stamper);

                    ExportTextures(stamper,directoryPath);

                }

            }

            if( useEventTrigger && onRender != null ) {

                onRender.Invoke();

            }

        }

        [ContextMenu("Scatter")]
        public void Scattering() {

            var terrain = GetComponent<Terrain>();

            if (terrain != null) {

                using (var stamper = new AtlasStamper(new AtlasTerrainData(transform, new Vector3(terrain.terrainData.size.x, terrain.terrainData.size.y * 2f, terrain.terrainData.size.z), new Vector2Int(1, 1)), terrain.terrainData.heightmapResolution - 1, terrain.terrainData.alphamapResolution, true, false)) {

                    stamper.Render(true);

                    Scatter(terrain, stamper);

                }

            }

        }

        [ContextMenu("Clear Scattering")]
        public void ClearScattering() {

            var terrain = GetComponent<Terrain>();

            if (terrain != null) {

                terrain.terrainData.treeInstances = new TreeInstance[0];

            }

        }

        [ContextMenu("Clear Detail")]
        public void ClearDetail() {

            var terrain = GetComponent<Terrain>();

            if (terrain != null) {

                for( var i = 0; i < terrain.terrainData.detailPrototypes.Length; i++) {

                    terrain.terrainData.SetDetailLayer(0, 0, i, new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight]);

                }

            }

        }

        private void ApplyTerrain(Terrain terrain, AtlasStamper stamper) {

            var heightMap = stamper.renderPool.GetRenderTexture(AtlasStamper.RenderPool.RenderTextureType.FinalHeight);

            RenderTexture.active = heightMap;

            terrain.terrainData.CopyActiveRenderTextureToHeightmap(new RectInt(0, 0, heightMap.width, heightMap.height), new Vector2Int(0, 0), TerrainHeightmapSyncControl.HeightAndLod);

            if (terrain.terrainData.alphamapLayers > 0) {

                var splat1Map = stamper.renderPool.GetRenderTexture(AtlasStamper.RenderPool.RenderTextureType.FinalSplat1);

                RenderTexture.active = splat1Map;

                if (terrain.terrainData.alphamapResolution != splat1Map.width) {

                    terrain.terrainData.alphamapResolution = splat1Map.width;

                }

                terrain.terrainData.CopyActiveRenderTextureToTexture("alphamap", 0, new RectInt(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight), new Vector2Int(0, 0), false);

                if (terrain.terrainData.alphamapLayers > 4) {

                    var splat2Map = stamper.renderPool.GetRenderTexture(AtlasStamper.RenderPool.RenderTextureType.FinalSplat2);

                    RenderTexture.active = splat2Map;

                    terrain.terrainData.CopyActiveRenderTextureToTexture("alphamap", 1, new RectInt(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight), new Vector2Int(0, 0), false);

                }

            }

            terrain.terrainData.SetBaseMapDirty();

            terrain.Flush();

            RenderTexture.active = null;

        }

        private void Scatter(Terrain terrain, AtlasStamper stamper) {

            if (scatterRuleAssets != null && scatterRuleAssets.Count > 0) {

                using (var scatterer = new AtlasScatterer(stamper, terrain, scatterRuleAssets)) {

                    scatterer.ClearTreeInstances(preTreePrototypeIndexes);

                    scatterer.ClearDetails(preDetailPrototypeIndexes);

                    scatterer.Render();

                    preTreePrototypeIndexes = scatterer.GetTreePrototypeIndexesToScatter();

                    preDetailPrototypeIndexes = scatterer.GetDetailPrototypeIndexesToScatter();

                }

            }

        }

        private void ExportTextures(AtlasStamper stamper, string exportDirectoryPath) {

#if UNITY_EDITOR

            if( string.IsNullOrEmpty(exportDirectoryPath)) {

                return;

            }

            if( !AtlasUtils.IsValidPath(exportDirectoryPath, out var error)) {

                Debug.LogError("Atlas.Unity.AtlasUnityTerrainRenderer.ExportTextures: invalid path: " + error);

                return;

            }

            if (exportColorMap || exportHeightMap || exportNormalMap || exportSplatMaps) {

                //var exportDirectoryPath = "Assets/Atlas Terrain Editor/Export";
                var exportFilename = gameObject.name + "_" + gameObject.GetInstanceID().ToString();

                if (exportColorMap) {

                    stamper.renderPool.Export(AtlasStamper.RenderPool.RenderTextureType.FinalColor, exportDirectoryPath, exportFilename);

                }

                if (exportHeightMap) {

                    stamper.renderPool.Export(AtlasStamper.RenderPool.RenderTextureType.FinalHeight, exportDirectoryPath, exportFilename);

                }

                if (exportNormalMap) {

                    stamper.renderPool.Export(AtlasStamper.RenderPool.RenderTextureType.FinalNormal, exportDirectoryPath, exportFilename);

                }

                if (exportSplatMaps) {

                    stamper.renderPool.Export(AtlasStamper.RenderPool.RenderTextureType.FinalSplat1, exportDirectoryPath, exportFilename);
                    stamper.renderPool.Export(AtlasStamper.RenderPool.RenderTextureType.FinalSplat2, exportDirectoryPath, exportFilename);
                    stamper.renderPool.Export(AtlasStamper.RenderPool.RenderTextureType.FinalSplat3, exportDirectoryPath, exportFilename);
                    stamper.renderPool.Export(AtlasStamper.RenderPool.RenderTextureType.FinalSplat4, exportDirectoryPath, exportFilename);

                }

                UnityEditor.AssetDatabase.Refresh();

            }

#endif

        }

        public void SetEventToRuntimeAndEditor() {

            if (onRender != null) {

                for (var i = 0; i < onRender.GetPersistentEventCount(); i++) {

                    onRender.SetPersistentListenerState(i, UnityEventCallState.EditorAndRuntime);

                }

            }

        }

    }

}