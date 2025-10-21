using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace NewKris.Runtime {
    public class PlayerPawn : MonoBehaviour {
        public float moveSpeed;

        private Vector2 _colliderSize;
        
        public void Move(float direction) {
            transform.position += Vector3.up * (direction * moveSpeed * Time.deltaTime);
            transform.position = Bounds.ClampPosition(transform.position, _colliderSize);
        }

        private void Awake() {
            _colliderSize = GetComponentInChildren<Collider2D>().bounds.size;
        }
    }
}