using System;
using System.Globalization;
using NewKris.Runtime.PongServer;
using TMPro;
using UnityEngine;

namespace NewKris.Runtime.PongClient {
    public class ScoreDisplay : MonoBehaviour {
        public PlayerTeam showTeamScore;

        private void Awake() {
            if (showTeamScore == PlayerTeam.LEFT) {
                GameManager.LeftScore.OnValueChanged += UpdateDisplay;
            }
            else {
                GameManager.RightScore.OnValueChanged += UpdateDisplay;
            }
        }

        private void OnDestroy() {
            if (showTeamScore == PlayerTeam.LEFT) {
                GameManager.LeftScore.OnValueChanged -= UpdateDisplay;
            }
            else {
                GameManager.RightScore.OnValueChanged -= UpdateDisplay;
            }
        }

        private void UpdateDisplay(float oldValue, float newValue) {
            GetComponent<TextMeshProUGUI>().text = newValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}