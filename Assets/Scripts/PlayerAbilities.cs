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
        foreach(GameObject tar in Targets){
            bool visible;
            if (i == currTarget){
                visible = hasLOS(tar);
            } else {
                visible = hasLOS(tar);
            }
            
            visibleTargets[i] = visible;
            if (i == currTarget && !visible){
                Targets[currTarget].GetComponent<GenericUI>().unTarget();
                currTarget = -1;
                myUI.updateCast(0);
                castTime = -1.0f;
            }
            i += 1;        
        }

        if (castTime != -1.0f){
            castTime += Time.deltaTime;

            float valid;
            float range = 0.0f;
            bool tar = currTarget != -1;
            if (tar){
                range = Vector3.Distance(Targets[currTarget].transform.position, this.gameObject.transform.position);
            }
            valid = myState.validCast(castingSpellSlot, tar, range);
            if (castTime >= totalCastTime){
                if (valid != -1.0f){
                    Debug.Log("finished casting");
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

        parseInput();
    }

    void parseInput()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0)){
            newTarget();
        }

        float valid;
        float range = 0.0f;
        bool tar = currTarget != -1;
        if (tar){
            range = Vector3.Distance(Targets[currTarget].transform.position, this.gameObject.transform.position);
        }
        valid = myState.validCast(0, tar, range);

        if (Input.GetKeyUp("1") && valid != -1.0f ){
            this.cast(0, valid);
        }
        
        valid = myState.validCast(1, tar, range);
        if (Input.GetKeyUp("2") && valid != -1.0f){
            this.cast(1, valid);
        }

        valid = myState.validCast(2, tar, range);
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
            Transform myTar = null;
            if (spell.reqTarget){
                myTar = Targets[currTarget].transform;
            }
            spell.onCastGeneral(transform, myTar, slot);
        }
    }

    void newTarget(){
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
        return goList;
    }
}
