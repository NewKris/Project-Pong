using System;
using System.Collections.Generic;
using NewKris.Runtime.Common;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewKris.Runtime.PongClient {
    public class PlayerController : NetworkBehaviourExtended {
        public static event Action<PlayerController> OnPlayerSpawned;
        public static List<PlayerController> players = new List<PlayerController>(2);

        private PlayerPawn _pawn;

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            
            DoOnAll(() => {
                _pawn = gameObject.GetComponent<PlayerPawn>();
                SetPlayerName();
                players.Add(this);
            });

            DoOnOwner(NotifySpawnRpc);
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
