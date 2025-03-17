using UnityEngine;
using System.Threading.Tasks;
using App;

public class LoseCondition : MonoBehaviour
{
    public static LoseCondition Instance {get; set;}
    public bool isLoggedIn = false;
    public SceneLoader sceneLoader;
    // [SerializeField] private Timer timer;
    [SerializeField] private float timerTime = 3f;
    [SerializeField] private CanvasGroup loginCanvas;

    private async void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            isLoggedIn = false;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            // Slow down time for effect
            Time.timeScale = 0.25f;

            // Stop the gameplay counter and start the timer
            Timer.Instance.StopGameplayCounter();
            Timer.Instance.BeginCouting(timerTime);

            // Check if a user is logged in
            var user = SupabaseStuff.Instance.GetLoggedInUser();

            if (user == null)
            {
                // No user is logged in, open the login canvas
                OpenLoginCanvas();

                // Save the score (if ScoreManager exists)
                if (ScoreManager.Instance == null)
                {
                    Debug.LogError("ScoreManager instance is null.");
                }

            }
            else
            {
                // User is logged in, save the score and load the leaderboard
                if (ScoreManager.Instance != null)
                {
                    await ScoreManager.Instance.SaveScoreToSupabase(); // Save the score
                    sceneLoader.LoadScene("Leaderboard"); // Load the leaderboard scene
                }
                else
                {
                    Debug.LogError("ScoreManager instance is null.");
                }
            }
        }
    }

    private void OpenLoginCanvas()
    {
        // GameManager.Instance.DestroyPlatforms();
        if (loginCanvas != null)
        {
            Time.timeScale = 0f;
            loginCanvas.gameObject.SetActive(true); // Show the login canvas
        }
        else
        {
            Debug.LogError("Login Canvas is not assigned.");
        }
    }
}