using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    public Button startHostButton;
    public Button startServerButton;
    public Button startClientButton;
    public InputField joinCodeInput;
    public RelayManager relayManager;
    
    private void Awake() {
        Cursor.visible = true;
    }

    private void Start() {
        startHostButton.onClick.AddListener(async () => {
            if (relayManager.IsRelayEnabled) {
                joinCodeInput.text = (await relayManager.SetupRelay()).JoinCode;
                joinCodeInput.readOnly = true;
            }

            if (NetworkManager.Singleton.StartHost()) {
                Debug.Log("Host Started");
            } else {
                Debug.Log("Host Not Started");
            }
        });
        startServerButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartServer()) {
                Debug.Log("Server Started");
            }
            else {
                Debug.Log("Server Not Started");
            }
        });
        startClientButton.onClick.AddListener(async () => {
            if (relayManager.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text)) {
                await relayManager.JoinRelay(joinCodeInput.text);
                joinCodeInput.readOnly = true;
            }

            if (NetworkManager.Singleton.StartClient()) {
                Debug.Log("Client Started");
            }
            else {
                Debug.Log("Client Not Started");
            }
        });
    }

    private void Update() {
        
    }
}
