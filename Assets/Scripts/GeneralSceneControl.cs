using UnityEngine;

public class GlobalQuitManager : MonoBehaviour
{
    // Singleton instance
    public static GlobalQuitManager Instance;

    void Awake()
    {
        // Make sure there's only one instance of GlobalQuitManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    // Quit game method
    private void QuitGame()
    {
        // Exit the game
        Application.Quit();

        // If you're in the Unity Editor, stop play mode
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
