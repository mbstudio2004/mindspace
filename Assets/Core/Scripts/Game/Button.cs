using System;
using Nocci.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Nocci
{
    public class Button : PowerConnector
    {
        public PowerConnector otherConnector;

        public Rigidbody buttonTop;
        public Collider buttonDown;
        public Vector3 buttonUpperLimit;
        public Vector3 buttonLowerLimit;
        public new float threshold;
        public float force = 10;

        private float upperLowerDiff;
        private bool isPressed;
        private bool prevPressedState;
        private bool isPlayerStanding = false;

        public event Action OnButtonPressed;
        public event Action OnButtonReleased;

        [SerializeField] private UnityEvent OnButtonPressedUnityEvent;
        [SerializeField] private UnityEvent OnButtonReleasedUnityEvent;

        private void Start()
        {
            Physics.IgnoreCollision(buttonDown, buttonTop.GetComponent<Collider>());
            upperLowerDiff = buttonUpperLimit.y - buttonLowerLimit.y;

            UpdateManager.AddFixedUpdate(OnFixedUpdate);

            isPowerSource = true;
        }


        private void OnDestroy()
        {
            UpdateManager.RemoveFixedUpdate(OnFixedUpdate);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
        }

        public override void OnFixedUpdate()
        {
            /*buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y, 0);
            buttonTop.transform.localEulerAngles = Vector3.zero;

            if (buttonTop.transform.localPosition.y >= buttonUpperLimit.y)
            {
                buttonTop.transform.localPosition = buttonUpperLimit;
            }
            else
            {
                buttonTop.AddForce(buttonTop.transform.up * (force * Time.fixedDeltaTime), ForceMode.Force);
            }

            if (buttonTop.transform.localPosition.y <= buttonLowerLimit.y)
            {
                buttonTop.transform.localPosition = buttonLowerLimit;
            }

            isPressed = Vector3.Distance(buttonTop.transform.localPosition, buttonLowerLimit) <  upperLowerDiff * threshold;

            switch (isPressed)
            {
                case true when prevPressedState != isPressed:
                    Pressed();
                    break;
                case false when prevPressedState != isPressed:
                    Unpressed();
                    break;
            }*/

            HandlePressure();
            HandleLogic();
        }


        private void HandleLogic()
        {
            if (prevPressedState == isPressed) return;
            /*  if (toggleButton)
              {
                  if (IsPressed)
                      ChangeStatus(Status == PowerState.Powered ? PowerState.Unpowered : PowerState.Powered);
              }
              else
              {*/
            if (isPressed)
            {
                Pressed();
            }
            else
            {
                Unpressed();
            }
            //  ChangeStatus(IsPressed ? PowerState.Powered : PowerState.Unpowered);
            //  }

            prevPressedState = isPressed;
        }

        private void HandlePressure()
        {
            buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y, 0);
            buttonTop.transform.localEulerAngles = Vector3.zero;

            if (buttonTop.transform.localPosition.y <= buttonLowerLimit.y)
                buttonTop.transform.localPosition = buttonLowerLimit;

            if (buttonTop.transform.localPosition.y >= buttonUpperLimit.y)
                buttonTop.transform.localPosition = buttonUpperLimit;
            else
            {
                buttonTop.AddForce(buttonTop.transform.up * force * Time.fixedDeltaTime * (isPlayerStanding ? -1 : 1));
            }

            buttonTop.velocity = Vector3.zero;

            isPressed = Vector3.Distance(buttonTop.transform.localPosition, buttonLowerLimit) <
                        upperLowerDiff * threshold;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            isPlayerStanding = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            isPlayerStanding = true;
        }


        private void Pressed()
        {
            prevPressedState = isPressed;
            OnButtonPressed?.Invoke();
            OnButtonPressedUnityEvent?.Invoke();

            PowerConnectionManager.AddConnection(this, otherConnector);
            PowerConnectionManager.RefreshAllConnections();
        }

        private void Unpressed()
        {
            prevPressedState = isPressed;
            OnButtonReleased?.Invoke();
            OnButtonReleasedUnityEvent?.Invoke();

            PowerConnectionManager.RemoveConnection(this, otherConnector);
            PowerConnectionManager.RefreshAllConnections();
        }
    }
}