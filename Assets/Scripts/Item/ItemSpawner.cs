using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
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
            //Debug.Log(spawnParent.GetChild(i).gameObject.tag);
            spawnParent.GetChild(i).gameObject.tag = spawnParent.GetChild(i).gameObject.tag == "Untagged" ? assignRandomTagToObject() : spawnParent.GetChild(i).gameObject.tag;
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
        Transform crysParent = transform.GetChild(1);
        Transform platformParent = transform.GetChild(0);
        if (currItems < maxItems){
            int i = 0;// = Random.Range(0, spawnPoints.Length);
            while (items[i] != null){
                i = Random.Range(0, spawnPoints.Length);
            }
            items[i] = Instantiate(toSpawnObjectAt(platformParent.GetChild(i)), spawnPoints[i], Quaternion.identity);
            items[i].transform.parent = crysParent;
            currItems += 1;
        }
    }

    public void itemDestroyed(){
        currItems -= 1;
    }

    private string assignRandomTagToObject()
    {
        string[] tagList = {"UltItem", "ManaItem", "HealthItem" };
        int index = Random.Range(0, tagList.Length);
        
        return tagList[index];
    }

    private GameObject toSpawnObjectAt(Transform platform)
    {
        switch (platform.tag)
        {
            case "Untagged":
                break;
            case "ManaItem":
                return itemPrefabs[0];
            case "HealthItem":
                return itemPrefabs[1];
            case "UltItem":
                return itemPrefabs[2];
        }
        return itemPrefabs[Random.Range(0, 3)];
    }
}
