using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantableSapling : MonoBehaviour
{
    public GameObject Tree;
    private bool TouchesGround = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReleasedFromHand()
    {
        if (TouchesGround)
        {
            print("PLANT");
        }
        else
        {
            print("DONT PLANT");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            TouchesGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            TouchesGround = false;
        }
    }
}
