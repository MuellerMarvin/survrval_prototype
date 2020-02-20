using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSuicide : MonoBehaviour
{
    public float change = 0.02f;

    private bool _isInvisible;
    public bool isInvisible
    {
        get
        {
            return _isInvisible;
        }
        set
        {
            _isInvisible = value;
            GetComponent<Renderer>().enabled = !value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("trigger enter");
        if(!isInvisible)
        {
            print("make more red");
            isInvisible = true;
            // make it a little more red
            Color color = RenderSettings.skybox.GetColor("_EmissionColor");
            color.r += 0.1f;
            RenderSettings.skybox.SetColor("_EmissionColor", color);
        }
    }

    private void OnApplicationQuit()
    {
        // reset to default
        RenderSettings.skybox.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
    }
}
