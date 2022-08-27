using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCollision : MonoBehaviour
    {
        [Serializable]
        public struct CollisionData
        {
            public Vector2 position;
            public Vector2 size;
        }
        
        [Header("Layers")]
        public LayerMask groundLayer;

        [Space]

        public bool onGround;
        public bool onWall;
        public bool onRightWall;
        public bool onLeftWall;
        public int wallSide;

        [Space]

        [Header("Collision")]

        public CollisionData bottom, right, left;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 pos = transform.position;
            onGround = Physics2D.OverlapBox(pos + bottom.position, bottom.size, 0, groundLayer);

            onRightWall = Physics2D.OverlapBox(pos + right.position, right.size, 0, groundLayer);
            
            onLeftWall = Physics2D.OverlapBox(pos + left.position, left.size, 0, groundLayer);

            onWall = onRightWall || onLeftWall;

            wallSide = onRightWall ? -1 : 1;
        }

#if UNITY_EDITOR

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector2 pos = transform.position;

            Gizmos.DrawWireCube(pos + bottom.position, bottom.size);
            Gizmos.DrawWireCube(pos + right.position, right.size);
            Gizmos.DrawWireCube(pos + left.position, left.size);
        }
        #endif
    }
}