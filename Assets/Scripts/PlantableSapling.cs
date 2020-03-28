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
            Tree.SendMessage("SetWasPlanted", true);
            Instantiate(Tree, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.Euler(-90f, 0, 0));
            Destroy(this.gameObject);
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

    private void OnTriggerStay(Collider other)
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
