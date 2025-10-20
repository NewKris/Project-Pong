using System;
using UnityEngine;

namespace NewKris.Runtime {
    public class Bounds : MonoBehaviour {
        private static Bounds Instance;
        
        public float maxHeight;
        public float minHeight;

        public static Vector2 ClampPosition(Vector2 position, Vector2 colliderSize) {
            float maxOffset = Instance.maxHeight - colliderSize.y * 0.5f;
            float minOffset = Instance.minHeight + colliderSize.y * 0.5f;
            
            position.y = Mathf.Clamp(position.y, minOffset, maxOffset);
            return position;
        }

        private void Awake() {
            Instance = this;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.white;
            
            Vector2 p1 = new Vector2(-100, maxHeight);
            Vector2 p2 = new Vector2(100, maxHeight);
            Gizmos.DrawLine(p1, p2);
            
            Vector2 p3 = new Vector2(-100, minHeight);
            Vector2 p4 = new Vector2(100, minHeight);
            Gizmos.DrawLine(p3, p4);
        }
    }
}