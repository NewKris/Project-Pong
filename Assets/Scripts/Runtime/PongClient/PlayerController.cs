using System;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace NewKris.Runtime.PongClient {
    public class PlayerController : NetworkBehaviour {
        public static event Action<PlayerController> OnPlayerSpawned;

        private float _movement;
        private PlayerPawn _pawn;

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            
            _pawn = gameObject.GetComponent<PlayerPawn>();
        }

        public void Move(InputAction.CallbackContext context) {
            _movement = context.ReadValue<float>();
        }

        private void Update() {
            _pawn.Move(_movement);
        }
    }
}
