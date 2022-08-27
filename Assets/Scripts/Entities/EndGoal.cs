using System;
using Player;
using UnityEngine;

namespace Entities
{
    public class EndGoal : MonoBehaviour
    {
        public Action onGoal;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out PlayerController player)) return;
            
            onGoal?.Invoke();
        }
    }
}