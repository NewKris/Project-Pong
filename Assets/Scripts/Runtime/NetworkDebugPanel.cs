using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewKris.Runtime {
    public class NetworkDebugPanel : MonoBehaviour {
        private bool _display = false;

        private void Awake() {
            InputSystem.actions["Toggle Debug"].performed += _ => _display = !_display;
            InputSystem.actions.actionMaps[1].Enable();
        }

        private void OnDestroy() {
            InputSystem.actions["Toggle Debug"].Dispose();
        }

        private void OnGUI() {
            if (!_display) {
                return;
            }
            
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            if (!NetworkExists()) {
                DrawButtons();
            }
            else {
                DrawInfo();
            }
            
            GUILayout.EndArea();
        }

        private bool NetworkExists() {
            if (!NetworkManager.Singleton) {
                return false;
            }
            
            return NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer;
        }

        private void DrawInfo() {
            string currentMode = NetworkManager.Singleton.IsHost ? "Host" 
                : NetworkManager.Singleton.IsServer ? "Server" 
                : "Client";
            
            GUILayout.Label($"Mode: {currentMode}");
        }

        private void DrawButtons() {
            if (GUILayout.Button("Start Host")) {
                CreateHost();
            }

            if (GUILayout.Button("Start Client")) {
                CreateClient();
            }
        }

        private void CreateHost() {
            NetworkManager.Singleton.StartHost();
        }

        private void CreateClient() {
            NetworkManager.Singleton.StartClient();
        }
    }
}
