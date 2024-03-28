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

    void Start()
    {
        // Example of accessing a Rigidbody component safely
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Rigidbody component exists, safe to use it
        }
        else
        {
            // Rigidbody component doesn't exist, handle accordingly
        }
        // Disable gravity initially
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
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
        //// Constant forward movement
        //transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        //// Pitch control
        //float pitch = -Input.GetAxis("Vertical") * pitchSpeed * Time.deltaTime;
        //transform.Rotate(pitch, 0f, 0f, Space.Self);

        //// Roll control
        //float roll = Input.GetAxis("Horizontal") * rollSpeed * Time.deltaTime;
        //transform.Rotate(0f, 0f, -roll, Space.Self); // Negative for intuitive left/right roll
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        float rotation = Input.GetAxis("Horizontal") * rollSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0, Space.World);

        float rotationVertical = -Input.GetAxis("Vertical") * rollSpeed * Time.deltaTime;
        transform.Rotate(rotationVertical, 0, 0, Space.Self);
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
        // Check if the collision is with a laser beam not shot by the player
        if (collision.gameObject.tag == "EnemyLaser")
        {
            health -= 10; // Decrease health by 10, adjust value as needed
            Destroy(collision.gameObject); // Optionally destroy the laser on collision

            // Check health status
            if (health <= 0)
            {
                // Trigger any effects or behaviors for when the plane is destroyed
                OnPlaneDestroyed();
            }
        }
    }

    void OnPlaneDestroyed()
    {
        // Implement any additional effects upon destruction, like an explosion
        Debug.Log("Plane Destroyed");
        // Note: Consider adding an explosion effect or sound here
    }
}