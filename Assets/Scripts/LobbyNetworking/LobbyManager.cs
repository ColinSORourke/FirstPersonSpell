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
    public Vector3[] spawnPoints;

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
    private int playerColor;
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

        StartCoroutine(WaitForSceneToSpawn("Net Arches Map Prototype"));
    }

    public IEnumerator EndGameCountdown() {
        if (isCountdown) yield break;
        isCountdown = true;
        countdown = 8;
        SetCountdownTextClientRpc(countdown.ToString());
        SetCountdownActiveClientRpc(true);
        for (; countdown > 0; countdown--) {
            SetCountdownTextClientRpc(countdown.ToString());
            yield return new WaitForSeconds(1);
        }
        SetCountdownTextClientRpc(countdown.ToString());
        yield return new WaitForSeconds(0.5f);
        EndGameServerRpc();
        isCountdown = false;
        yield break;
    }

    private IEnumerator WaitForSceneToSpawn(string sceneName) {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneName);

        if (IsServer) {
            int i = 0;
            
            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList) {
                Vector3 playerPosition = spawnPoints[i];
                GameObject go = Instantiate(playerPrefab, playerPosition, playerPrefab.transform.rotation);
                go.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
                i += 1;
            }
        }
    }

    public void OnLeaveClicked() {
        if (IsHost) {
            MakeClientLeaveClientRpc();
            StartCoroutine(WaitToLeaveHost());
        } else {
            if (SceneManager.GetActiveScene().name != "NetMenu") {
                RemoveLeaverTargetServerRpc(NetworkManager.Singleton.LocalClientId);
            } else {
                gameNetPortal.transitioning = true;
                GameNetPortal.Instance.RequestDisconnect();
            }
        }
    }

    private IEnumerator WaitToLeaveHost() {
        yield return new WaitUntil(() => playerManager.PlayersInGame == 1);
        yield return new WaitForSeconds(1);
        gameNetPortal.transitioning = true;
        GameNetPortal.Instance.RequestDisconnect();
    }

    public void OnReadyClicked() {
        ToggleReadyServerRpc();
        if (readyImage.color == Color.green) readyImage.color = Color.white;
        else readyImage.color = Color.green;
    }

    private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState) {
        if (SceneManager.GetActiveScene().name == "NetMenu") {
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

            GameObject model = lobbyPlayerModels[j];
            SkinnedMeshRenderer modelRenderer = model.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>();
            modelRenderer.material = model.GetComponent<ModelSkinStorage>().characterMaterials[playerColor];

            j++;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void GetPlayerNameServerRpc(ulong userId, ulong clientId) {
        SetNameStringClientRpc(serverGameNetPortal.GetPlayerData(userId).Value.PlayerName, serverGameNetPortal.GetPlayerData(userId).Value.SelectedColor, new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = new ulong[] { clientId }
            }
        });
    }

    [ClientRpc]
    private void SetNameStringClientRpc(string playerNameFromServer, int playerColorFromServer, ClientRpcParams clientRpcParams = default) {
        playerName = playerNameFromServer;
        playerColor = playerColorFromServer;
        nameUpdated = true;
    }

    [ClientRpc]
    public void MakeClientLeaveClientRpc(ClientRpcParams clientRpcParams = default) {
        if (!IsHost) {
            gameNetPortal.transitioning = true;
            GameNetPortal.Instance.RequestDisconnect();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RemoveLeaverTargetServerRpc(ulong leaverId) {
        RemoveLeaverTargetClientRpc(leaverId);
        MakeClientLeaveClientRpc(new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = new ulong[] { leaverId }
            }
        });
    }

    [ClientRpc]
    public void RemoveLeaverTargetClientRpc(ulong leaverId) {
        if (NetworkManager.Singleton.LocalClientId != leaverId) GameObject.Find("Player " + NetworkManager.Singleton.LocalClientId).GetComponent<PlayerAbilities>().RemoveTarget(leaverId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void EndGameServerRpc() {
        MakeClientLeaveClientRpc();
        StartCoroutine(WaitToLeaveHost());
    }

    public void OnDeckSelected(int selectedDeck) {
        DeckSelectServerRPC(NetworkManager.Singleton.LocalClientId, selectedDeck);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeckSelectServerRPC(ulong clientId, int sDeck){
        ServerGameNetPortal.Instance.SetClientDeck(clientId, sDeck);
    }

    public void OnColorSelected(int selectedColor) {
        ColorSelectServerRPC(NetworkManager.Singleton.LocalClientId, selectedColor);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ColorSelectServerRPC(ulong clientId, int sColor){
        ServerGameNetPortal.Instance.SetClientColor(clientId, sColor);
        ColorSelectClientRPC(clientId, sColor);
    }
    
    [ClientRpc]
    public void ColorSelectClientRPC(ulong clientId, int sColor){
        int ind = PlayerManager.Instance.PlayerIdsInGame.IndexOf(clientId);
        if (clientId == NetworkManager.Singleton.LocalClientId){
            ind = 0;
        } else if (clientId < NetworkManager.Singleton.LocalClientId){
            ind += 1;
        }
        GameObject model = lobbyPlayerModels[ind];
        SkinnedMeshRenderer modelRenderer = model.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>();
        modelRenderer.material = model.GetComponent<ModelSkinStorage>().characterMaterials[sColor];
    }
}