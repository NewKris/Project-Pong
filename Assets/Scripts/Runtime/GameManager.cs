using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewKris.Runtime {
    public class GameManager : MonoBehaviour {
        public Transform player1Spawn;
        public Transform player2Spawn;

        private bool _gameInProgress;
        private List<PlayerController> _players = new List<PlayerController>(2);
        
        private void Awake() {
            PlayerController.OnPlayerSpawned += RegisterPlayer;
        }

        private void OnDestroy() {
            PlayerController.OnPlayerSpawned -= RegisterPlayer;
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