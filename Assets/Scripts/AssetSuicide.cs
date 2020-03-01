using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSuicide : MonoBehaviour
{
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
        if(isVisible)
        {
            RenderSettings.fog = true;
            RenderSettings.fogEndDistance = RenderSettings.fogEndDistance - 15;
            isVisible = false;
            print("nyomed");
        }
    }
}
