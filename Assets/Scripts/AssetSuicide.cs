using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSuicide : MonoBehaviour
{
    public float red = 0.7f;
    public float green = 0.7f;
    public float blue = 0.7f;
    public float change = 0.02f;

    private void Start()
    {
        SetColorToValues();
    }

    private void SetColorToValues()
    {
        RenderSettings.skybox.SetColor("_EmissionColor", new Color(red, green, blue));
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Should poof");
        GetComponent<Renderer>().enabled = false;

        // make sky a little more red
        red += change;
        green -= change;
        blue -= change;
        SetColorToValues();
    }

    private void OnApplicationQuit()
    {
        red = 0.5f;
        green = 0.5f;
        blue = 0.5f;
        SetColorToValues();
    }
}
