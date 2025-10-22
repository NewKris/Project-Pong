using System;
using UnityEngine;

namespace NewKris.Runtime.PongClient {
    public class PlayerPawn : MonoBehaviour {
        public float moveSpeed;

        private Vector2 _colliderSize;
        
        public float MovementInput { get; set; }

        private void Awake() {
            _colliderSize = GetComponentInChildren<Collider2D>().bounds.size;
        }

        private void LateUpdate() {
            transform.position += Vector3.up * (MovementInput * moveSpeed * Time.deltaTime);
            transform.position = Bounds.ClampPosition(transform.position, _colliderSize);            
        }
    }
}