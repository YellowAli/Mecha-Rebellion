using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the Hoops Manager that the player has passed through the hoop
            HoopsManager.Instance.PlayerPassedThroughHoop(gameObject);
        }
    }
}
