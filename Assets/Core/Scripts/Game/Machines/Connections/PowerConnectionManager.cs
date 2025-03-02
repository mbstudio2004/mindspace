using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nocci
{
    public static class PowerConnectionManager
    {
        public static readonly List<(PowerConnector, PowerConnector)> Connections = new();
        public static readonly List<PowerConnector> AllConnectors = new();

        public static event Action<(PowerConnector, PowerConnector, bool)> OnConnectionChanged = delegate { };


        public static void RefreshAllConnections()
        {
            var list = new List<PowerConnector>();
            foreach (var connector in AllConnectors)
            {
                if (connector.isPowerSource)
                {
                    connector.EnablePower();
                    list.Add(connector);
                }
                else
                    connector.DisablePower();
            }

            foreach (var powerSources in list)
            {
                //bfs algorithm from powersources to turn on all connected connectors
                var queue = new Queue<PowerConnector>();
                queue.Enqueue(powerSources);
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    foreach (var connection in Connections)
                    {
                        if (connection.Item1 == current && !connection.Item2.IsPowered &&
                            !queue.Contains(connection.Item2))
                        {
                            connection.Item2.EnablePower();
                            queue.Enqueue(connection.Item2);
                        }

                        if (connection.Item2 == current && !connection.Item1.IsPowered &&
                            !queue.Contains(connection.Item1))
                        {
                            connection.Item1.EnablePower();
                            queue.Enqueue(connection.Item1);
                        }
                    }
                }
            }
        }

        public static void AddConnector(PowerConnector connector)
        {
            if (!AllConnectors.Contains(connector))
                AllConnectors.Add(connector);
        }

        public static void RemoveConnector(PowerConnector connector)
        {
            AllConnectors.Remove(connector);
        }

        public static void AddConnection(PowerConnector connector1, PowerConnector connector2)
        {
            if (Connections.Contains((connector1, connector2)) &&
                Connections.Contains((connector2, connector1))) return;

            Connections.Add((connector1, connector2));
            
            OnConnectionChanged.Invoke((connector1, connector2, true));
        }

        public static void RemoveConnection(PowerConnector connector1, PowerConnector connector2)
        {
            Connections.Remove((connector1, connector2));
            Connections.Remove((connector2, connector1));

            OnConnectionChanged.Invoke((connector1, connector2, false));
        }
    }
}