using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public float MinimumFogIntensity;
    public float MaximumFogIntensity;

    private int MaximumTrees = 0;
    private int CurrentTrees = 0;
    private float FogIntensityGoal;
    private float CurrentFogIntensity;

    private void Start()
    {
        CurrentFogIntensity = 0f;
        RenderSettings.fogDensity = CurrentFogIntensity;
        RenderSettings.skybox.SetFloat("_FogIntens", CurrentFogIntensity);
        RenderSettings.fog = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentFogIntensity != FogIntensityGoal)
        {
            RenderSettings.fogDensity = FogIntensityGoal;
            RenderSettings.skybox.SetFloat("_FogIntens", FogIntensityGoal);
            CurrentFogIntensity = FogIntensityGoal;
        }
    }

    /// <summary>
    /// Should be called by a tree when it has been harvested.
    /// </summary>
    /// <returns>The current count of trees in the world.</returns>
    public int CallTreeHarvested()
    {
        print("Tree Harvested called.");
        CurrentTrees--;
        CalculateFogGoal();
        return CurrentTrees;
    }

    /// <summary>
    /// Increases the maximum and current tree counts, without refreshing the fog-goal - this is for when the game is initiated with its initial count of trees.
    /// </summary>
    /// <returns>The current count of trees in the world.</returns>
    public int CallMaximumTreeCountIncrease()
    {
        MaximumTrees++;
        CurrentTrees++;
        return CurrentTrees;
    }

    /// <summary>
    /// Recalculates what amount of fog should be in the world at a given time
    /// </summary>
    /// <returns>The new fog-goal</returns>
    private float CalculateFogGoal()
    {
        print("Current: " + CurrentTrees + " Max: " + MaximumTrees);
        FogIntensityGoal = ((MaximumFogIntensity - MinimumFogIntensity) / MaximumTrees) * (MaximumTrees - CurrentTrees);
        return FogIntensityGoal;
    }

    /// <summary>
    /// Is called when the game quits and resets the fog-values to their defaults.
    /// </summary>
    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_FogIntens", 0f);
        print("Values were reset.");
    }
}
