using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSuicide : MonoBehaviour
{
    public GameObject stick;
    private bool _isVisible = true;
    public bool isVisible
    {
        get
        {
            return _isVisible;
        }
        set
        {
            _isVisible = value;
            this.gameObject.SetActive(value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Axe" && isVisible)
        {
            RenderSettings.fog = true;
            RenderSettings.fogDensity = RenderSettings.fogDensity + 0.1f;
            isVisible = false;
            print("Tree removed.");
            Instantiate(stick, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity); ;
            print("Stick spawned.");
        }
        RenderSettings.skybox.SetFloat("_FogIntens", RenderSettings.fogDensity);
    }

    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_FogIntens", 0f);
        print("Values were reset.");
    }
}
