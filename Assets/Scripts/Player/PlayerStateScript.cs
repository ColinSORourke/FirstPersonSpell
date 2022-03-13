using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStateScript : NetworkBehaviour
{
    public float maxHealth = 50.0f;
    //public float currentHealth = 50.0f;
    private NetworkVariable<float> _currentHealth = new NetworkVariable<float>(50.0f);
    public float currentHealth => _currentHealth.Value;
    //public float currentBonus = 20.0f;
    private NetworkVariable<float> _currentBonus = new NetworkVariable<float>(20.0f);
    public float currentBonus => _currentBonus.Value;
    public float healthThreshold = 20.0f;

    public float maxMana = 50.0f;
    //public float currMana = 50.0f;
    private NetworkVariable<float> _currMana = new NetworkVariable<float>(50.0f);
    public float currMana => _currMana.Value;
    public float manaThreshold = 30.0f;
    public bool doManaRegen = true;

    public float maxUlt = 20.0f;
    //public float currUlt = 0.0f;
    private NetworkVariable<float> _currUlt = new NetworkVariable<float>(0.0f);
    public float currUlt => _currUlt.Value;

    public int maxShields = 3;
    public int currShields = 3;
    public float shieldTime = 4.0f;
    public float shieldDur = -1.0f;

    public AudioSource audioSource;

    public List<liveAura> auras = new List<liveAura>();

    public static int playerCardDeckId = 1; //ID for Card Decks, 901 will be default ID for default Card Deck (when applicable)
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

    public bool alive = true;
    //public AliveManager aliveManager;

    // Start is called before the first frame update
    void Start()
    {
        var myUIs = this.GetComponents<GenericUI>();
        foreach (GenericUI UI in myUIs){
            if (UI.enabled){
                myUI = UI;
                break;
            }
        }
        allDecks = this.GetComponent<SelectDeck>();
        audioSource = this.GetComponent<AudioSource>();

        //Create Card Deck
        /* Debug.Log("Card Deck ID: " + playerCardDeckId);
        spellDeck = allDecks.spellDecks[playerCardDeckId].getSpellDeck();
        //Create Ult
        ultSpell = allDecks.spellDecks[playerCardDeckId].getUltSpell(); */

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
                changeManaServerRpc(0.5f);
            } else {
                changeManaServerRpc(0.25f);
            }
        }

        // Decay bonus healths
        if (currentBonus > 0.0f){
            changeBonusServerRpc(-0.25f);
        }

        // Decay shield duration.
        if (shieldDur > 0.0f){
            shieldDur -= 0.25f;
            if (shieldDur == 0.0f){
                shieldDur = -1.0f;
                myUI.removeShield();
            }
        }
       
        //myUI.updateHealth(currentHealth/maxHealth, currentBonus/maxHealth);
        //myUI.updateMana(currMana/maxMana);

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

        if (AliveManager.Instance.AlivesInGame < 2 && alive) {
            alive = false;
            EndGameServerRpc();
            transform.Find("KeyUI/Victory").gameObject.SetActive(true);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndGameServerRpc() {
        StartCoroutine(FindObjectOfType<LobbyManager>().EndGameCountdown());
    }

    public void OnEnable() {
        _currentHealth.OnValueChanged += OnHealthChanged;
        _currentBonus.OnValueChanged += OnBonusChanged;
        _currMana.OnValueChanged += OnManaChanged;
        _currUlt.OnValueChanged += OnUltChanged;
    }

    public void OnDisable() {
        _currentHealth.OnValueChanged -= OnHealthChanged;
        _currentBonus.OnValueChanged -= OnBonusChanged;
        _currMana.OnValueChanged -= OnManaChanged;
        _currUlt.OnValueChanged -= OnUltChanged;
    }

    public void OnHealthChanged(float oldValue, float newValue) {
        myUI.updateHealth(currentHealth / maxHealth, currentBonus / maxHealth);
    }

    public void OnBonusChanged(float oldValue, float newValue) {
        myUI.updateHealth(currentHealth / maxHealth, currentBonus / maxHealth);
    }

    public void OnManaChanged(float oldValue, float newValue) {
        myUI.updateMana(currMana / maxMana);
    }

    public void OnUltChanged(float oldValue, float newValue) {
        myUI.updateUlt(currUlt / ultSpell.ultCost);
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
            changeBonusServerRpc(-maxHealth);
            changeHealthServerRpc(-dam);
        } else if (currentBonus > 0){
            changeBonusServerRpc(-dam);
        } else {
            changeHealthServerRpc(-dam);
        }

        //myUI.updateHealth(currentHealth/maxHealth, currentBonus/maxHealth);

        if (currentHealth <= 0){
            // Trigger death
            DeathDisablesServerRpc(NetworkManager.Singleton.LocalClientId);
            AliveManager.Instance.RemoveAliveIdServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeathDisablesServerRpc(ulong leaverId) {
        FindObjectOfType<LobbyManager>().RemoveLeaverTargetClientRpc(leaverId);
        DeathDisablesClientRpc();
    }

    [ClientRpc]
    private void DeathDisablesClientRpc() {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        foreach (Collider collider in GetComponentsInChildren<Collider>()) collider.gameObject.layer = 12;
        foreach (Canvas canvas in GetComponentsInChildren<Canvas>()) {
            if (canvas.gameObject.tag != "Key") canvas.gameObject.SetActive(false);
        }
        GetComponent<PlayerController>().DisableCasting();
        alive = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeHealthServerRpc(float value) {
        _currentHealth.Value += value;
        //myUI.updateHealth(currentHealth/maxHealth, currentBonus/maxHealth);
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeBonusServerRpc(float value) {
        _currentBonus.Value += value;
        _currentBonus.Value = Mathf.Clamp(currentBonus, 0, maxHealth);
        //myUI.updateHealth(currentHealth/maxHealth, currentBonus/maxHealth);
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeManaServerRpc(float value){
        _currMana.Value += value;
        _currMana.Value = Mathf.Clamp(currMana, 0, maxMana);
        //myUI.updateMana(currMana/maxMana);
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeUltServerRpc(float value) {
        _currUlt.Value += value;
        //myUI.updateUlt(currUlt/ultSpell.ultCost);
    }

    public void pickupManaCrystal(){
        changeManaServerRpc(15.0f);
        manaPickedUp += 1;
        //myUI.updateMana(currMana/maxMana);
    }

    public void pickupUltCrystal(){
        changeUltServerRpc(3.0f);
        if (currUlt >= ultSpell.ultCost){
            changeUltServerRpc(-ultSpell.ultCost);
            spellQueue.Add(ultSpell);
        }
        //myUI.updateUlt(currUlt/ultSpell.ultCost);
    }

    public void pickupHealthCrystal(){
        changeBonusServerRpc(15.0f);
        //myUI.updateHealth(currentHealth/maxHealth, currentBonus/maxHealth);
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
        changeManaServerRpc(-castSpell.manaCost * manaCostMult);
        changeBonusServerRpc(0.0f);
        spellQueue.RemoveAt(slot);
        if (!castSpell.exhaust){
            spellQueue.Add(castSpell);
        }
        spellsCast += 1;

        //myUI.updateMana(currMana/maxMana);
        myUI.shiftSpells(slot, spellQueue[3].icon);
    }
}
