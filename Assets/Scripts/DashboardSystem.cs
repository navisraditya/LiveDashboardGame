using System.Linq;
using App;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DashboardSystem : MonoBehaviour
{
    public static DashboardSystem Instance {get; set;}

    [SerializeField] TMP_Text leaderboard;
    [SerializeField] TMP_Text statsDetail;
    [SerializeField] TMP_Text topPlayerDetail;
    [SerializeField] TMP_Text yourRankDetail;
    [SerializeField] int topScoreLimit = 20;
    [SerializeField] CanvasGroup loading;
    [SerializeField] CanvasGroup loginBanner;

    
    int scoreIdx = 1;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        if(SupabaseStuff.Instance != null) {
            UpdateLeaderboardUI();
        } else {
            _ = SupabaseStuff.Instance.GetSupabaseClient();
            UpdateLeaderboardUI();
        }
    }

    private void UpdateLeaderboardUI() {
        loading.gameObject.SetActive(true);
        
        UpdateLeaderboard().ContinueWith(() => {
            Debug.Log("udah selesai");
            loading.gameObject.SetActive(false);
        }).Forget();
    }
    private async UniTask UpdateLeaderboard() {
        var user = SupabaseStuff.Instance.GetLoggedInUser();
        
        scoreIdx = 1;

        int currRank = 0;
        int playerAttempt = 0;
        float totalPlaytime = 0;
        int highestScore = 0;

        var topscores = await ScoreManager.Instance.FetchScores(topScoreLimit);
        Debug.Log("udah dapet");
        Debug.Log(topscores.Count());

        if(topscores != null) {
            foreach(var score in topscores) {
                leaderboard.text += $"{scoreIdx}.     {score.Player_id,-15} {score.Score}\n";
                if(user != null) {
                    if(user.UserMetadata.Username == score.Player_id) {
                        if(currRank == 0) {
                            currRank = scoreIdx;
                        }
                        playerAttempt++;
                        totalPlaytime += score.Playtime;
                        if(highestScore >= score.Score) {
                            highestScore = score.Score;
                        }
                    }
                }
                scoreIdx++;
            }
        } else {
            Debug.Log("kosong");
        }

        if(user == null) {
            loginBanner.gameObject.SetActive(true);
        } else {
            loginBanner.gameObject.SetActive(false);
            statsDetail.text += $"Total Attempts:     {playerAttempt,15} attempts\n";
            statsDetail.text += $"Highest Scores:     {highestScore,15} points\n";
            statsDetail.text += $"Total Playtimes:     {totalPlaytime,15} seconds\n";
        }

        topPlayerDetail.text += $"{topscores.First().Player_id}";
        Debug.Log($"{topscores.First().Player_id}");

        yourRankDetail.text += $"{currRank}";
    }
}