using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopsManager : MonoBehaviour
{
    public static HoopsManager Instance { get; private set; }

    private int currentHoopIndex = 0;
    private GameObject[] hoops;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of the manager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Assuming hoops are tagged "Hoop" and sorted in the order they are to be passed through
        hoops = GameObject.FindGameObjectsWithTag("Hoop");
        System.Array.Sort(hoops, (a, b) => string.Compare(a.name, b.name, System.StringComparison.Ordinal));
    }

    public void PlayerPassedThroughHoop(GameObject hoop)
    {
        // Check if the passed hoop is the next one in the sequence
        if (hoops[currentHoopIndex] == hoop)
        {
            currentHoopIndex++;
        }
        else
        {
            // If not, the player has missed a hoop
            JetBehaviour playerPlane = FindObjectOfType<JetBehaviour>();
            if (playerPlane != null)
            {
                playerPlane.DestroyPlane();
            }
        }
    }

    public GameObject GetCurrentHoop()
    {
        if (currentHoopIndex >= 0 && currentHoopIndex < hoops.Length)
        {
            return hoops[currentHoopIndex];
        }
        return null; // No more hoops or invalid index
    }
}