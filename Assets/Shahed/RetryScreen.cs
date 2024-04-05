using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryScreen : MonoBehaviour

{
    private void OnEnable()
    {
        // Ensure the cursor is visible and unlocked when the retry screen is active
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    // This method will be called when the retry button is pressed.
    public void OnRetryButtonPressed()
    {
        SceneManager.LoadScene(2);
        Debug.Log("clicked");
        // Calls the RetryLevel method on the singleton instance of LevelTracker.
        LevelTracker.Instance.RetryLevel();
    }
}
