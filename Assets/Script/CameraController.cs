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

    void Update()
    {
        if (TrackingObject == null)
            return;

        TrackingObjectPosition = TrackingObject.transform.position;
        transform.position = new Vector3(TrackingObjectPosition.x,
                                         TrackingObjectPosition.y,
                                         -100);

    }
}
