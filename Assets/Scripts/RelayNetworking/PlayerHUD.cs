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

        private bool nameSet = false;

        public override void OnNetworkSpawn() {
            if (IsServer) {
                playerName.Value = serverGameNetPortal.GetPlayerData(OwnerClientId).Value.PlayerName;
            }
        }

        public void SetName() {
            var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            localPlayerOverlay.text = playerName.Value;
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