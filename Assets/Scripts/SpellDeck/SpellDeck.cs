using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellDeck", menuName = "ScriptableObjects/SpellDeck", order = 1)]
public class SpellDeck : ScriptableObject
{
    public int id;
    public baseSpellScript[] spellDeck;
    public baseSpellScript ultSpell;

    public virtual baseSpellScript[] getSpellDeck() { return spellDeck; }
    public virtual baseSpellScript getUltSpell() { return ultSpell; }
}
