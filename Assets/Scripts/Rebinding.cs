using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Rebinding : MonoBehaviour
{
    [SerializeField] public InputActionAsset actions;
    //[SerializeField] public Controls controls = null;
    //[SerializeField] private InputAction inputAction = null;
    [SerializeField] public InputActionReference inputActionReference = null;
    [SerializeField] private Text bindingDisplayNameText = null;
    [SerializeField] private int bindingIndex = 0;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation = null;

    public void OnEnable()
    {
        bindingDisplayNameText = this.GetComponentInChildren<Text>();
        //LoadKeybindSettings(actions);
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            Debug.Log("Load Existing Keybinds");
            actions.LoadBindingOverridesFromJson(rebinds);
        }
        UpdateDisplayText();
    }
    public void OnDisable()
    {
        SaveKeybindSettings();
    }

    public void OnClick()
    {
        actions.Disable();
        rebindingOperation = inputActionReference.action.PerformInteractiveRebinding()
            .OnComplete(operation => UpdateDisplayText())
            .Start();      
    }
    private void UpdateDisplayText()
    {
        actions.Enable();
        bindingDisplayNameText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
        if (rebindingOperation != null) rebindingOperation.Dispose();
    }
    

    public void SaveKeybindSettings()
    {
        string rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        Debug.Log("Saving Keybinds");
    }
    /*
    public void LoadKeybindSettings(InputActionAsset inputActions)
    {
        if (PlayerPrefs.HasKey("rebinds")) {
            Debug.Log("In");
            var rebinds = PlayerPrefs.GetString("rebinds");
            if (!string.IsNullOrEmpty(rebinds)) inputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }
    */
}

