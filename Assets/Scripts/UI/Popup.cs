using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public Canvas canvas;
    public bool active = false;

    public void popup(){
        if (!active){
            active = true;
            canvas.enabled = true;
        }
        else{
            active = false;
            canvas.enabled = false;
        }
    }
}
