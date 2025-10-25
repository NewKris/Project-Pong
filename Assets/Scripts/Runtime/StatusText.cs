using System;
using NewKris.Runtime.Common;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace NewKris.Runtime {
    public class StatusText : NetworkBehaviourExtended {
        private TextMeshProUGUI _text;

        private bool _pulsing;
        private string _baseText;

        [Rpc(SendTo.Everyone)]
        public void SetPulseRpc(bool pulsing) {
            _pulsing = pulsing;
            _text.text = _baseText;
        }
        
        [Rpc(SendTo.Everyone)]
        public void SetStatusTextRpc(string text) {
            _baseText = text;
            _text.text = text;
        }

        public override void OnNetworkSpawn() {
            DoOnServer(() => {
                SetStatusTextRpc("Waiting for players");
                SetPulseRpc(true);
            });
        }

        private void Awake() {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            if (!_pulsing) {
                return;
            }

            _text.text = _baseText + CreateDots();
        }

        private string CreateDots() {
            int dotsCount = Mathf.FloorToInt(Time.time % 4);
            return new string('.', dotsCount);
        }
    }
}