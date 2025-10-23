using System;
using System.Collections.Generic;
using NewKris.Runtime.Common;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace NewKris.Runtime {
    public class PlayerController : NetworkBehaviourExtended {
        public static event Action<PlayerController> OnPlayerSpawned;
        public static readonly List<PlayerController> Players = new List<PlayerController>(2);

        private PlayerPawn _pawn;

        [Rpc(SendTo.Server)]
        public void SetMobilityRpc(bool canMove) {
            _pawn.CanMove = canMove;
        }
        
        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            
            DoOnAll(() => {
                _pawn = gameObject.GetComponent<PlayerPawn>();
            });

            DoOnServer(() => {
                Players.Add(this);
                NotifySpawn();
            });
        }

        public override void OnNetworkDespawn() {
            base.OnNetworkDespawn();
            
            DoOnServer(() => {
                Players.Clear();
            });
        }

        public void ReceiveMoveInput(InputAction.CallbackContext context) {
            UpdateMovementInputRpc(context.ReadValue<float>());
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
