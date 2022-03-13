using UnityEngine;
using System;

namespace DapperDino.UMT.Lobby.Networking {
    public struct PlayerData {
        public string PlayerName { get; private set; }
        public ulong ClientId { get; private set; }

        public int SelectedDeck;
        public int SelectedColor; 

        public PlayerData(string playerName, ulong clientId, int selectedDeck, int selectedColor) {
            PlayerName = playerName;
            ClientId = clientId;
            SelectedDeck = selectedDeck;
            SelectedColor = selectedColor;
        }
    }
}