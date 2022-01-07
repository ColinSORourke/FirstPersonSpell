Colin O'Rourke
csorourk@ucsc.edu
CMPM 121 Assignment Final

This is my skeleton of Wizard Arena / First Person SpellCaster

This is NOT a complete game, but meant to have implemented all the interlocking systems of a game so I can build upon it.
I plan on pitching this to a larger group in 171 to find collaborators to work with fleshing this out.

So what's in this?

---------------------Spell & Aura Scripting---------------------

The game is focused on casting spells, so I wrote a robust system of scriptable objects to represent those spells.

A spell has
* An ID
* A display icon
* A mana cost
* A cast time (could be 0 for instant cast)
* A Damage Value
* A boolean of whether the spell 'exhausts'
* Can be either instant or projectile
    * If Projectile contains a prefab foor the projectile object
    * A Projectile speed
* References to 2 Aura objects
* Reference to another Spell possibly
* Reference to 2 Particle Systems

* On Cast function (to be overridden by derived objects)
* On Hit function (to be overridden by derived objects)

The player has a 'deck' of 7 spells & a hand of 3 spells. The player also has 50 mana points.
The spells in the players hand have their icons displyed above the player's mana bar (as well as the next spell to be drawn)
They can cast any of the 3 spells in their hand if they have the mana, and a target in line of sight.
And all of the above properties are respected so that I can design a wide array of spells, and override the functions to write special functionality for every spell.

An Aura is a status effect caused by a spell. Every Aura has:
* An ID
* A display icon
* A tickspeed
* A 'stackable boolean'
* A maxStacks integer

* Virtual functions to be overridden for OnApply, OnTick, OnExpire, OnStack

Auras are applied to enemies, and have their icons displayed above the enemy's health bar.
Live Auras have a duration that the enemy keeps track of, and the auras are removed when duration goes below 0.

The majority of work was setting up this system of ScriptableObjects so I could design complex spells and play out the system without error.
After I finished writing & testing this framework, I implemented a whole Fire Themed set of spells in about 30 minutes with minimal coding - this was exactly the goal.

---------------------The actual parts in the game---------------------

Player
    * Very simple first person character controller. 
        * Mouse to look around, 
        * Horizontal/Vertical axis to move relative to viewpoint. 
        * Space to Jump
            * You can still move WASD while jumping, but it is less effective, prioritizing your momentum from the start of the jump.
    * Mana Component
        * Player has a Mana Bar with 50 Mana. This is displayed as a blue rectangle on the UI, which scales as the player gains/loses mana.
    * Targeting Component
        * Pressing F selects & stores a target from the target layer in the player's line of sight * field of view. 
            * Pressing F repeatedly cycles through available targets.
            * The current target is highlighted with a yellow squaare above their head.
            * When a target exits line of sight, it is untargeted.
            * To select a target, the target needs to be visible on the screen, but it remains targeted even if player looks away as long as it's not obscured.
        * The player has a deck & hand of spells.
            * The current hand of spells is displayed above the players mana bar.
            * Pressing 1, 2, or 3, casts the 1st, 2nd, or 3rd (Left->right) spell at the current target if the player has enough mana.
                * If the spell has a cast time, a cast bar will appear showing progress on cast time. Mana will not be spent until cast finishes.
                    * Casting will cancel if the player's target changes.
            * Spells that are cast will resolve their effects, the cast spell is put on the bottom of the deck, and the spells shift over.
                * The cast spell is not returned to deck if spell is tagged exhaust.
    * Player UI
        * Mana bar on the bottom of screen reflecting players mana status.
        * Spell icons in center of screen reflecting players hand of spells
        * Cast Bar above spells while player is casting a spell


Target Dummies
    * Health Component
        * Tracks and displays a Health & Bonus Health value in red & yellow bars above the enemies head
        * Tracks and displays all Auras above the enemies head.
    * Above head UI 
        * Health/BonusHealth bar red and yellow
        * Dynamic list of Aura icons above health bar
        * Yellow TargetMarker Square at top when targeted by player.

Mana Crystals
    * Manager
        * Stores a list of active Crystals and Crystal Spawnpoints (derivd from child game objects)
        * Spawns a new crystal at a random unoccupied spawn point every 10 seconds
            * As long as there are no more than 3 Crystals
    * Mana Crystals
        * Float around
        * Restore 10 mana when touched by player.

Projectiles
    * Spawned when the player casts a projectile spell
    * Move towards a target game object by speed every update
    * on collision with target, resolves spell effect, and destroys self

(In the demo, there is no case where you have multiple different auras on a target, but that does work, and is entirely in place.)


-------------The Implemented Spells---------------------

BurnAura - Ticks for 1 damage every second. Emits orange particles while target is burned. Denoted by Red Square icon

FireSpellA - Costs 5 mana, takes 1 second to cast. Shoots a small fireball projectile that deals 2 damage to a target, and emits a particle burst on hit.
FireSpellB - Costs 8 mana, takes 2 seconds to cast. Instantly applies a 5 second burn to the target. If Target is already burnt, applies a 15 second burn instead.
FireSpellC - Costs 14 mana, is cast instantly. Immediately Deals 5 damage to target & applies a 5 second burn.
FireSpellD - Costs 15 mana, takes 2 second to cast. Shoots a small fireball projectile that deals 8 damage to target. Refunds 10 mana (& plays a particle burst) if target has Burn aura.
FireSpellE - Costs 8 mana, is cast instantly. Immediately deals 5 damage to target. If target has a burn aura, removes the burn and deals extra damage equal to double what the burn would have done.
FireSpellF - Costs 5 mana, takes 2 seconds to cast. Shoots a small fireball projectile that deals 2 damage to a target. If it is cast from the players left-most spell position, adds a powerful spellG to the deck. (Also plays a particle effect on player on success)

FireSpellG - Not normally in deck - costs 5 mana, takes 5 seconds to cast. Shoots a small fireball projectile that deals 20 damage to a target, and emits a particle burst on hit. Exhausts, so it does not return to queue after being cast.

Reminder: After I implemented my framework, all of these spells including their effects took like 30 minutes to implement.