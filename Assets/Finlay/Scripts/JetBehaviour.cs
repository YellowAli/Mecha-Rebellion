using UnityEngine;

public class JetBehaviour : MonoBehaviour
{
    public GameObject laserBeam;
    public float tiltAmount = 30.0f;
    public float health = 100f; // Set the initial health of the plane

    public float forwardSpeed = 10.0f;
    public float pitchSpeed = 100.0f; // Speed of pitching up or down
    public float rollSpeed = 100.0f;  // Speed of rolling left or right

    private Rigidbody rb; // Reference to the Rigidbody component
    private KeyCode boostKey = KeyCode.LeftShift;

    void Start()
    {
        // Attempt to get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Check if Rigidbody component was found
        if (rb != null)
        {
            // Disable gravity initially, Rigidbody component exists
            rb.useGravity = false;
        }
        else
        {
            // Rigidbody component doesn't exist, so add it
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            // Set other Rigidbody properties as needed
        }
    }


    void Update()
    {
        
        if (health > 0)
        {
            // Move the plane forward constantly
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

            // Handle rotation and banking
            HandleMovement();

            // Shooting laser beam
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootLaser();
            }
        }
        else
        {
            // Enable gravity to make the plane fall when health runs out
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    void HandleMovement()
    {
        Vector3 movement = Vector3.forward * forwardSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + transform.TransformDirection(movement));

        float rotationHorizontal = Input.GetAxis("Horizontal") * rollSpeed * Time.deltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotationHorizontal, 0f);
        rb.MoveRotation(rb.rotation * turn);

        float pitch = -Input.GetAxis("Vertical") * pitchSpeed * Time.deltaTime;
        Quaternion pitchRotation = Quaternion.Euler(pitch, 0f, 0f);
        rb.MoveRotation(rb.rotation * pitchRotation);

        var eulerRotation = rb.rotation.eulerAngles;
        eulerRotation.z = 0;
        rb.rotation = Quaternion.Euler(eulerRotation);

        // Boost input
        if (Input.GetKeyDown(boostKey))
        {
            forwardSpeed = 30.0f;
        }
        if (Input.GetKeyUp(boostKey))
        {
            forwardSpeed = 10.0f;
        }
    }


    void ShootLaser()
    {
        // Calculate the spawn position a bit in front of the plane
        Vector3 spawnPosition = transform.position + transform.forward * 2f; // Adjust 2f to control the distance

        Quaternion spawnRotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);
        Instantiate(laserBeam, spawnPosition, spawnRotation);
    }

    void OnCollisionEnter(Collision collision)
    {
        rb.angularVelocity = Vector3.zero;
        // Check if the collision is with a laser beam not shot by the player
        if (collision.gameObject.tag == "EnemyLaser")
        {
            health -= 10; // Decrease health by 10, adjust value as needed
            Destroy(collision.gameObject); // Optionally destroy the laser on collision

            // Check health status
            if (health <= 0)
            {
                // Trigger any effects or behaviors for when the plane is destroyed
                DestroyPlane();
            }
        }
    }

    public void DestroyPlane()
    {
        // Here you can add an explosion effect or sound if you want
        Destroy(gameObject);

        // Also consider what to do after the plane is destroyed
        // For example, triggering a game over UI or respawning the plane
    }
}