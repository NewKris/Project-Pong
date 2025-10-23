using Unity.Netcode;
using UnityEngine;

namespace NewKris.Runtime {
    public class NetworkDebugPanel : MonoBehaviour {
        private void OnGUI() {
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