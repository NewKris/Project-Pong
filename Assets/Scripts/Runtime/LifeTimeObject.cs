using System;
using UnityEngine;

namespace NewKris.Runtime {
    public class LifeTimeObject : MonoBehaviour {
        public float lifeTime = 3;

        private float _timer;
        
        private void OnEnable() {
            _timer = 0;
        }

        private void Update() {
            _timer += Time.deltaTime;
            
            if (_timer >= lifeTime) {
                gameObject.SetActive(false);
            }
        }
    }
}
