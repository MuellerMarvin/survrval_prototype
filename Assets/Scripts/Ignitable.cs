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
    public bool ignited
    { get { return _ignited; }
        set
        {
            _ignited = value;
            this.fire.SetActive(_ignited);
        }
    }
    public bool _ignited = false;
    private float minHeat = 0;
    private float spreadHeat = 100;
    private float fullyBlackHeat = 2f;
    private float destroyHeat = 1.5f;
    private bool destroyed = false;
    private float destroyedSince = 0;
    private float respawnTime = 100;
    public float heatIncreasePerFrame = 0.0005f;
    public float spreadProbabilityPerFrame = 0.001f;
    private List<GameObject> ignitableObjects = new List<GameObject>();
    private Renderer grasRenderer;
    private Color startingColor;
    public float blackLevel
    {
        get
        {
            return _blackLevel;
        }
        set
        {
            _blackLevel = value;
        }
    }
   private float _blackLevel = 0;

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
        foreach(Transform child in this.transform)
        {
            if(child.name == "Fire")
            {
                this.fire = child.gameObject;
            }
            else
            {
                this.grasRenderer = child.gameObject.GetComponent<Renderer>();
            }
        }
        // get default values
        startingColor = grasRenderer.material.GetColor("_Color");
        print(this.fire);
        currentHeat = 0;

        // add some time to the respawntime to make it seem more natural
        respawnTime += Random.Range(0, 100);
    }

    void Update()
    {
        if (destroyed)
        {
            this.gameObject.SetActive(false);
            this.fire.SetActive(false);
            destroyedSince += Time.deltaTime;
            if(destroyedSince > respawnTime)
            {
                // reset the object
                this.gameObject.SetActive(true);
                ignited = false;
                currentHeat = 0;
            }
        }
        else if(ignited)
        {
            this.fire.SetActive(true);
            currentHeat += heatIncreasePerFrame;
            if(currentHeat > destroyHeat)
            {
                destroyed = true;
                this.gameObject.SetActive(false);
                ignited = false;
                return;
            }
            // else
            RecalculateBlackLevel();
        }
    }

    private void UpdateColor()
    {
        grasRenderer.material.SetColor("_Color", new Color(startingColor.r * (1- blackLevel), startingColor.g * (1 - blackLevel), startingColor.b * (1 - blackLevel)));
    }

    private void RecalculateBlackLevel()
    {
        if (currentHeat > fullyBlackHeat)
        {
            blackLevel = 1.0f;
        }
        else
        {
            blackLevel = ((fullyBlackHeat - minHeat) / 100) * currentHeat;
        }
        UpdateColor();
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
