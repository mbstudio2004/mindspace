using Nocci.Character;
using Nocci.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Nocci
{
    public class FPSGravityGunInput : CharacterInputBehaviour
    {
        [SerializeField] private GravityGun gravityGun;
        [Title("Actions")] [SerializeField] private InputActionReference m_ShootInput;
        [SerializeField] private InputActionReference m_DropInput;


        protected override void OnInputEnabled()
        {
            m_ShootInput.RegisterPerformed(OnShoot);
            m_DropInput.RegisterPerformed(OnDrop);
        }

        private void OnDrop(InputAction.CallbackContext obj)
        {
            gravityGun.DropObject();
        }

        private void OnShoot(InputAction.CallbackContext obj)
        {
            gravityGun.ShootObject();
        }


        protected override void OnInputDisabled()
        {
            m_ShootInput.UnregisterPerfomed(OnShoot);
            m_DropInput.UnregisterPerfomed(OnDrop);
        }
    }
}