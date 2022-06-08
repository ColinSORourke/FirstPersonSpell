using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSourceVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolumePreference");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
