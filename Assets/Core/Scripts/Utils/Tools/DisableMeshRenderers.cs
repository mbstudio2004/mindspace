using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci
{
    public class DisableMeshRenderers : MonoBehaviour
    {
        [Button]
        public void DisableMesh()
        { 
            foreach (var VARIABLE in GetComponentsInChildren<Renderer>())
            {
                VARIABLE.enabled = false;
            }
        }

        [Button]
        public void EnableMesh()
        {
            foreach (var VARIABLE in GetComponentsInChildren<Renderer>())
            {
                VARIABLE.enabled = true;
            }
        }
    }
}