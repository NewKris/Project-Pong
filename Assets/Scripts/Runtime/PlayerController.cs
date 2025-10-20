using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewKris.Runtime {
    public class PlayerController : NetworkBehaviour {
        public PlayerPawn pawn;

        private InputAction _moveAction;
        
        public override void OnNetworkSpawn() {
            _moveAction = InputSystem.actions["Move"];
            InputSystem.actions.Enable();
            pawn.SetSpawn(SpawnPoint.GetNextSpawnPoint());
        }

        public override void OnNetworkDespawn() {
            _moveAction.Dispose();
            InputSystem.actions.Disable();
        }

        private void Update() {
            pawn.Move(_moveAction.ReadValue<float>());
        }
    }
}
