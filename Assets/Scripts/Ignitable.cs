using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignitable : MonoBehaviour
{
    public GameObject fire;
    public float currentHeat = 0;
    public bool ignited = false;
    private float minHeat = 0;
    private float maxHeat = 1;
    private float heatIncreasePerFrame = 0.01f;
    private float spreadProbabilityPerFrame = 1f;//0.0001f;
    private bool fireSpawned = false;

    void Update()
    {
        if(ignited)
        {
            if(!fireSpawned)
            {
                Instantiate(this.fire, transform.position, Quaternion.identity);
                fireSpawned = true;
            }

            currentHeat += heatIncreasePerFrame;
            if(currentHeat >= maxHeat)
            {
                currentHeat = maxHeat;

                Spread();
            }
        }
    }

    private void Spread()
    {
        if(Random.Range(0f, 1f) < spreadProbabilityPerFrame)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.1f);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "Ignitable")
                {
                    hitColliders[i].gameObject.GetComponent(this.GetType()).SendMessage("Ignite");
                }
            }
        }
    }

    private bool Ignite()
    {
        this.ignited = true;
        return this.ignited;
    }
}
