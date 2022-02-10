using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Rebinding : MonoBehaviour
{
    [SerializeField] public InputActionAsset actions;
    [SerializeField] public InputActionReference inputActionReference = null;
    [SerializeField] private Text bindingDisplayNameText = null;
    [SerializeField] private GameObject waitingForInputText = null;
    [SerializeField] private int bindingIndex = 0;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation = null;

    public void OnEnable()
    {
        bindingDisplayNameText = this.GetComponentInChildren<Text>();
        LoadKeybindSettings();
        UpdateDisplayText();
    }
    public void OnDisable()
    {
        SaveKeybindSettings();
    }

    public void OnClick()
    {
        actions.Disable();
        waitingForInputText.SetActive(true);
        rebindingOperation = inputActionReference.action.PerformInteractiveRebinding(bindingIndex)
            //.WithTargetBinding(bindingIndex)
            .OnComplete(operation => {
                if (CheckDuplicateBinding())
                {
                    inputActionReference.action.RemoveBindingOverride(bindingIndex);
                }
                actions.Enable();
                waitingForInputText.SetActive(false);
                CleanUp();
                UpdateDisplayText();
                
            })
            .Start();      
    }

    private void UpdateDisplayText()
    {
        bindingDisplayNameText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
    }

    private void CleanUp()
    {
        if (rebindingOperation != null) rebindingOperation.Dispose();
    }
    
    private bool CheckDuplicateBinding()
    {
        InputBinding newbinding = inputActionReference.action.bindings[bindingIndex];
        Debug.Log("newBinding: " + newbinding.id);
        foreach (InputBinding binding in actions.bindings)
        {
            if (binding.id == newbinding.id) continue;
            if (binding.effectivePath == newbinding.effectivePath)
            {
                Debug.Log("Duplicate Bindings found: " + newbinding.effectivePath + " vs. " + binding);
                return true;
            }
        }
        return false;
    }

    public void SaveKeybindSettings()
    {
        string rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        Debug.Log("Saving Keybinds");
    }
    
    public void LoadKeybindSettings()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            Debug.Log("Load Existing Keybinds");
            actions.LoadBindingOverridesFromJson(rebinds);
        }
    }
    
    public void ResetBinding()
    {
        actions.RemoveAllBindingOverrides();
        SaveKeybindSettings();
    }
}

