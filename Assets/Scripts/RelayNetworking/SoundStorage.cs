using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundStorage : MonoBehaviour
{
    public AudioClip[] allSounds;

    public AudioClip[] localSounds;

    public int findIndex(AudioClip a) {
        int i = 0;
        while (i < allSounds.Length) {
            if (allSounds[i] == a) {
                return i;
            }
            i += 1;
        }
        return -1;
    }

    public AudioClip selectSoundByInt(int index) {
        int validIndex = index % allSounds.Length;
        return allSounds[validIndex];
    }

    public AudioClip selectLocalSound(int index){
        int validIndex = index % localSounds.Length;
        return localSounds[validIndex];
    }
}
