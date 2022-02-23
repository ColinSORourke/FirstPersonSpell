using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SpellRpcs>().projectiles.Add(this.gameObject);
    }
}
