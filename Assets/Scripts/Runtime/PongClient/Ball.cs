using NewKris.Runtime.Common;
using Unity.Netcode.Components;
using UnityEngine;

namespace NewKris.Runtime.PongClient {
    public class Ball : NetworkBehaviourExtended {
        public Vector2 startPosition;
        public float maxSpeed;
        
        private NetworkRigidbody2D _rigidBody;

        public void ResetPosition() {
            _rigidBody.SetPosition(startPosition);
            _rigidBody.SetLinearVelocity(Vector2.zero);
        }
        
        public void Putt(Vector2 direction) {
            _rigidBody.SetLinearVelocity(direction.normalized * maxSpeed);
        }

        private void Awake() {
            _rigidBody = GetComponent<NetworkRigidbody2D>();
        }
    }
}