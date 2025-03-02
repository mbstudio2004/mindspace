#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci
{
    [ExecuteInEditMode]
    public class ScaleLimiter : MonoBehaviour
    {
        [InfoBox("These values are not meant to be changed.\nCHANGING THEM MIGHT BREAK THE GAME.", InfoMessageType.Error)]
        [FoldoutGroup("Settings")] public float minScale = 1f;
        [FoldoutGroup("Settings")] public float maxScale = 2f;

        

        void Update()
        {
            if (Application.isPlaying) return;
            if (transform.hasChanged)
            {
                transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.x, minScale, maxScale);
            }
        }


    }
} 

#endif