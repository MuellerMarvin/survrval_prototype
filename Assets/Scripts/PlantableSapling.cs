using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantableSapling : MonoBehaviour
{
    public GameObject tree;
    public EnvironmentManager environmentManager;
    private bool TouchesGround = false;

    public void ReleasedFromHand()
    {
        if (TouchesGround)
        {
            print("PLANT");
            ((HarvestableTree)tree.gameObject.GetComponent(typeof(HarvestableTree))).wasPlanted = true; // tell the tree it was planted, not spawned
            ((HarvestableTree)tree.gameObject.GetComponent(typeof(HarvestableTree))).environmentManager = this.environmentManager; // pass the tree the environment manager of this scene
            GameObject plantedTree = Instantiate(this.tree, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.Euler(-90f, Random.Range(0,360), 0));
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
