using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDeck : MonoBehaviour
{
    public SpellDeck[] spellDecks;

    public virtual SpellDeck selectDeckByInt(int index)
    {
        int validIndex = index % spellDecks.Length;
        return spellDecks[validIndex];
    }
}
