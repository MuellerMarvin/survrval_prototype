using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Fillable : MonoBehaviour
{
    // Configuration
    public GameObject waterMesh;
    public Transform emptyPosition;
    public Transform fullPosition;
    public float maxFillAmount = 1;
    public float currentFillAmount
    {
        get
        {
            return _currentFillAmount;
        }
        set
        {
            _currentFillAmount = value;
            UpdateWater();
        }
    }
    private float _currentFillAmount = 0;


    // Utility Properties
    /// <summary>
    /// Checks if the bucket is full or still has room for fluids
    /// </summary>
    public bool isFull {
        get
        {
            if(currentFillAmount >= maxFillAmount)
            {
                return true; // the container is full
            }
            return false; // it isn't full
        }
    }
    /// <summary>
    /// Checks if the bucket is in a position to be filled with fluids
    /// </summary>
    public bool fillable // determines if the container can receive fluids
    {
        get
        {
            return true; // TO-DO (based on rotation)
        }
    }

    void Update()
    {
        // TO-DO see if the container is being tilted, to see if it should 
        Fill(0.1f);
    }

    /// <summary>
    /// Fills the container fully
    /// </summary>
    void Fill()
    {
        // fill the container completely
        currentFillAmount = maxFillAmount;
    }

    /// <summary>
    /// Fills the container by a certain amount
    /// </summary>
    /// <param name="amount"></param>
    void Fill(float amount)
    {
        // if the bucket is fillable, add the amount
        if(fillable)
        {
            currentFillAmount += amount;
        }

        // if it is overfilled, set the container to its fullest state
        if(isFull)
        {
            Fill();
        }
    }

    /// <summary>
    /// Gets run every time the fill-amount has changed, to change the visuals accordingly
    /// </summary>
    private void UpdateWater()
    {
        waterMesh.transform.position = emptyPosition.position + fullPosition.localPosition * (currentFillAmount / maxFillAmount);
    }
}
