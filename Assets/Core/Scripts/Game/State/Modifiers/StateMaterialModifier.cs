using System.Collections.Generic;
using UnityEngine;

namespace Nocci.Modifiers
{
    public class StateMaterialModifier : PuzzleObjectStateListener
    {
        [SerializeField] private new Renderer renderer;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private int index;


        [SerializeField] private List<Material> materialByIndex;

        public override void OnStateChanged(int oldState, int newState)
        {
            var materials = renderer.sharedMaterials;
            materials[index] = newState < materialByIndex.Count && newState >= 0 && materialByIndex[newState] != null
                ? materialByIndex[newState]
                : defaultMaterial;
            renderer.sharedMaterials = materials;
        }
    }
}