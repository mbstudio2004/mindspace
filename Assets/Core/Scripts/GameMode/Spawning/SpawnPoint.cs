using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Player.Base
{
    public class SpawnPoint : MonoBehaviour
    {
        private static readonly List<SpawnPoint> s_SpawnPoints = new();
        public static SpawnPoint[] SpawnPoints => s_SpawnPoints.ToArray();

        private void Awake()
        {
            s_SpawnPoints.Add(this);
        }

        private void OnDestroy()
        {
            s_SpawnPoints.Remove(this);
        }


        public virtual SpawnPointData GetSpawnPoint()
        {
            var yAngle = Random.Range(0f, 360f);
            var rotation = Quaternion.Euler(0f, yAngle, 0f);

            return new SpawnPointData(transform.position, rotation);
        }

#if UNITY_EDITOR
        [Button]
        public void SnapToGround()
        {
            // Snaps the spawn point position to the ground.
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out var hitInfo, 10f))
                transform.position = hitInfo.point + Vector3.up * 0.05f;
            else if (Physics.Raycast(transform.position + Vector3.up * 3f, Vector3.down, out hitInfo, 10f))
                transform.position = hitInfo.point + Vector3.up * 0.05f;
        }

        private void OnDrawGizmos()
        {
            var prevColor = Gizmos.color;
            Gizmos.color = new Color(0.1f, 0.9f, 0.1f, 0.35f);

            const float gizmoWidth = 0.5f;
            const float gizmoHeight = 1.8f;

            Gizmos.DrawCube(
                new Vector3(transform.position.x, transform.position.y + gizmoHeight / 2, transform.position.z),
                new Vector3(gizmoWidth, gizmoHeight, gizmoWidth));

            Gizmos.color = prevColor;
        }
#endif
    }
}