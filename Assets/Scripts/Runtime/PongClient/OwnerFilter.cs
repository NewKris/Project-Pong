using System;
using NewKris.Runtime.Common;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace NewKris.Runtime.PongClient {
    public class OwnerFilter : NetworkBehaviour {
        public PlayerInput playerInput;
        public PlayerController playerController;
        public PlayerPawn pawn;
        
        private void Awake() {
            playerInput.enabled = false;
            playerController.enabled = false;
            pawn.enabled = false;
        }

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();

            NetworkAction.DoOnOwner(this, () => {
                playerInput.enabled = true;
                playerController.enabled = true;
            });

            NetworkAction.DoOnServer(this, () => {
                pawn.enabled = true;
            });
        }
    }
}