//using MonadGames.Toolkit;

using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Zayebunny.GameDB.GameParameters
{
    [DefaultExecutionOrder(5)]
    public abstract class SyncedBehaviour : MonoBehaviour
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif
    {
        [SerializeField] [FoldoutGroup("Param sync settings", 999)]
        protected bool syncOnStart = true;


        private bool _synchronize;


        protected virtual void Awake()
        {
            /* if (MTK_TickManager.HasInstance)
             {
                 MTK_TickManager.Instance.AddSubscriber(this);
             }*/

            _synchronize = true;
        }

        protected virtual void Start()
        {
            if (!syncOnStart) return;
            Sync();
        }

        private void Sync()
        {
            if (!_synchronize)
            {
                this.SyncRemoteGameParameters(true);
                _synchronize = true;
            }
            else
            {
                this.SyncRemoteGameParameters();
            }
        }

#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            /*  if (UnityEditor.EditorApplication.isPlaying) return;
              this.SyncRemoteGameParameters();*/
        }


        public void OnAfterDeserialize()
        {
        }
#endif
    }
}