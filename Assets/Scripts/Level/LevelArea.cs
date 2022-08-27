using System;
using Entities;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
#endif

namespace Level
{
    public class LevelArea : MonoBehaviour
    {
        public static UnityEvent<int, int> OnCollectableCountUpdate = new();

        [SerializeField] private Vector3[] safePoints;
        [SerializeField] private BoxCollider2D collider;
        [SerializeField] private Collectible[] collectibles;
        [HideInInspector] public UnityEvent<LevelArea> onEnter;

        private int collectedCount;

        public void Initialize()
        {
            foreach (var collectible in collectibles)
            {
                collectible.ResetItem();
                collectible.onCollected += OnCollected;
            }
        }

        private void OnCollected(Collectible item)
        {
            item.onCollected -= OnCollected;
            collectedCount++;
            OnCollectableCountUpdate?.Invoke(collectedCount, collectibles.Length);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out PlayerController controller)) return;
            
            onEnter?.Invoke(this);
            OnCollectableCountUpdate?.Invoke(collectedCount, collectibles.Length);
            controller.SetRespawnPoint(GetClosestSafePoint(controller.transform.position));
        }

        public (int, int) GetTotalCollectibleCount() => (collectedCount, collectibles.Length);

        public Vector3 GetClosestSafePoint(Vector3 enterArea)
        {
            Vector3 safestDistance = Vector3.zero;
            float distance = Single.MaxValue;
            
            foreach (var point in safePoints)
            {
                var thisDistance = Vector3.Distance(enterArea, point);
                if (thisDistance < distance)
                {
                    distance = thisDistance;
                    safestDistance = point;
                }
            }
            
            return safestDistance;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (safePoints.Length <= 0) safePoints = new[] {transform.position};

            var cam = Camera.main;
            if (cam == null) return;
            
            if (collider == null) collider = GetComponent<BoxCollider2D>();

            var size = cam.orthographicSize;
            collider.size = new Vector2(cam.aspect * size * 2, size * 2);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (var point in safePoints)
                Gizmos.DrawSphere(point, 0.5f);
            
            if (collider == null) collider = GetComponent<BoxCollider2D>();
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, collider.bounds.extents * 2f);

            if (Selection.activeGameObject == null || !Selection.activeGameObject.Equals(gameObject)) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, collider.bounds.extents * 1.99f);
        }
        #endif
    }
}