// Attach this script to the player
// PlayerCollision.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    EnemyBehaviour enemy1, enemy2, enemy3, enemy4, enemy6,enemy5;

    [SerializeField] private string sceneNameToLoad; // The name of the scene to load

    void Start()
    {
       enemy1 = GameObject.Find("CannonMachine Variant 2").GetComponent<EnemyBehaviour>();
        enemy2 = GameObject.Find("CannonMachine Variant 2 (1)").GetComponent<EnemyBehaviour>();
        enemy3 = GameObject.Find("CannonMachine Variant 2 (2)").GetComponent<EnemyBehaviour>();
        enemy4 = GameObject.Find("CannonMachine Variant 2 (3)").GetComponent<EnemyBehaviour>();
        enemy6 = GameObject.Find("CannonMachine Variant 2 (4)").GetComponent<EnemyBehaviour>();
        enemy5 = GameObject.Find("CannonMachine Variant 2 (5)").GetComponent<EnemyBehaviour>();

    }


    void OnCollisionEnter(Collision collision)
    {
        // Compare the collision object tag
        if (collision.gameObject.CompareTag("Plane") && !enemy5.alive && !enemy1.alive && !enemy2.alive && !enemy3.alive && !enemy4.alive && !enemy6.alive)
        {
            // Record the selected plane
            SelectPlane(collision.gameObject);

            // Transition to the next level
            SceneManager.LoadScene(sceneNameToLoad); // Replace with your actual Level 2 scene name or index
        }
    }
    private void SelectPlane(GameObject plane)
    {
        // Assuming the plane's name is the prefab name you want to save
        PersistentData.Instance.selectedPlanePrefabName = plane.name;

        // Optionally, you can also instantiate the selected plane directly if needed
        // Instantiate(plane, desiredPosition, desiredRotation);
    }
}

