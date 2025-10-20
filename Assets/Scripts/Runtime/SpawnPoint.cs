using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NewKris.Runtime {
    public class SpawnPoint : MonoBehaviour {
        private static HashSet<SpawnPoint> SpawnPoints = new HashSet<SpawnPoint>();

        public bool mirrorShape;
        
        private bool _taken;
        
        public static SpawnPoint GetNextSpawnPoint() {
            return SpawnPoints.FirstOrDefault(p => !p._taken);
        }
        
        private void Awake() {
            SpawnPoints.Add(this);
        }

        private void OnDestroy() {
            SpawnPoints.Remove(this);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}