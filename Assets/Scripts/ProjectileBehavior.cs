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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Target.transform.position - this.gameObject.transform.position;
        dir.Normalize();
        transform.position += (dir * speed);
    }

    void OnTriggerEnter(Collider collider){
        if (collider.GetComponent<Transform>().parent != null){
            Transform Hit = collider.GetComponent<Transform>().parent;
            if (Hit == Target){
                GameObject.Destroy(this.gameObject);
                spell.onHit(Source, Target, slot);
            }
        }
        
    }
}
