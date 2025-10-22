using System;
using System.Collections;
using System.Collections.Generic;
using NewKris.Runtime.Common;
using NewKris.Runtime.PongClient;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace NewKris.Runtime.PongServer {
    public class GameManager : NetworkBehaviourExtended {
        public Transform player1Spawn;
        public Transform player2Spawn;
        public Ball ball;
        public float countDownDuration;

        private bool _gameInProgress;
        private readonly List<PlayerController> _registeredPlayers = new List<PlayerController>(2);
        
        public override void OnNetworkSpawn() {
            DoOnServer(() => {
                PlayerController.OnPlayerSpawned += RegisterPlayer;
                RegisterExistingPlayers();
            });
        }

        public override void OnNetworkDespawn() {
            base.OnNetworkDespawn();
            
            DoOnServer(() => {
                PlayerController.OnPlayerSpawned -= RegisterPlayer;
            });
        }

        private void RegisterExistingPlayers() {
            foreach (PlayerController playerController in PlayerController.Players) {
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
            
            foreach (PlayerController registeredPlayer in _registeredPlayers) {
                PositionPlayer(registeredPlayer);
                registeredPlayer.SetMobilityRpc(false);
            }
            
            ball.ResetPosition();
            
            yield return CountDown();
            
            foreach (PlayerController registeredPlayer in _registeredPlayers) {
                registeredPlayer.SetMobilityRpc(true);
            }
            
            ball.Putt(new Vector2(1, 1));
            
            Debug.Log("Go!");
        }
        
        private void PositionPlayer(PlayerController player) {
            if (_registeredPlayers.IndexOf(player) == 0) {
                player.transform.position = player1Spawn.position;
            }
            else {
                player.transform.position = player2Spawn.position;
            }
        }

        private IEnumerator CountDown() {
            float t = 0;
            float lastSecond = -0.5f;
            
            while (t < countDownDuration) {
                t += Time.deltaTime;

                if (Mathf.Floor(t) > lastSecond) {
                    lastSecond += 1;
                    Debug.Log(Mathf.Floor(t));
                }
                
                yield return null;
            }
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