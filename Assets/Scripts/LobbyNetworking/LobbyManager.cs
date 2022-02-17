using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.UMT.Lobby.Networking;
using DapperDino.UMT.Lobby.UI;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : NetworkBehaviour
{
    public GameObject[] lobbyPlayerModels;
    public Text countdownText;
    public Image readyImage;
    public GameObject playerPrefab;
    public PlayerManager playerManager;
    public ServerGameNetPortal serverGameNetPortal;
    public GameNetPortal gameNetPortal;

    private int countdown;
    private bool isCountdown;

    private NetworkList<LobbyPlayerState> lobbyPlayers;
    private string playerName;
    private bool nameUpdated;
    private bool containsId;

    private void Awake() {
        lobbyPlayers = new NetworkList<LobbyPlayerState>();
        containsId = false;

        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn() {
        if (IsClient) {
            lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
        }
        
        if (IsServer) {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList) {
                HandleClientConnected(client.ClientId);
            }
        }
    }

    public override void OnDestroy() {
        base.OnDestroy();

        lobbyPlayers.OnListChanged -= HandleLobbyPlayersStateChanged;

        if (NetworkManager.Singleton) {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }
    }

    private bool IsEveryoneReady() {
        foreach (var player in lobbyPlayers) {
            if (!player.IsReady) {
                return false;
            }
        }

        return true;
    }

    private void HandleClientConnected(ulong clientId) {
        StartCoroutine(WaitForPlayer(clientId));
    }

    private IEnumerator WaitForPlayer(ulong clientId) {
        yield return new WaitUntil(() => {
            CheckIdServerRpc(clientId);
            return containsId;
        });
        HandleClientConnectedReady(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CheckIdServerRpc(ulong clientId) {
        SetIdBoolClientRpc(!playerManager.PlayerIdsInGame.Contains(clientId), new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = new ulong[] { clientId }
            }
        });
    }

    [ClientRpc]
    private void SetIdBoolClientRpc(bool containsIdFromServer, ClientRpcParams clientRpcParams = default) {
        containsId = containsIdFromServer;
    }

    private void HandleClientConnectedReady(ulong clientId) {
        Debug.Log("Connecting " + clientId);
        var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);

        if (!playerData.HasValue) { return; }

        lobbyPlayers.Add(new LobbyPlayerState(
            clientId,
            playerData.Value.PlayerName,
            false
        ));
        Debug.Log(playerData.Value.PlayerName);
    }

    public void HandleClientDisconnect(ulong clientId) {
        for (int i = 0; i < lobbyPlayers.Count; i++) {
            if (lobbyPlayers[i].ClientId == clientId) {
                lobbyPlayers.RemoveAt(i);
                playerManager.RemovePlayerIdServerRpc(clientId);
                break;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default) {
        for (int i = 0; i < lobbyPlayers.Count; i++) {
            if (lobbyPlayers[i].ClientId == serverRpcParams.Receive.SenderClientId) {
                lobbyPlayers[i] = new LobbyPlayerState(
                    lobbyPlayers[i].ClientId,
                    lobbyPlayers[i].PlayerName,
                    !lobbyPlayers[i].IsReady
                );
                if (IsEveryoneReady()) {
                    countdown = 3;
                    if (!isCountdown) StartCoroutine(StartGameCountdown());
                }
            }
        }
    }

    private IEnumerator StartGameCountdown() {
        isCountdown = true;
        SetCountdownActiveClientRpc(true);
        for (; countdown > 0; countdown--) {
            SetCountdownTextClientRpc(countdown.ToString());
            yield return new WaitForSeconds(1);
            if (!IsEveryoneReady()) {
                SetCountdownActiveClientRpc(false);
                isCountdown = false;
                yield break;
            }
        }
        SetCountdownTextClientRpc(countdown.ToString());
        yield return new WaitForSeconds(0.5f);
        StartGameServerRpc();
        isCountdown = false;
        yield break;
    }

    [ClientRpc]
    private void SetCountdownActiveClientRpc(bool active) {
        countdownText.gameObject.SetActive(active);
    }

    [ClientRpc]
    private void SetCountdownTextClientRpc(string text) {
        countdownText.text = text;
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartGameServerRpc(ServerRpcParams serverRpcParams = default) {
        if (!IsEveryoneReady()) { return; }

        lobbyPlayers.OnListChanged -= HandleLobbyPlayersStateChanged;

        ServerGameNetPortal.Instance.StartGame();

        StartCoroutine(WaitForSceneToSpawn("NetGameplay"));
    }

    private IEnumerator WaitForSceneToSpawn(string sceneName) {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneName);

        if (IsServer) {
            Vector3 playerPosition = new Vector3(-24f, 1.5f, 0f);
            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList) {
                GameObject go = Instantiate(playerPrefab, playerPosition, playerPrefab.transform.rotation);
                go.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
                playerPosition.x += 12f;
            }
        }
    }

    /*[ClientRpc]
    private void SpawnPlayerClientRpc() {
        GameObject go = Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
    }*/

    public void OnLeaveClicked() {
        if (IsHost) {
            MakeClientLeaveClientRpc();
            StartCoroutine(WaitToLeaveHost());
        } else {
            gameNetPortal.transitioning = true;
            GameNetPortal.Instance.RequestDisconnect();
        }
    }

    private IEnumerator WaitToLeaveHost() {
        yield return new WaitUntil(() => playerManager.PlayersInGame == 1);
        gameNetPortal.transitioning = true;
        GameNetPortal.Instance.RequestDisconnect();
    }

    public void OnReadyClicked() {
        ToggleReadyServerRpc();
        if (readyImage.color == Color.green) readyImage.color = Color.white;
        else readyImage.color = Color.green;
    }

    private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState) {
        for (int i = 0; i < lobbyPlayerModels.Length; i++) {
            if (lobbyPlayers.Count > i) {
                lobbyPlayerModels[i].SetActive(true);
            }
            else {
                lobbyPlayerModels[i].SetActive(false);
            }
        }

        StartCoroutine(WaitForPlayerName(lobbyPlayers.Count));
    }

    private IEnumerator WaitForPlayerName(int playersCount) {
        Debug.Log("OCI: " + NetworkManager.Singleton.LocalClientId);
        yield return new WaitUntil(() => playerManager.PlayersInGame >= playersCount);
        nameUpdated = false;
        GetPlayerNameServerRpc(NetworkManager.Singleton.LocalClientId, NetworkManager.Singleton.LocalClientId);
        yield return new WaitUntil(() => nameUpdated);
        Debug.Log("Id: " + NetworkManager.Singleton.LocalClientId + "  " + "Name: " + playerName);
        var localPlayerOverlay = lobbyPlayerModels[0].GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = playerName;
        int j = 1;
        foreach (ulong clientId in playerManager.PlayerIdsInGame) {
            Debug.Log("All Ids: " + clientId);
            if (clientId == NetworkManager.Singleton.LocalClientId) continue;

            nameUpdated = false;
            GetPlayerNameServerRpc(clientId, NetworkManager.Singleton.LocalClientId);
            yield return new WaitUntil(() => nameUpdated);
            localPlayerOverlay = lobbyPlayerModels[j].GetComponentInChildren<TextMeshProUGUI>();
            localPlayerOverlay.text = playerName;
            j++;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void GetPlayerNameServerRpc(ulong userId, ulong clientId) {
        SetNameStringClientRpc(serverGameNetPortal.GetPlayerData(userId).Value.PlayerName, new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = new ulong[] { clientId }
            }
        });
    }

    [ClientRpc]
    private void SetNameStringClientRpc(string playerNameFromServer, ClientRpcParams clientRpcParams = default) {
        playerName = playerNameFromServer;
        nameUpdated = true;
    }

    [ClientRpc]
    public void MakeClientLeaveClientRpc(ClientRpcParams clientRpcParams = default) {
        if (!IsHost) {
            gameNetPortal.transitioning = true;
            GameNetPortal.Instance.RequestDisconnect();
        }
    }
}