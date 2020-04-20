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
    public Transform waterOutlet;
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
            _currentFillAmount = Mathf.Clamp(_currentFillAmount, 0, maxFillAmount);
            UpdateWater();
        }
    }
    public float _currentFillAmount = 0;


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
    public float emptyingSpeedPerSecond = 5;

    private void Start()
    {
        UpdateWater();
    }

    void Update()
    {

        // if none of the 2 rotations is larger than 90°, just don't pour
        float highestRoation = System.Math.Abs(this.transform.rotation.normalized.x) < System.Math.Abs(this.transform.rotation.normalized.z) ? System.Math.Abs(this.transform.rotation.normalized.z) : System.Math.Abs(this.transform.rotation.normalized.x);
        
        // if the angle is too small, don't drip onto objects
        if(highestRoation < 0.5)
        {
            return;
        }

        // raycast downwards
        RaycastHit hit;
        Physics.Raycast(waterOutlet.position, -Vector3.up, out hit);
        Debug.DrawLine(waterOutlet.position, hit.point, Color.cyan);

        float pourAmountThisFrame = Mathf.Clamp(emptyingSpeedPerSecond * Time.deltaTime, 0, currentFillAmount);
        hit.collider.gameObject.SendMessage("Fill", pourAmountThisFrame);
        this.currentFillAmount -= pourAmountThisFrame;

    }

    /// <summary>
    /// Fills the container fully
    /// </summary>
    //public void Fill()
    //{
    //    // fill the container completely
    //    currentFillAmount = maxFillAmount;
    //}

    /// <summary>
    /// Fills the container by a certain amount
    /// </summary>
    /// <param name="amount"></param>
    public void Fill(float amount)
    {
        // if the bucket is fillable, add the amount
        if(fillable)
        {
            currentFillAmount += amount;
        }

        // if it is overfilled, set the container to its fullest state
        if(isFull)
        {
            currentFillAmount = maxFillAmount;
        }
    }

    void Drip(float amount)
    {

    }

    /// <summary>
    /// Gets run every time the fill-amount has changed, to change the visuals accordingly
    /// </summary>
    public void UpdateWater()
    {
        waterMesh.transform.localPosition = emptyPosition.localPosition + fullPosition.localPosition * (currentFillAmount / maxFillAmount);
    }
}
