using DapperDino.UMT.Lobby.Networking;
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

    public int playerCardDeckId = 0; //ID for Card Decks, 901 will be default ID for default Card Deck (when applicable)
    public baseSpellScript[] spellDeck = new baseSpellScript[7];
    public baseSpellScript ultSpell;
    public List<baseSpellScript> spellQueue = new List<baseSpellScript>();

    public int spellsCast = 0;
    public int manaPickedUp = 0;

    public float takeDamageMult = 1.0f;
    public float manaCostMult = 1.0f;
    public float castTimeMult = 1.0f;

    public float moveSpeed = 12.0f;
    public Movement myMove;

    public GenericUI myUI;
    

    private SelectDeck allDecks;

    public bool alive = true;
    //public AliveManager aliveManager;


    [SerializeField]
    private NetworkVariable<Vector2> moveState = new NetworkVariable<Vector2>();

    [SerializeField]
    private NetworkVariable<bool> jumpState = new NetworkVariable<bool>();

    [SerializeField]
    private NetworkVariable<bool> groundState = new NetworkVariable<bool>();

    [SerializeField]
    private NetworkVariable<bool> casting = new NetworkVariable<bool>();

    [SerializeField]
    private NetworkVariable<int> castInstant = new NetworkVariable<int>();

    [SerializeField]
    private NetworkVariable<bool> castFail = new NetworkVariable<bool>();

    [SerializeField]
    private NetworkVariable<int> hitsAnim = new NetworkVariable<int>();


    private Vector2 oldMoveState = new Vector2(0.0f, 0.0f);
    private bool oldJumpState = false;
    private bool oldGroundState = true;
    private bool oldCastState = false;
    private int oldInstant = 0;
    private int oldHits = 0;

    private Animator animator;

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
        
        audioSource = this.GetComponent<AudioSource>();

        InvokeRepeating("tick", 0.0f, 0.25f);
        myUI.updateUlt(currUlt, 0.0f);

        animator = GetComponentInChildren<Animator>();
        myUI.startManaPS();
    }

    public void setDeck(){
        allDecks = this.GetComponent<SelectDeck>();
        //Create Card Deck
        Debug.Log(OwnerClientId + " Card Deck ID: " + playerCardDeckId);
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
    }

    // Update is called once per frame
    void Update()
    {
        if (oldMoveState != moveState.Value) {
            oldMoveState = moveState.Value;
            animator.SetInteger("LR", (int) Mathf.Round(oldMoveState.x));
            animator.SetInteger("Forward", (int) Mathf.Round(oldMoveState.y));
        }
        if (oldJumpState != jumpState.Value){
            oldJumpState = jumpState.Value;
            animator.SetBool("Jumped", oldJumpState);
        }
        if (oldGroundState != groundState.Value){
            oldGroundState = groundState.Value;
            animator.SetBool("OnGround", oldGroundState);
        }
        if (oldCastState != casting.Value){
            oldCastState = casting.Value;
            if (oldCastState){
                Debug.Log("Start Cast Anim");
                animator.SetTrigger("StartCasting");
            } else {
                if (!castFail.Value){
                    Debug.Log("Finish Cast Anim");
                    animator.SetTrigger("FinishCasting");
                } else {
                    Debug.Log("Fail Cast Anim");
                    animator.SetTrigger("FailCast");
                }
            }
        }

        if (oldInstant != castInstant.Value){
            oldInstant = castInstant.Value;
            Debug.Log("Instant Cast Anim");
            animator.SetTrigger("StartCasting");
            animator.SetTrigger("FinishCasting");
        }

        if (oldHits != hitsAnim.Value){
            oldHits = hitsAnim.Value;
            animator.SetTrigger("Hit");
        }
    }

    // Called every quarter second
    void tick(){
        // Regen mana. Regen is greater if Health is low.
        if (currMana < manaThreshold && doManaRegen){
            if (currentHealth <= healthThreshold){
                changeManaServerRpc(0.4f);
            } else {
                changeManaServerRpc(0.20f);
            }
        }

        // Decay bonus healths
        if (currentBonus > 0.0f){
            changeBonusServerRpc(-0.20f);
        }

        // Decay shield duration.
        if (shieldDur > 0.0f){
            myUI.updateShield(currShields, shieldDur / shieldTime);
            shieldDur -= 0.25f;
            if (shieldDur == 0.0f){
                shieldDur = -1.0f;
                myUI.ShieldFill.fillAmount = 1f;

                if (currShields == 0) myUI.removeShield();

                ShieldActiveServerRpc(false);
            }
        }
       
        //myUI.updateHealth(currentHealth/maxHealth, currentBonus/maxHealth);
        //myUI.updateMana(currMana/maxMana);

        // Decay Auras
        int i = 0; 
        while (i < auras.Count){
            liveAura a = auras[i];
            myUI.updateAura(a);
            int tickInfo = a.update(0.25f);
            if (tickInfo == -1){
                if (GetComponent<NetworkObject>().IsLocalPlayer){
                    auras[i].startRemove();
                    this.removeAura(i);
                } else {
                    i += 1;
                }
            } else {
                i += 1;
            }
        }

        if (AliveManager.Instance.AlivesInGame < 2) {
            if (alive) {
                alive = false;
                EndGameServerRpc();
            }
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
        myUI.updateHealth(currentHealth, currentHealth / maxHealth, currentBonus / maxHealth);

        if (currentHealth <= 0 && GetComponent<NetworkObject>().IsLocalPlayer) {
            // Trigger death
            Debug.Log("Died: " + NetworkManager.Singleton.LocalClientId);
            alive = false;
            transform.Find("KeyUI/Victory").gameObject.GetComponent<UnityEngine.UI.Text>().text = "Defeat!";
            transform.Find("KeyUI/SpectateText").gameObject.SetActive(true);
            DeathDisablesServerRpc(NetworkManager.Singleton.LocalClientId);
            AliveManager.Instance.RemoveAliveIdServerRpc(NetworkManager.Singleton.LocalClientId);
        }

    }

    public void OnBonusChanged(float oldValue, float newValue) {
        myUI.updateHealth(currentHealth, currentHealth / maxHealth, currentBonus / maxHealth);
    }

    public void OnManaChanged(float oldValue, float newValue) {
        myUI.updateMana(currMana, currMana / maxMana);
        if (currMana == maxMana){
            myUI.startManaPS();
  
        }
        else{
            myUI.stopManaPS();
        }
    }

    public void OnUltChanged(float oldValue, float newValue) {
        myUI.updateUlt(currUlt, currUlt / ultSpell.ultCost);
        if (currUlt >= ultSpell.ultCost){
            changeUltServerRpc(-ultSpell.ultCost);
            spellQueue.Add(ultSpell);
        }
        if (currUlt == maxUlt){
            myUI.startUltPS();
  
        }
        else{
            myUI.stopUltPS();
        }
    }

    public void applyAura(Transform src, baseAuraScript aura, float duration){
        int index = this.GetComponent<AuraStorage>().findIndex(aura);
        this.ApplyAuraServerRpc(index, duration);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ApplyAuraServerRpc(int index, float duration){
        ApplyAuraClientRpc(index, duration);
    }

    [ClientRpc]
    public void ApplyAuraClientRpc(int index, float duration){
        baseAuraScript aura = this.GetComponent<AuraStorage>().allAuras[index];

        // INCORRECT AND TEMPORARY
        Transform src = this.transform;

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
            liveAura toApply;
            if (IsLocalPlayer){
                toApply = new liveAura();
            } else {
                toApply = new fakeAura();
            }
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
        removeAuraServerRpc(i);
    }

    [ServerRpc(RequireOwnership = false)]
    public void removeAuraServerRpc(int i){
        removeAuraClientRpc(i);
    }

    [ClientRpc]
    public void removeAuraClientRpc(int i){
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

    public void changeSpeed(float value){
        moveSpeed += value;
        myMove.setMovementSpeed(moveSpeed);
    }

    public void takeDamage(float dam, bool mult = true){
        if (mult){
            dam *= takeDamageMult;
            this.hitAnimServerRPC();
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
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeathDisablesServerRpc(ulong leaverId) {
        FindObjectOfType<LobbyManager>().RemoveLeaverTargetClientRpc(leaverId);
        DeathDisablesClientRpc();
    }

    [ClientRpc]
    private void DeathDisablesClientRpc() {
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        foreach (Collider collider in GetComponentsInChildren<Collider>()) collider.gameObject.layer = 12;
        foreach (Canvas canvas in GetComponentsInChildren<Canvas>()) {
            if (canvas.gameObject.tag != "Key") canvas.gameObject.SetActive(false);
        }
        GetComponent<PlayerController>().DisableCasting();
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeHealthServerRpc(float value) {
        
        _currentHealth.Value += value;
        _currentHealth.Value = Mathf.Clamp(currentHealth, 0, maxHealth);
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
        
        //myUI.updateUlt(currUlt/ultSpell.ultCost);
    }

    public void pickupHealthCrystal(){
        changeHealthServerRpc(5.0f);
        //myUI.updateHealth(currentHealth/maxHealth, currentBonus/maxHealth);
    }

    public float validCast(int slot, bool Target, float distance){
        if (!alive) return -1.0f;

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
        myUI.shiftSpells(slot, spellQueue[3]);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateMoveStateServerRpc(Vector2 Move) {
        moveState.Value = Move;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateJumpStateServerRpc(bool b) {
        jumpState.Value = b;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateGroundStateServerRpc(bool b) {
        groundState.Value = b;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateCastServerRPC(bool b, bool f = false){
        casting.Value = b;
        castFail.Value = f;
    }

    [ServerRpc(RequireOwnership = false)]
    public void InstantCastServerRPC(){
        castInstant.Value += 1;
    }

    [ServerRpc(RequireOwnership = false)]
    public void hitAnimServerRPC(){
        hitsAnim.Value += 1;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShieldActiveServerRpc(bool state) {
        ShieldActiveClientRpc(state);
    }

    [ClientRpc]
    public void ShieldActiveClientRpc(bool state) {
        if (!IsLocalPlayer) transform.Find("ShieldPlaceholder").gameObject.SetActive(state);
    }
}
