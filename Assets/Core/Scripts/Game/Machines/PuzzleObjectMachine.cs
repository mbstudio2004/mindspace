using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci
{
    public class PuzzleObjectMachine : PuzzleObjectBehaviour
    {
        [ShowInInspector] [ReadOnly] public bool IsRunning { get; private set; }

        public event Action OnStartRunning = delegate { };
        public event Action OnStopRunning = delegate { };

        [DisableInPlayMode] public bool defaultState = false;


        [SerializeField] private PowerConnector connector;

        [SerializeField] private bool useAdditionalConnectors;

        [ShowIf(nameof(useAdditionalConnectors))] [SerializeField]
        private List<PowerConnector> connectors;


        private void Start()
        {
            if (connector)
                connector.OnPowerChanged += OnPowerChanged;
            if (defaultState)
            {
                StartMachine();
            }
        }

        private bool AreAllConnectorsPowered()
        {
            if (useAdditionalConnectors)
            {
                foreach (var powerConnector in connectors)
                {
                    if (!powerConnector.IsPowered)
                        return false;
                }
            }

            return true;
        }

        private void OnPowerChanged(bool value)
        {
            if (value)
            {
                if (!useAdditionalConnectors)
                    StartMachine();
                else
                {
                    if (AreAllConnectorsPowered())
                    {
                        StartMachine();
                    }
                }
            }
            else
            {
                StopMachine();
            }
        }

        public override void OnBehaviourDisabled()
        {
            if (connector)
                connector.OnPowerChanged -= OnPowerChanged;

            StopMachine();
        }

        [Button]
        public void StartMachine()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            OnStartRunning?.Invoke();
        }


        [Button]
        public void StopMachine()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            OnStopRunning?.Invoke();
        }
    }
}