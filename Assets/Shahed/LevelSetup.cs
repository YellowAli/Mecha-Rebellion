using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetup : MonoBehaviour
{
    // The index of this level in the build settings
    public int levelIndex;

    void Start()
    {
        LevelTracker.Instance.SetCurrentLevel(levelIndex);
    }
}
