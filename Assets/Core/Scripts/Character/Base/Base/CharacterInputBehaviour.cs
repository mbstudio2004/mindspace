using Nocci.Input;
using Nocci.Utils;
using UnityEngine;

namespace Nocci.Character
{
    public abstract class CharacterInputBehaviour : InputBehaviour
    {

#if UNITY_EDITOR
        protected CharacterBase Character { get; private set; }
#else
        protected CharacterBase Character;
#endif

        private bool m_Initialized;


        protected virtual void OnBehaviourEnabled(ICharacter character)
        {
        }

        protected virtual void OnBehaviourDisabled(ICharacter character)
        {
        }

        protected sealed override void OnEnable()
        {
            if (Character == null)
                Character = transform.root.GetComponentInChildren<CharacterBase>();

            if (Character == null)
            {
                Debug.LogError($"({gameObject.name}) No parent character found.", this);
                return;
            }

            if (Character.IsInitialized)
            {
                OnBehaviourEnabled(Character);
                m_Initialized = true;
                base.OnEnable();
            }
            else
            {
                Character.Initialized += OnInitialized;
            }
        }

        protected sealed override void OnDisable()
        {
            if (!m_Initialized || UnityUtils.IsQuittingPlayMode)
                return;

            OnBehaviourDisabled(Character);

            base.OnDisable();
        }

        private void OnInitialized()
        {
            Character.Initialized -= OnInitialized;
            OnBehaviourEnabled(Character);
            m_Initialized = true;
            base.OnEnable();
        }
    }
}