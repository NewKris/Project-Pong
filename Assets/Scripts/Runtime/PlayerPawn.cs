using UnityEngine;

namespace NewKris.Runtime {
    public class PlayerPawn : MonoBehaviour {
        public float moveSpeed;

        private Vector2 _colliderSize;
        
        public float MovementInput { get; set; }
        public bool CanMove { get; set; }

        private void Awake() {
            _colliderSize = GetComponentInChildren<Collider2D>().bounds.size;
            CanMove = true;
        }

        private void LateUpdate() {
            if (!CanMove) {
                return;
            }
            
            transform.position += Vector3.up * (MovementInput * moveSpeed * Time.deltaTime);
            transform.position = Bounds.ClampPosition(transform.position, _colliderSize);            
        }
    }
}