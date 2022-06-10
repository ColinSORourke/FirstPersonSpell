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
        if (mySources[0].volume == 0){
            mySources[0].Pause();
        }
        mySources[1].volume = PlayerPrefs.GetFloat("VolumePreference");
    }

    public void changeVolume(float newVolume){
        mySources[1].volume = newVolume;
    }

    public void changeMusic(float newVolume){
        mySources[0].volume = newVolume;
        if (mySources[0].volume == 0){
            mySources[0].Pause();
        } else {
            if (!mySources[0].isPlaying){
                mySources[0].Play();
            }
        }
    }
}
