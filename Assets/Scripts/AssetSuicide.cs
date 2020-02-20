using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSuicide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Should poof");
        GetComponent<Renderer>().enabled = false;
    }
}
