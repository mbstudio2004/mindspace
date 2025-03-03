using Nocci.Character.Character;
using Nocci.Character.Interfaces;
using UnityEngine;

namespace Nocci.Character
{
    public sealed class CharacterAccuracyHandler : CharacterBehaviour, IAccuracyHandler
    {
        [SerializeField] private float m_AirborneAccuracyMod = 0.5f;

        [SerializeField] private float m_LowerHeightAccuracyMod = 1.1f;

        [SerializeField] private AnimationCurve m_SpeedAccuracyCurve;

        [SerializeField] [Range(0.1f, 10f)] private float m_LerpSpeed = 0.5f;

        private float m_Accuracy = 1f;

        private ICharacterMotor m_Motor;

        public float GetAccuracyMod()
        {
            var targetAccuracy = m_SpeedAccuracyCurve.Evaluate(m_Motor.Velocity.magnitude);

            if (!m_Motor.IsGrounded)
                targetAccuracy *= m_AirborneAccuracyMod;

            if (m_Motor.Height < m_Motor.DefaultHeight)
                targetAccuracy *= m_LowerHeightAccuracyMod;

            m_Accuracy = Mathf.Lerp(m_Accuracy, targetAccuracy, m_LerpSpeed * Time.fixedDeltaTime);
            return m_Accuracy;
        }


        protected override void OnBehaviourEnabled()
        {
            GetModule(out m_Motor);
        }
    }
}