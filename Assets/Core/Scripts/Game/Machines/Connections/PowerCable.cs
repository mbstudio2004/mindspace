using UnityEngine;

namespace Nocci
{
    public class PowerCable : MonoBehaviour
    {
        public PuzzleCableEnd start;
        public PuzzleCableEnd end;

        private PowerConnector startConnector;
        private PowerConnector endConnector;

        private void OnEnable()
        {
            start.OnConnectionChanged += OnConnectionChanged;
            end.OnConnectionChanged += OnConnectionChanged;
        }

        private void OnDisable()
        {
            start.OnConnectionChanged -= OnConnectionChanged;
            end.OnConnectionChanged -= OnConnectionChanged;
        }

        private void OnConnectionChanged((PuzzleCableEnd, PowerConnector, PowerConnector) obj)
        {
            if (obj.Item1 == start && obj.Item3 != null && startConnector == null)
            {
                startConnector = obj.Item3;

                if (endConnector != null)
                {
                    PowerConnectionManager.AddConnection(startConnector, endConnector);
                    PowerConnectionManager.RefreshAllConnections();
                }
            }
            else if (obj.Item1 == start && obj.Item3 == null && startConnector != null)
            {
                if (endConnector != null)
                {
                    PowerConnectionManager.RemoveConnection(startConnector, endConnector);
                    PowerConnectionManager.RefreshAllConnections();
                }

                startConnector = null;
            }

            if (obj.Item1 == end && obj.Item3 != null && endConnector == null)
            {
                endConnector = obj.Item3;

                if (startConnector != null)
                {
                    PowerConnectionManager.AddConnection(startConnector, endConnector);
                    PowerConnectionManager.RefreshAllConnections();
                }
            }
            else if (obj.Item1 == end && obj.Item3 == null && endConnector != null)
            {
                if (startConnector != null)
                {
                    PowerConnectionManager.RemoveConnection(startConnector, endConnector);
                    PowerConnectionManager.RefreshAllConnections();
                }

                endConnector = null;
            }
        }
    }
}