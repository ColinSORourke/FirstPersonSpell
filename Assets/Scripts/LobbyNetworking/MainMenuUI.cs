using DapperDino.UMT.Lobby.Networking;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DapperDino.UMT.Lobby {
    public class MainMenuUI : MonoBehaviour {
        [Header("References")]
        public GameObject[] hostObjectActives;
        public bool[] hostBoolActives;
        public GameObject[] clientObjectActives;
        public bool[] clientBoolActives;
        public MainMenuScene scene;

        [SerializeField] private InputField displayNameInputField;
        [SerializeField] private Image displayNameImage;
        [SerializeField] private InputField joinCodeInputField;
        [SerializeField] private Image joinCodeImage;

        private void Start() {
            PlayerPrefs.GetString("PlayerName");
        }

        public void OnHostClicked() {
            if (!String.IsNullOrEmpty(displayNameInputField.text) && displayNameInputField.text.Length <= 12) {
                displayNameImage.color = Color.white;
                joinCodeImage.color = Color.white;
                for (int i = 0; i < hostObjectActives.Length; i++) hostObjectActives[i].SetActive(hostBoolActives[i]);

                PlayerPrefs.SetString("PlayerName", displayNameInputField.text);

                scene.rotateCamera();
                GameNetPortal.Instance.StartHost();
            } else {
                displayNameImage.color = Color.red;
            }
        }

        public void OnClientClicked() {
            if (joinCodeInputField.text.Length != 6) {
                joinCodeImage.color = Color.red;
            } else if (!String.IsNullOrEmpty(displayNameInputField.text) && displayNameInputField.text.Length <= 12) {
                displayNameImage.color = Color.white;
                joinCodeImage.color = Color.white;
                for (int i = 0; i < clientObjectActives.Length; i++) clientObjectActives[i].SetActive(clientBoolActives[i]);

                PlayerPrefs.SetString("PlayerName", displayNameInputField.text);

                scene.rotateCamera();
                ClientGameNetPortal.Instance.StartClient();
            } else {
                displayNameImage.color = Color.red;
            }
        }
    }
}
