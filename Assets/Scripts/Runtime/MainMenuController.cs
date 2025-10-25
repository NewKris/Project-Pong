using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NewKris.Runtime {
    public class MainMenuController : MonoBehaviour {
        public TMP_InputField joinCodeInput;
        public LoadingButton[] loadingButtons;
        
        public async void HostGame() {
            await StartHostAsync();
        }

        public async void JoinGame() {
            await StartClientAsync();
        }

        public void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void Start() {
            ResetLoadingButtons();
        }

        private async Task StartClientAsync() {
            try {
                JoinAllocation allocationTask = await RelayService.Instance.JoinAllocationAsync(joinCodeInput.text);
                
                RelayServerData serverData = allocationTask.ToRelayServerData("dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
            
                NetworkManager.Singleton.StartClient();
            }
            catch (Exception e) {
                Debug.LogError($"Failed to start Client: {e.Message}");
                ResetLoadingButtons();
            }
        }

        private async Task StartHostAsync() {
            try {
                Allocation allocationTask = await RelayService.Instance.CreateAllocationAsync(2);
                
                RelayServerData serverData = allocationTask.ToRelayServerData("dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
                
                string joinCodeTask = await RelayService.Instance.GetJoinCodeAsync(allocationTask.AllocationId);
                SessionCodeDisplay.SetSessionCode(joinCodeTask);
                
                NetworkManager.Singleton.StartHost();
                NetworkManager.Singleton.SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
            }
            catch (Exception e) {
                Debug.LogError($"Failed to start Host: {e.Message}");
                ResetLoadingButtons();
            }
        }

        private IEnumerator WaitForTask<T>(Task<T> task) {
            while (!task.IsCompleted) {
                yield return null;
            }
        }

        private void ResetLoadingButtons() {
            foreach (LoadingButton loadingButton in loadingButtons) {
                loadingButton.ResetState();
            }
        }
    }
}
