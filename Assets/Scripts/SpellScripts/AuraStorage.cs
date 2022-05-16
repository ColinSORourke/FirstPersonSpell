using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraStorage : MonoBehaviour
{
    public baseAuraScript[] allAuras;

    public int findIndex(baseAuraScript a){
        int i = 0;
        while (i < allAuras.Length){
            if (allAuras[i].id == a.id){
                return i;
            }
            i += 1;
        }
        return -1;
    }
}
