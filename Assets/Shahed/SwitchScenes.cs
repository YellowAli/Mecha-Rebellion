// Attach this script to the player
// PlayerCollision.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    [SerializeField] private string sceneNameToLoad; // The name of the scene to load

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            // Assuming your planes in the scene are linked to their prefabs,
            // store the prefab name in the PersistentData
            PersistentData.Instance.selectedPlanePrefabName = collision.gameObject.name;

            // Load the new scene
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
