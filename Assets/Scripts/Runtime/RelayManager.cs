using System;
using TMPro;
using UnityEngine;

namespace NewKris.Runtime {
    public class RelayManager : MonoBehaviour {
        public LoadingButton hostButton;
        public LoadingButton joinButton;
        public TMP_InputField joinCodeInput;

        private void Awake() {
            hostButton.AddListener(StartHost);
            joinButton.AddListener(JoinGame);
        }

        private void StartHost() {
            
        }

        private void JoinGame() {
            
        }
    }
}