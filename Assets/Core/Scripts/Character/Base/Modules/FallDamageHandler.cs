using Nocci.Character.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Character.Modules
{
    public class FallDamageHandler : CharacterBehaviour
    {
        private IHealthManager HealthManager;
        
        
        [SerializeField]
        private bool m_EnableDamage = true;

        [Space]

        [InfoBox ("At which landing speed, the character will start taking damage.")]
        [SerializeField, Range(1f, 30f)] 
        private float m_MinFallSpeed = 12f;

        [Space]

        [InfoBox("At which landing speed, the character will take maximum damage (die).")]
        [SerializeField, Range(1f, 50f)]
        private float m_FatalFallSpeed = 30f;


        protected override void OnBehaviourEnabled()
        {
            HealthManager = Character.GetModule<IHealthManager>();
            GetModule<ICharacterMotor>().FallImpact += OnFallImpact;
        }

        private void OnFallImpact(float impactSpeed)
        {
            if (!m_EnableDamage)
                return;

            if (impactSpeed >= m_MinFallSpeed)
                HealthManager.ReceiveDamage(-100f * (impactSpeed / m_FatalFallSpeed));
        }
    }
}