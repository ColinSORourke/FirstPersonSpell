using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LaggyPlayerReturn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<NetworkObject>().IsLocalPlayer) other.transform.position = new Vector3(140, 74, 235);
    }
}
