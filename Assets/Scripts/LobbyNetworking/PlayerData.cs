using UnityEngine;

namespace DapperDino.UMT.Lobby.Networking {
    public struct PlayerData {
        public string PlayerName { get; private set; }
        public ulong ClientId { get; private set; }

        public int SelectedDeck { get; private set; }
        public int SelectedColor { get; private set; }

        public PlayerData(string playerName, ulong clientId, int selectedDeck, int selectedColor) {
            PlayerName = playerName;
            ClientId = clientId;
            SelectedDeck = selectedDeck;
            SelectedColor = selectedColor;
        }

        public void SetDeck(int selectedDeck) {
            SelectedDeck = selectedDeck;
            Debug.Log(ClientId + " Deck: " + SelectedDeck);
        }

        public void SetColor(int selectedColor) {
            SelectedColor = selectedColor;
        }
    }
}