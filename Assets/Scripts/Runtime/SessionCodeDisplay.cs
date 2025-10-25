using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewKris.Runtime {
    public class SessionCodeDisplay : MonoBehaviour {
        private static string SessionCode = "";
        private static event Action<string> OnCodeUpdated; 

        public TextMeshProUGUI codeDisplay;
        public TextMeshProUGUI copiedFeedback;
        public Button copyButton;

        public static void SetSessionCode(string code) {
            SessionCode = code;
            OnCodeUpdated?.Invoke(code);
        }
        
        private void Awake() {
            copyButton.onClick.AddListener(CopyCodeToClipboard);
            OnCodeUpdated += UpdateDisplay;
            
            UpdateDisplay(SessionCode);
        }

        private void OnDestroy() {
            OnCodeUpdated -= UpdateDisplay;
        }

        private void UpdateDisplay(string code) {
            codeDisplay.text = code;
        }
        
        private void CopyCodeToClipboard() {
            TextEditor te = new () {
                text = SessionCode
            };
            
            te.SelectAll();
            te.Copy();
            te.Delete();
            
            copiedFeedback.gameObject.SetActive(true);
        }
    }
}
