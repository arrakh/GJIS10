using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class AreaFocus : MonoBehaviour
    {
        [SerializeField] private AnimationCurve moveCurve;
        [SerializeField] private float duration;
        
        private Coroutine moveCoroutine = null;

        public void MoveTo(Vector2 position)
        {
            transform.position = new Vector3(position.x, position.y, transform.position.z);
            Debug.Log($"MOVED TO {position}");
        }
    }
}