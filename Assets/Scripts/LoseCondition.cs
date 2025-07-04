using UnityEngine;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            isLoggedIn = false;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            Time.timeScale = 0.25f;

            Timer.Instance.StopGameplayCounter();
            Timer.Instance.BeginCouting(timerTime);

            var user = SupabaseStuff.Instance.GetLoggedInUser();

            if (user == null)
            {
                Debug.LogError("ini di LoseCondition, user kosong");
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
                    Debug.Log(user);
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
            GameManager.Instance.InactivePlatformAndPlayer();
            Time.timeScale = 0f;
            loginCanvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Login Canvas is not assigned.");
        }
    }
}