using UnityEngine;
using System.Threading.Tasks;
using App;

public class LoseCondition : MonoBehaviour
{
    public static LoseCondition Instance { get; set; }
    public SceneLoader sceneLoader;
    [SerializeField] Timer timer; // Remove direct assignment
    [SerializeField] private float timerTime = 3f;
    [SerializeField] private CanvasGroup loginCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Initialize the Timer reference
        timer = Timer.Instance;
    }

    private async void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            Time.timeScale = 0.25f;

            // Check if Timer instance exists
            if (timer != null)
            {
                timer.StopGameplayCounter();
                timer.BeginCouting(timerTime);
            }
            else
            {
                Debug.LogError("Timer instance is null.");
            }

            var user = SupabaseStuff.Instance.GetLoggedInUser();
            if (user == null)
            {
                OpenLoginCanvas();
                if (ScoreManager.Instance == null)
                {
                    Debug.LogError("ScoreManager instance is null.");
                }
            }
            else
            {
                if (ScoreManager.Instance != null)
                {
                    await ScoreManager.Instance.SaveScoreToSupabase();
                    sceneLoader.LoadScene("Leaderboard");
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
        if (loginCanvas != null)
        {
            Time.timeScale = 0f;
            loginCanvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Login Canvas is not assigned.");
        }
    }
}