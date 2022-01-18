using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    List<GameObject> Targets;
    List<bool> visibleTargets;
    int currTarget = -1;

    baseSpellScript castingSpell;
    int castingSpellSlot;
    float castTime = -1.0f;
    float totalCastTime;

    PlayerStateScript myState;

    // Start is called before the first frame update
    void Start()
    {
        Targets = FindGameObjectsInLayer(7);
        visibleTargets = new List<bool>();

        int i = 0;

        while(i < Targets.Count)
        {
            visibleTargets.Add(false);
            i += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach(GameObject tar in Targets){
            bool visible;
            if (i == currTarget){
                visible = hasLOS(tar, true);
            } else {
                visible = hasLOS(tar);
            }
            
            visibleTargets[i] = visible;
            if (i == currTarget && !visible){
                Targets[i].GetComponent<Health>().unTarget();
                currTarget = -1;
                // Update Casting UI
                castTime = -1.0f;
            }
            i += 1;        
        }

        if (castTime != -1.0f){
            castTime += Time.deltaTime;
            if (castTime >= totalCastTime){
                myState.castSpell(castingSpellSlot);
                castingSpell.onCastGeneral(transform, Targets[currTarget].transform, castingSpellSlot);
                castTime = -1.0f;
                // Update Casting UI
            } else {
                // Update Casting UI
            }
        }

        parseInput();
    }

    void parseInput()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0)){
            newTarget();
        }

        bool tar = currTarget != -1;
        float valid = myState.validCast(0, tar);

        if (Input.GetKeyUp("1") && valid != -1.0f ){
            this.cast(0, valid);
        }
        
        valid = myState.validCast(1, tar);
        if (Input.GetKeyUp("2") && valid != -1.0f){
            this.cast(1, valid);
        }

        valid = myState.validCast(2, tar);
        if (Input.GetKeyUp("3") && valid != -1.0f){
            this.cast(2, valid);
        }

        if (Input.GetKeyUp("f")){
            myState.shieldDur = myState.shieldTime;
            // Update Shield UI
        }
    }

    void cast(int slot, float Time){
        var spell = myState.spellQueue[slot];
        if (spell.castTime > 0){
            castTime = 0.0f;
            totalCastTime = Time;
            castingSpell = spell;
            castingSpellSlot = slot;
        } else {
            myState.castSpell(slot);
            spell.onCastGeneral(transform, Targets[currTarget].transform, slot);
        }
    }

    void newTarget(){
        int oldTar = currTarget;
        if (oldTar != -1){
            // Update Target UI
        }

        RaycastHit[] hits;

        Vector3 dir = this.transform.GetChild(1).transform.forward; 
        Ray ry = new Ray(); 
        ry.origin = this.gameObject.transform.position; 
        ry.direction = dir; 
 

        hits = Physics.RaycastAll(ry);
        Debug.DrawRay (this.gameObject.transform.position, dir * 10000.0f, Color.cyan, 1.0f);

        foreach(RaycastHit hit in hits){  
            if (hit.transform.parent != null){
                if (hit.transform.parent.gameObject.layer == 7){
                    GameObject tar = hit.transform.parent.gameObject;
                    int i = 0;
                    while (i < Targets.Count){
                        if (Targets[i] == tar){
                            currTarget = i;
                            break;
                        }
                        i += 1;
                    }
                    
                    break;
                }
            }
        }

        if (currTarget == oldTar){
            currTarget = -1;
            // Update Cast UI
            castTime = -1.0f;
            // Update Target UI
        } else {
            // Update Target UI
        }
    }

    bool hasLOS(GameObject tar, bool print = false){
        RaycastHit[] hits; 
        bool obj_hit = false; 

        Vector3 dir = tar.transform.position - this.gameObject.transform.position; 
        Ray ry = new Ray(); 
        ry.origin = this.gameObject.transform.position; 
        ry.direction = dir; 

        hits = Physics.RaycastAll (ry, dir.magnitude); 
        Debug.DrawRay (this.gameObject.transform.position, dir, Color.cyan, 1.0f); 

        foreach(RaycastHit hit in hits){  
            if (!obj_hit){
                if (hit.transform.parent != null){
                    if (hit.transform.parent.gameObject != tar){
                        obj_hit = true; 
                    }
                } else {
                    obj_hit = true;
                }
            }
        } 

        return(!obj_hit);
    }

    public static List<GameObject> FindGameObjectsInLayer(int layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList;
    }
}
