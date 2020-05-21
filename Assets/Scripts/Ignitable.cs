using System.Collections.Generic;
using UnityEngine;

public class Ignitable : MonoBehaviour
{
    // Optional BurntCounter
    public BurntCounter burntCounter;

    // fire and meshObject
    public GameObject fireObject;
    public GameObject meshObject;

    // Public variables and properties
    public bool drawScanDistance = false;
    public float scanForOthersDistance = 2f;
    public bool ignited = false;
    public float currentHeat = 0f;
    public float changePerSecond = 0.05f;
    public float spreadProbabilityPerFrame = 0.05f;
    public float spreadHeat = 1f;
    public float fullyBlackHeat = 2f;
    public float destroyHeat = 3f;
    public Color burntColor;
    public float blackLevel { get; set; }
    public bool extinguishable = true;
    public bool receiveWater = true;
    public float coolingTillExtinguished = 1;
    public float cooled = 0;

    // Internal Variables
    private bool destroyed = false;
    private Color defaultMaterialColor;
    private List<GameObject> ignitableObjects;

    /// <summary>
    /// This function gets called once at the start of the game
    /// </summary>
    private void Start()
    {
        // Scan for other Ignitable Objects
        ScanForIgnitables();

        // save default color for black-level adjustment
        defaultMaterialColor = this.meshObject.GetComponent<MeshRenderer>().material.color;

        // first-time calculate blackLevel and ajust it
        AdjustBlackLevel(CalculateBlackLevel());
    }

    /// <summary>
    /// This function gets called on every rendered frame
    /// </summary>
    private void Update()
    {
        AdjustBlackLevel(blackLevel);
        if(ignited)
        {
            if(cooled > coolingTillExtinguished && extinguishable) // checks if the fire has been cooled down, so that it would stop burning
            {
                ignited = false;
                return;
            }

            ShowFire(true);
            currentHeat += changePerSecond * Time.deltaTime; // increases heat
            cooled = Mathf.Clamp(cooled - (changePerSecond * Time.deltaTime), 0, float.MaxValue); // decreases external cooling

            // if it has burnt down
            if(currentHeat > destroyHeat)
            {
                ShowSelf(false);
                ShowFire(false);
                ignited = false;
                destroyed = true;
                // count burnt
                burntCounter.CountBurnt();
            }
            else if(currentHeat > spreadHeat)
            {
                Spread(spreadProbabilityPerFrame);
            }
        }
        else
        {
            ShowFire(false);
            cooled = 0;

            // check if the current heat is below 0 - lock it to 0 as a minimum
            if(currentHeat <= 0)
            {
                currentHeat = 0;
            }
            else
            {
                currentHeat -= changePerSecond * Time.deltaTime;
            }
        }

        // check if it's destroyed and if it's below 0 heat, show the object again
        if (destroyed && currentHeat == 0)
        {
            ShowSelf(true);
            ShowFire(false);
        }

        AdjustBlackLevel(CalculateBlackLevel()); // recalculate the blackLevel and adjust the color of the object to it
    }

    /// <summary>
    /// This function will ignite the object, if it isn't destroyed - this should be the only way the object gets ignited at runtime
    /// </summary>
    /// <returns></returns>
    public bool AttemptIgnite()
    {
        if(destroyed)
        {
            return false;
        }
        else
        {
            return ignited = true;
        }
    }

    /// <summary>
    /// Spread to one object in range
    /// </summary>
    /// <returns>If it was spread to another object</returns>
    public bool Spread()
    {
        Spread(1f);
        return true;
    }

    /// <summary>
    /// Spread to one object in range, with a certain probability
    /// </summary>
    /// <param name="probability">probability of spread from 0 to 1</param>
    /// <returns>If the fire was spread</returns>
    public bool Spread(float probability)
    {
        if(ignitableObjects.Count == 0)
        {
            print("Nowhere to spread to.");
        }

        if(Random.Range(0f, 1f) <= probability)
        {
            // Spread to a random neighbour
            ignitableObjects[Random.Range(0, ignitableObjects.Count - 1)].SendMessage("AttemptIgnite");
        }
        return false;
    }

    /// <summary>
    /// Shows or hides the fire.
    /// </summary>
    /// <param name="fireVisible">Visibility of the fire.</param>
    public void ShowFire(bool fireVisible)
    {
        this.fireObject.SetActive(fireVisible);
    }

    /// <summary>
    /// Shows or hides the entire GameObject.
    /// </summary>
    /// <param name="selfVisible">Visibility of the GameObject.</param>
    public void ShowSelf(bool selfVisible)
    {
        this.meshObject.SetActive(selfVisible);
    }

    /// <summary>
    /// Changes the amount of black applied to the GameObject's material.
    /// </summary>
    /// <param name="blackLevel"></param>
    private void AdjustBlackLevel(float blackLevel)
    {
        this.meshObject.GetComponent<MeshRenderer>().material.color = (defaultMaterialColor * (1 - blackLevel)) + (burntColor * blackLevel);
    }

    /// <summary>
    /// Recalculates the blackLevel variable, where the minimum is 0 and the maximum is 1.
    /// </summary>
    /// <returns>blackLevel</returns>
    private float CalculateBlackLevel()
    {
        if(currentHeat == 0)
        {
            blackLevel = 0;
        }
        else if(currentHeat >= fullyBlackHeat)
        {
            blackLevel = 1;
        }
        else
        {
            blackLevel = currentHeat / fullyBlackHeat;
        }

        return blackLevel;
    }

    private void ScanForIgnitables()
    {
        // Create a new empty list
        ignitableObjects = new List<GameObject>();

        // fill the list with all ignitable objects in 'scanForOthersDistance' distance
        foreach(Collider collider in Physics.OverlapSphere(this.gameObject.transform.position, scanForOthersDistance))
        {
            if (collider.gameObject.tag.ToLower() == "ignitable")
            {
                ignitableObjects.Add(collider.gameObject);
            }
        }
    }

    /// <summary>
    /// Can be called to attempt to cool down the fire and extinguish it
    /// </summary>
    /// <param name="amount"></param>
    public void CoolDown(float amount)
    {
        cooled += amount;
    }

    public void Fill(float amount)
    {
        if(receiveWater)
        {
            CoolDown(amount);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a red sphere the size of the configured scan-distance, if the option is enables
        if(drawScanDistance)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, scanForOthersDistance);
        }
    }
}
