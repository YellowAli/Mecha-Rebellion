using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    private Vector3 desiredPosition = new Vector3(975f, 6.77f, 209f);
    private Quaternion desiredRotation = Quaternion.identity; // Set this to how you want the plane rotated

    void Start()
    {
        string planeName = PersistentData.Instance.selectedPlanePrefabName;

        // Load the prefab by name from the Resources/Prefabs folder
        GameObject planePrefab = Resources.Load<GameObject>("Prefabs/" + planeName);

        if (planePrefab != null)
        {
            GameObject instantiatedPrefab = Instantiate(planePrefab, desiredPosition, desiredRotation);

            // Now attach the main camera to the instantiated prefab
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.transform.SetParent(instantiatedPrefab.transform);

                // Set the camera's local position relative to the prefab (adjust these values as needed)
                mainCamera.transform.localPosition = new Vector3(0, 5, -10); // Example offset
                mainCamera.transform.localRotation = Quaternion.Euler(10, 0, 0); // Example rotation
            }
        }
        else
        {
            Debug.LogError("Could not find prefab: " + planeName);
        }
    }
}
