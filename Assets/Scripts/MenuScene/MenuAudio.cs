using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{

    public AudioSource[] mySources;

    // Start is called before the first frame update
    public void Awake(){
        mySources = this.GetComponents<AudioSource>();
        mySources[0].volume = PlayerPrefs.GetFloat("MusicVolumePreference");
        mySources[1].volume = PlayerPrefs.GetFloat("VolumePreference");
    }

    public void changeVolume(float newVolume){
        mySources[1].volume = newVolume;
    }

    public void changeMusic(float newVolume){
        mySources[0].volume = newVolume;
    }
}
