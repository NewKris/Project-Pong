using System;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace NewKris.Runtime.PongClient {
    public class PlayerController : NetworkBehaviour {
        public static event Action<PlayerController> OnPlayerSpawned;

        private PlayerPawn _pawn;
        private InputAction _moveAction;

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            
            _pawn = gameObject.GetComponent<PlayerPawn>();
            _moveAction = InputSystem.actions["Move"];
            
            InputSystem.actions.Enable();
        }

        private void Update() {
            _pawn.Move(_moveAction.ReadValue<float>());
        }
    }
}
