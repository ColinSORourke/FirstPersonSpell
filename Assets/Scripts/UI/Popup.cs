using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    //public Canvas canvas;
    public bool active = false;
    public GameObject menuUI;
    public GameObject gameplayUI;
    public GameObject player;

    public void popup(){
        active = menuUI.activeSelf;
        //canvas.enabled = active;
        menuUI.SetActive(!active);
        gameplayUI.SetActive(active);

        Cursor.visible = menuUI.activeSelf;
        if (Cursor.visible){
            Cursor.lockState = CursorLockMode.None;
            player.GetComponent<MouseLook>().enabled = false;
            player.GetComponent<Movement>().enabled = false;
        } 
        else {
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<MouseLook>().enabled = true;
            player.GetComponent<Movement>().enabled = true;
        }
        
    }
}
