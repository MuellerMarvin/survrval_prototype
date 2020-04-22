using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Well : MonoBehaviour
{
    public CircularDrive handleDrive;
    public Transform bucketUp;
    public Transform bucketDown;

    private GameObject bucket;
    public bool isBucketAttached {
        get
        {
            return _isBucketAttached;
        }
        set
        {
            handleDrive.enabled = value;
            _isBucketAttached = value;
        }
    }
    public bool isBucketUp
    {
        get
        {
            if(bucketHeight > 0.9)
            {
                return true;
            }
            return false;
        }
    }
    public bool isBucketDown
    {
        get
        {
            if(bucketHeight < 0.1)
            {
                return true;
            }
            return false;
        }
    }
    public float bucketHeight
    {
        get
        {
            return ((handleDrive.outAngle - handleDrive.minAngle) / (handleDrive.maxAngle - handleDrive.minAngle));
        }
    }
    public bool isBucketGrabbable
    {
        get
        {
            if(isBucketAttached)
            {
                return _isBucketGrabbable;
            }
            return false;
        }
        set
        {
            if(isBucketAttached)
            {
                bucket.GetComponent<Throwable>().enabled = value;
                bucket.GetComponent<Interactable>().enabled = value;
                bucket.GetComponent<Rigidbody>().freezeRotation = !value;
            }
        }
    }
    private bool _isBucketGrabbable;
    private bool _isBucketAttached;

    private void Start()
    {
        isBucketAttached = false;
    }

    /// <summary>
    /// Runs each frame
    /// </summary>
    private void Update()
    {
        // Update the bucket's position, to keep it in one place
        UpdateBucket();

        if(isBucketDown && isBucketAttached)
        {
            bucket.GetComponent<Fillable>().Fill(float.MaxValue);
        }

        if(isBucketUp && isBucketAttached)
        {
            // make it available to grab
            isBucketGrabbable = true;
        }

        if(!isBucketUp && isBucketAttached)
        {
            // make it unavailable to grab
            isBucketGrabbable = false;
        }
    }

    public GameObject DetachBucket()
    {
        this.isBucketAttached = false;
        GameObject returnBucket = this.bucket;
        this.bucket = null;
        return returnBucket;
    }

    public void AttachBucket(GameObject bucket)
    {
        this.bucket = bucket;
        this.isBucketAttached = true;
    }

    void UpdateBucket()
    {
        if (!isBucketAttached) return; // if the bucket isn't attached, don't run this function

        // Position the bucket
        print(bucketHeight);
        bucket.transform.position = (1 - bucketHeight) * bucketDown.position + bucketUp.position * bucketHeight;
        bucket.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
