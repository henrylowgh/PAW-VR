using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    private Vector3 lastPosition;
    [SerializeField] private float totalDistance;
    [SerializeField] private bool isTrackingDistance;

    void Start()
    {
        // Initialize the lastPosition with the starting position of the object
        lastPosition = transform.position;
        isTrackingDistance = false;
    }

    void Update()
    {
        if (isTrackingDistance)
        {
            // Calculate the distance traveled since the last frame
            float distance = Vector3.Distance(transform.position, lastPosition);

            // Add it to the total distance
            totalDistance += distance;

            // Update the last position for the next frame
            lastPosition = transform.position;

            //Debug.Log("Total Distance Travelled: " + totalDistance + " units");
        }
    }

    public void DisableTracking()
    {
        isTrackingDistance = false;
    }

    public void EnableTracking()
    {
        isTrackingDistance = true;
    }

    public float GetTotalDistance()
    {
        return totalDistance;
    }
}

