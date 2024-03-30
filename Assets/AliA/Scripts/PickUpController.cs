using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public ProjectileGunTutorial gunScript;
    public Transform player, gunContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;
    public float dropDuration = 1.0f; // Duration for the gun to drop down

    public bool equipped;
    public static bool slotFull;

    public TextMeshProUGUI ammunitionDisplay; // Add this reference for the ammunition display

    private void Start()
    {
        // Setup
        if (!equipped)
        {
            gunScript.enabled = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        // Check if player is in range and "E" is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
            PickUp();

        // Drop if equipped and "Q" is pressed
        if (equipped && Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(DropCoroutine());
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        // Make weapon a child of the camera and move it to the default position
        transform.SetParent(gunContainer);
        transform.localPosition = new Vector3(0.6f, -0.4f, 0.6f); // Set desired position
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = new Vector3(2f, 2f, 2f); // Set scale to 2 for all axes

        // Enable script
        gunScript.enabled = true;

        // Update ammunition display
        if (ammunitionDisplay != null)
            ammunitionDisplay.gameObject.SetActive(true);
    }



    private IEnumerator DropCoroutine()
    {
        equipped = false;
        slotFull = false;

        // Set parent to null
        transform.SetParent(null);

        // Disable shooting
        gunScript.enabled = false;

        // Calculate drop distance and direction
        float dropDistance = 0f;
        Vector3 dropDirection = Vector3.down;

        // Loop until drop distance reaches the desired value
        while (dropDistance < dropDuration)
        {
            // Calculate the amount to move based on the time elapsed
            float moveDistance = Mathf.Min(dropDuration - dropDistance, Time.deltaTime);
            transform.Translate(dropDirection * moveDistance / dropDuration, Space.World);
            dropDistance += moveDistance;

            yield return null;
        }

        // Ensure gun falls to ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            transform.position = hit.point;
        }
        else
        {
            // If no ground is found, just drop the gun at its current position
            Debug.LogWarning("No ground found under gun. Dropping at current position.");
        }

        // Add random rotation
        float random = Random.Range(-1f, 1f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f));

        // Update ammunition display
        if (ammunitionDisplay != null)
            ammunitionDisplay.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the drop direction with a ray
        Debug.DrawRay(transform.position, Vector3.down * 10f, Color.blue);
    }
}
