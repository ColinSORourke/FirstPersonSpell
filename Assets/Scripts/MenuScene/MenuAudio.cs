using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{

    public AudioSource[] mySources;

    // Start is called before the first frame update
    public void Start()
    {  
        mySources = this.GetComponents<AudioSource>();
        int i = 0;
        while (i < mySources.Length){
            mySources[i].volume = PlayerPrefs.GetFloat("VolumePreference");
            i += 1;
        }
    }

    public void changeVolume(float newVolume){
        int i = 0;
        while (i < mySources.Length){
            mySources[i].volume = newVolume;
            i += 1;
        }
    }
}
