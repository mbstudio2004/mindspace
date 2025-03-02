using System;
using Nocci.Character;
using Nocci.Interaction;
using Nocci.Managers;
using Nocci.Pickuping;
using UnityEngine;

namespace Nocci
{
    public class GravityGun : CharacterBehaviour
    {
        [SerializeField] private InteractionHandler m_InteractionHandler;

        [SerializeField] private float pickupForce = 400f;
        [SerializeField] private float dropForce = 0f;
        [SerializeField] private float shootForce = 50f;
        [SerializeField] private Vector3 offset = new(0, 0, 2f);

        [SerializeField] private Collider playerCollider;
        

        public PickupableObject CurrentObject { get; private set; } = null;
        public Quaternion CurrentObjectRotation { get; private set; } = Quaternion.identity;

        protected override void OnBehaviourEnabled()
        {
            m_InteractionHandler.Interacted += OnInteract;
            UpdateManager.AddFixedUpdate(OnFixedUpdate);
        }

        protected override void OnBehaviourDisabled()
        {
            m_InteractionHandler.Interacted -= OnInteract;
            UpdateManager.RemoveFixedUpdate(OnFixedUpdate);
        }

        private void OnInteract(IInteractable interactable)
        {
            if (interactable is PickupableObject pickupableObject)
            {
                PickupObject(pickupableObject);
            }
        }

        public void PickupObject(PickupableObject pickupableObject)
        {
            if(CurrentObject != null) return;
            
    
            pickupableObject.Rigidbody.useGravity = false;
            pickupableObject.Rigidbody.velocity = Vector3.zero;
            pickupableObject.Rigidbody.angularVelocity = Vector3.zero;
            pickupableObject.Rigidbody.isKinematic = false;
            
   
            CurrentObjectRotation = pickupableObject.transform.rotation;
            CurrentObject = pickupableObject;
            
           // Debug.Log("Picked up object");
        }

        public void DropObject()
        {
            if(CurrentObject == null) return;
            CurrentObject.Rigidbody.useGravity = true;
            CurrentObject.Rigidbody.AddForce(Character.ViewTransform.forward * dropForce, ForceMode.Impulse);
          
            CurrentObject = null;
            CurrentObjectRotation = Quaternion.identity;
           // Debug.Log("Dropped object");
        }

        public void ShootObject()
        {
            if(CurrentObject == null) return;

            CurrentObject.Rigidbody.useGravity = true;
            CurrentObject.Rigidbody.AddForce(Character.ViewTransform.forward * shootForce, ForceMode.Impulse);

            CurrentObjectRotation = Quaternion.identity;
            CurrentObject = null;
        }



        private Vector3 GetForwardVector()
        {
            var good = Quaternion.AngleAxis(120, GetRightVector()) * Character.transform.up;
           // return Character.ViewTransform.forward;
            return Vector3.Angle(Character.transform.up, Character.ViewTransform.forward) >
                   Vector3.Angle(Character.transform.up, good) ? good : Character.ViewTransform.forward;
        }
        
        private Vector3 GetRightVector()
        {
            return Character.ViewTransform.right;
        }
        
        private Vector3 GetUpVector()
        {
            return Vector3.Cross(GetForwardVector(), GetRightVector()).normalized;
        }
        
        private void OnFixedUpdate()
        {
            if (!CurrentObject) return;

            var targetPosition = Character.ViewTransform.position + GetForwardVector() * offset.z +
                                 GetRightVector() * offset.x + GetUpVector() * offset.y;
            
            if (Vector3.Distance(CurrentObject.transform.position, Character.ViewTransform.position) > 5f)
            {
                DropObject();
                return;
            }
            

            if(Vector3.Dot(Vector3.down, Character.ViewTransform.forward) > 0.9f) 
            {
                DropObject();
                return;
            }
            
            var targetRotation = CurrentObjectRotation;
            
            targetRotation = Quaternion.Lerp(CurrentObject.transform.rotation, targetRotation,
                pickupForce * Time.fixedDeltaTime);
            
            CurrentObject.Rigidbody.isKinematic = false;

            CurrentObject.Rigidbody.AddForce( (targetPosition - CurrentObject.transform.position) * pickupForce * CurrentObject.Rigidbody.mass, ForceMode.Force);
            CurrentObject.Rigidbody.MoveRotation(targetRotation);
            
            CurrentObject.Rigidbody.velocity = Vector3.zero;
            CurrentObject.Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}