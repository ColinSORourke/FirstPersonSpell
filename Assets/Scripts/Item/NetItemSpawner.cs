using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetItemSpawner : NetworkBehaviour
{
    public static NetItemSpawner Instance => instance;
    private static NetItemSpawner instance;

    public GameObject[] itemPrefabs = new GameObject[5];
    public int maxItems = 3;
    int currItems = 0;
    Vector3[] spawnPoints;
    public GameObject[] items;

    private void Awake() {
        if (!NetworkManager.Singleton.IsServer) {
            this.enabled = false;
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Transform spawnParent = transform.GetChild(0);
        Transform crysParent = transform.GetChild(1);
        int number = spawnParent.childCount;
        spawnPoints = new Vector3[number];
        items = new GameObject[number];
        for (int i = 0; i < number; i ++) {
            Vector3 pos = spawnParent.GetChild(i).position;
            pos.x -= 0.5f;
            pos.y += 1.2f;
            spawnPoints[i] = pos;
            //crystals[i] = Instantiate(crystalPrefab, pos, Quaternion.identity);
            //crystals[i].transform.parent = crysParent;
        }

        InvokeRepeating("spawnItem", 10.0f, 10.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnItem(){
        GameObject toSpawn = itemPrefabs[Random.Range(0, 3)];
        //Transform crysParent = transform.GetChild(1);
        if (currItems < maxItems){
            int i = Random.Range(0, spawnPoints.Length);
            while (items[i] != null){
                i = Random.Range(0, spawnPoints.Length);
            }
            items[i] = Instantiate(toSpawn, spawnPoints[i], Quaternion.identity);
            items[i].GetComponent<NetworkObject>().Spawn();
            //items[i].transform.parent = crysParent;
            currItems += 1;
        }
    }

    public void itemDestroyed(){
        currItems -= 1;
    }
}
