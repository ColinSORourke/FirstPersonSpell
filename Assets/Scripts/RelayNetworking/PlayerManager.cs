using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager Instance => instance;
    private static PlayerManager instance;

    //private NetworkVariable<int> playersInGame = new NetworkVariable<int>();
    private NetworkList<ulong> playerIdsInGame;

    public int PlayersInGame {
        get {
            return playerIdsInGame.Count;
        }
    }

    public NetworkList<ulong> PlayerIdsInGame {
        get {
            Debug.Log("Number: " + PlayersInGame);
            return playerIdsInGame;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        playerIdsInGame = new NetworkList<ulong>();
    }

    private void Start() {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) => {
            if (IsServer) {
                Debug.Log(id + " just connected");
                //playersInGame.Value++;
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) => {
            if (IsServer) {
                Debug.Log(id + " just disconnected");
                //playersInGame.Value--;
            }
        };
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddPlayerIdServerRpc(ulong clientId) {
        playerIdsInGame.Add(clientId);
        Debug.Log("Adding: " + clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemovePlayerIdServerRpc(ulong clientId) {
        if (playerIdsInGame.Contains(clientId)) playerIdsInGame.Remove(clientId);
    }
}
