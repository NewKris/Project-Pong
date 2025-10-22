using System.Collections;
using System.Collections.Generic;
using NewKris.Runtime.Common;
using NewKris.Runtime.PongClient;
using Unity.Netcode;
using UnityEngine;

namespace NewKris.Runtime.PongServer {
    public class GameManager : NetworkBehaviour {
        public Transform player1Spawn;
        public Transform player2Spawn;

        private bool _gameInProgress;
        private List<PlayerController> _players = new List<PlayerController>(2);
        
        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            
            NetworkAction.DoOnServer(this, () => {
                PlayerController.OnPlayerSpawned += RegisterPlayer;
            });
            
            NetworkAction.DoOnClient(this, () => {
                gameObject.SetActive(false);
            });
        }

        public override void OnNetworkDespawn() {
            base.OnNetworkDespawn();
            
            NetworkAction.DoOnServer(this, () => {
                PlayerController.OnPlayerSpawned -= RegisterPlayer;
            });
        }

        private void RegisterPlayer(PlayerController player) {
            _players.Add(player);
            PositionPlayer(player);
            TryStartGame();
        }
        
        private void TryStartGame() {
            if (_gameInProgress || _players.Count < 2) {
                return;
            }

            StartCoroutine(RunStartSequence());
        }

        private IEnumerator RunStartSequence() {
            _gameInProgress = true;
            
            yield return CountDown();
            
            StartGame();
        }
        
        private void PositionPlayer(PlayerController player) {
            
        }

        private void StartGame() {
            
        }

        private IEnumerator CountDown() {
            yield break;
        }
        
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