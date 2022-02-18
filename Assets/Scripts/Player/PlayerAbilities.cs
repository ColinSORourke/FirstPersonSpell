using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public List<GameObject> Targets;
    public List<bool> visibleTargets;
    public int currTarget = -1;

    baseSpellScript castingSpell;
    int castingSpellSlot;
    public float castTime = -1.0f;
    public float totalCastTime;

    PlayerStateScript myState;
    GenericUI myUI;

    // Start is called before the first frame update
    void Start()
    {
        myUI = this.GetComponent<GenericUI>();
        myState = this.GetComponent<PlayerStateScript>();

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
        foreach(GameObject tarObjs in Targets){
            bool visible;
            Debug.Log("Drawing Ray to " + tarObjs.transform + " at " + tarObjs.transform.position);
            if (i == currTarget){
                visible = hasLOS(tarObjs, true);
            } else {
                visible = hasLOS(tarObjs);
            }
            
            visibleTargets[i] = visible;
            if (i == currTarget && !visible){
                Debug.Log("Lost sight on " + currTarget);
                Targets[currTarget].GetComponent<GenericUI>().unTarget();
                currTarget = -1;
                myUI.updateCast(0);
                castTime = -1.0f;
            }
            i += 1;        
        }

        float range = 0.0f;
        bool tar = currTarget != -1;
        if (tar)
        {
            range = Vector3.Distance(Targets[currTarget].transform.position, this.gameObject.transform.position);
            Targets[currTarget].GetComponent<GenericUI>().updateRange(range);
        }

        if (castTime != -1.0f){
            castTime += Time.deltaTime;

            float valid;
            if (tar){
                range = Vector3.Distance(Targets[currTarget].transform.position, this.gameObject.transform.position);
            }
            valid = myState.validCast(castingSpellSlot, tar, range);
            if (castTime >= totalCastTime){
                if (valid != -1.0f){
                    //finished casting
                    myState.castSpell(castingSpellSlot);
                    castTime = -1.0f;
                    Transform myTar = null;
                    if (castingSpell.reqTarget){
                        myTar = Targets[currTarget].transform;
                    }
                    castingSpell.onCastGeneral(transform, myTar, castingSpellSlot);
                    myUI.updateCast(0);
                } else {
                    castTime = -1.0f;
                    myUI.updateCast(0);
                }
                
            } else {
                myUI.updateCast(castTime/totalCastTime);
            }
        }

        
        //parseInput();
    }

    public void parseInput()
    {
        /*
        if (Input.GetKeyUp(KeyCode.Mouse0)){
            newTarget();
        }
        */

        float valid;
        float range = 0.0f;
        bool tar = currTarget != -1;
        

        /*
        if (Input.GetKeyUp("1") && valid != -1.0f ){
            this.cast(0, valid);
        }
        */
        valid = myState.validCast(1, tar, range);
        /*
        if (Input.GetKeyUp("2") && valid != -1.0f){
            this.cast(1, valid);
        }
        */
        valid = myState.validCast(2, tar, range);

        /*
        if (Input.GetKeyUp("3") && valid != -1.0f){
            this.cast(2, valid);
        }

        if (Input.GetKeyUp(KeyCode.F)){
            Debug.Log("Pressed F");
            if (myState.currShields> 0){
                myState.shieldDur = myState.shieldTime;
                myState.currShields -= 1;
                myUI.displayShield();
            }
        }
        */
    }

    public void castSpell(int slot){
        float valid = 0;
        float range = 0.0f;
        bool tar = currTarget != -1;
        if (tar)
        {
            range = Vector3.Distance(Targets[currTarget].transform.position, this.gameObject.transform.position);
            //Targets[currTarget].GetComponent<GenericUI>().updateRange(range);
        }

        valid = myState.validCast(slot, tar, range);

        var spell = myState.spellQueue[slot];
        if (spell.castTime > 0){
            // Start Casting
            castTime = 0.0f;
            totalCastTime = valid;
            castingSpell = spell;
            castingSpellSlot = slot;
        } else {
            myState.castSpell(slot);
            Transform myTar = null;
            if (spell.reqTarget){
                myTar = Targets[currTarget].transform;
            }
            spell.onCastGeneral(transform, myTar, slot);
        }
    }

    public void castShield(){
        Debug.Log("Pressed Shield Button");
        if (myState.currShields > 0)
        {
            myState.shieldDur = myState.shieldTime;
            myState.currShields -= 1;
            myUI.displayShield();
        }
    }
    public void newTarget(){
        int oldTar = currTarget;
        if (oldTar != -1){
            Targets[oldTar].GetComponent<GenericUI>().unTarget();
        }

        RaycastHit[] hits;

        Vector3 dir = this.transform.GetChild(1).transform.forward; 
        Ray ry = new Ray(); 
        ry.origin = this.gameObject.transform.position; 
        ry.direction = dir; 
 

        hits = Physics.RaycastAll(ry);
        Debug.DrawRay (this.gameObject.transform.position, dir * 10000.0f, Color.cyan, 1.0f);

        foreach(RaycastHit hit in hits){  
            Debug.Log(hit.transform);
            if (hit.transform.parent != null){
                if (hit.transform.parent.gameObject.layer == 7 && hit.transform.parent != transform) {
                    GameObject tar = hit.transform.parent.gameObject;
                    int i = 0;
                    while (i < Targets.Count){
                        if (Targets[i] == tar){
                            currTarget = i;
                            Debug.Log("Targeted " + i);
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
            myUI.updateCast(0);
            castTime = -1.0f;
        } else {
            Targets[currTarget].GetComponent<GenericUI>().target();
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
        if (print){
            Debug.DrawRay (this.gameObject.transform.position, dir, Color.red, 1.0f);
        } else {
            Debug.DrawRay (this.gameObject.transform.position, dir, Color.cyan, 1.0f);
        }
         

        foreach(RaycastHit hit in hits){ 
            if (!obj_hit){
                if (hit.transform.parent != null){
                    if (hit.transform.parent.gameObject != tar && hit.transform.parent != transform) {
                        obj_hit = true; 
                        if (print){
                            Debug.Log(hit.transform.parent.gameObject);
                        }
                        
                    }
                } else {
                    obj_hit = true;
                    if (print){
                        Debug.Log(hit.transform);
                    }
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
        return goList;
    }
}
