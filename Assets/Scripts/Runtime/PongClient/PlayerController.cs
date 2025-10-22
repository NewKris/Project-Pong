using System;
using NewKris.Runtime.Common;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewKris.Runtime.PongClient {
    public class PlayerController : NetworkBehaviour {
        public static event Action<PlayerController> OnPlayerSpawned;

        private PlayerPawn _pawn;

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            
            NetworkAction.DoOnAll(() => {
                _pawn = gameObject.GetComponent<PlayerPawn>();
                SetPlayerName();
            });

            NetworkAction.DoOnOwner(this, NotifySpawnRpc);
        }

        public void ReceiveMoveInput(InputAction.CallbackContext context) {
            UpdateMovementInputRpc(context.ReadValue<float>());
        }

        private void SetPlayerName() {
            string currentMode = NetworkManager.Singleton.IsHost ? "Host" 
                : NetworkManager.Singleton.IsServer ? "Server" 
                : "Client";
            
            gameObject.name = $"{currentMode} Player";
        }
        
        [Rpc(SendTo.Server)]
        private void UpdateMovementInputRpc(float movementInput) {
            _pawn.MovementInput = movementInput;
        }

        [Rpc(SendTo.Server)]
        private void NotifySpawnRpc() {
            OnPlayerSpawned?.Invoke(this);
        }
    }
}
