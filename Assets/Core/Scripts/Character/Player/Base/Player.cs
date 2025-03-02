using UnityEngine;
using UnityEngine.Events;

namespace Nocci.Character.Player
{
    public class Player : CharacterBase
    {
        public delegate void PlayerChangedDelegate(Player player);

        private static Player s_LocalPlayer;

        public static Player LocalPlayer
        {
            get => s_LocalPlayer;
            private set
            {
                if (s_LocalPlayer == value)
                    return;

                s_LocalPlayer = value;
                LocalPlayerChanged?.Invoke(s_LocalPlayer);
            }
        }

        public Camera WorldCamera { get; private set; }


        protected override void SetupBaseReferences()
        {
            base.SetupBaseReferences();
            WorldCamera = GetComponentInChildren<Camera>();
        }
        
        protected override void Start()
        {
            base.Start();
            AfterInitialized?.Invoke();
        }

        private void OnDestroy()
        {
            if (LocalPlayer == this)
                LocalPlayer = null;
        }

        /// <summary>
        ///     This message will be sent after the first initialized action.
        /// </summary>
        public event UnityAction AfterInitialized;

        /// <summary>
        ///     Player: Current Player
        /// </summary>
        public static event PlayerChangedDelegate LocalPlayerChanged;
    }
}