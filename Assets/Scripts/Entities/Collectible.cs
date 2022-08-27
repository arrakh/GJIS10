using System;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public class Collectible : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject visual;

        [HideInInspector] public Action<Collectible> onCollected;

        private bool isCollected;
        
        public void OnInteract(PlayerController interactor)
        {
            if (isCollected) return;
            
            visual.SetActive(false);
            
            onCollected?.Invoke(this);
        }

        public void ResetItem()
        {
            isCollected = false;
            visual.SetActive(true);
        }
    }
}
