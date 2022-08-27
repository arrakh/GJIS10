using System;
using Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D rigidBody;
        public PlayerMovementController movementController;
        
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
            rigidBody.simulated = false;
            movementController.enabled = false;
            Invoke(nameof(Respawn), 3f);
        }

        public void Respawn()
        {
            transform.position = respawnPoint;
            
            rigidBody.simulated = true;
            movementController.enabled = true;
        }

        public void SetRespawnPoint(Vector3 point) => respawnPoint = point;
    }
}