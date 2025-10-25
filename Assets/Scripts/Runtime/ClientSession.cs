using System;
using System.Collections;
using NewKris.Runtime.Common;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace NewKris.Runtime {
    public class ClientSession : MonoBehaviour {
        private static ClientSession Instance;

        public GameObject networkPrefab;
        
        private async void Awake() {
            if (Singleton.CreateSingleton(ref Instance, this)) {
                Instantiate(networkPrefab);
                
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private void OnDestroy() {
            if (Singleton.UnsetSingleton(ref Instance, this)) {
                AuthenticationService.Instance.SignOut();
            }
        }
    }
}
