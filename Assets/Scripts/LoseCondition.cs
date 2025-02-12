using UnityEngine;
using System.Threading.Tasks;

public class LoseCondition : MonoBehaviour
{
    public SceneLoader sceneLoader;

    async void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        Time.timeScale = 0.1f;

        if (rb != null && collision.gameObject.CompareTag("Player"))
        {
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
