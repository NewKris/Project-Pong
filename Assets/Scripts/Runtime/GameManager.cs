using System;
using UnityEngine;

namespace NewKris.Runtime {
    public class GameManager : MonoBehaviour {
        public Transform player1Spawn;
        public Transform player2Spawn;

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            
            if (player1Spawn) {
                Gizmos.DrawWireCube(player1Spawn.position, Vector3.one);
            }

            if (player2Spawn) {
                Gizmos.DrawWireCube(player2Spawn.position, Vector3.one);
            }
        }
    }
}