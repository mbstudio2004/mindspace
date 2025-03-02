using System;
using Nocci.Character;
using Nocci.Zayebunny.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Nocci.Interaction
{
    public class Interactable : MonoBehaviour, IInteractable, IHoverable
    {
        [SerializeField]
        [Tooltip("Is this object interactable, if not, this object will be treated like a normal one.")]
        protected bool m_InteractionEnabled = true;

        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip(
            "How time it takes to interact with this object. (e.g. for how many seconds should the Player hold the interact button).")]
        private float m_HoldDuration;

        [Space]
        [SerializeField]
        [Tooltip("Interactable text (could be used as a name), shows up in the UI when looking at this object.")]
        private string m_InteractTitle;

        [SerializeField]
        [Multiline]
        [Tooltip("Interactable description, shows up in the UI when looking at this object.")]
        private string m_InteractDescription;

        [Space]
        [SerializeField]
        [Tooltip("Unity event that will be called when a character interacts with this object.")]
        protected InteractEvent m_OnInteract;

        [SerializeField] private InteractEvent m_OnHoverStart;

        [SerializeField] private InteractEvent m_OnHoverEnd;

        

        //  [Space] [SerializeField] [ChildGameObjectsOnly]
        // private MaterialEffect m_MaterialEffect;

        protected bool HoverActive { get; private set; }

#if UNITY_EDITOR
        protected void Reset()
        {
            Title = GetType().Name.ToUnityLikeNameFormat();
        }
#endif
        public bool IsHoverable => m_InteractionEnabled;

        public string Title
        {
            get => m_InteractTitle;
            protected set => m_InteractTitle = value;
        }

        public string Description
        {
            get => m_InteractDescription;
            protected set
            {
                m_InteractDescription = value;
                DescriptionChanged?.Invoke();
            }
        }

        public event UnityAction<ICharacter> HoverStarted
        {
            add => m_OnHoverStart.AddListener(value);
            remove => m_OnHoverStart.RemoveListener(value);
        }

        public event UnityAction<ICharacter> HoverEnded
        {
            add => m_OnHoverEnd.AddListener(value);
            remove => m_OnHoverEnd.RemoveListener(value);
        }

        public event UnityAction DescriptionChanged;

        /// <summary>
        ///     Called when a character starts looking at this object.
        /// </summary>
       // [ServerRpc(RunLocally = true)]
        public virtual void OnHoverStart(ICharacter character)
        {
            if (!InteractionEnabled)
                return;

            // if (m_MaterialEffect != null)
            //        m_MaterialEffect.EnableDefaultEffect();

            HoverActive = true;
            
            m_OnHoverStart.Invoke(character);
        }

        /// <summary>
        ///     Called when a character stops looking at this object.
        /// </summary>
       // [ServerRpc(RunLocally = true)]
        public virtual void OnHoverEnd(ICharacter character)
        {
            if (!InteractionEnabled)
                return;

            //   if (m_MaterialEffect != null)
            //      m_MaterialEffect.DisableActiveEffect();

            HoverActive = false;
            m_OnHoverEnd.Invoke(character);
        }

        public float HoldDuration => m_HoldDuration;

        public virtual bool InteractionEnabled
        {
            get => m_InteractionEnabled;
            set
            {
                if (value == m_InteractionEnabled)
                    return;

                m_InteractionEnabled = value;
                InteractionEnabledChanged?.Invoke();
            }
        }

        public event UnityAction<ICharacter> Interacted
        {
            add => m_OnInteract.AddListener(value);
            remove => m_OnInteract.RemoveListener(value);
        }

        public event UnityAction InteractionEnabledChanged;


        /// <summary>
        ///     Called when a character interacts with this object.
        /// </summary>
        //[ServerRpc(RequireOwnership = false, RunLocally = false)]
        public virtual void OnInteract(ICharacter character)
        {
            if (m_InteractionEnabled)
                m_OnInteract.Invoke(character);
            
        }

        #region Internal

        [Serializable]
        public class InteractEvent : UnityEvent<ICharacter>
        {
        }

        #endregion
    }
}