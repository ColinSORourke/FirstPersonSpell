using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DapperDino.UMT.Lobby.Networking {
    public class GameNetPortal : MonoBehaviour {
        public static GameNetPortal Instance => instance;
        private static GameNetPortal instance;

        public event Action OnNetworkReadied;

        public event Action<ConnectStatus> OnConnectionFinished;
        public event Action<ConnectStatus> OnDisconnectReasonReceived;

        public event Action<ulong, int> OnClientSceneChanged;

        public event Action OnUserDisconnectRequested;

        public InputField joinCodeOutput;
        public RelayManager relayManager;
        public LobbyManager lobbyManager;
        public PlayerManager playerManager;
        public GameObject readyButton;
        public GameObject deckSelectGroup;
        public GameObject colorSelectGroup;

        public GameObject[] toDestroy;
        public bool transitioning;

        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            transitioning = false;
        }

        private void Start() {
            NetworkManager.Singleton.OnServerStarted += HandleNetworkReady;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        }

        private void OnDestroy() {
            if (NetworkManager.Singleton != null) {
                NetworkManager.Singleton.OnServerStarted -= HandleNetworkReady;
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;

                if (NetworkManager.Singleton.SceneManager != null) {
                    NetworkManager.Singleton.SceneManager.OnSceneEvent -= HandleSceneEvent;
                }

                if (NetworkManager.Singleton.CustomMessagingManager == null) { return; }

                UnregisterClientMessageHandlers();
            }
        }

        public void StartHost() {
            //NetworkManager.Singleton.StartHost();
            StartHostAsync();

            //RegisterClientMessageHandlers();
        }

        public async void StartHostAsync() {
            if (relayManager.IsRelayEnabled) {
                joinCodeOutput.text = (await relayManager.SetupRelay()).JoinCode;
                joinCodeOutput.readOnly = true;
            }

            if (NetworkManager.Singleton.StartHost()) {
                Debug.Log("Host Started");
                RegisterClientMessageHandlers();
                playerManager.AddPlayerIdServerRpc(NetworkManager.Singleton.LocalClientId);
                SetReadyActive();
            }
            else {
                Debug.Log("Host Not Started");
            }
        }

        public void RequestDisconnect() {
            OnUserDisconnectRequested?.Invoke();
        }

        private void HandleClientConnected(ulong clientId) {
            if (clientId != NetworkManager.Singleton.LocalClientId) { return; }

            HandleNetworkReady();
            NetworkManager.Singleton.SceneManager.OnSceneEvent += HandleSceneEvent;
        }

        private void HandleSceneEvent(SceneEvent sceneEvent) {
            if (sceneEvent.SceneEventType != SceneEventType.LoadComplete) return;

            OnClientSceneChanged?.Invoke(sceneEvent.ClientId, SceneManager.GetSceneByName(sceneEvent.SceneName).buildIndex);
        }

        private void HandleNetworkReady() {
            if (NetworkManager.Singleton.IsHost) {
                OnConnectionFinished?.Invoke(ConnectStatus.Success);
            }

            OnNetworkReadied?.Invoke();
        }

        public void SetReadyActive() {
            readyButton.SetActive(true);
            deckSelectGroup.SetActive(true);
            colorSelectGroup.SetActive(true);
        }

        #region Message Handlers

        private void RegisterClientMessageHandlers() {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ServerToClientConnectResult", (senderClientId, reader) => {
                reader.ReadValueSafe(out ConnectStatus status);
                OnConnectionFinished?.Invoke(status);
            });

            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ServerToClientSetDisconnectReason", (senderClientId, reader) => {
                reader.ReadValueSafe(out ConnectStatus status);
                OnDisconnectReasonReceived?.Invoke(status);
            });
        }

        private void UnregisterClientMessageHandlers() {
            NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler("ServerToClientConnectResult");
            NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler("ServerToClientSetDisconnectReason");
        }

        #endregion

        #region Message Senders

        public void ServerToClientConnectResult(ulong netId, ConnectStatus status) {
            var writer = new FastBufferWriter(sizeof(ConnectStatus), Allocator.Temp);
            writer.WriteValueSafe(status);
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ServerToClientConnectResult", netId, writer);
        }

        public void ServerToClientSetDisconnectReason(ulong netId, ConnectStatus status) {
            var writer = new FastBufferWriter(sizeof(ConnectStatus), Allocator.Temp);
            writer.WriteValueSafe(status);
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ServerToClientSetDisconnectReason", netId, writer);
        }

        #endregion
    }
}