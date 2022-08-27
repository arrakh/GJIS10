using System;
using Entities;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D rigidBody;
        public PlayerMovementController movementController;
        public MMF_Player killFeedback;
        public MMF_Player respawnFeedback;
        
        [HideInInspector]
        public UnityEvent onPlayerDeath = new();

        private Vector3 respawnPoint;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<IInteractable>(out var interactable))
                interactable.OnInteract(this);
        }

        public void Kill()
        {
            SetEnable(false);
            Invoke(nameof(Respawn), 3f);
            killFeedback.PlayFeedbacks();
        }

        public void Respawn()
        {
            transform.position = respawnPoint;
            
            SetEnable(true);
            
            respawnFeedback.PlayFeedbacks();
        }

        public void SetEnable(bool enable)
        {
            rigidBody.simulated = enable;
            movementController.enabled = enable;
            if (enable) movementController.ResetMovement();
        }

        public void SetRespawnPoint(Vector3 point) => respawnPoint = point;
    }
}