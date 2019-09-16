using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject TrackingObject;
    [SerializeField]
    private GameObject limLeftPoint;
    [SerializeField]
    private GameObject limRightPoint;
    [SerializeField]
    private GameObject limUpPoint;
    [SerializeField]
    private GameObject limDownPoint;

    private float limLeft;
    private float limRight;
    private float limUp;
    private float limDown;

    private Vector3 TrackingObjectPosition;
    
    void Start()
    {
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = camHalfHeight * Camera.main.aspect;
        limLeft = limLeftPoint.transform.position.x + camHalfWidth;
        limRight = limRightPoint.transform.position.x - camHalfWidth;
        limUp = limUpPoint.transform.position.y - camHalfHeight;
        limDown = limDownPoint.transform.position.y + camHalfHeight;
    }

    void Update()
    {
        if (TrackingObject == null)
            return;

        TrackingObjectPosition = TrackingObject.transform.position;
        TrackingObjectPosition.z = -10;
        TrackingObjectPosition.x = Mathf.Min(Mathf.Max(limLeft, TrackingObjectPosition.x), limRight);
        TrackingObjectPosition.y = Mathf.Min(Mathf.Max(limDown, TrackingObjectPosition.y), limUp);

        transform.position = TrackingObjectPosition;

    }
}
