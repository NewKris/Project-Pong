using System;
using Unity.Netcode;
using UnityEngine;

namespace NewKris.Runtime {
    public class NetworkInstantiator : MonoBehaviour {
        public GameObject networkPrefab;
        
        private void Awake() {
            if (NetworkManager.Singleton == null) {
                Instantiate(networkPrefab);
            }
        }
    }
}
