using System;
using UnityEngine;
using UnityEngine.UI;

namespace NewKris.Runtime {
    public class LoadingButton : MonoBehaviour {
        public Transform loadingOverlay;

        public void SetLoading(bool isLoading) {
            loadingOverlay.gameObject.SetActive(isLoading);
        }

        public void AddListener(Action callback) {
            GetComponent<Button>().onClick.AddListener(() => callback());
        }
    }
}