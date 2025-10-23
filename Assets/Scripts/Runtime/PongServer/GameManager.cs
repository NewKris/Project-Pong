using System;
using System.Collections;
using System.Collections.Generic;
using NewKris.Runtime.Common;
using NewKris.Runtime.PongClient;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NewKris.Runtime.PongServer {
    public class GameManager : NetworkBehaviourExtended {
        public static readonly NetworkVariable<float> LeftScore = new NetworkVariable<float>(0);
        public static readonly NetworkVariable<float> RightScore = new NetworkVariable<float>(0);
        
        public Transform player1Spawn;
        public Transform player2Spawn;
        public Ball ball;
        public float countDownDuration;

        private bool _gameInProgress;
        private PlayerTeam _lastWinner;
        private readonly List<PlayerController> _registeredPlayers = new List<PlayerController>(2);
        
        public override void OnNetworkSpawn() {
            DoOnServer(() => {
                PlayerController.OnPlayerSpawned += RegisterPlayer;
                Goal.OnGoal += AwardPoint;
                RegisterExistingPlayers();
            });
        }

        public override void OnNetworkDespawn() {
            base.OnNetworkDespawn();
            
            DoOnServer(() => {
                PlayerController.OnPlayerSpawned -= RegisterPlayer;
                Goal.OnGoal -= AwardPoint;
            });
        }

        private void RegisterExistingPlayers() {
            foreach (PlayerController playerController in PlayerController.Players) {
                RegisterPlayer(playerController);
            }
        }

        private void AwardPoint(PlayerTeam toTeam) {
            if (toTeam == PlayerTeam.LEFT) {
                LeftScore.Value++;
            }
            else {
                RightScore.Value++;
            }
            
            StartCoroutine(RunStartRoundSequence());
        }
        
        private void RegisterPlayer(PlayerController player) {
            _registeredPlayers.Add(player);
            PositionPlayer(player);
            TryStartNewGame();
        }
        
        private void TryStartNewGame() {
            if (_gameInProgress || _registeredPlayers.Count < 2) {
                return;
            }

            LeftScore.Value = 0;
            RightScore.Value = 0;
            _lastWinner = PlayerTeam.LEFT;
            
            StartCoroutine(RunStartRoundSequence());
        }

        private IEnumerator RunStartRoundSequence() {
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

            float randAngle = Mathf.Lerp(30, 45, Random.value);
            Vector2 dir = _lastWinner == PlayerTeam.LEFT ? Vector2.left : Vector2.right;
            dir = Quaternion.AngleAxis(randAngle, Vector3.forward) * dir;
            
            ball.Putt(dir);
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