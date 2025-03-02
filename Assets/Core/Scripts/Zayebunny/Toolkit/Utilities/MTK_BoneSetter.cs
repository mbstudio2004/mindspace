using Nocci.Zayebunny.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Zayebunny.Utilities
{
    public class MTK_BoneSetter : MonoBehaviour
    {
        [SerializeField] private Transform skeletonRoot;
        [SerializeField] private string[] boneNames;
        [SerializeField] private Transform[] bones;


        private void Awake()
        {
            SetBones();
        }

        [Button]
        public void GetBoneNames()
        {
            var sRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (sRenderer == null)
                return;

            var sRendererBones = sRenderer.bones;

            boneNames = new string[sRendererBones.Length];

            for (var i = 0; i < sRendererBones.Length; i++)
                boneNames[i] = sRendererBones[i].name;
        }


        [Button]
        public void SetBones()
        {
            if (skeletonRoot == null)
            {
                UnityEngine.Debug.LogError("Root object is not set!");
                return;
            }

            bones = new Transform[boneNames.Length];

            for (var i = 0; i < boneNames.Length; i++) bones[i] = skeletonRoot.FindInChildren(boneNames[i]);

            var sRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            sRenderer.bones = bones;
            sRenderer.rootBone = skeletonRoot;
        }
    }
}