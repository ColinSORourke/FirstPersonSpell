using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Targeting : MonoBehaviour
{   
    List<GameObject> Targets;
    List<bool> visibleTargets;
    int currTarget = -1;
    Canvas playerUI;
    Image castBar;
    Mana playerMana;
    baseSpellScript castingSpell;
    int castingSpellSlot;
    float castTime = -1.0f;

    public baseSpellScript[] spellDeck = new baseSpellScript[7];
    public List<baseSpellScript> spellQueue = new List<baseSpellScript>();
    public Image[] spellIcons = new Image[4];

    // Start is called before the first frame update
    void Start()
    {
        playerMana = this.GetComponent<Mana>();
        playerUI = this.gameObject.transform.Find("SpellUI").GetComponent<Canvas>();
        castBar = this.gameObject.transform.Find("SpellUI").Find("CastBar").GetComponent<Image>();
        Targets = FindGameObjectsInLayer(7);
        visibleTargets = new List<bool>();
        

        int i = 0;

        while(i < Targets.Count){
            visibleTargets.Add(false);
            i += 1;
        }

        i = 0;
         
        while (i < 7){
            if (i < 4){
                spellIcons[i] = addIcon(spellDeck[i].icon, i);
            }
            spellQueue.Add(spellDeck[i]);
            i += 1;
        }
    }

    void castSpell(int slot){
        var spell = spellQueue[slot];
        if (playerMana.currMana >= spell.manaCost){
            if (spell.castTime > 0){
                castTime = 0.0f;
                castingSpell = spell;
                castingSpellSlot = slot;
            } else {
                spell.onCastGeneral(transform, Targets[currTarget].transform, slot);
                this.shiftSpells(slot);
            }
        }
    }

    void shiftSpells(int slot){
        var castSpell = spellQueue[slot];
        spellQueue.RemoveAt(slot);
        if (!castSpell.exhaust){
            spellQueue.Add(castSpell);
        }

        Destroy(spellIcons[slot].gameObject);
        int j = slot + 1; 
        while (j < spellIcons.Length){
            var spellTrans = spellIcons[j].GetComponent<Transform>();
            spellIcons[j].GetComponent<Transform>().localPosition = spellTrans.localPosition - new Vector3(80, 0, 0);
            if (j == 3){
                spellIcons[j].GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
            }
            spellIcons[j].name = "Spell" + (j-1);
            spellIcons[j-1] = spellIcons[j];

            j += 1;
        }

        spellIcons[3] = this.addIcon(spellQueue[3].icon, 3);
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
                castBar.fillAmount = 0;
                castTime = -1.0f;
            }
            i += 1;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)){
            newTarget();
        }

        if (Input.GetKeyUp("1") && (currTarget != -1 || !spellQueue[0].reqTarget)){
            this.castSpell(0);
        }

        if (Input.GetKeyUp("2") && (currTarget != -1 || !spellQueue[1].reqTarget)){
            this.castSpell(1);
        }

        if (Input.GetKeyUp("3") && (currTarget != -1 || !spellQueue[2].reqTarget)){
            this.castSpell(2);
        }

        if (castTime != -1.0f){
            castTime += Time.deltaTime;
            if (castTime >= castingSpell.castTime){
                castingSpell.onCastGeneral(transform, Targets[currTarget].transform, castingSpellSlot);
                castBar.fillAmount = 0;
                castTime = -1.0f;
                shiftSpells(castingSpellSlot);
            } else {
                castBar.fillAmount = castTime/castingSpell.castTime;
            }
        } 
    }

    void newTarget(){
        int oldTar = currTarget;
        if (oldTar != -1){
            Targets[oldTar].GetComponent<Health>().unTarget();
        }

        RaycastHit[] hits;

        Vector3 dir = this.transform.GetChild(1).transform.forward; 
        Ray ry = new Ray(); 
        ry.origin = this.gameObject.transform.position; 
        ry.direction = dir; 
 

        hits = Physics.RaycastAll(ry);
        Debug.DrawRay (this.gameObject.transform.position, dir * 10000.0f, Color.cyan, 1.0f);
        Debug.Log(ry);

        foreach(RaycastHit hit in hits){  
            //Debug.Log(hit.transform);
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

        /* int j = currTarget + 1;
        while (j < visibleTargets.Length){
            if (visibleTargets[j]){
                if(Targets[j].transform.GetChild(0).GetComponent<Renderer>().isVisible){
                    currTarget = j;
                    Targets[j].GetComponent<Health>().Target();
                    j = visibleTargets.Length;
                }
            }
            j += 1;
        } */
        if (currTarget == oldTar){
            currTarget = -1;
            castBar.fillAmount = 0;
            castTime = -1.0f;
        } else {
            Targets[currTarget].GetComponent<Health>().Target();
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
            // here we could look at an attached script (if one exists) on the object and 
            // decide whether or not this should actually constitute a hit 
            // Debug.Log("LOS test hit from "+this.gameObject.transform.position+" to "+tar.transform.position+" = "+hit.transform.parent.gameObject.name); 
            if (!obj_hit){
                if (hit.transform.parent != null){
                    if (hit.transform.parent.gameObject != tar){
                        obj_hit = true; 
                        if (print){
                            Debug.Log(hit.transform);
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

    public Image addIcon(Sprite icon, int slot){
        GameObject imgObject = new GameObject("Spell" + slot); 
        //Create the GameObject
        Image NewImage = imgObject.AddComponent<Image>(); //Add the Image Component script
        NewImage.sprite = icon; //Set the Sprite of the Image Component on the new GameObject

        var imgtrans = imgObject.GetComponent<RectTransform>();
        imgtrans.SetParent(playerUI.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        imgtrans.localRotation = Quaternion.Euler(new Vector3(0,0,0));
        imgtrans.localPosition = new Vector3(-80 + (80 * slot),-130,0);
        imgtrans.sizeDelta = new Vector2(60, 60);
        if (slot == 3){
            imgtrans.sizeDelta = new Vector2(40, 40);
        }
        imgObject.SetActive(true);
        return NewImage;
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

    public void removeTarget(GameObject tar){
        int i = 0;
        int tarInd = -1; 
        while (i < Targets.Count){
            if (Targets[i] == tar){
                tarInd = i;
                i = Targets.Count;
            }
            i += 1;
        }

        Targets.RemoveAt(tarInd); 
        visibleTargets.RemoveAt(tarInd);

        if (currTarget >= tarInd){
            currTarget = -1;
            castBar.fillAmount = 0;
            castTime = -1.0f;
        }
    }
}

