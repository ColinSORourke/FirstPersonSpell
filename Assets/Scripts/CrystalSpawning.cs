using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawning : MonoBehaviour
{
    public GameObject crystalPrefab;
    public int maxCrystals = 3;
    int currCrystals = 0;
    Vector3[] spawnPoints;
    public GameObject[] crystals;
    // Start is called before the first frame update
    void Start()
    {
        Transform spawnParent = transform.GetChild(0);
        Transform crysParent = transform.GetChild(1);
        int number = spawnParent.childCount;
        spawnPoints = new Vector3[number];
        crystals = new GameObject[number];
        int i = 0;
        while (i < number){
            Vector3 pos = spawnParent.GetChild(i).position;
            pos.x -= 0.5f;
            pos.y += 1.2f;
            spawnPoints[i] = pos;
            //crystals[i] = Instantiate(crystalPrefab, pos, Quaternion.identity);
            //crystals[i].transform.parent = crysParent;
            i += 1;
        }

        InvokeRepeating("spawnCrystal", 10.0f, 10.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnCrystal(){
        Transform crysParent = transform.GetChild(1);
        if (currCrystals < maxCrystals){
            int i = Random.Range(0, spawnPoints.Length);
            while (crystals[i] != null){
                i = Random.Range(0, spawnPoints.Length);
            }
            crystals[i] = Instantiate(crystalPrefab, spawnPoints[i], Quaternion.identity);
            crystals[i].transform.parent = crysParent;
            currCrystals += 1;
        }
    }

    public void crystalDestroyed(){
        currCrystals -= 1;
    }
}
