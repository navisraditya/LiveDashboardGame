using UnityEngine;
using System.Threading.Tasks;

public class LoseCondition : MonoBehaviour
{
    public SceneLoader sceneLoader;
    [SerializeField] Timer timer;
    [SerializeField] float timerTime = 3f;

    async void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        Time.timeScale = 0.25f;

        if (rb != null && collision.gameObject.CompareTag("Player"))
        {
            timer.StopGameplayCounter();
            timer.BeginCouting(timerTime);
            if (ScoreManager.Instance != null)
            {
                await ScoreManager.Instance.SaveScoreToSupabase(); // Ensure score is saved before switching scenes
            }
            else
            {
                Debug.LogError("ScoreManager instance is null.");
            }

            sceneLoader.LoadScene("Leaderboard");
        }
    }
}
