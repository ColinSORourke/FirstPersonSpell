using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningUlt : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 150) * Time.deltaTime);
    }
}
