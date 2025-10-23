using System;
using UnityEngine;

namespace NewKris.Runtime {
    public enum PlayerTeam {
        LEFT,
        RIGHT
    }
    
    public class Goal : MonoBehaviour {
        public static event Action<PlayerTeam> OnGoal; 
        
        public PlayerTeam givePointTo;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Ball")) {
                OnGoal?.Invoke(givePointTo);
            }
        }
    }
}