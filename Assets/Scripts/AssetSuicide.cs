﻿using System.Collections;
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
            RenderSettings.fogDensity = RenderSettings.fogDensity + 0.1f;
            isVisible = false;
            print("nyomed");
        }
        RenderSettings.skybox.SetFloat("_FogIntens", RenderSettings.fogDensity);
    }

    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_FogIntens", 0f);
    }
}
