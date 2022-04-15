using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AliveManager : NetworkBehaviour
{
    public static AliveManager Instance => instance;
    private static AliveManager instance;

    //private NetworkVariable<int> alivesInGame = new NetworkVariable<int>();
    private NetworkList<ulong> aliveIdsInGame;

    public int AlivesInGame {
        get {
            return aliveIdsInGame.Count;
        }
    }

    public NetworkList<ulong> AliveIdsInGame {
        get {
            Debug.Log("Number: " + AlivesInGame);
            return aliveIdsInGame;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);

        aliveIdsInGame = FindObjectOfType<PlayerManager>().PlayerIdsInGame;
    }

    private void Start() {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) => {
            if (IsServer) {
                Debug.Log(id + " just connected");
                //alivesInGame.Value++;
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) => {
            if (IsServer) {
                Debug.Log(id + " just disconnected");
                //alivesInGame.Value--;
            }
        };
    }

    public override void OnDestroy() {
        // do nothing
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddAliveIdServerRpc(ulong clientId) {
        aliveIdsInGame.Add(clientId);
        Debug.Log("Adding: " + clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveAliveIdServerRpc(ulong clientId) {
        Debug.Log("Removing: " + clientId);
        if (aliveIdsInGame.Contains(clientId)) aliveIdsInGame.Remove(clientId);
    }
}
