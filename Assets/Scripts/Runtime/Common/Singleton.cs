using UnityEngine;

namespace NewKris.Runtime.Common {
    public static class Singleton {
        public static bool CreateSingleton<T>(ref T singleton, T instance) where T: MonoBehaviour {
            if (singleton == null) {
                singleton = instance;
                Object.DontDestroyOnLoad(instance.gameObject);
                return true;
            }
            else {
                Object.Destroy(instance.gameObject);
                return false;
            }
        }

        public static bool UnsetSingleton<T>(ref T singleton, T instance) where T : MonoBehaviour {
            if (singleton == instance) {
                singleton = null;
                return true;
            }

            return false;
        }
    }
}
