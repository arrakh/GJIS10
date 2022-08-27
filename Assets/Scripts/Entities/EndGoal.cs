using System;
using Player;
using UnityEngine;

namespace Entities
{
    public class EndGoal : MonoBehaviour
    {
        public Action onGoal;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerController player)) return;
            
            onGoal?.Invoke();
        }
    }
}