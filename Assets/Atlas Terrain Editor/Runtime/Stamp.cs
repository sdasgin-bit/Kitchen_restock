﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Atlas.Unity {

    public class Stamp : StampBase {

        public override bool MayDrawIcon(out string path) {

            //path = "Packages/com.atlas.atlas-terrain-editor/Runtime/Icons/Atlas_icon.tif";

            path = "AtlasIcon.tif";

            return true;

        }

        public override bool MayRender() {

            return !(stamp == null);

        }

        private void OnDrawGizmosSelected() {

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color =  new Color(1, 1, 1, 0.25f);

            Gizmos.DrawWireCube(new Vector3(center.x,size.y*0.5f,center.z), size);

            Gizmos.color = new Color(1, 1, 1, 0.1f);

            Gizmos.DrawCube(new Vector3(center.x,0, center.z), new Vector3(size.x,0,size.z));

        }

        public override void DrawGizmos() {

            Gizmos.matrix = transform.localToWorldMatrix;

            if (AtlasPainter.editing && AtlasPainter.currentStamp == this) {

                Gizmos.color = Color.cyan;

                Gizmos.DrawWireCube(new Vector3(center.x, 0, center.z), new Vector3(size.x, 0, size.z));

                Gizmos.DrawWireCube(new Vector3(center.x, 0, center.z), new Vector3(size.x * 0.99f, 0, size.z * 0.99f));

            }

        }

        public override void DrawMesh(AtlasStamper stampTerrainBase, bool forMask = false) {

            GetCorners(out var p1, out var p2, out var p3, out var p4);

            var p1f = AtlasUtils.LocalPointToTerrainRelativePoint(this, p1, stampTerrainBase);
            var p2f = AtlasUtils.LocalPointToTerrainRelativePoint(this, p2, stampTerrainBase);
            var p3f = AtlasUtils.LocalPointToTerrainRelativePoint(this, p3, stampTerrainBase);
            var p4f = AtlasUtils.LocalPointToTerrainRelativePoint(this, p4, stampTerrainBase);

            var p1t = AtlasUtils.LocalPointToTerrainRelativePoint(this, p1 + (Vector3.up * size.y), stampTerrainBase);
            var p2t = AtlasUtils.LocalPointToTerrainRelativePoint(this, p2 + (Vector3.up * size.y), stampTerrainBase);
            var p3t = AtlasUtils.LocalPointToTerrainRelativePoint(this, p3 + (Vector3.up * size.y), stampTerrainBase);
            var p4t = AtlasUtils.LocalPointToTerrainRelativePoint(this, p4 + (Vector3.up * size.y), stampTerrainBase);

            GL.Begin(GL.QUADS);

            GL.MultiTexCoord2(0, 1, 1);
            GL.MultiTexCoord2(1, p1f.y, p1t.y);
            GL.MultiTexCoord2(2, 1, 1);
            GL.Vertex3(p1f.x, p1f.z, 0);

            GL.MultiTexCoord2(0, 1, 0);
            GL.MultiTexCoord2(1, p2f.y, p2t.y);
            GL.MultiTexCoord2(2, 1, 1);
            GL.Vertex3(p2f.x, p2f.z, 0);

            GL.MultiTexCoord2(0, 0, 0);
            GL.MultiTexCoord2(1, p3f.y, p3t.y);
            GL.MultiTexCoord2(2, 1, 1);
            GL.Vertex3(p3f.x, p3f.z, 0);

            GL.MultiTexCoord2(0, 0, 1);
            GL.MultiTexCoord2(1, p4f.y, p4t.y);
            GL.MultiTexCoord2(2, 1, 1);
            GL.Vertex3(p4f.x, p4f.z, 0);

            GL.End();

        }

        public void GetCorners(out Vector3 p1, out Vector3 p2, out Vector3 p3, out Vector3 p4) {

            p1 = new Vector3(size.x * -0.5f, 0, size.z * 0.5f);
            p2 = new Vector3(size.x * 0.5f, 0, size.z * 0.5f);
            p3 = new Vector3(size.x * 0.5f, 0, size.z * -0.5f);
            p4 = new Vector3(size.x * -0.5f, 0, size.z * -0.5f);

        }

    }

}