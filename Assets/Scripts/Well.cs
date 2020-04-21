using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Well : MonoBehaviour
{
    // Game objects and transforms
    public GameObject handle;
    public GameObject bucket;
    public Transform bucketDown;
    public Transform bucketUp;

    // Other configurations
    public float maxHandleRotations;

    #region Properties
    // External Scripts
    private Fillable bucketScript {
        get
        {
            return (Fillable)bucket.GetComponent<Fillable>();
        }
    }
    private HandleTracker handleTracker
    {
        get
        {
            return (HandleTracker)handle.GetComponent<HandleTracker>();
        }
    }

    // Other Propertie
    public bool bucketAttached
    {
        get { return _bucketAttached; }
        set
        {
            foreach(MeshRenderer childRenderer in bucket.GetComponentsInChildren<MeshRenderer>())
            {
                childRenderer.enabled = value;
            }
            _bucketAttached = value;
        }
    }
    public float attachedBucketFillAmount
    {
        get
        {
            return bucketScript.currentFillAmount;
        }
        set
        {
            bucketScript.currentFillAmount = value;
        }
    }
    public float attachedBucketMaxFillAmount
    {
        get
        {
            return bucketScript.maxFillAmount;
        }
        set
        {
            bucketScript.maxFillAmount = value;
        }
    }
    public float bucketHeight
    {
        get
        {
            return _bucketHeight;
        }
        set
        {
            print("Valueee:" + value);
            _bucketHeight = Mathf.Clamp01(value);
            bucket.gameObject.transform.localPosition = bucketDown.localPosition + (bucketUp.localPosition * _bucketHeight);
        }
    }
    public float currentHandleRotations
    {
        get
        {
            print(handle.transform.localEulerAngles);
            return 0f;
        }
        set { }
    }

    // Properties Storage
    private bool _bucketAttached;
    private float _bucketHeight;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bucketHeight = (handleTracker.rotationsMade / maxHandleRotations);
    }
}
