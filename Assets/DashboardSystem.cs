using System.Threading.Tasks;
using App;
using TMPro;
using UnityEngine;

public class DashboardSystem : MonoBehaviour
{
    [SerializeField] TMP_Text leaderboard;
    [SerializeField] int topScoreLimit = 20;
    
    int scoreIdx = 1;
    
    async void Awake()
    {
        if(SupabaseStuff.Instance != null) {
            await UpdateLeaderboardUI();
        } else {
            SupabaseStuff.Instance.GetSupabaseClient();
            await UpdateLeaderboardUI();
        }
    }

    private async Task UpdateLeaderboardUI() {
        scoreIdx = 1;
        var topscores = await ScoreManager.Instance.FetchScores(topScoreLimit);
        foreach(var score in topscores) {
            leaderboard.text += $"{scoreIdx}.     {score.PlayerId.PadRight(15)} {score.ScoreValue}\n";
            scoreIdx++;
        }
        Debug.Log("udah selesai");
    }
}
