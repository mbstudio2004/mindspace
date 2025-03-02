using Nocci.Zayebunny.GameDB.Base;
using UnityEngine;
using UnityEngine.Playables;


namespace Nocci.Interaction.GameDB
{
    public class InteractionObject : BaseObject
    {
        public PlayableAsset Asset => GetTimeline();
        
        public bool InteractingWithObject
        {
            get => interactingWithObject;
        }

        [SerializeField] private PlayableAsset _asset;
        [SerializeField] private bool interactingWithObject;
        
        private PlayableAsset GetTimeline()
        {
            return _asset;
        }
    }
}