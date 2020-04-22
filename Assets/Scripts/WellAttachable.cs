using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WellAttachable : MonoBehaviour
{
    private Well attachedToScript;
    private bool attached = false;

    private void OnTriggerEnter(Collider other)
    {
        // if it's not a well, ignore it
        if (!other.CompareTag("Well")) return;

        // attach me
        Attach(other.gameObject);
    }

    public void Attach(GameObject wellGameObject)
    {
        attachedToScript = wellGameObject.GetComponent<Well>();
        attachedToScript.AttachBucket(this.gameObject);
        attached = true;
    }

    public void Detach()
    {
        if (!attached) return; // if it isn't attached, it can't detach

        attachedToScript.DetachBucket();
        attachedToScript = null;
        attached = false;
    }
}
