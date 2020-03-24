using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSuicide : MonoBehaviour
{
    // Configuration
    public GameObject stick;
    public int AmountOfSticksToSpawn;
    public float DropSquareSize;

    // Internal Values
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
            // Increase fog
            RenderSettings.fog = true;
            RenderSettings.fogDensity = RenderSettings.fogDensity + 0.1f;

            // Make tree invisible
            this.isVisible = false;
            print("Tree removed.");

            // Spawns sticks
            for(int i = 0; i < this.AmountOfSticksToSpawn; i++)
            {
                Instantiate(this.stick, // configure what will spawn
                    this.transform.position + new Vector3(Random.Range(-DropSquareSize, DropSquareSize), 0.5f, Random.Range(-DropSquareSize, DropSquareSize)), // decide on the position it will spawn in
                    Quaternion.Euler(90f, Random.Range(90f, 360f), 0)); // configure the rotation it will spawn in
            }
            print("Sticks spawned.");
        }
        RenderSettings.skybox.SetFloat("_FogIntens", RenderSettings.fogDensity);
    }

    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_FogIntens", 0f);
        print("Values were reset.");
    }
}
