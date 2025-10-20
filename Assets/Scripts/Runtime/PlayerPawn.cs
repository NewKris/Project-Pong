using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace NewKris.Runtime {
    public class PlayerPawn : MonoBehaviour {
        public float moveSpeed;

        private Vector2 _colliderSize;

        public void SetSpawn(SpawnPoint spawnPoint) {
            float scaleX = spawnPoint.mirrorShape ? -1 : 1;
            
            transform.position = spawnPoint.transform.position;
            transform.localScale = new Vector3(scaleX, 1, 1);
        }
        
        public void Move(float direction) {
            transform.position += Vector3.up * (direction * moveSpeed * Time.deltaTime);
            transform.position = Bounds.ClampPosition(transform.position, _colliderSize);
        }

        private void Awake() {
            _colliderSize = GetComponentInChildren<Collider2D>().bounds.size;
        }
    }
}