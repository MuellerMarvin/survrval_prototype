using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves to route to the actual "Fillable"-script of the object (in case the checks hit a different part)
/// </summary>
public class FillableRouter : MonoBehaviour
{
    public Fillable parentScript;

    public void Fill()
    {
        parentScript.Fill();
    }

    public void Fill(float amount)
    {
        parentScript.Fill(amount);
    }
}
