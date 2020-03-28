using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBack : MonoBehaviour
{
    public string tagName;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "WorldBoundary")
        {
            this.transform.position = new Vector3(-0.78f, 0.442f, -0.6756546f);
            this.transform.rotation = new Quaternion(0, 0, 43.152f, 0);
        }
    }
}
