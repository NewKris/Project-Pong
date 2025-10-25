using System;
using TMPro;
using UnityEngine;

namespace NewKris.Runtime {
    public class LoadingDots : MonoBehaviour {
        private TextMeshProUGUI _text;

        private void OnEnable() {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            _text.text = CreateDots();
        }
        
        private string CreateDots() {
            int dotsCount = Mathf.FloorToInt(Time.time % 4);
            return new string('.', dotsCount);
        }
    }
}
