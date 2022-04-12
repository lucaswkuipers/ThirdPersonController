using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float groundDetectionHeight;
    public float rideHeight;
    public float rideSpringStrength;
    public float rideSpringDamper;
    private Rigidbody rb;
    private float distanceToFeet;
    private Collider playerCollider;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerCollider = gameObject.GetComponent<Collider>();
        distanceToFeet = playerCollider.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        Vector3 feetPosition = new Vector3(transform.position.x, transform.position.y - distanceToFeet, transform.position.z);
        Ray landingRay = new Ray(feetPosition, Vector3.down);


        if (Physics.Raycast(landingRay, out raycastHit, groundDetectionHeight))
        {
            Debug.Log("Ray hit something");
            Debug.DrawRay(feetPosition, Vector3.down * groundDetectionHeight, Color.red);

            Vector3 playerVelocity = rb.velocity;
            Vector3 rayDirection = transform.TransformDirection(Vector3.down);

            Vector3 otherVelocity = Vector3.zero;
            Rigidbody hitBody = raycastHit.rigidbody;

            if (hitBody != null)
            {
                otherVelocity = hitBody.velocity;
            }

            float rayDirectionVelocity = Vector3.Dot(rayDirection, playerVelocity);
            float otherDirectionVelocity = Vector3.Dot(rayDirection, otherVelocity);

            float relativeVelocity = rayDirectionVelocity - otherDirectionVelocity;
            float rideHeightOffset = raycastHit.distance - rideHeight;
            float springForce = ((rideHeightOffset * rideSpringStrength * 100f) - (relativeVelocity * rideSpringDamper)) * rb.mass;

            rb.AddForce(rayDirection * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDirection * -springForce, raycastHit.point);
            }
        }

        else
        {
            Debug.DrawRay(feetPosition, Vector3.down * groundDetectionHeight);
        }
    }
}
