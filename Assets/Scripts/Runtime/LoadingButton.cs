using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NewKris.Runtime {
    public class LoadingButton : MonoBehaviour {
        public GameObject loadingOverlay;
        public UnityEvent onClick;

        public void ResetState() {
            loadingOverlay.SetActive(false);
        }
        
        public void Invoke() {
            loadingOverlay.SetActive(true);
            onClick.Invoke();
        }
    }
}
