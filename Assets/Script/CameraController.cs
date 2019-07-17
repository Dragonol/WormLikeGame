using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject TrackingObject;

    private Vector3 TrackingObjectPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TrackingObjectPosition = TrackingObject.transform.position;
        transform.position = new Vector3(TrackingObjectPosition.x,
                                         TrackingObjectPosition.y,
                                         -100);
    }
}
