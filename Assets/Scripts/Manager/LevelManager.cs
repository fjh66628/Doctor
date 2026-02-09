using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelIndex = 0;

    public void LoadLevel(int index)
    {
        levelIndex = index;
        // Hook into SceneManager.LoadScene(index) if desired
    }

    public void CompleteLevel()
    {
        // advance or notify GameManager
//        if (GameManager.Instance != null)
 //          GameManager.Instance.currentLevel = levelIndex;
    }
}
