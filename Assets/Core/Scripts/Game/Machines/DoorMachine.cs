using UnityEngine;


namespace Nocci
{
    public class DoorMachine : PuzzleObjectMachineListener
    {
        public Transform doorTransform;
        
        public bool isOpenFirst = false;
        public float speed = 5;
        public Vector3 openPosition;
        public Vector3 closePosition;
        
        private Vector3 m_TargetPosition;

        public override void OnUpdate()
        {
            if (Vector3.Distance(transform.position, m_TargetPosition) < 0.1f) return;
            doorTransform.position = Vector3.MoveTowards(doorTransform.position, m_TargetPosition + transform.position, Time.deltaTime * speed);
        }

        private void Start()
        {
            m_TargetPosition = isOpenFirst ? openPosition : closePosition;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + openPosition, Vector3.one);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + closePosition, Vector3.one);
        }

        protected override void OnMachineStop()
        {
            m_TargetPosition = isOpenFirst ? openPosition : closePosition;
        }

        protected override void OnMachineStart()
        {
           m_TargetPosition = isOpenFirst ? closePosition : openPosition;
        }
    }
}
