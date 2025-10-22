using System;
using System.Collections;
using System.Collections.Generic;
using NewKris.Runtime.Common;
using NewKris.Runtime.PongClient;
using Unity.Netcode;
using UnityEngine;

namespace NewKris.Runtime.PongServer {
    public class GameManager : NetworkBehaviourExtended {
        public Transform player1Spawn;
        public Transform player2Spawn;

        private bool _gameInProgress;
        private List<PlayerController> _registeredPlayers = new List<PlayerController>(2);
        
        public override void OnNetworkSpawn() {
            DoOnServer(() => {
                PlayerController.OnPlayerSpawned += RegisterPlayer;
                RegisterExistingPlayers();
            });
            
            DoOnClient(() => {
                gameObject.SetActive(false);
            });
        }

        public override void OnNetworkDespawn() {
            base.OnNetworkDespawn();
            
            DoOnServer(() => {
                PlayerController.OnPlayerSpawned -= RegisterPlayer;
            });
        }

        private void RegisterExistingPlayers() {
            foreach (PlayerController playerController in PlayerController.players) {
                RegisterPlayer(playerController);
            }
        }
        
        private void RegisterPlayer(PlayerController player) {
            _registeredPlayers.Add(player);
            PositionPlayer(player);
            TryStartGame();
        }
        
        private void TryStartGame() {
            if (_gameInProgress || _registeredPlayers.Count < 2) {
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
            if (_registeredPlayers.IndexOf(player) == 0) {
                player.transform.position = player1Spawn.position;
            }
            else {
                player.transform.position = player2Spawn.position;
            }
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