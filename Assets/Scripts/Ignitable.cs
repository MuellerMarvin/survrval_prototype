using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignitable : MonoBehaviour
{
    public GameObject fire;
    public float currentHeat
    {
        get
        {
            return _currentHeat;
        }
        set
        {
            _currentHeat = value;
        }
    }
    public float _currentHeat = 0;
    public bool ignited = false;
    private float minHeat = 0;
    private float maxHeat = 1;
    public float heatIncreasePerFrame = 0.0005f;
    public float spreadProbabilityPerFrame = 0.001f;
    private List<GameObject> ignitableObjects = new List<GameObject>();


    void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.1f); // list of all objects near this one

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Ignitable") // filter for ignitable objects
            {
                ignitableObjects.Add(hitColliders[i].gameObject);
            }
        }
        fire = Instantiate(fire, transform.position, Quaternion.Euler(-90f, 0, 0));
        this.fire.SetActive(false);
        currentHeat = 0;
    }

    void Update()
    {
        if(ignited)
        {
            this.fire.SetActive(true);
            currentHeat += heatIncreasePerFrame;
            if(currentHeat >= maxHeat)
            {
                currentHeat = maxHeat;
                if(Random.Range(0f, 1f) < spreadProbabilityPerFrame)
                {
                    Spread();
                }
            }
        }
        else
        {
            this.fire.SetActive(false);
        }
    }

    private void Spread()
    {
        if(ignitableObjects.Count > 0)
        {
            print("spreading.");
            int chosenOne = Random.Range(0, ignitableObjects.Count); // choose which one to ignite
            ignitableObjects[chosenOne].GetComponent(this.GetType()).SendMessage("Ignite"); // ignite it
        }
    }

    private bool Ignite()
    {
        this.ignited = true;
        return this.ignited;
    }

    private bool Extinguish()
    {
        this.ignited = false;
        currentHeat = 0;
        return this.ignited;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(ignited && other.tag == "Axe")
        {
            Extinguish(); // extinguish on contact with the axe
            print("Extinguish");
        }
    }
}
