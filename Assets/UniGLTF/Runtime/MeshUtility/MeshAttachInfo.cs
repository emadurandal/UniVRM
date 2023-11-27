using UnityEngine;

namespace UniGLTF.MeshUtility
{
    public class MeshAttachInfo
    {
        public Mesh Mesh;
        public Material[] Materials;
        public Transform[] Bones;
        public Transform RootBone;
        public void AttachTo(GameObject dst)
        {
            if (Bones != null)
            {
                var dstRenderer = dst.AddComponent<SkinnedMeshRenderer>();
                dstRenderer.sharedMesh = Mesh;
                dstRenderer.sharedMaterials = Materials;
                dstRenderer.bones = Bones;
                dstRenderer.rootBone = RootBone;
            }
            else
            {
                var dstFilter = dst.AddComponent<MeshFilter>();
                dstFilter.sharedMesh = Mesh;
                var dstRenderer = dst.gameObject.AddComponent<MeshRenderer>();
                dstRenderer.sharedMaterials = Materials;
            }
        }
    }
}