using UnityEngine;

public class RotationUpright : MonoBehaviour
{
    Rigidbody rb;
    float upgrightSpringJoinStrength;
    float upgrightSpringJoinSpringDamper;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = transform.rotation;
        Quaternion currentRotation = transform.rotation;
        Quaternion goalRotation = ShortestRotation(currentRotation, targetRotation);

        Vector3 rotationAxis;
        float rotationDegrees;

        goalRotation.ToAngleAxis(out rotationDegrees, out rotationAxis);
        rotationAxis.Normalize();

        float rotationRadians = rotationDegrees * Mathf.Deg2Rad;

        rb.AddTorque((rotationAxis * (rotationRadians * upgrightSpringJoinStrength)) - (rb.angularVelocity * upgrightSpringJoinSpringDamper));
    }

    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }
        else return a * Quaternion.Inverse(b);
    }

    public static Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }
}