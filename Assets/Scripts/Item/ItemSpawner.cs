using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs = new GameObject[5];
    public int maxItems = 3;
    int currItems = 0;
    Vector3[] spawnPoints;
    public GameObject[] items;
    // Start is called before the first frame update
    void Start()
    {
        Transform spawnParent = transform.GetChild(0);
        Transform crysParent = transform.GetChild(1);
        int number = spawnParent.childCount;
        spawnPoints = new Vector3[number];
        items = new GameObject[number];
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

        InvokeRepeating("spawnItem", 10.0f, 10.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnItem(){
        GameObject toSpawn = itemPrefabs[Random.Range(0, 3)];
        Transform crysParent = transform.GetChild(1);
        if (currItems < maxItems){
            int i = Random.Range(0, spawnPoints.Length);
            while (items[i] != null){
                i = Random.Range(0, spawnPoints.Length);
            }
            items[i] = Instantiate(toSpawn, spawnPoints[i], Quaternion.identity);
            items[i].transform.parent = crysParent;
            currItems += 1;
        }
    }

    public void itemDestroyed(){
        currItems -= 1;
    }
}
