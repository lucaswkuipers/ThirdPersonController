using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 600f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float maximumFallingSpeed = 100f;
    [SerializeField] float maximumWalkingSpeed = 10f;

    private Vector3 inputDirection;
    private Rigidbody rb;
    private float turnSmoothVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void FixedUpdate()
    {
        if (inputDirection.magnitude > 0.1f)
        {
            // Rotate
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDirection.normalized * speed * Time.deltaTime);
        }
    }
}
