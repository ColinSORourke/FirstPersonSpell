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

    public float healthRepeatTime = 10.0f;
    public float manaRepeatTime = 0.5f;
    private bool isManaSpawnMethodActive = false;
    private bool isHealthUltSpawnMethodActive = false;
    private string secondCategoryTag = "HealthItem";
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

        //InvokeRepeating("spawnItem", 10.0f, 10.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isManaSpawnMethodActive) StartCoroutine(spawnItem("ManaItem", manaRepeatTime));
        if (!isHealthUltSpawnMethodActive) StartCoroutine(spawnItem("HealthItem", healthRepeatTime));
    }

    /*
    void spawnItem(){
        Transform crysParent = transform.GetChild(1);
        Transform platformParent = transform.GetChild(0);
        if (currItems < maxItems){
            int i = 0;// = Random.Range(0, spawnPoints.Length);
            while (items[i] != null){
                i = Random.Range(0, spawnPoints.Length);
            }
            Transform platform = platformParent.GetChild(i);
       
            items[i] = Instantiate(toSpawnObjectAt(platform), spawnPoints[i], Quaternion.identity);
            platform.tag = switchTag(platform.tag);

            items[i].transform.parent = crysParent;
            currItems += 1;
        }
    }
    */

    IEnumerator spawnItem(string tagName, float repeatTime)
    {
        //Debug.Log("Tag Name: " + tagName + " | isHealth: " + isHealthUltSpawnMethodActive + " | isMana: " + isManaSpawnMethodActive);
        if (tagName == "ManaItem") isManaSpawnMethodActive = true;
        if (tagName == "HealthItem" || tagName == "UltItem") isHealthUltSpawnMethodActive = true;
        //Debug.Log("Tag Name: " + tagName + " | isHealth: " + isHealthUltSpawnMethodActive + " | isMana: " + isManaSpawnMethodActive);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(repeatTime);

        Transform crysParent = transform.GetChild(1);
        Transform platformParent = transform.GetChild(0);
        if (currItems < maxItems)
        {
            int i = 0;// = Random.Range(0, spawnPoints.Length);
            // repeat until find vacant slot and same tagname to spawn correct type
            while (items[i] != null && items[i].transform.tag != tagName)
            {
                i = Random.Range(0, spawnPoints.Length);
            }
            Transform platform = platformParent.GetChild(i);
            items[i] = Instantiate(toSpawnObjectAt(platform), spawnPoints[i], Quaternion.identity);
            //platform.tag = switchTag(platform.tag);
            items[i].transform.parent = crysParent;
            currItems += 1;
            Debug.Log("Spawning new " + tagName + " after " + repeatTime + " seconds at " + platform.name);
        }

        if (tagName == "HealthItem" || tagName == "UltItem") secondCategoryTag = (Random.Range(0, 2) == 1) ? "HealthItem" : "UltItem";
        yield return new WaitForEndOfFrame();
        if (tagName == "ManaItem" && isManaSpawnMethodActive) isManaSpawnMethodActive = false;
        if ((tagName == "HealthItem" || tagName == "UltItem") && isHealthUltSpawnMethodActive) isHealthUltSpawnMethodActive = false;

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

    private string switchTag(string tag, bool needSwitch = true)
    {
        if (!needSwitch) return "Untagged";

        if (tag == "ManaItem")
        {
            int x = Random.Range(0, 2);
            return x == 0 ? "HealthItem" : "UltItem";
        } else if (tag == "HealthItem" || tag == "UltItem") {
            return "ManaItem";
        }
        return "Untagged";
    }
}
