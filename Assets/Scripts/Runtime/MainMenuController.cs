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
        
        public void HostGame() {
            StartCoroutine(StartHostAsync());
        }

        public void JoinGame() {
            StartCoroutine(StartClientAsync());
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

        private IEnumerator StartClientAsync() {
            yield return ClientSession.WaitForNetworkInstantiation();
            
            Task<JoinAllocation> allocationTask = RelayService.Instance.JoinAllocationAsync(joinCodeInput.text);
            yield return WaitForTask(allocationTask);

            if (!allocationTask.IsCompletedSuccessfully) {
                Debug.LogError("Failed to join Relay Server");
                ResetLoadingButtons();
                yield break;
            }
            
            RelayServerData serverData = allocationTask.Result.ToRelayServerData("dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
            
            NetworkManager.Singleton.StartClient();
        }

        private IEnumerator StartHostAsync() {
            yield return ClientSession.WaitForNetworkInstantiation();

            Task<Allocation> allocationTask = RelayService.Instance.CreateAllocationAsync(2);
            yield return WaitForTask(allocationTask);

            if (!allocationTask.IsCompletedSuccessfully) {
                Debug.LogError("Failed to secure allocation from RelayService");
                ResetLoadingButtons();
                yield break;
            }

            RelayServerData serverData = allocationTask.Result.ToRelayServerData("dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
            
            Task<string> joinCodeTask = RelayService.Instance.GetJoinCodeAsync(allocationTask.Result.AllocationId);
            yield return WaitForTask(joinCodeTask);

            if (!joinCodeTask.IsCompletedSuccessfully) {
                Debug.LogError("Failed to get join code from RelayService");
                ResetLoadingButtons();
                yield break;
            }
            
            SessionCodeDisplay.SetSessionCode(joinCodeTask.Result);
            
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
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
