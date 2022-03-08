using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateScript : MonoBehaviour
{
    public float maxHealth = 50.0f;
    public float currentHealth = 50.0f;
    public float currentBonus = 20.0f;
    public float healthThreshold = 20.0f;

    public float maxMana = 50.0f;
    public float currMana = 50.0f;
    public float manaThreshold = 30.0f;
    public bool doManaRegen = true;

    public float maxUlt = 20.0f;
    public float currUlt = 0.0f;

    public int maxShields = 3;
    public int currShields = 3;
    public float shieldTime = 4.0f;
    public float shieldDur = -1.0f;

    public List<liveAura> auras = new List<liveAura>();

    public static int playerCardDeckId = 0; //ID for Card Decks, 901 will be default ID for default Card Deck (when applicable)
    public baseSpellScript[] spellDeck = new baseSpellScript[7];
    public baseSpellScript ultSpell;
    public List<baseSpellScript> spellQueue = new List<baseSpellScript>();

    public int spellsCast = 0;
    public int manaPickedUp = 0;

    public float takeDamageMult = 1.0f;
    public float manaCostMult = 1.0f;
    public float castTimeMult = 1.0f;

    public float moveSpeed = 12.0f;

    public GenericUI myUI;

    private SelectDeck allDecks;

    // Start is called before the first frame update
    void Start()
    {
        myUI = this.GetComponent<GenericUI>();
        allDecks = this.GetComponent<SelectDeck>();

        //Create Card Deck
        Debug.Log("Card Deck ID: " + playerCardDeckId);
        spellDeck = allDecks.spellDecks[playerCardDeckId].getSpellDeck();
        //Create Ult
        ultSpell = allDecks.spellDecks[playerCardDeckId].getUltSpell();

        int[] shuffleOrder = { 0,1,2,3,4,5,6 };
        int i = 6;
        while (i >= 1){
            int j = Random.Range(0,i+1);
            if (j != i){
                int temp = shuffleOrder[i];
                shuffleOrder[i] = shuffleOrder[j];
                shuffleOrder[j] = temp;
            }
            i -= 1;
        }
        while (i < 7){
            baseSpellScript added = spellDeck[ shuffleOrder[i] ];
            spellQueue.Add(added);
            if (i < 4){
                myUI.addIcon(added, i);
            }
            i += 1;
        }

        InvokeRepeating("tick", 0.0f, 0.25f);
        myUI.updateUlt(currUlt, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called every quarter second
    void tick(){
        // Regen mana. Regen is greater if Health is low.
        if (currMana < manaThreshold && doManaRegen){
            if (currentHealth <= healthThreshold){
                currMana += 0.5f;
            } else {
                currMana += 0.25f;
            }
        }

        // Decay bonus healths
        if (currentBonus > 0.0f){
            currentBonus -= 0.25f;
        }

        // Decay shield duration.
        if (shieldDur > 0.0f){
            shieldDur -= 0.25f;
            if (shieldDur == 0.0f){
                shieldDur = -1.0f;
                myUI.removeShield();
            }
        }
       
        myUI.updateHealth(currentHealth, currentHealth/maxHealth, currentBonus/maxHealth);
        myUI.updateMana(currMana, currMana/maxMana);

        // Decay Auras
        int i = 0; 
        while (i < auras.Count){
            liveAura a = auras[i];
            myUI.updateAura(i, a);
            int tickInfo = a.update(0.25f);
            if (tickInfo == -1){
                this.removeAura(i);
            } else {
                i += 1;
            }
        }
    }

    public void applyAura(Transform src, baseAuraScript aura, float duration){
        int matchInd = hasAura(aura.id);
        // Check if we already have this type of Aura
        if (matchInd != -1){
            Debug.Log("Already have this aura");
            // Make sure we aren't shortening the duration
            if (auras[matchInd].duration < duration){
                auras[matchInd].duration = duration;
            }
            bool canStack = auras[matchInd].onStack();
            if (canStack){
                for (int i = 0; i < auras.Count; i++){
                    if (auras[i].aura.id == aura.id){
                        myUI.stackAura(auras[i], auras[i].stacks);
                        break;
                    }
                }
            }
            
        } else {
            liveAura toApply = new liveAura();
            toApply.aura = aura;
            toApply.on = this.transform;
            toApply.src = src;
            toApply.duration = duration;
            toApply.stacks = 1;
            toApply.tickNum = 0;
            toApply.onApply();
            auras.Add(toApply);
            Debug.Log(auras);
            myUI.addAura(aura.icon, aura.id, 1);
        }
    }

    public void removeAura(int i){
        auras[i].onExpire();
        auras.RemoveAt(i);
        myUI.removeAura(i);
    }

    public int hasAura(int id){
        int i = 0;
        while (i < auras.Count){
            if (auras[i].aura.id == id){
                return i;
            }
            i += 1;
        }
        return -1;
    }
    
    public bool isShielded(){
        return (shieldDur > 0.0f);
    }

    public void takeDamage(float dam, bool mult = true){
        if (mult){
            dam *= takeDamageMult;
        }
        if (dam > currentBonus){
            dam -= currentBonus;
            currentBonus = 0;
            currentHealth -= dam;
        } else if (currentBonus > 0){
            currentBonus -= dam;
        } else {
            currentHealth -= dam;
        }

        myUI.updateHealth(currentHealth, currentHealth/maxHealth, currentBonus/maxHealth);

        if (currentHealth <= 0){
            // Trigger death
        }
    }

    public void changeMana(float value){
        currMana += value;
        currMana = Mathf.Clamp(currMana, 0, maxMana);
        myUI.updateMana(currMana, currMana/maxMana);
    }

    public void pickupManaCrystal(){
        currMana += 15.0f;
        currMana = Mathf.Clamp(currMana, 0, maxMana);
        manaPickedUp += 1;
        myUI.updateMana(currMana, currMana/maxMana);
    }

    public void pickupUltCrystal(){
        currUlt += 3.0f;
        if (currUlt >= ultSpell.ultCost){
            currUlt -= ultSpell.ultCost;
            spellQueue.Add(ultSpell);
        }
        myUI.updateUlt(currUlt, currUlt/ultSpell.ultCost);
    }

    public void pickupHealthCrystal(){
        currentBonus += 15.0f;
        currentBonus = Mathf.Clamp(currentBonus, 0, maxHealth);
        myUI.updateHealth(currentHealth, currentHealth/maxHealth, currentBonus/maxHealth);
    }

    public float validCast(int slot, bool Target, float distance){
        if ((Target && distance < spellQueue[slot].range) || !spellQueue[slot].reqTarget){
            if (spellQueue[slot].manaCost * manaCostMult <= currMana){
                return spellQueue[slot].castTime * castTimeMult;
            } else {
                return -1.0f;
            }
        } else {
            return -1.0f;
        }
    }

    public void castSpell(int slot){
        var castSpell = spellQueue[slot];
        currMana -= castSpell.manaCost * manaCostMult;
        if(manaCostMult != 1.0f) Debug.Log(manaCostMult.ToString("n1"));
        if (currMana < 0.0f) currMana = 0.0f; // Mana should never be below 0
        currentBonus = Mathf.Clamp(currentBonus, 0, maxHealth);
        spellQueue.RemoveAt(slot);
        if (!castSpell.exhaust){
            spellQueue.Add(castSpell);
        }
        spellsCast += 1;

        myUI.updateMana(currMana, currMana/maxMana);
        myUI.shiftSpells(slot, spellQueue[3]);
    }
}
