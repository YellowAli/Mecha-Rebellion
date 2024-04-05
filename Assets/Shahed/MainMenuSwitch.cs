using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSwitch : MonoBehaviour
{
    public void StartGame()
    {
        // Assuming "Level1" is the name of your game scene
        Debug.Log("starting");
        SceneManager.LoadScene("Level1");
    }
}
