using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableTree : MonoBehaviour
{
    // Configuration
    public bool wasPlanted;
    public EnvironmentManager environmentManager;
    public GameObject sapling;
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

    private void Start()
    {
        environmentManager.CallMaximumTreeCountIncrease();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Axe" && isVisible)
        {
            // Make tree invisible
            this.isVisible = false;
            print("Tree invisible.");

            // Spawn sticks
            for(int i = 0; i < this.AmountOfSticksToSpawn; i++)
            {
                Instantiate(this.stick, // configure what will spawn
                    this.transform.position + new Vector3(Random.Range(-DropSquareSize, DropSquareSize), 0.5f, Random.Range(-DropSquareSize, DropSquareSize)), // decide on the position it will spawn in
                    Quaternion.Euler(90f, Random.Range(90f, 360f), 0)); // configure the rotation it will spawn in
            }
            print("Sticks spawned.");

            // Spawn sapling
            ((PlantableSapling)sapling.gameObject.GetComponent(typeof(PlantableSapling))).environmentManager = this.environmentManager; // pass the sapling the environment manager of this scene
            Instantiate(this.sapling, this.transform.position + new Vector3(0,1f,0), Quaternion.identity);

            // Call that the tree was removed to the EnvironmentManager
            environmentManager.CallTreeHarvested();
        }
    }
}
