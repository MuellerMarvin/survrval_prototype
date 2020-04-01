using System.Collections.Generic;
using UnityEngine;

public enum CalculationOption { Linear, Exponential, AntiExponential };

public class EnvironmentManager : MonoBehaviour
{
    #region Public Variables
    public CalculationOption FogCalculationMethod;
    public float MinimumFogIntensity;
    public float MaximumFogIntensity;
    public float FogChangePerFrame;
    public float PercentageDeadBeforeFire;
    public float BaseFireProbabilityPerFrame = 0.000001f;
    public float ignitableObjectSearchSize = 10000f;
    #endregion

    #region Private Variables
    public int MaximumTrees
    {
        get
        {
            return _MaximumTrees;
        }
        set
        {
            _MaximumTrees = value;
        }
    }
    public int CurrentTrees
    {
        get
        {
            return _CurrentTrees;
        }
        set
        {
            _CurrentTrees = value;
            CalculateFogGoal();
            CalculateFireProbability();
        }
    }
    private float FogIntensityGoal = 0;
    private float CurrentFogIntensity = 0;
    private float FireProbabilityPerFrame = 0;
    private List<GameObject> ignitableObjects = new List<GameObject>();
    #endregion

    #region HolderVariables
    private int _MaximumTrees = 0;
    private int _CurrentTrees = 0;
    #endregion

    #region Utility Properties
    public float PercentageOfTreesAlive
    {
        get
        {
            if(MaximumTrees == 0)
            {
                return 100;
            }
            return CurrentTrees * (100 / MaximumTrees);
        }
    }
    public float PercentageOfTreesDead
    {
        get
        {
            return (100 - PercentageOfTreesAlive);
        }
    }
    #endregion

    #region Start / Update Methods
    private void Start()
    {
        CurrentFogIntensity = 0f;
        RenderSettings.fogDensity = CurrentFogIntensity;
        RenderSettings.skybox.SetFloat("_FogIntens", CurrentFogIntensity);
        RenderSettings.fog = true;

        // get all ignitable objects
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ignitableObjectSearchSize); // list of all objects near this one
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Ignitable") // filter for ignitable objects
            {
                ignitableObjects.Add(hitColliders[i].gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region FireSpawning
        if (FireProbabilityPerFrame > 0)
        {
            if (Random.Range(0f, 1f) < FireProbabilityPerFrame)
            {
                print("igniting something!");
                int chosenOne = Random.Range(0, ignitableObjects.Count); // choose which one to ignite
                ignitableObjects[chosenOne].GetComponent(typeof(Ignitable)).SendMessage("Ignite"); // ignite it
            }
        }
        #endregion

        #region FogChange
        if (CurrentFogIntensity == FogIntensityGoal) { return; } // if the CurrentFog and the goal are equal, no action is needed

        // Calculate new fog intensity
        if (CurrentFogIntensity < FogIntensityGoal) // If the CurrentFog is less than the goal
        {
            if ((FogIntensityGoal - CurrentFogIntensity) < FogChangePerFrame) // calculate the difference between them
            {
                CurrentFogIntensity = FogIntensityGoal; // if the difference is smaller than a frame-step, just set the current fog to the goal
            }
            else // otherwise
            {
                CurrentFogIntensity += FogChangePerFrame; // move the intensity a little closer to the goal
            }
        }
        else if (CurrentFogIntensity > FogIntensityGoal) // If the CurrentFog is more than the Goal
        {
            if ((CurrentFogIntensity - FogIntensityGoal) < FogChangePerFrame) // calculate the difference between them
            {
                CurrentFogIntensity = FogIntensityGoal; // if the difference is smaller than a frame-step, just set the current fog to the goal
            }
            else // otherwise
            {
                CurrentFogIntensity -= FogChangePerFrame; // move the intensity a little closer to the goal
            }
        }

        // Set new intensity
        RenderSettings.fogDensity = CurrentFogIntensity;
        RenderSettings.skybox.SetFloat("_FogIntens", CurrentFogIntensity);
        #endregion
    }
#endregion

    #region Call Methods
    /// <summary>
    /// Should be called by a tree when it has been harvested.
    /// </summary>
    /// <returns>The current count of trees in the world.</returns>
    public int CallTreeHarvested()
    {
        print("Tree Harvested called.");
        CurrentTrees--;
        return CurrentTrees;
    }

    public int CallTreePlanted()
    {
        print("Tree Planted called.");
        CurrentTrees++;
        return CurrentTrees;
    }

    /// <summary>
    /// Increases the maximum and current tree counts, without refreshing the fog-goal - this is for when the game is initiated with its initial count of trees.
    /// </summary>
    /// <returns>The current count of trees in the world.</returns>
    public int CallMaximumTreeCountIncrease()
    {
        print("MaximumTreeCountIncreased called.");
        MaximumTrees++;
        CurrentTrees++;
        return CurrentTrees;
    }
    #endregion

    #region Recalculation Methods
    /// <summary>
    /// Recalculates what amount of fog should be in the world at a given time
    /// </summary>
    /// <returns>The new fog-goal</returns>
    private float CalculateFogGoal()
    {
        print("Current: " + CurrentTrees + " Max: " + MaximumTrees);
        if(FogCalculationMethod == CalculationOption.Linear)
        {
            FogIntensityGoal = (MaximumTrees - CurrentTrees) * ((MaximumFogIntensity - MinimumFogIntensity) / MaximumTrees);
        }
        else if (FogCalculationMethod == CalculationOption.Exponential)
        {
            Mathf.Pow((MaximumTrees - CurrentTrees), (MaximumFogIntensity - MinimumFogIntensity + 1) / MaximumTrees);
        }
        print("FogGoal: " + FogIntensityGoal);
        return FogIntensityGoal;
    }

    /// <summary>
    /// Recalculates with what probabilty fire should spawn each frame
    /// </summary>
    private float CalculateFireProbability()
    {
        if(PercentageOfTreesDead > PercentageDeadBeforeFire) // after "percentagebeforefire"% of trees have been killed, start spawning fires
        {
            FireProbabilityPerFrame = ((1 + PercentageOfTreesDead) * BaseFireProbabilityPerFrame);
            print("Dead: " + PercentageOfTreesDead + "%");
            print("Alive: " + PercentageOfTreesAlive + "%");
            print("FireProbab: " + FireProbabilityPerFrame);
        }
        else
        {
            FireProbabilityPerFrame = 0;
        }
        return FireProbabilityPerFrame;
    }
    #endregion


    /// <summary>
    /// Is called when the game quits and resets the fog-values to their defaults.
    /// </summary>
    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_FogIntens", 0f);
        print("Values were reset.");
    }
}
