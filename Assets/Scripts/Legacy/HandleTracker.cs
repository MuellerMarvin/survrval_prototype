using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleTracker : MonoBehaviour
{
    private float eulerRotationsMade;
    private Vector3 lastRotation;
    public bool debug = false;

    public float rotationsMade
    {
        get
        {
            return (eulerRotationsMade / 360f);
        }
    }

    private void Start()
    {
        lastRotation = transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        eulerRotationsMade += Vector3.SignedAngle(transform.up, lastRotation, transform.right);
        lastRotation = transform.up;
        if(debug)
        {
            Debug.Log("Rotations made: " + rotationsMade);
        }
    }
}
