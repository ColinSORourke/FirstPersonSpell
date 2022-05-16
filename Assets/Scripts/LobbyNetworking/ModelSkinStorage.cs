using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSkinStorage : MonoBehaviour
{
    public Material[] characterMaterials;
    public GameObject[] characterPoses;
    
    // Start is called before the first frame update
    void Start()
    {
        int i = Random.Range(0,5);
        float rotationMod = i == 4 ? 60.0f : 0.0f;
        var body = Instantiate(characterPoses[i], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        body.transform.parent = this.transform;
        body.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        body.transform.rotation = Quaternion.Euler(0.0f, -120.0f - rotationMod, 0.0f);
        body.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = characterMaterials[0];
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
