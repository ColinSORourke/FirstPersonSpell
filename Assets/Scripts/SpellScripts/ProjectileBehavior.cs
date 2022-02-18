using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
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
        Debug.Log("Projectile entered Collider");
        Debug.Log(collider);
        if (collider.GetComponent<Transform>().parent != null){
            Transform Hit = collider.GetComponent<Transform>().parent;
            if (Hit == Target){
                Debug.Log("Spell Hit");
                GameObject.Destroy(this.gameObject);
                if( ! Target.GetComponent<PlayerStateScript>().isShielded() ){
                    spell.onHit(Source, Target, slot);
                }
            }
        }
        
    }
}
