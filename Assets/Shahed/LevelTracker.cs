using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTracker : MonoBehaviour
{
    private static LevelTracker instance;

    // The current level index
    private int currentLevelIndex = 0;

    public static LevelTracker Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Makes sure the object persists across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Ensures no duplicate instances are created
        }
   
}

// Method to set the current level index
public void SetCurrentLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
    }

    // Method to load the next level
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentLevelIndex);
        }
        else
        {
            Debug.LogWarning("No more levels available.");
        }
    }

    // Method to reload the current level
    public void RetryLevel()
    {
        Debug.Log("redirecting");
        SceneManager.LoadScene(currentLevelIndex);
    }
}
