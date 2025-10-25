using System;
using System.Collections;
using NewKris.Runtime.Common;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace NewKris.Runtime {
    public class ClientSession : MonoBehaviour {
        private static bool NetworkInstantiated = false;
        private static ClientSession Instance;

        public GameObject networkPrefab;
        
        public static IEnumerator WaitForNetworkInstantiation() {
            while (!NetworkInstantiated) {
                yield return null;
            }
        }
        
        private async void Awake() {
            if (Singleton.CreateSingleton(ref Instance, this)) {
                NetworkInstantiated = false;
                
                Instantiate(networkPrefab);
                
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                
                NetworkInstantiated = true;
            }
        }

        private void OnDestroy() {
            if (Singleton.UnsetSingleton(ref Instance, this)) {
                AuthenticationService.Instance.SignOut();
                NetworkInstantiated = false;
            }
        }
    }
}
