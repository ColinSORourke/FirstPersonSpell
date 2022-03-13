using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DapperDino.UMT.Lobby.Networking {
    public class PlayerHUD : NetworkBehaviour
    {
        public ServerGameNetPortal serverGameNetPortal;

        private NetworkVariable<NetworkString> playerName = new NetworkVariable<NetworkString>();

        private NetworkVariable<int> colorInt = new NetworkVariable<int>();
        private NetworkVariable<int> deckInt = new NetworkVariable<int>();

        private bool nameSet = false;

        public MeshRenderer wizRenderer;
        public Material[] characterMaterials;

        public override void OnNetworkSpawn() {
            if (IsServer) {
                playerName.Value = serverGameNetPortal.GetPlayerData(OwnerClientId).Value.PlayerName;
                colorInt.Value = serverGameNetPortal.GetPlayerData(OwnerClientId).Value.SelectedColor;
                deckInt.Value = serverGameNetPortal.GetPlayerData(OwnerClientId).Value.SelectedDeck;
            }
        }

        public void SetName() {
            var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            localPlayerOverlay.text = playerName.Value;
            wizRenderer.material = characterMaterials[colorInt.Value];
            gameObject.GetComponent<PlayerStateScript>().playerCardDeckId = deckInt.Value;
            gameObject.GetComponent<PlayerStateScript>().setDeck();
        }

        void Awake()
        {
            serverGameNetPortal = GameObject.Find("NetPortals").GetComponent<ServerGameNetPortal>();
        }

        private void Update() {
            if (!nameSet && !string.IsNullOrEmpty(playerName.Value)) {
                SetName();
                nameSet = true;
            }
        }
    }
}