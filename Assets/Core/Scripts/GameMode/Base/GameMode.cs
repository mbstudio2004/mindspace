using UnityEngine;


namespace Nocci.GameMode
{
    [DefaultExecutionOrder(-900)]
    public abstract class GameMode : MonoBehaviour
    {
        /*public static event Action OnGameModeLoaded;
        
        [SerializeField] protected GameObject m_SceneCamera;


        private void Start()
        {
            WorldManager.GameLoaded += OnGameLoaded;

            if (!WorldManager.IsLoading)
                OnGameLoaded();
            if (m_SceneCamera != null)
            {
                m_SceneCamera.SetActive(false);
                Destroy(m_SceneCamera);
            }
        }
        

        protected virtual void OnDestroy()
        {
            WorldManager.GameLoaded -= OnGameLoaded;
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            if (m_SceneCamera == null)
            {
                var camera = GetComponentInChildren<Camera>(true);

                if (camera != null)
                    m_SceneCamera = camera.gameObject;
            }
        }
#endif

        protected virtual void OnGameLoaded()
        {
            WorldManager.GameLoaded -= OnGameLoaded;
            
        }

        protected void SetPlayerPosition(SpawnPointData spawnData)
        {
        }

        public virtual void TryGetPlayer(Guid guid, out GameObject player)
        {
            player = default;
        }
   
        public virtual SpawnPointData GetSpawnData()
        {
            var spawnPoint = SpawnPointData.Default;

            if (spawnPoint == SpawnPointData.Default)
            {
                var spawnPoints = SpawnPoint.SpawnPoints;
                if (spawnPoints.Length > 0)
                    spawnPoint = spawnPoints.SelectRandom().GetSpawnPoint();
            }

            if (spawnPoint != SpawnPointData.Default)
                return spawnPoint;

            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out var hitInfo, 10f))
                spawnPoint = new SpawnPointData(hitInfo.point + Vector3.up * 0.1f, transform.rotation);
            else
                spawnPoint = new SpawnPointData(transform.position, transform.rotation);

            return spawnPoint;
        }*/
    }
}