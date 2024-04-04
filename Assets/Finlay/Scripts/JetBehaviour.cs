using System.Collections;
using UnityEngine;

public class JetBehaviour : MonoBehaviour
{
    public GameObject laserBeam;
    public float tiltAmount = 30.0f;
    public float health = 100f; // Set the initial health of the plane

    private bool controlsEnabled = true;
    public float forwardSpeed = 150.0f;
    public float pitchSpeed = 100.0f; // Speed of pitching up or down
    public float rollSpeed = 100.0f;  // Speed of rolling left or right

    private Rigidbody rb; // Reference to the Rigidbody component
    private KeyCode boostKey = KeyCode.LeftShift;

    public LayerMask groundMask;

    public GameObject fire;

    public bool alive = true;



    void Start()
    {
        // Attempt to get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        alive = true;

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
        if (controlsEnabled)
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
                forwardSpeed *= 5;
            }
            if (Input.GetKeyUp(boostKey))
            {
                forwardSpeed /= 5;
            }
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
        if ((groundMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            DestroyPlane();
        }
    }

    public void DestroyPlane()
    {
        alive = false;
        // Here you can add an explosion effect or sound if you want
        controlsEnabled = false;
        // Stop forward movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Enable gravity
        rb.useGravity = true;
        Instantiate(fire, transform.position, Quaternion.identity);
    }
}