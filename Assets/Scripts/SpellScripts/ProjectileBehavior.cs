using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileBehavior : NetworkBehaviour
{
    public Transform Target;
    public float speed;
    public baseSpellScript spell;
    public Transform Source;
    public int index;
    public int slot;
    
    public float lifespan;

    //public bool timerRunning;

    public SpellRpcs spellRpcs;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start called");
        //timerRunning = true;

        spellRpcs = FindObjectOfType<SpellRpcs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifespan > 0){
            lifespan -= Time.deltaTime;
        }
        else{
            Debug.Log("Spell expired");
            GameObject.Destroy(this.gameObject);
        }
        
        Vector3 dir = Target.transform.position - this.gameObject.transform.position;
        dir.Normalize();
        transform.position += (dir * speed);

    }

    void OnTriggerEnter(Collider collider){
        Transform Hit = collider.GetComponent<Transform>().parent;
        if (Hit != null && Hit == Target) {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider>().enabled = false;
            spellRpcs.DestroyProjectileClientRpc(Source.GetComponent<NetworkObject>().OwnerClientId, Target.GetComponent<NetworkObject>().OwnerClientId, spellRpcs.projectiles.IndexOf(this.gameObject), index, slot);
        }
    }
}
