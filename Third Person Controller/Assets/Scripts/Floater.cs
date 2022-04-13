using UnityEngine;

public class Floater : MonoBehaviour
{
    public float groundDetectionHeight;
    public float rideHeight;
    public float rideSpringStiffness;
    public float rideSpringDamper;

    private Rigidbody rb;
    private float distanceToFeet;
    private Collider objectCollider;
    private RaycastHit raycastHit;
    private Vector3 feetPosition;

    private void Start()
    {
        InitializeVariables();
    }

    private void FixedUpdate()
    {
        SetFeetPosition();
        FixObjectPositionIfNeeded();
    }

    private void InitializeVariables()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        objectCollider = gameObject.GetComponent<Collider>();
        distanceToFeet = objectCollider.bounds.extents.y;
    }

    private void FixObjectPositionIfNeeded()
    {
        if (!IsNearGround())
        {
            Debug.DrawRay(feetPosition, Vector3.down * groundDetectionHeight);
            return;
        }

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

        float springForce = ((rideHeightOffset * rideSpringStiffness) - (relativeVelocity * rideSpringDamper));
        //float springForce = rideHeightOffset * rideSpringStiffness - relative;

        //if (Mathf.Abs(springForce) <= 0.01f) { return; }

        Debug.Log("Ray hit ground (near ground)");
        print($"rayDirectionVelocity: {rayDirectionVelocity})");
        print($"otherDirectionVelocity: {otherDirectionVelocity})");
        print($"relativeVelocity: {relativeVelocity})");
        print($"rideHeightOffset: {rideHeightOffset})");
        print($"springForce: {rideHeightOffset})");

        rb.AddForce(rayDirection * springForce, ForceMode.Acceleration);

        if (hitBody != null)
        {
            hitBody.AddForceAtPosition(rayDirection * -springForce, raycastHit.point);
        }
    }

    private bool IsNearGround()
    {
        return Physics.Raycast(GetLandingRay(), out raycastHit, groundDetectionHeight);
    }

    private Ray GetLandingRay()
    {
        return new Ray(feetPosition, Vector3.down);
    }

    private void SetFeetPosition()
    {
        feetPosition = new Vector3(transform.position.x, transform.position.y - distanceToFeet + 0.1f, transform.position.z);
    }
}
