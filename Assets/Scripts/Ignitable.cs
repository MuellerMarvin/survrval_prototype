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
            this.fire.gameObject.transform.localScale = new Vector3(_currentHeat, _currentHeat, _currentHeat);
        }
    }
    public float _currentHeat = 0;
    public bool ignited = false;
    private float minHeat = 0;
    private float maxHeat = 1;
    private float heatIncreasePerFrame = 0.0001f;
    private float spreadProbabilityPerFrame = 0.0001f;
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
        fire = Instantiate(fire, transform.position, Quaternion.identity);
        currentHeat = 0;
    }

    void Update()
    {
        if(ignited)
        {
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
    }

    private void Spread()
    {
        if(ignitableObjects.Count > 0 && Random.Range(0f, 1f) < spreadProbabilityPerFrame)
        {
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
        }
    }
}
