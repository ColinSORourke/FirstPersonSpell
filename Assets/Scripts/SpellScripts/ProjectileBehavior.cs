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
    public int slot;
    
    public float lifespan;

    //public bool timerRunning;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start called");
        //timerRunning = true;
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
            if (Target.GetComponent<NetworkObject>().IsLocalPlayer) {
                Debug.Log("Spell Hit");
                GameObject.Destroy(this.gameObject);
                if( ! Target.GetComponent<PlayerStateScript>().isShielded() ){
                    spell.onHit(Source, Target, slot);
                }
            } else {
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
