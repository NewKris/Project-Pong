using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewKris.Runtime {
    public class PlayerController : MonoBehaviour {
        public PlayerPawn pawn;

        private InputAction _moveAction;

        private void Awake() {
            _moveAction = InputSystem.actions["Move"];
            InputSystem.actions.Enable();
        }

        private void OnDestroy() {
            _moveAction.Dispose();
            InputSystem.actions.Disable();
        }

        private void Update() {
            pawn.Move(_moveAction.ReadValue<float>());
        }
    }
}
