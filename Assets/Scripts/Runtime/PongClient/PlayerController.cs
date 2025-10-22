using System;
using System.Collections.Generic;
using NewKris.Runtime.Common;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewKris.Runtime.PongClient {
    public class PlayerController : NetworkBehaviourExtended {
        public static event Action<PlayerController> OnPlayerSpawned;
        public static readonly List<PlayerController> Players = new List<PlayerController>(2);

        private PlayerPawn _pawn;

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            
            DoOnAll(() => {
                _pawn = gameObject.GetComponent<PlayerPawn>();
                SetPlayerName();
            });

            DoOnServer(() => {
                Players.Add(this);
                NotifySpawn();
            });
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

        private void NotifySpawn() {
            OnPlayerSpawned?.Invoke(this);
        }
        
        [Rpc(SendTo.Server)]
        private void UpdateMovementInputRpc(float movementInput) {
            _pawn.MovementInput = movementInput;
        }
    }
}
