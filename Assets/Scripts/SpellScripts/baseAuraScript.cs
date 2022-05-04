using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Aura", menuName = "ScriptableObjects/baseAuraScript", order = 1)]
public class baseAuraScript : ScriptableObject
{
    public int id;
    public int selfIndex;
    public bool stackable = false;
    public int maxStacks = 1;
    public float tickSpeed = -1.0f;
    public float damage = 1.0f;

    // public Aura auraOther;
    // public baseSpellScript token;
    public ParticleSystem auraParticle;
    public Sprite icon;

    public virtual void onApply(Transform Player, Transform Target){
        Debug.Log("Aura " + id + " applied");
    }

    public virtual void onTick(Transform Player, Transform Target, int stack, int tickNum){
        Debug.Log("Aura " + id + " ticked");
    }

    public virtual void onExpire(Transform Player, Transform Target, int stack, int tickNum){
        
        Debug.Log("Aura " + id + " expired");
    }   

    public virtual void onStack(Transform Player, Transform Target, int stack){
        Debug.Log("Aura " + id + " stacked");
    }
}

public class liveAura {
    public baseAuraScript aura;

    public Transform on;
    public Transform src;
    public int stacks;
    public float duration;
    public float tickTime;
    public int tickNum;

    ParticleSystem activeParticle;

    public virtual void onApply(){
        if (aura.auraParticle != null)
        {
            activeParticle = GameObject.Instantiate(aura.auraParticle, on);
        }
        aura.onApply(src, on);

    }

    public virtual void onTick(){
        aura.onTick(src, on, stacks, tickNum);
    }

    public virtual void onExpire(){
        if (aura.auraParticle != null)
        {
            GameObject.Destroy(activeParticle.gameObject);
        }
        aura.onExpire(src, on, stacks, tickNum);
    }

    public virtual bool onStack(){
        if (stacks < aura.maxStacks)
        {
            stacks += 1;
            aura.onStack(src, on, stacks);
            return true;
        }
        return false;
    }

    public int update(float delta){
        tickTime += delta;
        duration -= delta;
        
        if (tickTime > aura.tickSpeed && aura.tickSpeed != -1){
            tickTime = 0.0f;
            tickNum += 1;
            this.onTick();
            return 1;  
        }
        if (duration < 0.0f){
            return -1;
        }
        return 0;
    }
}

public class fakeAura : liveAura {
    public baseAuraScript aura;

    public Transform on;
    public Transform src;
    public int stacks;
    public float duration;
    public float tickTime;
    public int tickNum;

    ParticleSystem activeParticle;

    public override void onApply(){
        if (aura.auraParticle != null)
        {
            activeParticle = GameObject.Instantiate(aura.auraParticle, on);
        }
    }

    public override void onTick(){
        // Nothing
    }

    public override void onExpire(){
        if (aura.auraParticle != null)
        {
            GameObject.Destroy(activeParticle.gameObject);
        }
    }

    public override bool onStack(){
        if (stacks < aura.maxStacks)
        {
            stacks += 1;
            return true;
        }
        return false;
    }
}