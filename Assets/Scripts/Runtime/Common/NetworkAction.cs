using System;
using Unity.Netcode;

namespace NewKris.Runtime.Common {
    public static class NetworkAction {
        public static void DoOnAll(Action callback) {
            callback();
        }

        public static void DoOnOwner(NetworkBehaviour networkBehaviour, Action callback) {
            if (networkBehaviour.IsOwner) {
                callback();
            }
        }

        public static void DoOnServer(NetworkBehaviour networkBehaviour, Action callback) {
            if (networkBehaviour.IsServer) {
                callback();
            }
        }

        public static void DoOnClient(NetworkBehaviour networkBehaviour, Action callback) {
            if (!networkBehaviour.IsServer) {
                callback();
            }
        }
    }
}